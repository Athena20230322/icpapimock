using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.PaymentCenter
{
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;

    public class CertificateRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public CertificateRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得公鑰
        /// </summary>
        /// <param name="Code">憑證代碼</param>
        /// <returns></returns>
        public DataResult<string> GetPublicKey(string Code)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Certificate_GetPublicKey_S";

            var args = new
            {
                Code
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<string>>(sql, args);
        }
    }
}
