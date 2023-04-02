using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Teach.Model
{
    internal class DataGridModel : NotifyProperty
    {
        private string _tables;
        public string Tables
        {
            get { return _tables; }
            set
            {
                if (_tables != value)
                {
                    _tables = value;
                    OnPropertyChanged(nameof(Tables));
                }
            }
        }

        private ObservableCollection<object> _data;
        public ObservableCollection<object> Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        public ICommand CloseTabCommand { get; }

        public DataGridModel()
        {
            CloseTabCommand = new RelayCommand(CloseTab);
        }

        private void CloseTab()
        {
            // код закрытия вкладки
        }
    }
}
