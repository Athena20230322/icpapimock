using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MerchantRepositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using ICP.Library.Models.MemberModels;
    using ICP.Library.Models.MerchantModels;

    public class MerchantInfoRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MerchantInfoRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得廠商資訊
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public MerchantDataModel GetMerchantData(long merchantID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Merchant_GetMerchantData_S";

            var args = new
            {
                MerchantID = merchantID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MerchantDataModel>(sql, args);
        }


        /// <summary>
        /// 取得廠商金鑰資訊
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public MerchantCertificateModel GetMerchantCertificateData(long merchantID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Certificate_GetMIDCertInfo_S";

            var args = new
            {
                MID = merchantID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MerchantCertificateModel>(sql, args);
        }
    }
}
