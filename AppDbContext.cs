using Microsoft.EntityFrameworkCore;

namespace motion_api
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Node> Nodes => Set<Node>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // ¡Magia pura! Esto busca todas las clases IEntityTypeConfiguration y las aplica.
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
  }
}
