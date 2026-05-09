using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace motion_api.Infrastructure;

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
  public void Configure(EntityTypeBuilder<ApiKey> builder)
  {
    builder.HasKey(e => e.Key);
    builder.Property(e => e.Key).HasMaxLength(200);

    builder.HasOne(e => e.Node)
           .WithMany(e => e.ApiKeys)
           .HasForeignKey(e => e.NodeId)
           .HasPrincipalKey(e => e.MacAddress);
  }
}
