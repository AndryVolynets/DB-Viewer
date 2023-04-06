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

namespace Teach.Models.MainWindowModels
{
    internal class ProcedureModel
    {
        public string Value { get; set; }
        public static ObservableCollection<TableModel> GetProcedures()
        {
            return DatabaseConnection.Instance.SelectBy(query: "SELECT name FROM sys.procedures", type: "name");
        }
        
        public static DataTable GetTableFromProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            DatabaseConnection.Instance.TryCatch(() =>
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlCommandBuilder.DeriveParameters(command);
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (var parameter in parameters)
                            {
                                if (!command.Parameters.Contains(parameter.ParameterName))
                                {
                                    throw new ArgumentException($"Procedure '{procedureName}' does not have parameter '{parameter.ParameterName}'");
                                }
                                else
                                {
                                    command.Parameters[parameter.ParameterName].Value = parameter.Value;
                                }
                            }
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            });
            return dataTable;
        }
    }
}
