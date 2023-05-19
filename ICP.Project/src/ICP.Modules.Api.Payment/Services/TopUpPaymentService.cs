using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AdminModels;
using ICP.Library.Models.TopUp;
using ICP.Library.Services.AdminServices;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.ACLink;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.CreateBarcode;
using ICP.Modules.Api.Payment.Models.TopUp;
using ICP.Modules.Api.Payment.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services
{
    public class TopUpPaymentService
    {
        private readonly TopUpRepository _topUpRepository = null;
        private readonly ACLinkRepository _acLinkRepository = null;
        private readonly AccountLinkService _accountLinkService = null;
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly TopUpService _topUpService = null;
        private readonly LibAdminService _libAdminService = null;
        private readonly ILogger _logger = null;

        public TopUpPaymentService(
            TopUpRepository topUpRepository,
            ACLinkRepository acLinkRepository,
            AccountLinkService accountLinkService,
            PaymentCommonService paymentCommonService,
            TopUpService topUpService,
            LibAdminService libAdminService,
            ILogger<TopUpPaymentService> logger)
        {
            _topUpRepository = topUpRepository;
            _acLinkRepository = acLinkRepository;
            _accountLinkService = accountLinkService;
            _paymentCommonService = paymentCommonService;
            _topUpService = topUpService;
            _libAdminService = libAdminService;
            _logger = logger;
        }
        
        #region 虛擬帳號儲值
        /// <summary>
        /// 組成建立儲值訂單所需資訊 (ATM)
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="tradeAmount"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfoModel> TopUpByATMTradeData(TopUpByATMReq model)
        {
            var result = new DataResult<TopUpTradeInfoModel>();
            result.SetError();

            TopUpTradeInfoModel rtnModel = new TopUpTradeInfoModel
            {
                MID = model.MID,
                Amount = model.Amount,
                PaymentTypeID = 3,
                PaymentSubTypeID = 1   // 一銀
            };

            result.SetSuccess(rtnModel);

            return result;
        }

        /// <summary>
        /// 取得儲值結果及組成回傳資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> GetTopUpResult(TopUpTradeInfoModel tradeModel)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"儲值結果: {JsonConvert.SerializeObject(tradeModel)}");

            if (tradeModel == null)
            {
                tradeModel = new TopUpTradeInfoModel();
                tradeModel.RtnCode = 0;
                tradeModel.RtnMsg = "Error";
            }

            // 取得會員儲值帳戶資訊
            TopUpLimitModel accountModel = _topUpService.GetMemberTopUpInfo(tradeModel.MID);
            string limitDate = !string.IsNullOrWhiteSpace(tradeModel.ATMExpireDate) ? string.Format("{0} 23:59:59", tradeModel.ATMExpireDate) : "";

            var rtnData = new
            {
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                IcpTradeNo = tradeModel.TransactionID,
                BankCode = tradeModel.BankCode,
                BankName = tradeModel.BankName,
                BankShortName = tradeModel.BankAppName,
                Amount = decimal.ToInt32(tradeModel.Amount),
                ATMAccount = tradeModel.VirtualAccount,
                LimitDate = limitDate,
                CreateDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                TotalCoins = accountModel.TopUpAmt,
                AvailableTopUp = accountModel.AvailableTopUp,
                TopUpLimit = accountModel.TopUpLimit
            };

            if (tradeModel.RtnCode == 1)
            {
                result.SetSuccess(rtnData);
            }
            else
            {
                result.SetCode(tradeModel.RtnCode, rtnData);
            }

            _logger.Trace($"儲值結果回傳資訊: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 取得儲值頁資料
        /// <summary>
        /// 取得虛擬帳號儲值頁資料
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<object> GetATMTopUpInfo(long mid)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得虛擬帳號儲值頁資料: mid:{mid}");

            // 取得虛擬帳號儲值銀行清單
            int bankCount = 1;
            object[] bankList = new object[bankCount];
            bankList[0] = new { BankCode = "007", BankName = "第一商業銀行", BankShortName = "第一銀行" };

            // 取得會員儲值帳戶資訊
            TopUpLimitModel accountModel = _topUpService.GetMemberTopUpInfo(mid);

            // 組成回傳資訊
            var rtnData = new
            {
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                TotalCoins = accountModel.TopUpAmt,
                AvailableTopUp = accountModel.AvailableTopUp,
                TopUpLimit = accountModel.TopUpLimit,
                BankList = bankList
            };

            result.SetSuccess(rtnData);

            _logger.Trace($"取得虛擬帳號儲值頁資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得連結帳戶儲值頁資料
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkTopUpInfo(long mid)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得連結帳戶儲值頁資料: mid:{mid}");

            // 取得已綁定的AccountLink
            var oACLinkList = _accountLinkService.ListBindACLink(mid);

            // 取得會員儲值帳戶資訊
            TopUpLimitModel accountModel = _topUpService.GetMemberTopUpInfo(mid);

            // 組成回傳資訊
            var rtnData = new
            {
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                TotalCoins = accountModel.TopUpAmt,
                AvailableTopUp = accountModel.AvailableTopUp,
                TopUpLimit = accountModel.TopUpLimit,
                ACLinkList = oACLinkList
            };

            result.SetSuccess(rtnData);

            _logger.Trace($"取得連結帳戶儲值頁資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得儲值通路清單
        /// </summary>
        /// <returns></returns>
        public DataResult<object> GetListChannelInfo(long mid)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得取得儲值通路清單");

            // 取得會員儲值帳戶資訊
            TopUpLimitModel accountModel = _topUpService.GetMemberTopUpInfo(mid);

            // 取得通路資料
            int count = 1;
            object[] ChannelList = new object[count];
            ChannelList[0] = new {
                MerchantID = "1000100",
                MerchantName = "統一超商",
                LogoImageURL = GlobalConfigUtil.Host_Payment_Domain + "/Areas/Payment/Content/images/711_icon.png",
                MerchantTopUpLimit = accountModel.TopUpLimit,
                MerchantTopUpLowest = 1
            };

            // 組成回傳資訊
            var rtnData = new
            {
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                TotalCoins = accountModel.TopUpAmt,
                AvailableTopUp = accountModel.AvailableTopUp,
                TopUpLimit = accountModel.TopUpLimit,
                ChannelList = ChannelList
            };

            result.SetSuccess(rtnData);

            _logger.Trace($"取得取得儲值通路清單: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 自動儲值設定
        /// <summary>
        /// 設定自動儲值條件
        /// </summary>
        /// <param name="autoTopUpModel"></param>
        /// <returns></returns>
        public DataResult<object> SetAutoTopUpCondition(AutoTopUpModel autoTopUpModel)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備新增/更新自動儲值條件: {JsonConvert.SerializeObject(autoTopUpModel)}");

            BaseResult updateResult = _topUpRepository.UpdateAutoTopUpCondition(autoTopUpModel);

            _logger.Trace($"自動儲值條件新增/更新完成: {JsonConvert.SerializeObject(updateResult)}");

            if (updateResult.IsSuccess)
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(updateResult.RtnCode);
            }

            return result;
        }

        /// <summary>
        /// 更新自動儲值開關設定
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="topupSwitch"></param>
        /// <returns></returns>
        public DataResult<object> UpdateAutoTopUpSwitch(long mid, int topupSwitch)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備更新自動儲值開關設定: mid:{mid}, topupSwitch:{topupSwitch}");

            BaseResult updateResult = _topUpRepository.UpdateAutoTopUpSwitch(mid, topupSwitch);

            _logger.Trace($"自動儲值開關設定更新完成: {JsonConvert.SerializeObject(updateResult)}");

            if (updateResult.IsSuccess)
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(updateResult.RtnCode);
            }

            return result;
        }

        /// <summary>
        /// 取得自動儲值設定
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<AutoTopUpModel> GetAutoTopUpInfo(long mid)
        {
            var result = new DataResult<AutoTopUpModel>();
            result.SetError();

            _logger.Trace($"準備取得自動儲值設定: mid:{mid}");

            AutoTopUpModel dataResult = _topUpRepository.GetAutoTopUpInfo(mid);

            if (dataResult == null)
            {
                dataResult = new AutoTopUpModel
                {
                    MID = mid
                };
            }

            _logger.Trace($"成功取得自動儲值設定: {JsonConvert.SerializeObject(dataResult)}");

            result.SetSuccess(dataResult);

            return result;
        }

        /// <summary>
        /// 取得自動儲值設定回傳資訊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> GetAutoTopUpReturnData(AutoTopUpModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得自動儲值設定回傳資訊: {JsonConvert.SerializeObject(model)}");

            string linkAccount = !string.IsNullOrWhiteSpace(model.BankAccount) ? _accountLinkService.GetBankAccountMask(model.BankAccount) : null;
            string bankName = !string.IsNullOrWhiteSpace(model.BankCode) ? _accountLinkService.GetACLinkBankInfo(model.BankCode).BankFullName : null;
            string bankShortName = !string.IsNullOrWhiteSpace(model.BankCode) ? _accountLinkService.GetACLinkBankInfo(model.BankCode).BankName : null;

            var rtnData = new
            {
                model.MID,
                model.TopUpSwitch,
                model.BankCode,
                model.AccountID,
                model.INDTAccount,
                LINKAccount = linkAccount,
                BankName = bankName,
                BankShortName = bankShortName,
                model.TopUpMode,
                model.TopUpUnit,
                model.LimitAmount,
                model.DailyLimitAmount
            };

            _logger.Trace($"成功取得自動儲值設定回傳資訊: {JsonConvert.SerializeObject(rtnData)}");

            result.SetSuccess(rtnData);

            return result;
        }
        #endregion

        #region 儲值交易處理
        /// <summary>
        /// 組成建立儲值訂單送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<CashierReq> TopUpAddTradePostData(TopUpTradeInfoModel model)
        {
            var result = new DataResult<CashierReq>();
            result.SetError();

            _logger.Trace($"準備組成建立儲值訂單送出資料: {JsonConvert.SerializeObject(model)}");

            model.PlatformID = 0;
            model.MerchantID = 0;
            model.MerchantTradeNo = DateTime.Now.ToString("yyMMddHHmmss") + GetRandomNumber();
            model.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            model.TradeType = 0;
            model.TradeModeID = 2;

            CashierReq cashierRequest = Mapper.Map<CashierReq>(model);
            cashierRequest.CheckMacValue = GetMacValue(cashierRequest);

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(cashierRequest)}");

            result.SetSuccess(cashierRequest);

            return result;
        }

        /// <summary>
        /// 取得訂單交易結果
        /// </summary>
        /// <param name="cashier"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfoModel> GetTradeResult(CashierRes cashier)
        {
            var result = new DataResult<TopUpTradeInfoModel>();
            result.SetError();

            _logger.Trace($"準備取得訂單交易結果: {JsonConvert.SerializeObject(cashier)}");

            result.RtnCode = cashier.RtnCode;
            result.RtnMsg = cashier.RtnMsg;
            result.RtnData = Mapper.Map<TopUpTradeInfoModel>(cashier);

            _logger.Trace($"成功取得交易結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 電支帳戶儲值入帳
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfoModel> AddTopUp(TopUpTradeInfoModel model)
        {
            var result = new DataResult<TopUpTradeInfoModel>();
            result.SetError();

            _logger.Trace($"準備儲值帳戶金額: {JsonConvert.SerializeObject(model)}");

            AddUserCoinsDbReq addModel = new AddUserCoinsDbReq
            {
                TradeNo = model.TransactionID,
                MID = model.MID,
                MerchantID = model.MerchantID,
                TradeModeID = model.TradeModeID,
                PaymentTypeID = model.PaymentTypeID,
                PaymentSubTypeID = model.PaymentSubTypeID,
                TradeRealCash = 0,
                TradeTopUpCash = model.Amount,
                Notes = "儲值"
            };

            BaseResult rtnModel = _topUpRepository.AddUserCoins(addModel);
            result.RtnCode = rtnModel.RtnCode;
            result.RtnMsg = rtnModel.RtnMsg;
            result.RtnData = model;

            _logger.Trace($"帳戶儲值完成: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 交易時自動儲值處理
        /// <summary>
        /// 檢查自動儲值
        /// 1.檢查是否有自動儲值設定
        /// 2.計算需自動儲值金額
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<ACLinkTopUpModel> CheckAutoTopUp(AutoTopUpReq request)
        {
            _logger.Info($"[CheckAutoTopUp][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<ACLinkTopUpModel>();
            var rtnModel = new ACLinkTopUpModel { MID = request.MID, Amount = 0 };
            result.SetSuccess(rtnModel);

            // 檢查自動儲值功能是否維護中
            if (!CheckAutoTopUpFunctionIsOpen())
            {
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
            var queryResult = this.GetAutoTopUpInfo(request.MID);
            if (!queryResult.IsSuccess)
            {
                result.SetError(queryResult);
                return result;
            }

            // 計算自動儲值金額
            var getTopUpAmount = this.GetAutoTopUpAmount(queryResult.RtnData, request.MID, request.Amount);
            if (!getTopUpAmount.IsSuccess)
            {
                result.SetError(getTopUpAmount);
                return result;
            }
            result.RtnData = getTopUpAmount.RtnData;

            _logger.Info($"[CheckAutoTopUp][Output] {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 計算需自動儲值的金額
        /// </summary>
        /// <param name="autoTopUp"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public DataResult<ACLinkTopUpModel> GetAutoTopUpAmount(AutoTopUpModel autoTopUp, long mid, int Amount)
        {
            var result = new DataResult<ACLinkTopUpModel>();
            result.SetError();
            int autoTopUpAmount = 0;

            _logger.Trace($"計算需自動儲值的金額: AutoTopUpModel:{JsonConvert.SerializeObject(autoTopUp)}, Amount:{Amount}");

            if (autoTopUp.TopUpSwitch == 1)
            {
                // 取得會員儲值帳戶資訊
                TopUpLimitModel accountModel = _topUpService.GetMemberTopUpInfo(mid);
                int accountAmount = accountModel.TopUpAmt;

                // 檢查餘額是否夠付款
                if (accountAmount < Amount)
                {
                    // 計算自動儲值金額
                    if (autoTopUp.TopUpMode == 1)
                    {
                        // 差額儲值
                        autoTopUpAmount = Amount - accountAmount;
                    }
                    else if (autoTopUp.TopUpMode == 2)
                    {
                        // 定額儲值
                        int unit = autoTopUp.TopUpUnit;             //定額儲值金額單位
                        int tempAmount = Amount - accountAmount;    //不足的金額
                        int x = tempAmount / unit;                  //需儲值的倍數
                        if (tempAmount - (x * unit) > 0)
                        {
                            x++;
                        }
                        autoTopUpAmount = x * unit;
                    }

                    _logger.Trace($"計算自動儲值金額: autoTopUpAmount:{autoTopUpAmount}, TopUpMode:{autoTopUp.TopUpMode}");

                    // 檢查是否超過儲值上限
                    int limitAmount = autoTopUp.LimitAmount;
                    int dailyLimitAmount = autoTopUp.DailyLimitAmount;
                    if (autoTopUpAmount > limitAmount || autoTopUpAmount > dailyLimitAmount)
                    {
                        _logger.Info($"超過儲值上限: 自動儲值金額:{autoTopUpAmount}, 單筆儲值上限:{limitAmount}, 單日儲值上限:{dailyLimitAmount}");
                        autoTopUpAmount = 0;
                    }
                    else
                    {
                        // 取得當日已儲值金額
                        int dailyTopUpAmount = _topUpRepository.GetTodayAutoTopUpAmount(mid);
                        if (dailyTopUpAmount + autoTopUpAmount > dailyLimitAmount)
                        {
                            _logger.Info($"超過單日儲值上限: 自動儲值金額:{autoTopUpAmount}, 當日已儲值金額:{dailyTopUpAmount}, 單日儲值上限:{dailyLimitAmount}");
                            autoTopUpAmount = 0;
                        }
                    }
                }
            }

            ACLinkTopUpModel rtnModel = new ACLinkTopUpModel
            {
                MID = mid,
                AccountID = autoTopUp.AccountID,
                BankCode = autoTopUp.BankCode,
                Amount = autoTopUpAmount
            };

            result.SetSuccess(rtnModel);

            _logger.Trace($"取得需自動儲值的金額: AutoTopUpModel:{JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 取得四碼隨機碼
        /// </summary>
        /// <returns></returns>
        public string GetRandomNumber()
        {
            Random random = null;
            random = new Random(Guid.NewGuid().GetHashCode());

            return string.Format("{0:0000}", random.Next(0, 9999));
        }

        /// <summary>
        /// 取得CheckMacValue
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public string GetMacValue(object objData)
        {
            string macValue = _paymentCommonService.GenerateCheckMacValue(objData, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            return macValue;
        }

        /// <summary>
        /// 檢查指定功能是否啟用中
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool CheckFunctionIsOpen(byte functionID)
        {
            var isOpen = false;
            string appName = "icp";

            // 取得開關設定
            List<AppFunctionSwitch> functionList = _libAdminService.ListAppFunctionSwitch(appName, functionID);
            int cnt = functionList.Where(x => x.Status == 1).Count();

            if (cnt > 0)
            {
                isOpen = true;
            }

            return isOpen;
        }

        /// <summary>
        /// 檢查儲值功能是否啟用中
        /// </summary>
        /// <returns></returns>
        public bool CheckTopUpFunctionIsOpen()
        {
            return CheckFunctionIsOpen(2);
        }

        /// <summary>
        /// 檢查虛擬帳號儲值功能是否啟用中
        /// </summary>
        /// <returns></returns>
        public bool CheckATMTopUpFunctionIsOpen()
        {
            return CheckFunctionIsOpen(3);
        }

        /// <summary>
        /// 檢查自動儲值功能是否啟用中
        /// </summary>
        /// <returns></returns>
        public bool CheckAutoTopUpFunctionIsOpen()
        {
            return CheckFunctionIsOpen(7);
        }
        #endregion
    }
}
