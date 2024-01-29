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

        //DatabaseConnection Connect();

        bool? CreateDatabase(string databaseName);

        DataTable GetSchema(string schemaName = "");

        void CreateTable(DataTable dataTable, string tableName, bool replaceSpaces = true);


        string BuildCreateStatement(DataTable dataTable,  string tableName, bool replaceSpaces);

        string GetDataType(string dataTypeName, string maxLength, string precicion, int scale);

        string InsertData (IDataReader dataReader, DataTable columns, string tableName);

    }
}
