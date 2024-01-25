using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadFoxProDBToSQL.Database
{
    public class DbaseConnection : IDatabaseConnection
    {
        public DatabaseConnection Connect(string connectionString, ILogger logger)
        {
            throw new NotImplementedException();
        }

        public void CreateTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchema(string schemaName)
        {
            throw new NotImplementedException();
        }
    }
}
