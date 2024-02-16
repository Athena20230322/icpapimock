using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class BonusService
    {
        private readonly BonusRepository _bonusRepository = null;
        private readonly ILogger _logger = null;

        public BonusService(
            BonusRepository bonusRepository,
            ILogger<BonusService> logger
        )
        {
            _bonusRepository = bonusRepository;
            _logger = logger;
        }

        /// <summary>
        /// 取得紅利交易明細
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public DataResult<List<QryBonusRes>> ListFinanceBonusDetail(QryBonusReq qryBonusReq)
        {
            DataResult<List<QryBonusRes>> result = new DataResult<List<QryBonusRes>>();
            result.SetError();

            List<QryBonusRes> bonusDetail = new List<QryBonusRes>();

            try
            {
                bonusDetail = _bonusRepository.ListFinanceBonusDetail(qryBonusReq);
            }
            catch(Exception ex)
            {
                _logger.Error($"ListFinanceBonusDetail Exception, Error={ex.ToString()}");

                return result;
            }

            result.SetSuccess(bonusDetail);

            return result;
        }

        /// <summary>
        /// 取得要匯出的紅利交易明細資訊
        /// </summary>
        /// <returns></returns>
        public Func<QryBonusRes, string[]> GetExcelDateil()
        {
            return t =>
            {
                string paymentTypeName = "-";

                if (t.PaymentTypeID == 1)
                {
                    paymentTypeName = "愛金帳戶";
                }
                else if (t.PaymentTypeID == 2)
                {
                    paymentTypeName = "連結扣款帳戶";
                }

                string refundStatusName = "-";

                if (t.RefundStatus == 2)
                {
                    refundStatusName = "已退款";
                }
                else if (t.RefundStatus == 3)
                {
                    refundStatusName = "已部分退款";
                }

                var values = new string[]
                {
                    t.PointType == 1?"OPENPOINT":"",
                    t.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.PaymentDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.RefundDate.HasValue ? t.RefundDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "-",
                    t.TradeNo,
                    t.MerchantTradeNo,
                    t.SellerICPMID,
                    t.SellerCName,
                    t.BuyerICPMID,
                    t.BuyerCName,
                    t.Amount.ToString("N0"),
                    t.DebitPoint.ToString("N0"),
                    t.BonusAmt.ToString("N0"),
                    t.RefundAmount.ToString("N0"),
                    paymentTypeName,
                    refundStatusName
                };

                return values;
            };
        }
    }
}
