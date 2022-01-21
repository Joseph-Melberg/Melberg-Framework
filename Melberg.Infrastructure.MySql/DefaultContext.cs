using System;
using System.Reflection;
using Melberg.Core.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Melberg.Infrastructure.MySql;
public class DefaultContext : DbContext
{
    private readonly IMySqlConnectionStringProvider _connectionStringProvider;
    public DefaultContext(IMySqlConnectionStringProvider provider){
        _connectionStringProvider = provider;
    } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _connectionStringProvider.GetConnectionString(this.GetType().Name)
            ?? throw new Exception($"Unable to find connection string: {this.GetType().Name}");
        optionsBuilder.UseMySQL(connectionString, builder => {});
    }

    //Not really sure why this isn't implemented correctly in the first place
    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new System.ArgumentNullException(nameof(entity));
        }

        var type = entity.GetType();
        var et = this.Model.FindEntityType(type);
        var key = et.FindPrimaryKey();

        var keys = new object[key.Properties.Count];
        var x = 0;
        foreach (var keyName in key.Properties)
        {
            var keyProperty = type.GetProperty(keyName.Name, BindingFlags.Public | BindingFlags.Instance);
            keys[x++] = keyProperty.GetValue(entity);
        }

        var originalEntity = Find(type, keys);
        if (Entry(originalEntity).State == EntityState.Modified)
        {
            return base.Update(entity);
        }

        Entry(originalEntity).CurrentValues.SetValues(entity);
        return Entry((TEntity)originalEntity);
    }
}