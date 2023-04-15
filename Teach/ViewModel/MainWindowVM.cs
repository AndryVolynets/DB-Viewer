using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Teach.Model;
using System.Linq;
using Teach.Kernel;
using System.Data;
using Teach.Helpers;
using System.Windows;
using System.Windows.Input;
using Teach.Models.MainWindowModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.Windows.Media.Animation;
using System.Windows.Controls;


namespace Teach.ViewModel
{
    internal class MainWindowVM : NotifyProperty
    {
        public MainWindowVM()
        {
            Tables = TableModel.GetTables();
            Procedures = ProcedureModel.GetProcedures();
            OpenedTabs = new ObservableCollection<DataGridModel>();
            ProcedureParameters = new List<ProcedureModel>();

            CloseTabCommand = new RelayCommand<object>(CloseTab);
            SavePageCommand = new RelayCommand<object>(SaveTab);

            tabControl.SelectionChanged += OnSelectedTabChanged;
        }

        public ObservableCollection<TableModel> Tables { get; set; }
        public ObservableCollection<TableModel> Procedures { get; set; }
        public List<ProcedureModel> ProcedureParameters { get; set; }

        /// <summary>
        /// DataGrid binding
        /// </summary>
        public ObservableCollection<DataGridModel> OpenedTabs { get; set; }
        

        public ICommand SearchCommand { get; set; }
        public ICommand CloseTabCommand { get; set; }
        public ICommand SavePageCommand { get; set; }


        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                    SetProperty(ref _selectedTabIndex, value);
            }
        }

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                SetProperty(ref searchTerm, value);
            }
        }

        public TableModel SelectedTable
        {
            get => _selectedTable; 
            set 
            {
                if (_selectedTable != value)
                {
                    SetProperty(ref _selectedTable, value);

                    if (!OpenedTabs.Any(t => t.TabName == value.Name))
                    {
                        OpenedTabs.Add(new DataGridModel { TabName = value.Name, Data = TableModel.GetTable(value.Name).DefaultView.Table, });
                    }
                }
            }
        }

        public TableModel SelectedProcedure
        {
            get => _selectedProcedure;
            set
            {
                if (_selectedProcedure == value) return;

                SetProperty(ref _selectedProcedure, value);

                LoadProcedureData(value.Name);
            }
        }

        private void LoadProcedureData(string procedureName)
        {
            DatabaseConnection.Instance.TryCatch(() =>
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(command);

                    if (!OpenedTabs.Any(t => t.TabName == procedureName))
                    {
                        var dataTable = new DataTable();

                        if (command.Parameters.Count <= 1)
                        {
                            dataTable = ProcedureModel.GetTableFromProcedure(procedureName).DefaultView.Table;
                        }
                        else
                        {
                            for (var i = 0; i < command.Parameters.Count; i++)
                            {
                                ProcedureParameters.Add(new ProcedureModel { Value = command.Parameters[i].ParameterName });
                            }
                        }

                        OpenedTabs.Add(new DataGridModel { TabName = procedureName, Data = dataTable });
                    }
                }
            });
        }

        private void CloseTab(object obj)
        {
            if (obj is DataGridModel tabItem)
            {
                OpenedTabs.Remove(tabItem);
            }
        }

        private void SaveTab(object obj)
        {
            
        }

        private void OnSelectedTabChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TabControl tabControl)
            {
                if (tabControl.SelectedIndex >= 0)
                {
                    _curentDataGrid = OpenedTabs[tabControl.SelectedIndex];
                }
            }
        }

        private void SaveChanges(DataGridModel dataGridModel)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                var command = new SqlCommand($"SELECT * FROM {dataGridModel.TabName}", connection);
                var adapter = new SqlDataAdapter(command);
                var builder = new SqlCommandBuilder(adapter);
                adapter.Update(dataGridModel.Data);
            }
        }

        private DataGridModel _curentDataGrid;

        private int _selectedTabIndex;
        private TableModel _selectedTable;
        private TableModel _selectedProcedure;
        private string searchTerm;

        private readonly TabControl tabControl = Application.Current.MainWindow.FindName("TabControl") as TabControl;
    }
}
