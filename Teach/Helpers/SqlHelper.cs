using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Teach.Kernel;
using Teach.Model;

namespace Teach.Helpers
{
    internal static class SqlHelper
    {
        public static void TryCatch(this DatabaseConnection connection, Action _try)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            else
            {
                try
                {
                    _try?.Invoke();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error selecting tables from database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting tables: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); ;
                }
            }
        }
        public static ObservableCollection<TableModel> SelectBy(this DatabaseConnection instance, string query, string type)
        {
            var tables = new ObservableCollection<TableModel>();

            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (!(string.IsNullOrEmpty(query) && string.IsNullOrEmpty(type)))
            {
                instance.TryCatch(() =>
                {
                    using (var connection = instance.GetConnection())
                    {
                        using (var command = new SqlCommand(query, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                int columnNumber = reader.GetOrdinal(type);
                                while (reader.Read())
                                {
                                    tables.Add(new TableModel { Name = reader.GetString(columnNumber) });
                                }
                            }
                        }
                    }
                });
            }
            return tables;
        }

    }
}
