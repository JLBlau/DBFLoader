using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace LoadFoxProDBToSQL
{
    public interface IDatabaseConnection
    {

        DatabaseConnection Connect(string connectionString, ILogger logger);

        DataTable GetSchema(string schemaName);

        void CreateTable(string tableName);

    }
}
