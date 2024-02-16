using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Interface
{
    public interface IPaymentMethod
    {
        #region 交易
        /// <summary>
        /// 交易資料驗證
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        DataResult<TradeResModel> Validate(TradeReqModel tradeRequestModel);

        /// <summary>
        /// 交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        DataResult<TradeResModel> Process(TradeReqModel tradeRequestModel);

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        BaseResult AddTrade(TradeReqModel tradeRequestModel);

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel);
        #endregion

        #region 退款
        /// <summary>
        /// 退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        DataResult<RefundResModel> RefundProcess(QryRefundTradeModel tradeRequestModel);

        /// <summary>
        /// 退款訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        QryRefundTradeModel QryRefundTrade(RefundReqModel tradeRequestModel);

        /// <summary>
        /// 新增退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        AddRefundTradeModel AddRefundTrade(RefundReqModel tradeRequestModel);

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> tradeResponseModel);

        /// <summary>
        /// 逾時退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        void TimeoutRefundProcess(TradeReqModel tradeRequestModel);
        #endregion

        #region 取消
        /// <summary>
        /// 取消交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        DataResult<ReversalResModel> ReversalProcess(QryReversalTradeModel tradeRequestModel);

        /// <summary>
        /// 取消訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        QryReversalTradeModel QryReversalTrade(ReversalReqModel tradeRequestModel);

        /// <summary>
        /// 新增取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        AddReversalTradeModel AddReversalTrade(ReversalReqModel tradeRequestModel);

        /// <summary>
        /// 更新取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> tradeResponseModel);
        #endregion
    }
}
