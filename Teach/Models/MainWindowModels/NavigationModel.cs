using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Teach.Model
{
    internal class NavigationModel
    {
        public ICommand FirstPageCommand { get; set; }

        public ICommand PreviousPageCommand { get; set; }

        public ICommand NextPageCommand { get; set; }

        public ICommand LastPageCommand { get; set; }
    }
}
