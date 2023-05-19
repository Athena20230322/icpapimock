using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.TopUp;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.TopUp;
using ICP.Modules.Api.Payment.Services;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Payment.Commands
{
    public class TopUpPaymentCommand : ChargeCommand
    {
        private readonly TopUpPaymentService _topupPaymentService = null;
        private readonly TopUpService _topUpService = null;
        private readonly ILogger _logger = null;

        public TopUpPaymentCommand(
            PaymentProvider paymentProviderService,
            PaymentService paymentService,
            CommonService commonService,
            PaymentCommonService paymentCommonService,
            TopUpPaymentService topUpPaymentService,
            AccountLinkService accountLinkService,
            TopUpService topUpService,
            ILogger<ChargeCommand> chargelogger,
            ILogger<TopUpPaymentCommand> logger
            ) : base(paymentProviderService, paymentService, commonService, paymentCommonService, topUpPaymentService, accountLinkService, chargelogger)
        {
            _topupPaymentService = topUpPaymentService;
            _topUpService = topUpService;
            _logger = logger;
        }
        
        #region 儲值交易處理
        /// <summary>
        /// 儲值交易流程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfoModel> TopUpProcess(TopUpTradeInfoModel model)
        {
            _logger.Info($"[TopUpProcess][Input] {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<TopUpTradeInfoModel>();
            result.SetError();

            // 驗證儲值金額
            var checkResult = _topUpService.CheckTopUpLimit(model.MID, decimal.ToInt32(model.Amount));
            if (!checkResult.IsSuccess)
            {
                result.SetError(checkResult);
                return result;
            }

            // 組成送出資訊
            var getPostData = _topupPaymentService.TopUpAddTradePostData(model);
            if (!getPostData.IsSuccess)
            {
                result.SetError(getPostData);
                return result;
            }

            // 送至交易流程
            CashierRes cashierResult = Payment(getPostData.RtnData);
            if (!cashierResult.IsSuccess)
            {
                result.SetError(cashierResult);
                return result;
            }

            //// 取得交易結果
            var tradeResult = _topupPaymentService.GetTradeResult(cashierResult);
            if (!tradeResult.IsSuccess)
            {
                result.SetError(tradeResult);
                return result;
            }

            result = tradeResult;

            _logger.Info($"[TopUpProcess][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 取得現金儲值條碼
        /// <summary>
        /// 取得現金儲值條碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> GetTopUpBarCode(TopUpBarcodeReq request)
        {
            _logger.Info($"[GetTopUpBarCode][Input] {JsonConvert.SerializeObject(request)}");

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

            // 驗證儲值金額
            var checkResult = _topUpService.CheckTopUpLimit(request.MID, request.Amount);
            if (!checkResult.IsSuccess)
            {
                result.SetError(checkResult);
                return result;
            }

            // 取得現金儲值條碼
            GetTopUpBarcodeReq getBarcode = new GetTopUpBarcodeReq { MID = request.MID, Amount = request.Amount, PaymentType = 1 };
            var dataResult = _topUpService.GetTopUpBarCode(getBarcode);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result.RtnCode = dataResult.RtnCode;
            result.RtnMsg = dataResult.RtnMsg;
            result.RtnData = new { Barcode = dataResult.RtnData };

            _logger.Info($"[GetTopUpBarCode][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 取得儲值頁資料
        /// <summary>
        /// 取得虛擬帳號儲值頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> GetTopUpByATMInfo(GetTopUpDataReq request)
        {
            _logger.Info($"[GetTopUpByATMInfo][Input] {JsonConvert.SerializeObject(request)}");

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

            // 取得ATM儲值頁資料
            var dataResult = _topupPaymentService.GetATMTopUpInfo(request.MID);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[GetTopUpByATMInfo][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得連結帳戶儲值頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> GetTopUpByAccountLinkInfo(GetTopUpDataReq request)
        {
            _logger.Info($"[GetTopUpByAccountLinkInfo][Input] {JsonConvert.SerializeObject(request)}");

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

            // 取得連結帳戶儲值頁資料
            var dataResult = _topupPaymentService.GetACLinkTopUpInfo(request.MID);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[GetTopUpByAccountLinkInfo][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得儲值通路清單
        /// </summary>
        /// <returns></returns>
        public DataResult<object> GetListChannelInfo(GetTopUpDataReq request)
        {
            _logger.Info($"[GetListChannelInfo][Input] {JsonConvert.SerializeObject(request)}");

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

            // 取得儲值通路清單
            var dataResult = _topupPaymentService.GetListChannelInfo(request.MID);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[GetListChannelInfo][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 虛擬帳號儲值
        /// <summary>
        /// 虛擬帳號儲值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> TopUpByATM(TopUpByATMReq request)
        {
            _logger.Info($"[TopUpByATM][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<object>();
            result.SetError();

            // 檢查儲值功能是否維護中
            if (!_topupPaymentService.CheckTopUpFunctionIsOpen())
            {
                result.SetCode(2100);
                return result;
            }

            // 檢查虛擬帳號儲值功能是否維護中
            if (!_topupPaymentService.CheckATMTopUpFunctionIsOpen())
            {
                result.SetCode(2101);
                return result;
            }

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 組成建立儲值訂單所需資訊
            var tradeData = _topupPaymentService.TopUpByATMTradeData(request);
            if (!tradeData.IsSuccess)
            {
                result.SetError(tradeData);
                return result;
            }

            // 建立儲值訂單
            var addTradeResult = TopUpProcess(tradeData.RtnData);
            if (!addTradeResult.IsSuccess)
            {
                result.SetError(addTradeResult);
                return result;
            }

            // 取得儲值結果及組成回傳資料
            var topUpResult = _topupPaymentService.GetTopUpResult(addTradeResult.RtnData);
            if (!topUpResult.IsSuccess)
            {
                result.SetError(topUpResult);
                return result;
            }
            result = topUpResult;

            _logger.Info($"[TopUpByATM][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 自動儲值設定
        /// <summary>
        /// 設定自動儲值條件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> SetAutoTopUpCondition(AutoTopUpConditionReq request)
        {
            _logger.Info($"[SetAutoTopUpCondition][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<object>();
            result.SetError();

            // 檢查自動儲值設定是否維護中
            if (!_topupPaymentService.CheckAutoTopUpFunctionIsOpen())
            {
                result.SetCode(2102);
                return result;
            }

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 設定自動儲值條件
            var model = Mapper.Map<AutoTopUpModel>(request);
            var dataResult = _topupPaymentService.SetAutoTopUpCondition(model);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[SetAutoTopUpCondition][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 設定自動儲值開關
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> SetAutoTopUpSwitch(AutoTopUpSwitchReq request)
        {
            _logger.Info($"[SetAutoTopUpSwitch][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<object>();
            result.SetError();

            // 檢查自動儲值設定是否維護中
            if (!_topupPaymentService.CheckAutoTopUpFunctionIsOpen())
            {
                result.SetCode(2102);
                return result;
            }

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 更新自動儲值開關設定
            var dataResult = _topupPaymentService.UpdateAutoTopUpSwitch(request.MID, request.TopUpSwitch);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[SetAutoTopUpSwitch][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得自動儲值設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<object> GetAutoTopUpInfo(QryMemberInfoReq request)
        {
            _logger.Info($"[GetAutoTopUpInfo][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<object>();
            result.SetError();

            // 檢查自動儲值設定是否維護中
            if (!_topupPaymentService.CheckAutoTopUpFunctionIsOpen())
            {
                result.SetCode(2102);
                return result;
            }

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 取得自動儲值設定
            var queryResult = _topupPaymentService.GetAutoTopUpInfo(request.MID);
            if (!queryResult.IsSuccess)
            {
                result.SetError(queryResult);
                return result;
            }

            // 組成回傳資訊
            var dataResult = _topupPaymentService.GetAutoTopUpReturnData(queryResult.RtnData);
            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);
                return result;
            }
            result = dataResult;

            _logger.Info($"[GetAutoTopUpInfo][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

    }
}
