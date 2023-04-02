using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using Teach.ViewModel;

namespace Teach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
        }
    }
}
