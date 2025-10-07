using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// Контекст базы данных для работы с мониторами через Entity Framework.
    /// </summary>
    public class MonitorDbContext : DbContext
    {
        public MonitorDbContext(DbContextOptions<MonitorDbContext> options) : base(options)
        {
        }

        public DbSet<MonitorItem> Monitors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MonitorItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Manufacturer).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Resolution).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PanelType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(500);
            });
        }
    }
}
