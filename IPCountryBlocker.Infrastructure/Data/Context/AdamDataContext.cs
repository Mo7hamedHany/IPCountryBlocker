using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPCountryBlocker.Infrastructure.Data.Context
{
    public class IPCountryBlockerDataContext : IdentityDbContext
    {
        public IPCountryBlockerDataContext(DbContextOptions<IPCountryBlockerDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional model configurations here
        }

        // Define DbSets for your entities here, e.g.:
        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}
