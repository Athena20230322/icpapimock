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
    public class ReversalCommand : ITradeCommand
    {
        private readonly IPaymentMethodFactory _paymentMethodFactory = null;
        private readonly PaymentCommonService _paymentCommonService = null;

        public ReversalCommand(IPaymentMethodFactory paymentMethodFactory,
                             PaymentCommonService paymentCommonService)
        {
            _paymentMethodFactory = paymentMethodFactory;
            _paymentCommonService = paymentCommonService;
        }

        // 取消交易流程
        public object Transaction(object requestModel)
        {
            var result = new DataResult<ReversalResModel>()
                        .SetSuccess(new ReversalResModel());

            var request = requestModel as ReversalReqModel;

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
                return CommonService.SetResult<ReversalResModel>(7001);
            }

            // 取得查詢退款方式
            var tempMethod = _paymentMethodFactory.Create(ePaymentType.ICash);

            // 訂單退款檢查
            var qryResult = tempMethod.QryReversalTrade(request);
            if (!qryResult.IsSuccess)
            {
                result.SetError(qryResult);
                return result;
            }

            // 取得實際PaymentType的退款方式
            var reversalMethod = _paymentMethodFactory.Create((ePaymentType)qryResult.PaymentTypeID);

            // 新增退款
            var addResult = reversalMethod.AddReversalTrade(request);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            // 退款執行
            var refundResult = reversalMethod.ReversalProcess(qryResult);
            result.SetError(refundResult);
            result.RtnData.PaymentCenterTradeID = addResult.SeqNo;
            result.RtnData.TradeDate = DateTime.Now;

            // 更新退款結果
            var updResult = reversalMethod.UpdateReversalTrade(request.PaymentCenterTradeID, result);
            if (!updResult.IsSuccess)
            {
                result.SetError(updResult);
                return result;
            }
            
            return result;
        }
    }
}
