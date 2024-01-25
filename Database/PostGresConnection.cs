using Npgsql;
using System;
using Serilog;
using System.Data;

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

        public void CreateTable(string tableName) 
        { 
            throw new NotImplementedException(); 
        }
    }
}
