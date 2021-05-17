using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace KhoaLuanTotNghiep.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<ADMIN> ADMINs { get; set; }
        public virtual DbSet<CHUDE> CHUDEs { get; set; }
        public virtual DbSet<NHOM> NHOMs { get; set; }
        public virtual DbSet<NHOMTAIKHOAN> NHOMTAIKHOANs { get; set; }
        public virtual DbSet<PHONGLUYENTAP> PHONGLUYENTAPs { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<TAIKHOANPHONGLUYENTAP> TAIKHOANPHONGLUYENTAPs { get; set; }
        public virtual DbSet<THONGTINADMIN> THONGTINADMINs { get; set; }
        public virtual DbSet<THONGTINTAIKHOAN> THONGTINTAIKHOANs { get; set; }
        public virtual DbSet<TUVUNGNHOM> TUVUNGNHOMs { get; set; }
        public virtual DbSet<TUVUNGONTAP> TUVUNGONTAPs { get; set; }
        public virtual DbSet<TUVUNGPHONGLUYENTAP> TUVUNGPHONGLUYENTAPs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAIKHOAN>()
                .HasMany(e => e.CHUDEs)
                .WithOptional(e => e.TAIKHOAN)
                .HasForeignKey(e => e.IDTK1);
        }
    }
}
