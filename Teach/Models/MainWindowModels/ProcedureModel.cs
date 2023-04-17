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
using static Teach.ViewModel.MainWindowVM;

namespace Teach.Models
{
    internal class ProcedureModel
    {
        public string Name { get; set; }
        public string Value { get; set; } = "";
        public DbType TypeName { get; set; }
        public int MaxLength { get; set; }
        public bool IsOutput { get; set; }
        public string TypeNameString { get; internal set; }


        public static ObservableCollection<TableModel> GetProcedures()
        {
            return DatabaseConnection.Instance.SelectBy(query: "SELECT name FROM sys.procedures", type: "name");
        }

        public static DataTable GetTableFromProcedure(string procedureName, DynamicParameters parameters = null)
        {
            var dataTable = new DataTable();

            DatabaseConnection.Instance.TryCatch(() =>
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    dataTable.Load(connection.ExecuteReader(procedureName, parameters, commandType: CommandType.StoredProcedure));
                }
            });

            return dataTable;
        }

        public static List<StoredProcedureParameter> GetProcedureParams(string MyProcedure)
        {
            string query = @"SELECT prm.name AS Name, t.name AS TypeNameString, prm.max_length AS MaxLength, prm.is_output AS IsOutput
                     FROM sys.procedures p
                     INNER JOIN sys.parameters prm ON p.object_id = prm.object_id INNER JOIN sys.types t ON prm.user_type_id = t.user_type_id
                     WHERE p.name = @ProcedureName ORDER BY prm.parameter_id";

            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                return connection
                    .Query<StoredProcedureParameter>(query, new { ProcedureName = MyProcedure })
                    .Select(param => new StoredProcedureParameter 
                    { 
                        Name = param.Name,                                            
                        TypeName = ParseDbType(param.TypeNameString), 
                        MaxLength = param.MaxLength, 
                        IsOutput = param.IsOutput 
                    })
                    .ToList();
            }
        }

        private static DbType ParseDbType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "bigint":
                    return DbType.Int64;
                case "binary":
                    return DbType.Binary;
                case "bit":
                    return DbType.Boolean;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "date":
                case "datetime":
                case "datetime2":
                    return DbType.DateTime;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "decimal":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "geography":
                case "geometry":
                case "hierarchyid":
                case "image":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "rowversion":
                case "sysname":
                case "text":
                case "time":
                case "timestamp":
                case "uniqueidentifier":
                case "varbinary":
                case "varchar":
                    return DbType.String;
                case "smallint":
                    return DbType.Int16;
                case "int":
                    return DbType.Int32;
                case "money":
                case "smallmoney":
                    return DbType.Currency;
                case "real":
                    return DbType.Single;
                case "sql_variant":
                    return DbType.Object;
                case "tinyint":
                    return DbType.Byte;
                case "xml":
                    return DbType.Xml;
                default:
                    return DbType.Object;
            }
        }

    }
}
