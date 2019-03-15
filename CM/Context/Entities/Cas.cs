using CM.Context.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CM.Context.Entities
{
    class Cas : BaseEntity
    {
        int casId;
        public int CasID
        {
            get { return casId; }
            set { SetValue(ref casId, value); }
        }

        int? number;
        public int? Number
        {
            get { return number; }
            set { SetValue(ref number, value); }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { SetValue(ref description, value); }
        }

        DateTime? startingDate;
        [DataType(DataType.Date)]
        public DateTime? StartingDate
        {
            get { return startingDate; }
            set { SetValue(ref startingDate, value); }
        }

        DateTime? endingDate;
        [DataType(DataType.Date)]
        public DateTime? EndingDate
        {
            get { return endingDate; }
            set { SetValue(ref endingDate, value); }
        }

        int? projectId;
        public int? ProjectID
        {
            get { return projectId; }
            set { SetValue(ref projectId, value); }
        }

        public virtual Project Project { get; set; }

        public virtual ICollection<CasFile> CasFiles { get; set; }
    }
}
