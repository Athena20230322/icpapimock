using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.EinvoiceLibrary;

namespace ICP.Modules.Api.CheckEinvoiceToken.Repositories
{
    public class CheckEinvoiceTokenRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public CheckEinvoiceTokenRepository(IDbConnectionPool IconnectionPool)
        {
            _dbConnectionPool = IconnectionPool;
        }

        /// <summary>
        /// 驗證Token 並回傳歸戶必要資訊比對
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="carruerNum"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public InvoiceBindReturn GetIssueBindDataByToken(string CarruerNum, string token)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateCarruerToken_SIU";

            var args = new
            {
                CarruerNum = CarruerNum,
                token = token
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<InvoiceBindReturn>(sql, args);
        }

        /// <summary>
        /// 接收到載具成功的訊息 更新發票狀態
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateIssueBindStatus(InvoiceBindReturn model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateCarruerToken_SIU";

            var args = new
            {
                card_ban = model.card_ban
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<InvoiceBindReturn>(sql, args);
        }

        /// <summary>
        /// 新增載具歸戶Log
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddConsolidateCarrierLog(InvoiceBindLogModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_AddConsolidateCarrier_I";

            var args = new
            {
                MID = model.MID,
                CarruerNum = model.CarruerNum,
                Status = model.Status,
                BindToken = model.BindToken,
                BindDate = model.BindToken,
                RealIP = model.RealIP,
                ProxyIP = model.ProxyIP
            };
            sql += db.GenerateParameter(args);

            db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}