namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TUVUNGPHONGLUYENTAP")]
    public partial class TUVUNGPHONGLUYENTAP
    {
        [Key]
        public int IDTV { get; set; }

        [StringLength(50)]
        public string TENTV { get; set; }

        [StringLength(50)]
        public string NGHIATV { get; set; }

        [StringLength(100)]
        public string PHIENAM { get; set; }

        [StringLength(100)]
        public string VDTV { get; set; }

        public string ANHTUVUNG { get; set; }

        public int? DIEMTT { get; set; }

        public int? IDPLT { get; set; }

        public virtual PHONGLUYENTAP PHONGLUYENTAP { get; set; }
    }
}
