namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHOMTAIKHOAN")]
    public partial class NHOMTAIKHOAN
    {
        [Key]
        public int IDNTK { get; set; }

        public int? IDN { get; set; }

        public int? IDTK { get; set; }

        public virtual NHOM NHOM { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }

        public NHOMTAIKHOAN(int iDN, int iDTK)
        {
            IDN = iDN;
            IDTK = iDTK;
        }

        public NHOMTAIKHOAN()
        {
        }
    }
}
