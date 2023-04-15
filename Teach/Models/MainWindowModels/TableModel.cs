using Dapper;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Teach.Helpers;
using Teach.Kernel;

namespace Teach.Model
{
    internal class TableModel
    {
        public string Name { get; set; }
        
        public static ObservableCollection<TableModel> GetTables()
        {
            const string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            return DatabaseConnection.Instance.SelectBy(query,"TABLE_NAME");
        }

        public static DataTable GetTable(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";

            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                var dataTable = new DataTable(tableName);
                using (var reader = connection.ExecuteReader(query))
                {
                    dataTable.Load(reader);
                }
                return dataTable;
            }
        }
    }
}
