namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THONGTINADMIN")]
    public partial class THONGTINADMIN
    {
        [Key]
        public int IDTTTK { get; set; }

        [StringLength(50)]
        public string TEN { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NGAYSINH { get; set; }

        public bool? GIOITINH { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string DIACHI { get; set; }

        [StringLength(15)]
        public string SODT { get; set; }

        public int? IDTK { get; set; }

        public string ANHACC { get; set; }

        public virtual ADMIN ADMIN { get; set; }
    }
}
