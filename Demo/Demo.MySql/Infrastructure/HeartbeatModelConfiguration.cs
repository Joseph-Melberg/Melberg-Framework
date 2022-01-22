using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.MySql.Infrastructure;

public class HeartbeatModelConfiguration : IEntityTypeConfiguration<HeartbeatModel>
{
    public void Configure(EntityTypeBuilder<HeartbeatModel> builder)
    {
        builder.ToTable("Heartbeat");

        builder.HasKey(_ => _.name);
    }
}