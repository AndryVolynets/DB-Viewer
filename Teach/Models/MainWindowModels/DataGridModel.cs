using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Teach.Kernel;
using Teach.Helpers;
using System.Runtime.Remoting.Messaging;

namespace Teach.Model
{
    internal class DataGridModel
    {
        public string TabName { get; set; }

        public DataTable Data { get; set; }

    }
}
