using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Teach.Kernel;

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

            try
            {
                _try();
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
}
