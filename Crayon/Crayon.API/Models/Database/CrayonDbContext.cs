using Crayon.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Crayon.API.Models.Database
{
    public class CrayonDbContext : DbContext
    {
        public CrayonDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SoftwareLicence> SoftwareLicences { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrayonDbContext).Assembly);

        }
    }
}
