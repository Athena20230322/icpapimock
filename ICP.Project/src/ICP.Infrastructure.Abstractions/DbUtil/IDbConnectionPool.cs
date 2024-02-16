using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.DbUtil
{
    public interface IDbConnectionPool : IDisposable
    {
        /// <summary>
        /// 建立 IDbConnection
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        IDbConnection Create(DatabaseName databaseName);
    }
}
