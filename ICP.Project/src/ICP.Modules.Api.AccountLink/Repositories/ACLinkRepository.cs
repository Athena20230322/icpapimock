using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.AccountLink.Models;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.AccountLink.Repositories
{
    public class ACLinkRepository
    {
        protected IDbConnectionPool _dbConnectionPool = null;

        public ACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得AccountLink設定
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public List<ACLinkSettingDbRes> ListAccountLinkSetting(string bankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkSetting_S]";

            var args = new
            {
                BankCode = bankCode
            };

            sql += db.GenerateParameter(args);
            return db.Query<ACLinkSettingDbRes>(sql, args);
        }

        /// <summary>
        /// 寫入綁定資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddAccountLink(ACLinkAddDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_AddAccountLink_I]";

            var args = new
            {
                model.MID,
                model.INDTAccount,
                model.BankCode,
                model.BankAccount,
                model.MsgNo,
                model.Status,
                IsDefault = 0
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新綁定資料 - 取消綁定AccountLink
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult CancelAccountLink(ACLinkCancelDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_CancelAccountLink_U]";

            var args = new
            {
                model.MID,
                model.INDTAccount,
                model.BankCode
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得MsgNo
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public string AddAccountLinkMsgNo(int type,string bankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_AddAccountLinkMsgNo_I]";

            var args = new
            {
                TYPE = type,
                BankCode = bankCode
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<string>(sql, args);
        }

        /// <summary>
        /// 取得會員指定銀行的AccountLink綁定資訊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkInfoDbRes GetAccountLinkInfo(ACLinkInfoDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_GetAccountLinkInfoByINDTAccount_S]";

            var args = new
            {
                model.MID,
                model.BankCode,
                model.INDTAccount
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<ACLinkInfoDbRes>(sql, args);
        }

        /// <summary>
        /// 取得會員所有的AccountLink綁定列表
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public List<ACLinkInfoDbRes> ListAccountLinkInfo(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkInfo_S]";

            var args = new
            {
                MID = mid
            };

            sql += db.GenerateParameter(args);
            return db.Query<ACLinkInfoDbRes>(sql, args).ToList();
        }

        /// <summary>
        /// 發送訊息通知
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="webSiteID"></param>
        [EnableDbProxy]
        public void AddNotifyMessage(long mid, string title, string body, int webSiteID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC [ICP_Share].[dbo].[ausp_NotifyMessage_AddNotifyMessage_I]";

            var args = new
            {
                MID = mid,
                Subject = title,
                SubjectURL = "",
                Body = body,
                WebSiteID = webSiteID
            };

            sql += db.GenerateParameter(args);
            db.Execute(sql, args);
        }

        /// <summary>
        /// 取得訂單資訊
        /// </summary>
        /// <param name="TradeNo"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public long GetTradeID(string TradeNo)
        {
            long tradeId = 0;
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTradeByTradeNo_S]";

            var args = new
            {
                TradeNo
            };

            sql += db.GenerateParameter(args);
            TradeInfoDbRes tradeModel = db.QuerySingleOrDefault<TradeInfoDbRes>(sql, args);
            if (tradeModel != null)
            {
                tradeId = tradeModel.TradeID;
            }

            return tradeId;
        }

        /// <summary>
        /// 更新Payment訂單的VirtualAccount
        /// </summary>
        /// <param name="TradeID"></param>
        /// <param name="VirtualAccount"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult UpdateTradeVirtualAccount(long TradeID, string VirtualAccount)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_UpdateACLinkVirtualAccount_U]";

            var args = new
            {
                TradeID,
                VirtualAccount
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
