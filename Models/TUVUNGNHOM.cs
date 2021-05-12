namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TUVUNGNHOM")]
    public partial class TUVUNGNHOM
    {
        [Key]
        public int IDTV { get; set; }

        [StringLength(50)]
        public string TENTV { get; set; }

        [StringLength(50)]
        public string NGHIATV { get; set; }

        [StringLength(100)]
        public string VDTV { get; set; }

        public string ANHTUVUNG { get; set; }

        public int? DIEMTT { get; set; }

        public int? IDN { get; set; }

        public virtual NHOM NHOM { get; set; }

        public TUVUNGNHOM(string tENTV, string nGHIATV, string vDTV, string aNHTUVUNG, int? iDN)
        {
            TENTV = tENTV;
            NGHIATV = nGHIATV;
            VDTV = vDTV;
            ANHTUVUNG = aNHTUVUNG;
            IDN = iDN;
        }

        public TUVUNGNHOM()
        {
        }
    }
}
