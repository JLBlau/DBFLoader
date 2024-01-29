using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;

namespace LoadFoxProDBToSQL
{
    public class SqlServerConnection : IDatabaseConnection, IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private ILogger _logger;
        private string _serverName;
        public SqlServerConnection(string connString, ILogger logger)
        {
            _connectionString = connString;
            _logger = logger;
            _connection = Connect();
        }
        private SqlConnection Connect()
        {
            bool success = false;
            //connect to SQL First
            var connection = new SqlConnection(_connectionString); ;
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
        
        public bool? CreateDatabase(string databaseName)
        {
            _connection.Open();
            bool success = false;
            try
            {

                //Create the DB to house the DB data
                SqlCommand createDatabase = new SqlCommand($"CREATE DATABASE {databaseName}", _connection);
                createDatabase.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"ERROR! occurred creating new DB {databaseName}. Ex:{ex.Message}" + Environment.NewLine);
            }
            return success;
        }

        public void CreateTable(DataTable dataTable, string tableName, bool _replaceSpaces)
        {
            var conn = _connection;
             _logger.Information($"Create table Started for Table: {tableName}" + Environment.NewLine);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            string createSql = "";
            try
            {
                using (_connection)
                {
                    createSql = BuildCreateStatement(dataTable, tableName);
                    SqlCommand createCmd = (SqlCommand)_connection.CreateCommand();
                    createCmd.CommandText = createSql;
                    createCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                 _logger.Information("Sql Exception:" + ex.Message + ", Stack Trace:" + ex.StackTrace + " for tablename: " + tableName + " sql statement " + createSql + Environment.NewLine);
            }
            catch (Exception ex2)
            {
                 _logger.Information("Create Table Error:" + ex2.Message + ", Stack Trace:" + ex2.StackTrace + " for tablename: " + tableName + " sql statement " + createSql + Environment.NewLine);

            }
            conn.Close();

             _logger.Information($"Table {tableName} created successfully." + Environment.NewLine);
           
        }

        public string BuildCreateStatement(DataTable dataTable, string tableName, bool replaceSpaces = true)
        {

            int i = 1;
            string sqlColumnCreate = "";
            foreach (var row in dataTable.AsEnumerable().OrderBy(x => x.ItemArray[6]))
            {
                var columnName = row[3].ToString();
                var dataTypeString = row[11].ToString();
                var maxLength = row[13].ToString();
                var precision = row[15].ToString();
                var dataType = GetDataType(dataTypeString, maxLength, precision);

                if (replaceSpaces)
                {
                    columnName = columnName.Replace(" ", "_");
                    tableName = tableName.Replace(" ", "_");
                }

                string primKey = "";
                if (i == 1)
                {
                    sqlColumnCreate = "CREATE TABLE [" + tableName + "] ([" + columnName.ToString().Trim() + "] " + dataType + ", ";
                }
                else if (i == dataTable.Rows.Count)
                    sqlColumnCreate = sqlColumnCreate + " [" + columnName.ToString().Trim() + "] " + dataType + "); ";
                else
                    sqlColumnCreate = sqlColumnCreate + " [" + columnName.ToString().Trim() + "] " + dataType + ", ";
                i++;
            }
            return sqlColumnCreate;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchema(string schemaName = "dbo")
        {
            DataTable dataTable = null;
            try
            {
               dataTable =  _connection.GetSchema(schemaName);
            }
            catch(Exception ex) 
            {
                _logger.Error(ex, $"Error occurred in {this.GetType().Name}.GetSchema");
            }
            return dataTable;
        }

        public string GetDataType(string dataType, string maxLength, string precision = "0", int scale = 0)
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

    }
}
