using Npgsql;
using System;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;
using Z.BulkOperations;

namespace LoadFoxProDBToSQL.Database
{
    public class PostgresConnection : IDatabaseConnection, IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private ILogger _logger;
        private string _serverName;
        public PostgresConnection(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
            _connection = Connect();
        }

        private NpgsqlConnection Connect()
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Cannot open connection to {_serverName}. Ex: {ex.Message}");
            }
            return connection;
        }

        public bool CreateDatabase(string databaseName)
        {

        }

        public DataTable GetSchema(string schemaName)
        {
            DataTable dataTable = null;
            try
            {
                var data = _connection.GetSchema();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred in {this.GetType().Name}.GetSchema");
            }
            return dataTable;
        }

        public void CreateTable(DataTable dataTable, string tableName, bool replaceSpaces = true) 
        {
           _logger.Information($"Create table Started for Table: {tableName}" + Environment.NewLine);
            

            using (_connection)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                string createSql = "";
                try
                {
                    using (_connection)
                    {
                        createSql = BuildCreateStatement(dataTable, tableName, false);
                        NpgsqlCommand createCmd = _connection.CreateCommand();
                        createCmd.CommandText = createSql;
                        createCmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                   _logger.Error(ex, $"Sql Exception:{ex.Message} Stack Trace:{ex.StackTrace} for" +
                       $" tablename: {tableName} sql statement {createSql}");         
                }
                catch (Exception ex2)
                {
                   _logger.Information($"Create Table Error: {ex2.Message} Stack Trace: {ex2.StackTrace} " +
                       $"for tablename: {tableName} sql statement {createSql}");
                    
                }
                _connection.Close();
            }
           _logger.Information($"Table {tableName} created successfully." + Environment.NewLine);
            

        }
        private string BuildCreateStatement(DataTable columnsDataTable, string tableName,  bool replaceSpaces)
        {
            _logger.Information($"Begin Building Create Statement");
            int i = 1;
            string sqlColumnCreate = "";
            foreach (var row in columnsDataTable.AsEnumerable().OrderBy(x => x.ItemArray[6]))
            {

                var columnName = row[3].ToString();
                var dataTypeString = row[11].ToString();
                var maxLength = row[13].ToString();
                var precision = row[15].ToString();
                var scaleString = row[16].ToString();
                int.TryParse(scaleString, out int scale);
                var dataType = GetDataType(dataTypeString, maxLength, precision, scale);

                if (columnName.Contains(" "))
                    columnName = columnName.Replace(" ", "_");
                if (replaceSpaces)
                {
                    columnName = columnName.Replace(" ", "_").ToLower();
                    tableName = tableName.Replace(" ", "_").ToLower();
                }
                string primKey = "";
                if (i == 1 && columnsDataTable.Rows.Count > 1)
                {
                    primKey = " PRIMARY KEY";
                    sqlColumnCreate = $"CREATE TABLE  \"{tableName.ToLower()}\" ( \"{columnName.ToLower().ToString().Trim()}\"   {dataType}, ";
                }
                else if (i == 1 && columnsDataTable.Rows.Count == 1)
                {
                    sqlColumnCreate = $"CREATE TABLE \"{tableName.ToLower()}\" ( \"{columnName.ToLower().ToString().Trim()}\"   {dataType}); ";

                }
                else if (i == columnsDataTable.Rows.Count)
                    sqlColumnCreate = sqlColumnCreate + $" \"{columnName.ToLower().ToString().Trim()}\" {dataType}); ";
                else
                    sqlColumnCreate = sqlColumnCreate + $" \"{columnName.ToLower().ToString().Trim()}\" {dataType}, ";
                i++;
            }
            return sqlColumnCreate;
        }

        public static string GetDataType(string dataType, string maxLength, string precision = "0", int scale = 0)
        {
            //254 is the max length of a column on FoxPro

            switch (dataType)
            {
                case "129":
                    dataType = $"Text";
                    break;
                case "7":
                case "133":
                    dataType = "Date";
                    //dataType = $"Character Varying(MAX)";
                    break;
                case "20":
                    dataType = "bigint";
                    break;
                case "128":
                    dataType = "bytea";
                    break;
                case "11":
                    dataType = "Boolean";
                    break;
                case "200":
                case "8":
                    maxLength = maxLength == String.Empty ? "65535" : maxLength;
                    dataType = $"Character Varying({maxLength})";
                    break;
                case "130":
                    dataType = "Text";
                    break;
                case "6":
                    dataType = "money";
                    break;
                case "134":
                    dataType = "Timestamp";
                    break;
                case "135":
                    dataType = "Timestamp";
                    break;
                case "131":
                case "5":
                case "14":
                    precision = precision == String.Empty ? "19" : precision;
                    //scale = scale == 0 ? 2 : scale;
                    dataType = $"NUMERIC({precision}, {scale})";
                    break;
                case "72":
                    dataType = $"UUID";
                    break;
                case "3":
                    dataType = "Integer";
                    break;
                default:
                    dataType = $"Character Varying(65535)";
                    break;
            };

            return dataType;

        }

        public void InsertData(IDataReader dataReader, DataTable columns, string tableName)
        {
            _logger.Information("Insert Data started for Table: " + tableName + Environment.NewLine);
            StringBuilder sb = new StringBuilder();
            if (_connection.State != ConnectionState.Open) 
            { 
                _connection.Open();
            }

            using (_connection)
            {


                var setEncoding = "set client_encoding = 'UTF8'";
                var command = _connection.CreateCommand();
                command.CommandText = setEncoding;
                command.ExecuteNonQuery();

                using (var bulk = new BulkOperation(_connection))
                {
                    try
                    {
                        bulk.Connection = _connection;
                        bulk.AutoMap = AutoMapType.ByName;
                        bulk.BatchSize = 10000;
                        bulk.BatchTimeout = 360;
                        bulk.Provider = ProviderType.PostgreSql;
                        bulk.DestinationSchemaName = "public";
                        bulk.DestinationTableName = tableName.Replace(" ", "_");
                        bulk.CaseSensitive = CaseSensitiveType.DestinationInsensitive;
                        bulk.ColumnMappings = BuildColumnMapping(columns);
                        bulk.AutoTruncate = true;
                        bulk.DataSource = dataReader;
                        bulk.UseLogDump = true;
                        bulk.LogDump = sb;
                        bulk.UseRowsAffected = true;

                        bulk.BulkInsert();
                         _logger.Information("Rows Inserted:" + bulk.ResultInfo.RowsAffectedInserted + Environment.NewLine);
                         _logger.Information("Insert Data Completed Successfully for Table: " + tableName + Environment.NewLine);
                    }
                    catch (SqlException ex)
                    {
                         _logger.Information($"LogDump:{sb.ToString()}" + Environment.NewLine);
                         _logger.Information("SQL Exception:" + ex.Message + ", Stack Trace:" + ex.StackTrace + Environment.NewLine);
                    }
                    catch (Exception ex2)
                    {
                         _logger.Information($"LogDump:{sb.ToString()}" + Environment.NewLine);
                         _logger.Information("SQL Exception:" + ex2.Message + ", Stack Trace:" + ex2.StackTrace + Environment.NewLine);
                    }
                    _connection.Close();
                }
            }

             _logger.Information($"StringBuilderLog for DataTable:{tableName} {sb.ToString()}");
 
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
