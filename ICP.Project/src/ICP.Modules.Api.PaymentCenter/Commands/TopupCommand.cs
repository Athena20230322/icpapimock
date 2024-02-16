using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Services;

namespace ICP.Modules.Api.PaymentCenter.Commands
{
    public class TopupCommand : ITradeCommand
    {
        private readonly ITransactionMethodFactory _transactionMethodFactory = null;
        private readonly IPaymentMethodFactory _paymentMethodFactory = null;
        private readonly PaymentCommonService _paymentCommonService = null;

        public TopupCommand(ITransactionMethodFactory transactionMethodFactory,
                                  IPaymentMethodFactory paymentMethodFactory,
                                  PaymentCommonService paymentCommonService)
        {
            _transactionMethodFactory = transactionMethodFactory;
            _paymentMethodFactory = paymentMethodFactory;
            _paymentCommonService = paymentCommonService;
        }

        // 交易流程
        public object Transaction(object requestModel)
        {
            var result = new DataResult<TradeResModel>()
                        .SetSuccess(new TradeResModel());

            var request = requestModel as TradeReqModel;

            // 參數驗證
            var validateResult = CommonService.Validate(request);
            if (!validateResult.IsSuccess)
            {
                result.SetError(validateResult);
                return result;
            }

            // 檢核碼檢查
            if (!CommonService.CheckMacValue(request, request.CheckMacValue, _paymentCommonService.GenerateCheckMacValue))
            {
                return CommonService.SetResult<TradeResModel>(7001);
            }

            // 取得交易模式
            var topupMethod = _transactionMethodFactory.Create(request.TradeModeID);
            if (topupMethod == null)
            {
                return CommonService.SetResult<TradeResModel>(7002);
            }

            // 驗證交易
            var validateTransactionResult = topupMethod.Validate(request);
            if (!validateTransactionResult.IsSuccess)
            {
                return CommonService.SetResult<TradeResModel>(validateTransactionResult);
            }

            // 新增訂單
            var addResult = topupMethod.AddTrade(request);
            if (!addResult.IsSuccess)
            {
                return CommonService.SetResult<TradeResModel>(addResult);
            }
            request.TradeID = addResult.tradeID;
            result.RtnData.PaymentCenterTradeID = addResult.tradeID;

            // 依付款方式進行交易
            var paymentMethod = _paymentMethodFactory.Create((ePaymentType)request.PaymentTypeID);
            if (paymentMethod == null)
            {
                return CommonService.SetResult<TradeResModel>(7022);
            }

            // 驗證付款方式
            var validatePaymentResult = paymentMethod.Validate(request);
            if (!validatePaymentResult.IsSuccess)
            {
                return CommonService.SetResult<TradeResModel>(validatePaymentResult);
            }

            // 新增訂單明細
            var addDetailResult = paymentMethod.AddTrade(request);
            if (!addDetailResult.IsSuccess)
            {
                result.SetCode(addDetailResult.RtnCode, addDetailResult.RtnMsg);
                return result;
            }

            // 儲值交易
            result = paymentMethod.Process(request);

            // AccountLink扣款成功,新增儲值金
            if(result.IsSuccess && (ePaymentType)request.PaymentTypeID == ePaymentType.AccountLink)
            {
                ((AccountLinkPaymentService)paymentMethod).AddTopupCash(request);
            }

            // 更新訂單
            if((ePaymentType)request.PaymentTypeID != ePaymentType.ATM)
            {
                var updateResult = topupMethod.UpdateTrade(result);
                if (!updateResult.IsSuccess)
                {
                    // 訂單已被取消
                    if (result.IsSuccess && updateResult.RtnCode.Equals(7044))
                    {
                        paymentMethod.TimeoutRefundProcess(request);
                    }
                    return CommonService.SetResult<TradeResModel>(updateResult, result.RtnData);
                }
            }

            // 更新訂單明細
            var updateDetailResult = paymentMethod.UpdateTrade(result);
            if (!updateDetailResult.IsSuccess)
            {
                return CommonService.SetResult<TradeResModel>(updateDetailResult, result.RtnData);
            }

            // 連線逾時後續處理
            if (result.RtnCode.Equals(7006))
            {
                paymentMethod.TimeoutRefundProcess(request);
            }

            return result;
        }
    }
}
