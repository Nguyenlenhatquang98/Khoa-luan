namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TUVUNGONTAP")]
    public partial class TUVUNGONTAP
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

        public int? IDCD { get; set; }

        public virtual CHUDE CHUDE { get; set; }

        public TUVUNGONTAP(string tENTV, string nGHIATV, string pHIENAM, string vDTV, string aNHTUVUNG, int? iDCD)
        {
            TENTV = tENTV;
            NGHIATV = nGHIATV;
            VDTV = vDTV;
            PHIENAM = pHIENAM;
            ANHTUVUNG = aNHTUVUNG;
            IDCD = iDCD;
        }
        public TUVUNGONTAP(string tENTV, string nGHIATV, string aNHTUVUNG, int? iDCD)
        {
            TENTV = tENTV;
            NGHIATV = nGHIATV;
            ANHTUVUNG = aNHTUVUNG;
            IDCD = iDCD;
        }





        public TUVUNGONTAP()
        {
        }
    }
}
