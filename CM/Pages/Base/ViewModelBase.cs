using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CM.Pages.Base
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
