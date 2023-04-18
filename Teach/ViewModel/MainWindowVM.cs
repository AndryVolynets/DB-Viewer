using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Teach.Model;
using System.Linq;
using Teach.Kernel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Teach.Models;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Controls;
using Dapper;
using System.Collections;
using System.Security.Cryptography;
using System;

namespace Teach.ViewModel
{
    internal class MainWindowVM : NotifyProperty
    {
        public MainWindowVM()
        {
            Tables = TableModel.GetTables();
            Procedures = ProcedureModel.GetProcedures();
            OpenedTabs = new ObservableCollection<DataGridModel>();

            NavigationButtons = new NavigationButtonsModel
            {
                FirstPageCommand = new RelayCommand(FirstPage),
                PreviousPageCommand = new RelayCommand(PreviousPage),
                NextPageCommand = new RelayCommand(NextPage),
                LastPageCommand = new RelayCommand(LastPage),
            };

            CloseTabCommand = new RelayCommand<object>(CloseTab);
            SavePageCommand = new RelayCommand(SaveTab);
        }

        public ObservableCollection<TableModel> Tables { get; set; }
        public ObservableCollection<TableModel> Procedures { get; set; }
        public NavigationButtonsModel NavigationButtons { get; set; }

        /// <summary>
        /// DataGrid binding
        /// </summary>
        public ObservableCollection<DataGridModel> OpenedTabs { get; set; }

        public ICommand CloseTabCommand
        {
            get => _closeTabCommand;
            set => SetProperty(ref _closeTabCommand, value);
        }
        public ICommand SavePageCommand
        {
            get => _savePageCommand;
            set => SetProperty(ref _savePageCommand, value);
        }

        public TableModel SelectedTable
        {
            get => _selectedTable;
            set
            {
                if (_selectedTable != value)
                {
                    SetProperty(ref _selectedTable, value);
                    AddTab(value.Name, TableModel.GetTable(value.Name).DefaultView.Table);
                }
            }
        }

        public TableModel SelectedProcedure
        {
            get => _selectedProcedure;
            set
            {
                if (_selectedProcedure != value)
                {
                    SetProperty(ref _selectedProcedure, value);
                    OpenInputDialogAndExecuteProcedure(value.Name);
                }
            }
        }

        private void OpenInputDialogAndExecuteProcedure(string procedureName)
        {
            List<StoredProcedureParameter> paramsList = ProcedureModel.GetProcedureParams(procedureName);

            if (paramsList.Count == 0)
            {
                AddTab(procedureName, ProcedureModel.GetTableFromProcedure(procedureName).DefaultView.Table);
            }
            else
            {
                var values = GetParameterValuesFromUser(procedureName, paramsList);

                var parameters = new DynamicParameters();

                foreach (var param in paramsList)
                {
                    if (values.TryGetValue(param.Name, out var _value))
                    {
                        parameters.Add(param.Name, _value, param.TypeName, ParameterDirection.Input);
                    }
                    else
                    {
                        throw new ArgumentException($"Value for parameter {param.Name} was not provided.");
                    }
                }

                AddTab(procedureName, ProcedureModel.GetTableFromProcedure(procedureName, parameters).DefaultView.Table);
            }
        }

        private Dictionary<string, string> GetParameterValuesFromUser(string procedureName, List<StoredProcedureParameter> paramsList)
        {
            var inputDialog = new InputDialog(paramsList.Select(x => x.Name).ToList());

            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.ResultDict;
            }
            else
            {
                throw new ArgumentException($"Input dialog for procedure {procedureName} was cancelled.");
            }
        }


        private void AddTab(string name, DataTable dataTable)
        {
            if (!OpenedTabs.Any(t => t.TabName == name))
            {
                OpenedTabs.Add(new DataGridModel { TabName = name, Data = dataTable });
            }
        }

        private void CloseTab(object obj)
        {
            if (obj is DataGridModel tabItem)
            {
                OpenedTabs.Remove(tabItem);
            }
        }


        private void FirstPage()
        {

        }

        private void PreviousPage()
        {
            // логика обработки команды "Предыдущая страница"
        }

        private void NextPage()
        {
            // логика обработки команды "Следующая страница"
        }

        private void LastPage()
        {
            // логика обработки команды "Последняя страница"
        }

        private void SaveTab()
        {
            var result = MessageBox.Show("Do you want to save changes?", 
                                         "Save changes", 
                                         MessageBoxButton.YesNo, 
                                         MessageBoxImage.Question
                                         );

            if (result == MessageBoxResult.Yes)
            {
                if (OpenedTabs.Count == 0)
                {
                    MessageBox.Show("Нечего сохранять!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    foreach (var item in OpenedTabs)
                    {
                        SaveChanges(item);
                    }
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

        public ICommand _closeTabCommand;
        public ICommand _savePageCommand;
        
        private TableModel _selectedTable;
        private TableModel _selectedProcedure;

        public class StoredProcedureParameter
        {
            public string Name { get; set; }
            public string Value { get; set; } = "";
            public DbType TypeName { get; set; }
            public int MaxLength { get; set; }
            public bool IsOutput { get; set; }
            public string TypeNameString { get; internal set; }
        }
    }

    public class InputDialog : Window
    {
        private readonly StackPanel _mainStackPanel = new StackPanel();
        private readonly Button _okButton = new Button();
        private readonly Button _cancelButton = new Button();

        public InputDialog(List<string> parameters)
        {
            Title = "Input";
            SizeToContent = SizeToContent.WidthAndHeight;

            foreach (var param in parameters)
            {
                var promptTextBlock = new TextBlock { Text = param };
                var textBox = new TextBox();
                _mainStackPanel.Children.Add(promptTextBlock);
                _mainStackPanel.Children.Add(textBox);
            }

            _okButton.Content = "OK";
            _cancelButton.Content = "Cancel";

            _okButton.Click += OnOkButtonClick;
            _cancelButton.Click += OnCancelButtonClick;

            var buttonsStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Children = { _okButton, _cancelButton }
            };

            _mainStackPanel.Margin = new Thickness(10);
            _mainStackPanel.Children.Add(buttonsStackPanel);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Content = _mainStackPanel;
        }

        public Dictionary<string, string> ResultDict { get; private set; }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            ResultDict = new Dictionary<string, string>();

            for (var i = 0; i < _mainStackPanel.Children.Count; i++)
            {
                if (_mainStackPanel.Children[i] is TextBlock textBlock && !string.IsNullOrEmpty(textBlock.Text))
                    if (_mainStackPanel.Children[i + 1] is TextBox textBox)
                        ResultDict[textBlock.Text] = textBox.Text;
            }
            DialogResult = true;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
