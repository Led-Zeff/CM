using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CM.Context.Entities.Base
{
    abstract class BaseEntity : INotifyPropertyChanged
    {
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ModificationDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void SetValue<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (!object.Equals(field, value))
            {
                field = value;
                NotifyPropertyChanged(propName);
            }
        }
    }
}
