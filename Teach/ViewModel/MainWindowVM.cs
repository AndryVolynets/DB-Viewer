using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Teach.Model;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Security.Cryptography;
using System.Linq;

namespace Teach.ViewModel
{
    internal class MainWindowVM : NotifyProperty
    {
        private TableModel _selectedTable;
        
        public ObservableCollection<TableModel> Tables { get; set; }
        public ObservableCollection<string> TableTabs { get; set; }
        public ObservableCollection<DataGridModel> OpenedTabs { get; set; }

        public TableModel SelectedTable
        {
            get { return _selectedTable; }
            set 
            {
                if (_selectedTable != value)
                {
                    _selectedTable = value;
                    OnPropertyChanged("SelectedTable");

                    if (!OpenedTabs.Any(t => t.TabName == value.Name))
                    {
                        OpenedTabs.Add(new DataGridModel
                        {
                            TabName = value.Name,
                            Data = DataGridModel.GetTable(value.Name).DefaultView.Table
                        });
                    }
                }
            }
        }
    

        public MainWindowVM()
        {
            Tables = TableModel.GetTables();

            // Создайте экземпляры DataGridModel и добавьте их в OpenedTabs
            OpenedTabs = new ObservableCollection<DataGridModel>();
        }
    }
}
