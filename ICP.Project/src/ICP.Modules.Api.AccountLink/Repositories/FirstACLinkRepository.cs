using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.First;

namespace ICP.Modules.Api.AccountLink.Repositories
{
    public class FirstACLinkRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public FirstACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 寫入送出記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddFirstSendLog(ACLinkSendLogDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_First_SendLog_I";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

        /// <summary>
        /// 寫入接收記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddFirstReceiveLog(ACLinkReceiveLogDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_First_ReceiveLog_I";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

        /// <summary>
        /// 檢查訊息序號綁定申請記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult CheckBindLog(string apiType, string msgNo, string mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AccountLink_First_CheckBindLog_S";

            var args = new
            {
                ApiType = apiType,
                MSG_NO = msgNo,
                EC_USER = mid
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得轉入虛擬帳號
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public VirtualAccountModel GetPayeeAccount(object obj)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);
            string sql = "EXEC ausp_PaymentCenter_VirtualAccount_AddGate_I";

            sql += db.GenerateParameter(obj);
            return db.QuerySingleOrDefault<VirtualAccountModel>(sql, obj);
        }

    }
}
