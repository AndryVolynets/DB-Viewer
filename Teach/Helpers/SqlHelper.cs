using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
                catch (SqlTypeException ex)
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
            instance.TryCatch(() => { });

            if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(type))
            {
                return new ObservableCollection<TableModel>();
            }
            else
            {
                using (var connection = instance.GetConnection())
                {
                    return new ObservableCollection<TableModel>(connection.Query<TableModel>($"SELECT {type} AS Name FROM ({query}) T"));
                }
            }
        }
    }
}
