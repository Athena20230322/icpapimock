using ICP.Infrastructure.Abstractions.DbUtil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.DbUtil
{
    public class DbConnectionPool : IDbConnectionPool
    {
        private readonly IDictionary<DatabaseName, IDbConnection> _dbConnections = null;

        public DbConnectionPool()
        {
            _dbConnections = new Dictionary<DatabaseName, IDbConnection>();
        }

        public IDbConnection Create(DatabaseName databaseName)
        {
            if (!_dbConnections.ContainsKey(databaseName))
            {
                _dbConnections.Add(databaseName, new DapperDbConnection(databaseName.ToString()));
            }

            return _dbConnections[databaseName];
        }

        public void Dispose()
        {
            foreach (var item in _dbConnections)
            {
                item.Value.Dispose();
            }

            _dbConnections.Clear();
        }
    }
}
