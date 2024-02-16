using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class BonusRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db;

        public BonusRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 取得紅利交易明細
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public List<QryBonusRes> ListFinanceBonusDetail(QryBonusReq qryBonusReq)
        {
            string sql = "EXEC ICP_Payment.dbo.ausp_Payment_Finance_ListBonusTrade_S";
            var args = new
            {
                DateType = qryBonusReq.DateType,
                StartDate = qryBonusReq.StartDate.ToString("yyyy/MM/dd"),
                EndDate = qryBonusReq.EndDate.AddDays(1).ToString("yyyy/MM/dd"),
                SellerICPMID = qryBonusReq.SellerICPMID,
                SellerCName = qryBonusReq.SellerCName,
                BuyerICPMID = qryBonusReq.BuyerICPMID,
                BuyerCName = qryBonusReq.BuyerCName,
                TradeNo = qryBonusReq.TradeNo,
                MerchantTradeNo = qryBonusReq.MerchantTradeNo,
                PointType = qryBonusReq.PointType,
                PageNo = qryBonusReq.PageNo,
                PageSize = qryBonusReq.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<QryBonusRes>(sql, args);
        }
    }
}
