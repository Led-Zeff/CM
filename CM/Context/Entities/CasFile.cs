using CM.Context.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace CM.Context.Entities
{
    enum CasFileType
    {
        AppFile, FrontFile, IgnoredFile = 0x99
    }

    class CasFile : BaseEntity
    {
        public int CasFileID { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public byte[] OriginalFile { get; set; }
        public byte[] ModifiedFile { get; set; }
        public CasFileType FileType { get; set; }
        public bool IsDeleted { get; set; }

        public int CasId { get; set; }
        public virtual Cas Cas { get; set; }
    }
}
