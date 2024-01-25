using Npgsql;
using System;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LoadFoxProDBToSQL.Database
{
    public class PostgresConnection
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private ILogger _logger;
        private string _serverName;
        public PostgresConnection(string connectionString, string serverName, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
            _connection = Connect();
            _serverName = serverName;
        }

        public NpgsqlConnection Connect()
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
                        createSql = BuildCreateStatement(dataTable, tableName);
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
        private static string BuildCreateStatement(DataTable columnsDataTable, string tableName,  bool replaceSpaces)
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

    }
}
