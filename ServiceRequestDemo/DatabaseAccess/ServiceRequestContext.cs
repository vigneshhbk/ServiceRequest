using Microsoft.EntityFrameworkCore;
using ServiceRequestDemo.DatabaseAccess.TableClasses;

namespace ServiceRequestDemo.DatabaseAccess
{
    public class ServiceRequestContext : DbContext
    {
        public ServiceRequestContext(DbContextOptions<ServiceRequestContext> options) : base(options) 
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
        }
    }
}
