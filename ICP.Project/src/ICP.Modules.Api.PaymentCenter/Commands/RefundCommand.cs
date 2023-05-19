using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Services;
using System;

namespace ICP.Modules.Api.PaymentCenter.Commands
{
    public class RefundCommand : ITradeCommand
    {
        private readonly IPaymentMethodFactory _paymentMethodFactory = null;
        private readonly PaymentCommonService _paymentCommonService = null;

        public RefundCommand(IPaymentMethodFactory paymentMethodFactory,
                             PaymentCommonService paymentCommonService)
        {
            _paymentMethodFactory = paymentMethodFactory;
            _paymentCommonService = paymentCommonService;
        }

        // 退款交易流程
        public object Transaction(object requestModel)
        {
            var result = new DataResult<RefundResModel>()
                        .SetSuccess(new RefundResModel());

            var request = requestModel as RefundReqModel;

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
                return CommonService.SetResult<RefundResModel>(7001);
            }

            // 取得查詢退款方式
            var tempMethod = _paymentMethodFactory.Create(ePaymentType.ICash);

            // 訂單退款檢查
            var qryResult = tempMethod.QryRefundTrade(request);
            if (!qryResult.IsSuccess)
            {
                result.SetError(qryResult);
                return result;
            }
            
            if ((request.Amount ?? 0) == 0)
            {
                request.Amount = qryResult.RefundAmount;
            }

            // 取得實際PaymentType的退款方式
            var refundMethod = _paymentMethodFactory.Create((ePaymentType)qryResult.PaymentTypeID);

            // 新增退款
            var addResult = refundMethod.AddRefundTrade(request);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            // 退款執行
            var refundResult = refundMethod.RefundProcess(qryResult);
            result.SetError(refundResult);
            result.RtnData.PaymentCenterTradeID = addResult.SeqNo;
            result.RtnData.RefundAmount = qryResult.RefundAmount;
            result.RtnData.TradeDate = DateTime.Now;

            // 更新退款結果
            var updResult = refundMethod.UpdateRefundTrade(request.PaymentCenterTradeID, result);
            if (!updResult.IsSuccess)
            {
                result.SetError(updResult);
                return result;
            }
            
            return result;
        }
    }
}
