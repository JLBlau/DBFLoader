using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadFoxProDBToSQL.Database
{
    public class DbaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private OleDbConnection _connection;
        private ILogger _logger;
        public DbaseConnection(string connectionString, ILogger logger) { 
            _connectionString = connectionString;
            _logger = logger;
            _connection = Connect();
        }
        private OleDbConnection Connect()
        {
            var conn = new OleDbConnection(_connectionString);
            try
            {
                conn.Open();
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Cannot open connection to {{_serverName}}. Ex: {{ex.Message}}");
            }
            return conn;
        }

        public void CreateTable(DataTable dataTable, string tableName, bool replaceSpaces = true)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchema(string schemaName)
        {
            DataTable tables = new DataTable();
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            using(_connection)
            { 
                tables = _connection.GetSchema("Tables");
            }
            return tables;
        }
        public string GetDataType(string dataType, string maxLength, string precision = "0", int scale = 0)
        {
            //254 is the max length of a column on FoxPro

            switch (dataType)
            {
                case "8":
                case "11":
                case "72":
                case "129":
                case "134":
                    dataType = $"Char({maxLength})";
                    break;
                case "130":
                    dataType = "MEMO";
                    break;
                case "7":
                case "133":
                case "135":
                    dataType = "DATE";
                    // dataType = $"VARCHAR(254)";
                    break;
                case "20":
                    dataType = $"DECIMAL({precision}, {scale})";
                    break;
                case "204":
                case "128":
                    dataType = "BINARY";
                    break;
                case "6":
                    dataType = $"DECIMAL({precision}, {scale})";
                    break;
                case "14":
                    dataType = $"DECIMAL({precision}, {scale})";
                    break;
                case "5":
                    //double
                    dataType = $"DECIMAL({precision}, {scale})";
                    break;

                default:
                    dataType = $"Text";
                    break;
            };

            return dataType;

        }

        public string BuildCreateStatement(DataTable dataTable, string tableName, bool replaceSpaces)
        {
            throw new NotImplementedException();
        }
    }
}
