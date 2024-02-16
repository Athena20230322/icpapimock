using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.DbUtil
{
    public interface IDbConnection : IDisposable
    {
        int Timeout { get; set; }

        IDbTransaction BeginTransaction();

        int Execute(string sql, object obj = null);

        string GenerateParameter(object obj);

        List<T> Query<T>(string sql, object obj = null);

        T QuerySingleOrDefault<T>(string sql, object obj = null);

        List<IEnumerable<object>> QueryMultiple(Type[] types, string sql, object obj = null);
    }
}
