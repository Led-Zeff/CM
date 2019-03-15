using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Context.Entities.Base
{
    interface IEntity
    {
        [DataType(DataType.DateTime)]
        DateTime RegistrationDate { get; set; }
        [DataType(DataType.DateTime)]
        DateTime? ModificationDate { get; set; }
    }
}
