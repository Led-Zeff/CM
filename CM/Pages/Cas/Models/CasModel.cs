using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Pages.Cas.Models
{
    class CasModel : CM.Pages.Base.ViewModelBase
    {
        int casId;
        public int CasID
        {
            get
            {
                return casId;
            }
            set
            {
                casId = value;
                OnPropertyChanged();
            }
        }
    }
}
