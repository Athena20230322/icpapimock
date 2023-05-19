using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;
using ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod;
using System;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class InvoicePaymentService : IPaymentMethod
    {
        private InvoicePaymentRepository _invoicePaymentRepository;

        public InvoicePaymentService(InvoicePaymentRepository invoicePaymentRepository)
        {
            _invoicePaymentRepository = invoicePaymentRepository;
        }

        #region 交易
        /// <summary>
        /// 交易資料驗證
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<TradeResModel> Validate(TradeReqModel tradeRequestModel)
        {
            var result = new DataResult<TradeResModel>();
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<TradeResModel> Process(TradeReqModel tradeRequestModel)
        {
            var deduResult = _invoicePaymentRepository.AddTopupCash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                MerchantID = tradeRequestModel.MerchantID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = "2_5_1_儲值交易入款(發票)",
                Amount = tradeRequestModel.Amount
            });
            var result = new DataResult<TradeResModel>()
                        .SetSuccess(new TradeResModel() { PaymentCenterTradeID = tradeRequestModel.TradeID, PaymentDate = DateTime.Now })
                        .SetError(deduResult);

            return result;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult AddTrade(TradeReqModel tradeRequestModel)
        {
            return new BaseResult().SetSuccess();
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            return new BaseResult().SetSuccess();
        }
        #endregion

        #region 退款
        /// <summary>
        /// 退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<RefundResModel> RefundProcess(QryRefundTradeModel tradeRequestModel)
        {
            var result = new DataResult<RefundResModel>()
                        .SetSuccess(new RefundResModel());

            var deduResult = _invoicePaymentRepository.ReduceTopupCash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = "2_5_1_儲值退款扣款(發票)",
                Amount = tradeRequestModel.RefundAmount
            });

            return result.SetError(deduResult);
        }

        /// <summary>
        /// 退款訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryRefundTradeModel QryRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _invoicePaymentRepository.QryRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddRefundTradeModel AddRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _invoicePaymentRepository.AddRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> tradeResponseModel)
        {
            return _invoicePaymentRepository.UpdateRefundTrade(tradeID, tradeResponseModel);
        }

        /// <summary>
        /// 逾時退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public void TimeoutRefundProcess(TradeReqModel tradeRequestModel)
        {
            throw new Exception("無退款功能");
        }
        #endregion

        #region 取消
        /// <summary>
        /// 取消交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<ReversalResModel> ReversalProcess(QryReversalTradeModel tradeRequestModel)
        {
            var result = new DataResult<ReversalResModel>()
                        .SetSuccess(new ReversalResModel());

            var deduResult = _invoicePaymentRepository.ReduceTopupCash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = "2_5_1_儲值取消扣款(發票)",
                Amount = tradeRequestModel.Amount
            });

            return result.SetError(deduResult);
        }

        /// <summary>
        /// 取消訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryReversalTradeModel QryReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _invoicePaymentRepository.QryReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddReversalTradeModel AddReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _invoicePaymentRepository.AddReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> tradeResponseModel)
        {
            return _invoicePaymentRepository.UpdateReversalTrade(tradeID, tradeResponseModel);
        }
        #endregion
    }
}
