using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace motion_api.Infrastructure;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
  public void Configure(EntityTypeBuilder<Node> builder)
  {
    builder.HasKey(e => e.MacAddress);
    builder.Property(e => e.MacAddress).HasMaxLength(17);
    builder.Property(e => e.Name).HasMaxLength(200);
    builder.Property(e => e.Location).HasMaxLength(200);
  }
}
