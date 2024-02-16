using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.AccountLinkApi;
using ICP.Modules.Api.Payment.Models.ACLink;
using ICP.Modules.Api.Payment.Services;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Payment.Commands
{
    public class AccountLinkCommand
    {
        private readonly AccountLinkService _acLinkService = null;
        private readonly ACLinkCommonService _commonService = null;
        private readonly TopUpPaymentCommand _topupPayment = null;
        private readonly TopUpPaymentService _topupPaymentService = null;
        private readonly ILogger _logger = null;

        public AccountLinkCommand(
            AccountLinkService accountLinkService,
            ACLinkCommonService aCLinkCommonService,
            TopUpPaymentCommand topUpPaymentCommand,
            TopUpPaymentService topUpPaymentService,
            ILogger<AccountLinkCommand> logger)
        {
            _acLinkService = accountLinkService;
            _commonService = aCLinkCommonService;
            _topupPayment = topUpPaymentCommand;
            _topupPaymentService = topUpPaymentService;
            _logger = logger;
        }

        #region 用AccountLink儲值帳戶金額
        /// <summary>
        /// 用AccountLink儲值帳戶金額
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> TopUpByAccountLink(ACLinkTopUpReq request)
        {
            _logger.Info($"[TopUpByAccountLink][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<object>();
            result.SetError();

            // 檢查儲值功能是否維護中
            if (!_topupPaymentService.CheckTopUpFunctionIsOpen())
            {
                result.SetCode(2100);
                return result;
            }

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 驗證AccountLink儲值邏輯
            var model = Mapper.Map<ACLinkTopUpModel>(request);
            var validateACTopUpResult = _acLinkService.ValidateACLinkTopUp(model);
            if (!validateACTopUpResult.IsSuccess)
            {
                result.SetError(validateACTopUpResult);
                return result;
            }

            // 組成建立儲值訂單所需資訊
            var tradeData = _acLinkService.TopUpTradeData(model);
            if (!tradeData.IsSuccess)
            {
                result.SetError(tradeData);
                return result;
            }

            // 建立儲值訂單
            var addTradeResult = _topupPayment.TopUpProcess(tradeData.RtnData);
            if (!addTradeResult.IsSuccess)
            {
                result.SetError(addTradeResult);
                return result;
            }

            // 取得儲值結果及組成回傳資料
            var topUpResult = _acLinkService.GetTopUpResult(addTradeResult.RtnData, model.AccountID);
            if (!topUpResult.IsSuccess)
            {
                result.SetError(topUpResult);
                return result;
            }
            result = topUpResult;

            _logger.Info($"[TopUpByAccountLink][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

    }
}
