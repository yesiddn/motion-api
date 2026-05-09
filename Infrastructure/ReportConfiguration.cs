using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace motion_api.Infrastructure;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
  public void Configure(EntityTypeBuilder<Report> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.EventType).HasConversion<string>();

    builder.HasOne(e => e.Node)
           .WithMany(e => e.Reports)
           .HasForeignKey(e => e.NodeId)
           .HasPrincipalKey(e => e.MacAddress);
  }
}