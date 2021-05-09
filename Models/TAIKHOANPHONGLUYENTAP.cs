namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAIKHOANPHONGLUYENTAP")]
    public partial class TAIKHOANPHONGLUYENTAP
    {
        [Key]
        public int IDTKPLT { get; set; }

        public int? IDTK { get; set; }

        public int? IDPLT { get; set; }

        public int? DIEMTHI { get; set; }

        public virtual PHONGLUYENTAP PHONGLUYENTAP { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }

        public TAIKHOANPHONGLUYENTAP(int? iDTK, int? iDPLT, int? dIEMTHI)
        {
            IDTK = iDTK;
            IDPLT = iDPLT;
            DIEMTHI = dIEMTHI;
        }

        public TAIKHOANPHONGLUYENTAP()
        {

        }
    }
}
