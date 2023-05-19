using AutoMapper;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Finance;
using ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail;
using ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail;
using ICP.Modules.Mvc.Admin.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class FinanceService
    {
        private FinanceRepository _financeRepository = null;

        public FinanceService(FinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }

        #region 每日收款交易金額監控

        /// <summary>
        /// 每日收款交易金額監控 - 查詢交易明細
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryTradeDetailRes>> ListTradeDetail(QryTradeDetailReq request)
        {
            var result = new DataResult<List<QryTradeDetailRes>>();

            QryTradeDetailDbReq dbReq = Mapper.Map<QryTradeDetailDbReq>(request);

            var dbResult = _financeRepository.ListTradeDetail(dbReq);

            if (dbResult == null)
            {
                result.SetError();
                return result;
            }

            List<QryTradeDetailRes> qryRes = Mapper.Map<List<QryTradeDetailRes>>(dbResult);

            result.SetSuccess(qryRes);

            return result;
        }

        #endregion

        #region 特店帳務進出明細

        /// <summary>
        /// 帳務類型選單
        /// </summary>
        /// <param name="tradeModeType">撈取類型</param>
        /// <remarks>0:全部 1:凍結款項除外</remarks>
        /// <returns></returns>
        public List<SelectListItem> ListTradeMode(int tradeModeType = 0)
        {
            var result = _financeRepository.ListTradeMode(tradeModeType);

            var item = result.ConvertAll(x => new SelectListItem() { Value = x.TradeModeID.ToString(), Text = x.TradeModeCName });

            return item;
        }

        /// <summary>
        /// 交易類型選單
        /// </summary>
        /// <param name="tradeModeID">帳務類型</param>
        /// <returns></returns>
        public List<SelectListItem> ListTradeType(int tradeModeID)
        {
            var result = _financeRepository.ListTradeType(tradeModeID);

            var item = result.ConvertAll(x => new SelectListItem() { Value = x.PaymentTypeID.ToString(), Text = x.PaymentNotes });

            return item;
        }

        /// <summary>
        /// 交易子類型選單
        /// </summary>
        /// <param name="paymentTypeID">交易類型</param>
        /// <returns></returns>
        public List<SelectListItem> ListTradeSubType(int paymentTypeID)
        {
            var result = _financeRepository.ListTradeSubType(paymentTypeID);

            var item = result.ConvertAll(x => new SelectListItem() { Value = x.PaymentSubTypeID.ToString(), Text = x.PaymentSubTypeNotes });

            return item;
        }

        /// <summary>
        /// 特店帳務進出明細 - 查詢明細
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryMerchantTradeDetailRes>> ListMerchantTradeDetail(QryMerchantTradeDetailReq request)
        {
            var result = new DataResult<List<QryMerchantTradeDetailRes>>();

            QryMerchantTradeDetailDbReq dbReq = Mapper.Map<QryMerchantTradeDetailDbReq>(request);

            var dbResult = _financeRepository.ListMerchantTradeDetail(dbReq);

            if (dbResult == null)
            {
                result.SetError();
                return result;
            }

            List<QryMerchantTradeDetailRes> qryRes = Mapper.Map<List<QryMerchantTradeDetailRes>>(dbResult);

            result.SetSuccess(qryRes);

            return result;
        }

        #endregion
    }
}
