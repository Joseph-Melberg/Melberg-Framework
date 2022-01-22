using Melberg.Core.MySql;
using Melberg.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;

namespace Demo.MySql.Infrastructure;
public class ReadWriteContext : DefaultContext
{
    public DbSet<HeartbeatModel> Heartbeats {get; set;}
    public ReadWriteContext(IMySqlConnectionStringProvider provider) : base(provider)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HeartbeatModelConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}