using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Teach.Model;
using System.Collections;


namespace Teach.ViewModel
{
    internal class MainWindowVM : NotifyProperty
    {
        private TableModel _selectedTable;

        public ObservableCollection<TableModel> Tables { get; set; }

        public TableModel SelectedTable
        {
            get { return _selectedTable; }
            set 
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        public MainWindowVM()
        {
            Tables = TableModel.GetTables();
        }
    }
}
