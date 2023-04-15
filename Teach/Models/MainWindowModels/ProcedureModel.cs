using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Helpers;
using Teach.Model;
using Teach.Kernel;
using Teach.Models.Interfaces;
using Dapper;

namespace Teach.Models.MainWindowModels
{
    internal class ProcedureModel
    {
        public string Value { get; set; }
        public static ObservableCollection<TableModel> GetProcedures()
        {
            return DatabaseConnection.Instance.SelectBy(query: "SELECT name FROM sys.procedures", type: "name");
        }
        public static DataTable GetTableFromProcedure(string procedureName, DynamicParameters parameters = null)
        {
            DataTable dataTable = new DataTable();

            DatabaseConnection.Instance.TryCatch(() =>
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    dataTable.Load(connection.ExecuteReader(procedureName, parameters, commandType: CommandType.StoredProcedure));
                }
            });

            return dataTable;
        }
    }
}
