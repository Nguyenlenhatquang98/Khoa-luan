namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHOM")]
    public partial class NHOM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHOM()
        {
            NHOMTAIKHOANs = new HashSet<NHOMTAIKHOAN>();
            TUVUNGNHOMs = new HashSet<TUVUNGNHOM>();
        }

        [Key]
        public int IDN { get; set; }

        [StringLength(50)]
        public string TENNHOM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHOMTAIKHOAN> NHOMTAIKHOANs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TUVUNGNHOM> TUVUNGNHOMs { get; set; }
        public NHOM(string tENNHOM)
        {
            TENNHOM = tENNHOM;
        }
    }
}
