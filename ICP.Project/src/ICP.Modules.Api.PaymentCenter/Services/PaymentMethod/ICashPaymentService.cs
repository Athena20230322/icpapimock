using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.PaymentCenterApi.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;
using ICP.Modules.Api.PaymentCenter.Repositories;
using System;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class ICashPaymentService : IPaymentMethod
    {
        private ICashPaymentRepository _ICashPaymentRepository;

        public ICashPaymentService(ICashPaymentRepository icashPaymentRepository)
        {
            _ICashPaymentRepository = icashPaymentRepository;
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

            var paymentType = (PaymentSubType_ICash)tradeRequestModel.PaymentSubTypeID;
            if (paymentType < PaymentSubType_ICash.Min ||
                paymentType > PaymentSubType_ICash.Max)
            {
                result.SetCode(7029);
            }
            
            return result;
        }

        /// <summary>
        /// 交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<TradeResModel> Process(TradeReqModel tradeRequestModel)
        {
            var deduResult = _ICashPaymentRepository.ReduceICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = "1_1_1_交易扣款",
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
            // 退款賣家依撥款負向處理,不先扣帳戶
            //var deduResult = _ICashPaymentRepository.ReduceICash(new ICashIncDecModel()
            //{
            //    TradeNo = tradeRequestModel.TradeNo,
            //    MID = tradeRequestModel.MerchantID,
            //    TradeModeID = 1,
            //    PaymentTypeID = 1,
            //    PaymentSubTypeID = 1,
            //    Notes = "1_1_1_交易退款扣款",
            //    Amount = tradeRequestModel.RefundAmount
            //});

            var result = new DataResult<RefundResModel>()
                        .SetSuccess(new RefundResModel());

            //if (!deduResult.IsSuccess)
            //{
            //    result.SetError(deduResult);
            //    return result;
            //}

            var addResult = _ICashPaymentRepository.AddICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = 1,
                PaymentTypeID = 1,
                PaymentSubTypeID = 1,
                Notes = "1_1_1_交易退款入款",
                Amount = tradeRequestModel.RefundAmount
            });

            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 退款訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryRefundTradeModel QryRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _ICashPaymentRepository.QryRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddRefundTradeModel AddRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _ICashPaymentRepository.AddRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> tradeResponseModel)
        {
            return _ICashPaymentRepository.UpdateRefundTrade(tradeID, tradeResponseModel);
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
            var deduResult = _ICashPaymentRepository.ReduceICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MerchantID,
                TradeModeID = 1,
                PaymentTypeID = 1,
                PaymentSubTypeID = 1,
                Notes = "1_1_1_交易取消扣款",
                Amount = tradeRequestModel.Amount
            });

            var result = new DataResult<ReversalResModel>()
                        .SetSuccess(new ReversalResModel());

            if (!deduResult.IsSuccess)
            {
                result.SetError(deduResult);
                return result;
            }

            var addResult = _ICashPaymentRepository.AddICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = 1,
                PaymentTypeID = 1,
                PaymentSubTypeID = 1,
                Notes = "1_1_1_交易取消入款",
                Amount = tradeRequestModel.Amount
            });
            result.SetError(addResult);
            return result;
        }

        /// <summary>
        /// 取消訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryReversalTradeModel QryReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _ICashPaymentRepository.QryReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddReversalTradeModel AddReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _ICashPaymentRepository.AddReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> tradeResponseModel)
        {
            return _ICashPaymentRepository.UpdateReversalTrade(tradeID, tradeResponseModel);
        }
        #endregion
    }
}
