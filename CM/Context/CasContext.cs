using CM.Context.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CM.Context
{
    class CasContext : DbContext
    {
        public CasContext() : base("CAS") { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Cas> Cases { get; set; }
        public DbSet<CasFile> CasFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
