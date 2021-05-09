namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHONGLUYENTAP")]
    public partial class PHONGLUYENTAP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHONGLUYENTAP()
        {
            TAIKHOANPHONGLUYENTAPs = new HashSet<TAIKHOANPHONGLUYENTAP>();
            TUVUNGPHONGLUYENTAPs = new HashSet<TUVUNGPHONGLUYENTAP>();
        }

        [Key]
        public int IDPLT { get; set; }

        [StringLength(50)]
        public string TENPHONG { get; set; }

        public int? SOCAUHOI { get; set; }

        public int? THOIGIAN { get; set; }

        public int? DIEMTHI { get; set; }

        public int? IDTK { get; set; }

        public virtual ADMIN ADMIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAIKHOANPHONGLUYENTAP> TAIKHOANPHONGLUYENTAPs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TUVUNGPHONGLUYENTAP> TUVUNGPHONGLUYENTAPs { get; set; }
    }
}
