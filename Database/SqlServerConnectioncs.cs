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

namespace LoadFoxProDBToSQL
{
    public class SqlServerConnection
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
            _connection = GetSqlConnection();
        }
        private SqlConnection GetSqlConnection() 
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


    }
}
