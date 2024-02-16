using Dapper;
using ICP.Infrastructure.Abstractions.DbUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.DbUtil
{
    public class DapperDbConnection : DbConnection
    {
        public DapperDbConnection(string name) : base(name) { }

        public override int Execute(string sql, object obj = null)
        {
            return SqlConnection.Value.Execute(sql, obj, commandTimeout: Timeout);
        }

        public override List<T> Query<T>(string sql, object obj = null)
        {
            return SqlConnection.Value.Query<T>(sql, obj, commandTimeout: Timeout).ToList();
        }

        public override T QuerySingleOrDefault<T>(string sql, object obj = null)
        {
            return SqlConnection.Value.QuerySingleOrDefault<T>(sql, obj, commandTimeout: Timeout);
        }

        public override List<IEnumerable<object>> QueryMultiple(Type[] types, string sql, object obj = null)
        {
            var results = new List<IEnumerable<object>>();

            var result = SqlConnection.Value.QueryMultiple(sql, obj, commandTimeout: Timeout);

            foreach (Type type in types)
            {
                results.Add(result.Read(type));
            }

            return results;
        }
    }
}
