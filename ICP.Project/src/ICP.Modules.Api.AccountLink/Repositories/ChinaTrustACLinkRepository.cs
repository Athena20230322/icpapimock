using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.ChinaTrust;
using System.Text;

namespace ICP.Modules.Api.AccountLink.Repositories
{
    public class ChinaTrustACLinkRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public ChinaTrustACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 寫入送出記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddChinaTrustSendLog(ACLinkSendLogModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC [ICP_Logging].[dbo].[ausp_Member_AccountLink_ChinaTrust_AddSendLog_I]";

            var args = new
            {
                model.LogType,
                model.MerchantType,
                model.MerchantId,
                model.LinkType,
                model.UserNo,
                model.UserId,
                model.HolderRelationship,
                model.Birth,
                model.DebitAccount,
                model.AgreeTime,
                model.TrxNo,
                model.TransactionId,
                model.AuthId,
                model.Otp,
                model.OrderNo,
                model.TrxType,
                model.PayerAccount,
                model.PayeeAccount,
                model.TrxAmt,
                model.TrxShopName,
                model.OriginalReferenceNo,
                model.TrxTime
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 寫入接收記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddChinaTrustReceiveLog(ACLinkReceiveLogModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC [ICP_Logging].[dbo].[ausp_Member_AccountLink_ChinaTrust_AddReceiveLog_I]";

            var args = new
            {
                model.LogType,
                model.TransactionId,
                model.ReturnCode,
                model.ReturnMessage,
                model.MerchantType,
                model.MerchantId,
                model.LinkType,
                model.UserNo,
                model.UserId,
                model.HolderRelationship,
                model.Birth,
                model.DebitAccount,
                model.TrxNo,
                model.TrxTime,
                model.AuthId,
                model.BindingStatus,
                model.TrxResult,
                model.ServiceCode,
                model.ServiceMessage
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 新增中國信託AccountLink入帳用虛擬帳號
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkVAccountDbRes AddChinaTrustVirtualAccount(ACLinkVAccountDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            StringBuilder szBuilder = new StringBuilder();
            szBuilder.AppendLine("DECLARE @@VirtualAccount VARCHAR(16), @@RtnCode INT, @@RtnMsg VARCHAR(200);");
            szBuilder.AppendLine("EXEC [ICP_PaymentCenter].[dbo].[ausp_PaymentCenter_VirtualAccount_AddChinaTrustBank_I] ");
            szBuilder.AppendLine("@PaymentTypeID, @PaymentSubTypeID, @Amount, @TradeID, @@VirtualAccount OUTPUT, @@RtnCode OUTPUT, @@RtnMsg OUTPUT;");
            szBuilder.AppendLine("SELECT @@VirtualAccount AS VirtualAccount, @@RtnCode AS RtnCode, @@RtnMsg AS RtnMsg;");

            var args = new
            {
                model.PaymentTypeID,
                model.PaymentSubTypeID,
                model.Amount,
                model.TradeID
            };

            return db.QuerySingleOrDefault<ACLinkVAccountDbRes>(szBuilder.ToString(), args);
        }
    }
}
