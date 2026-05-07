using Microsoft.EntityFrameworkCore;

namespace motion_api
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
  }
}
