using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Teach.Model
{
    internal class NavigationModel : NotifyProperty
    {
        private ICommand _firstPageCommand;
        private ICommand _previousPageCommand;
        private ICommand _nextPageCommand;
        private ICommand _lastPageCommand;

        public ICommand FirstPageCommand
        {
            get { return _firstPageCommand; }
            set
            {
                _firstPageCommand = value;
                OnPropertyChanged(nameof(FirstPageCommand));
            }
        }

        public ICommand PreviousPageCommand
        {
            get { return _previousPageCommand; }
            set
            {
                _previousPageCommand = value;
                OnPropertyChanged(nameof(PreviousPageCommand));
            }
        }

        public ICommand NextPageCommand
        {
            get { return _nextPageCommand; }
            set
            {
                _nextPageCommand = value;
                OnPropertyChanged(nameof(NextPageCommand));
            }
        }

        public ICommand LastPageCommand
        {
            get { return _lastPageCommand; }
            set
            {
                _lastPageCommand = value;
                OnPropertyChanged(nameof(LastPageCommand));
            }
        }

    }
}
