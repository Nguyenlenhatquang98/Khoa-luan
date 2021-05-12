namespace KhoaLuanTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHUDE")]
    public partial class CHUDE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHUDE()
        {
            TUVUNGONTAPs = new HashSet<TUVUNGONTAP>();
        }

        [Key]
        public int IDCD { get; set; }

        [StringLength(50)]
        public string TENCD { get; set; }

        [StringLength(50)]
        public string ANHCD { get; set; }

        public int? IDTK { get; set; }

        public int? IDTK1 { get; set; }

        public virtual ADMIN ADMIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TUVUNGONTAP> TUVUNGONTAPs { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }
        public CHUDE(string tENCD, int? iDTK, string aNHCD, int? iDTK1)
        {
            TENCD = tENCD;
            IDTK = iDTK;
            ANHCD = aNHCD;
            IDTK1 = iDTK1;
        }
    }
}
