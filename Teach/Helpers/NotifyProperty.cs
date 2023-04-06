using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Teach.Model
{
    internal abstract class NotifyProperty : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

    }
}
