using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.AccountLink.Models.Cathay;

namespace ICP.Modules.Api.AccountLink.Repositories
{
    public class CathayACLinkRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public CathayACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 記錄傳送請求資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddSendLog(ACLinkSendLogDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_Cathay_AddSendLog_I";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

        /// <summary>
        /// 記錄傳送回應資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddReceiveLog(ACLinkReceiveLogDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_Cathay_AddReceiveLog_I";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

        /// <summary>
        /// 取申請綁定的記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkBindLogDbRes GetBindLog(ACLinkBindLogDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_Cathay_GetBindLog_S";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<ACLinkBindLogDbRes>(sql, model);
        }

    }
}
