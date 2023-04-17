using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Teach.Model;
using Teach.ViewModel;

namespace Teach.View
{
    /// <summary>
    /// Логика взаимодействия для InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog(List<string> _params)
        {
            InitializeComponent();
           
        }

    }

    public class ViewModel
    {
        public ObservableCollection<InputField> InputFields { get; set; }

        public ViewModel()
        {
            InputFields = new ObservableCollection<InputField>
            {
                new InputField { Label = "Имя", Value = "" },
                new InputField { Label = "Фамилия", Value = "" },
                new InputField { Label = "Email", Value = "" }
            };
        }
    }

    public class InputField
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

}
