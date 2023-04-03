using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using Teach.Kernel;

namespace Teach.Model
{
    internal class TableModel : NotifyProperty
    {
        public string Name
        { 
            get => _tableName; 
            set 
            { 
                _tableName = value;
                OnPropertyChanged("Tables"); 
            } 
        }

        public TableModel SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged("SelectedTable");
            }
        }

        public static ObservableCollection<TableModel> GetTables()
        {
            const string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
            var tables = new ObservableCollection<TableModel>();

            try
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int columnNumber = reader.GetOrdinal("TABLE_NAME");
                        tables.Add(new TableModel { Name = reader.GetString(columnNumber) });
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error selecting tables from database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting tables: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);;
            }

            return tables;
        }

        private string _tableName;
        private TableModel _selectedTable;
    }
}
