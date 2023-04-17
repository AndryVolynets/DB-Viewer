using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Teach.Model;

namespace Teach.Models
{
    internal class NavigationButtonsModel : NotifyProperty
    {       
        public ICommand FirstPageCommand
        {
            get => _firstPageCommand;
            set => SetProperty(ref _firstPageCommand, value);
        }
        public ICommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set => SetProperty(ref _previousPageCommand, value);
        }
        public ICommand NextPageCommand
        {
            get { return _nextPageCommand; }
            set => SetProperty(ref _nextPageCommand, value);
        }
        public ICommand LastPageCommand
        {
            get => _lastPageCommand;
            set => SetProperty(ref _lastPageCommand, value);
        }

        private ICommand _firstPageCommand;
        private ICommand _previousPageCommand;
        private ICommand _nextPageCommand;
        private ICommand _lastPageCommand;

    }
}
