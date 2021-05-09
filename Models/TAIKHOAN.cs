namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAIKHOAN")]
    public partial class TAIKHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOAN()
        {
            NHOMTAIKHOANs = new HashSet<NHOMTAIKHOAN>();
            TAIKHOANPHONGLUYENTAPs = new HashSet<TAIKHOANPHONGLUYENTAP>();
            THONGTINTAIKHOANs = new HashSet<THONGTINTAIKHOAN>();
        }

        [Key]
        public int IDTK { get; set; }

        [StringLength(50)]
        public string TENDN { get; set; }

        [StringLength(50)]
        public string MATKHAU { get; set; }

        [Column(TypeName = "date")]
        public DateTime? THOIGIANDANGKY { get; set; }

        public bool? USING { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHOMTAIKHOAN> NHOMTAIKHOANs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAIKHOANPHONGLUYENTAP> TAIKHOANPHONGLUYENTAPs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONGTINTAIKHOAN> THONGTINTAIKHOANs { get; set; }
        public TAIKHOAN(string tENDN, string mATKHAU,DateTime tHOIGIANDANGKY , bool? uSING)
        {
            TENDN = tENDN;
            MATKHAU = mATKHAU;
            THOIGIANDANGKY = tHOIGIANDANGKY;
            USING = uSING;
        }
    }
}
