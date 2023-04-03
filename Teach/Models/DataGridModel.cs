using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Teach.Kernel;
using Teach.Helpers;
using System.Runtime.Remoting.Messaging;

namespace Teach.Model
{
    internal class DataGridModel : NotifyProperty
    {
        public string TabName
        {
            get { return _tabName; }
            set
            {
                if (_tabName != value)
                {
                    _tabName = value;
                    OnPropertyChanged("TabName");
                }
            }
        }

        public DataTable Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        public static DataTable GetTable(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";

            DatabaseConnection instance = DatabaseConnection.Instance;
            DataTable dataTable = new DataTable();

            instance.TryCatch(() =>
            {
                using (var connection = instance.GetConnection())
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    dataTable = new DataTable(tableName);
                    dataTable.Load(reader);
                }
            });

            return dataTable;
        }

        private string _tabName;
        private DataTable _data;
    }
}
