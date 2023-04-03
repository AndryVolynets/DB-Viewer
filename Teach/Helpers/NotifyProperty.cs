using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Teach.Model
{
    internal abstract class NotifyProperty : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: prop));
        }
    }
}
