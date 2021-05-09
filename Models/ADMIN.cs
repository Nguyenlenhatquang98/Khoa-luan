namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADMIN")]
    public partial class ADMIN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADMIN()
        {
            CHUDEs = new HashSet<CHUDE>();
            PHONGLUYENTAPs = new HashSet<PHONGLUYENTAP>();
            THONGTINADMINs = new HashSet<THONGTINADMIN>();
        }

        [Key]
        public int IDTK { get; set; }

        [StringLength(50)]
        public string TENDN { get; set; }

        [StringLength(50)]
        public string MATKHAU { get; set; }

        public bool? USING { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHUDE> CHUDEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHONGLUYENTAP> PHONGLUYENTAPs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONGTINADMIN> THONGTINADMINs { get; set; }
    }
}
