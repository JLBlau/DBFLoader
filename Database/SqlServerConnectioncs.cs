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

namespace LoadFoxProDBToSQL
{
    public class SqlServerConnection : IDatabaseConnection, IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private ILogger _logger;
        private string _serverName;
        public SqlServerConnection(string connString, string serverName)
        {
            _connectionString = connString;
            _logger = new LoggerConfiguration()
                            .WriteTo
                            .Console()
                             .WriteTo.Logger(lc => lc
                                .WriteTo.File("log.txt"))
                            .CreateLogger();
            _connection = Connect();
        }
        public SqlConnection Connect()
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

        public DatabaseConnection Connect(string connectionString, ILogger logger)
        {
            throw new NotImplementedException();
        }

        public void CreateDatabase(string databaseName)
        {
            _connection.Open();
            bool success = false;
            try
            {
                //Create the DB to house the DB data
                SqlCommand sqlCreateTableCmd = new SqlCommand($"CREATE DATABASE {databaseName}", _connection);
                sqlCreateTableCmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"ERROR! occurred creating new DB {databaseName}. Ex:{ex.Message}" + Environment.NewLine);
            }
        }

        public void CreateTable(string tableName)
        {
            throw new NotImplementedException();
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


    }
}
