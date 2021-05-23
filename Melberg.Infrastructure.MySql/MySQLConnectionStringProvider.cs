using System;
using System.Collections.Concurrent;
using System.Threading;
using Melberg.Core.MySql;
using Microsoft.Extensions.Configuration;

namespace Melberg.Infrastructure.MySql
{
    public class MySQLConnectionStringProvider : IMySqlConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, Lazy<string>> _connectionStrings = new ConcurrentDictionary<string, Lazy<string>>();

        public MySQLConnectionStringProvider(IConfiguration configRoot)
        {
            _configuration = configRoot;
        }
        public string GetConnectionString(string connectionStringName)
        {
            var connectionString = _connectionStrings.GetOrAdd(connectionStringName,
                connecStringName => new Lazy<string>(() => _configuration.GetConnectionString(connecStringName),
                    LazyThreadSafetyMode.ExecutionAndPublication));

            return connectionString.Value;
        }
    }
}