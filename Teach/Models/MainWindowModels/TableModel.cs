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

            return DatabaseConnection.Instance.SelectBy(query, type: "TABLE_NAME");
        }

        public static DataTable GetTable(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";

            DataTable dataTable = new DataTable();

            DatabaseConnection.Instance.TryCatch(() =>
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            dataTable = new DataTable(tableName);
                            dataTable.Load(reader);
                        }
                    }
                }
            });
            return dataTable;
        }
    }
}
