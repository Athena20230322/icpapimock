using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.TopUp;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.ACLink;
using ICP.Modules.Api.Payment.Models.TopUp;
using ICP.Modules.Api.Payment.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;

namespace ICP.Modules.Api.Payment.Services
{
    public class AccountLinkService
    {
        private readonly ACLinkRepository _acLinkRepository = null;
        private readonly TopUpService _topUpService = null;
        private readonly ILogger _logger = null;

        public AccountLinkService(
            ACLinkRepository acLinkRepository,
            TopUpService topUpService,
            ILogger<AccountLinkService> logger)
        {
            _acLinkRepository = acLinkRepository;
            _topUpService = topUpService;
            _logger = logger;
        }
        
        #region 用AccountLink儲值帳戶金額
        /// <summary>
        /// 驗證AccountLink儲值邏輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkTopUpModel> ValidateACLinkTopUp(ACLinkTopUpModel model)
        {
            var result = new DataResult<ACLinkTopUpModel>();

            _logger.Trace($"開始驗證AccountLink儲值邏輯: MID:{model.MID}, Amount:{model.Amount}");

            // 驗證儲值金額是否超過儲值額度
            var checkResult = _topUpService.CheckTopUpLimit(model.MID, model.Amount);
            if (!checkResult.IsSuccess)
            {
                result.SetError(checkResult);
                _logger.Trace($"欲儲值金額超過可儲值額度: MID:{model.MID}, Amount:{model.Amount}");
                return result;
            }

            // 驗證AccountLink帳號
            ACLinkInfoDbRes acLinkInfo = _acLinkRepository.GetACLinkInfo(model.MID, model.AccountID);
            if (acLinkInfo == null || acLinkInfo.Status != 1)
            {
                result.SetCode(2090, model);
                _logger.Trace($"取得AccountLink綁定資訊失敗: MID:{model.MID}, AccountID:{model.AccountID}, BankCode:{model.BankCode}");
                return result;
            }

            result.SetSuccess(model);

            return result;
        }

        /// <summary>
        /// 組成建立儲值訂單所需資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="tradeAmount"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfoModel> TopUpTradeData(ACLinkTopUpModel model)
        {
            var result = new DataResult<TopUpTradeInfoModel>();
            result.SetError();

            _logger.Trace($"開始組成建立儲值訂單所需資訊: {JsonConvert.SerializeObject(model)}");

            TopUpTradeInfoModel rtnModel = new TopUpTradeInfoModel
            {
                MID = model.MID,
                Amount = model.Amount,
                PaymentTypeID = 2,
                PaymentSubTypeID = GetACLinkBankInfo(model.BankCode).PaymentSubTypeID,
                AccountID = model.AccountID
            };

            result.SetSuccess(rtnModel);

            _logger.Trace($"建立儲值訂單所需資訊: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得儲值結果及組成回傳資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> GetTopUpResult(TopUpTradeInfoModel tradeModel, long accountId)
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

            var rtnData = new
            {
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                IcpTradeNo = tradeModel.TransactionID,
                BankCode = tradeModel.BankCode,
                AccountID = accountId,
                LINKAccount = GetLinkAccount(tradeModel.MID, accountId),
                BankName = tradeModel.BankName,
                BankShortName = tradeModel.BankAppName,
                Amount = tradeModel.Amount,
                TotalCoins = accountModel.TopUpAmt,
                AvailableTopUp = accountModel.AvailableTopUp,
                TopUpLimit = accountModel.TopUpLimit,
                TopUpDate = tradeModel.PaymentDate.ToString("yyyy/MM/dd HH:mm")
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

        #region 共用
        /// <summary>
        /// 取得銀行帳號(只顯示後5碼，前面隱碼)
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public string GetLinkAccount(long mid, long accountId)
        {
            string result = string.Empty;

            result = this.GetBankAccountMask(this.GetBankAccount(mid, accountId));

            return result;
        }

        /// <summary>
        /// 取得銀行帳號
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public string GetBankAccount(long mid, long accountId)
        {
            string result = string.Empty;

            // 取得銀行帳號
            ACLinkInfoDbRes acLinkInfo = _acLinkRepository.GetACLinkInfo(mid, accountId);
            if (acLinkInfo == null || string.IsNullOrWhiteSpace(acLinkInfo.BankAccount))
            {
                _logger.Trace($"取得銀行帳號失敗");
                return result;
            }

            result = acLinkInfo.BankAccount;

            return result;
        }

        /// <summary>
        /// 取得遮蔽後的銀行帳號 (只顯示後5碼)
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public string GetBankAccountMask(string bankAccount)
        {
            string result = string.Empty;

            string temp = "********************";
            int accountLen = bankAccount.Length;
            int showLen = 5;

            if (accountLen <= showLen)
            {
                return result;
            }

            result = string.Format("{0}{1}",
                temp.Substring(accountLen - showLen),
                bankAccount.Substring(accountLen - showLen, showLen));

            return result;
        }

        /// <summary>
        /// 取得會員綁定的AccountLink資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public List<ACLinkModel> ListBindACLink(long mid)
        {
            List<ACLinkModel> listACLinkInfo = new List<ACLinkModel>();

            List<ACLinkInfoDbRes> acLinkList = _acLinkRepository.ListBindACLink(mid);
            foreach(var item in acLinkList)
            {
                item.LINKAccount = !string.IsNullOrWhiteSpace(item.BankAccount) ? this.GetBankAccountMask(item.BankAccount) : null;
                item.BankName = !string.IsNullOrWhiteSpace(item.BankCode) ? this.GetACLinkBankInfo(item.BankCode).BankFullName : null;
                item.BankShortName = !string.IsNullOrWhiteSpace(item.BankCode) ? this.GetACLinkBankInfo(item.BankCode).BankName : null;
            }

            listACLinkInfo = Mapper.Map<List<ACLinkModel>>(acLinkList);

            return listACLinkInfo;
        }

        /// <summary>
        /// 依銀行代碼取得AccountLink銀行相關資訊
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public dynamic GetACLinkBankInfo(string bankCode)
        {
            string shortCName = string.Empty;
            string cName = string.Empty;
            string fullName = string.Empty;
            string eName = string.Empty;
            int paymentSubTypeID = 0;

            switch (bankCode)
            {
                case "007":
                    shortCName = "一銀";
                    cName = "第一銀行";
                    fullName = "第一商業銀行";
                    eName = "FIRST";
                    paymentSubTypeID = 1;
                    break;
                case "822":
                    shortCName = "中信";
                    cName = "中國信託";
                    fullName = "中國信託商業銀行";
                    eName = "CTBC";
                    paymentSubTypeID = 2;
                    break;
                case "013":
                    shortCName = "國泰";
                    cName = "國泰世華";
                    fullName = "國泰世華商業銀行";
                    eName = "CATHAY";
                    paymentSubTypeID = 3;
                    break;
            }

            var result = new
            {
                BankName = cName,
                BankShortName = shortCName,
                BankFullName = fullName,
                BankEName = eName,
                PaymentSubTypeID = paymentSubTypeID
            };

            return result;
        }
        #endregion
                
    }
}
