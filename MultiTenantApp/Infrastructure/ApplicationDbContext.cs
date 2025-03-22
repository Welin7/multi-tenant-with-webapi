using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Models;

namespace MultiTenantApp.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        public int? TenantId
        {
            get
            {
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.Items.TryGetValue("TenantId", out var tenantId))
                {
                    return tenantId as int?;
                }
                return null;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => p.TenantId);
        }
        public override int SaveChanges()
        {
            SetTenantId();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantId();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            var tenantId = TenantId; // Obtém o TenantId atual

            foreach (var entry in ChangeTracker.Entries<TenantEntity>())
            {
                if (entry.State == EntityState.Added && entry.Entity.TenantId == 0)
                {
                    entry.Entity.TenantId = tenantId ?? throw new InvalidOperationException("TenantId não pode ser nulo."); // Define o TenantId automaticamente
                }
            }
        }

        public DbSet<Product>? Products { get; set; }
    }

}
