using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Commands
{
    public class AtmCommand
    {
        private readonly QueryService _queryService = null;
        private readonly AtmFirstBankService _atmFirstBankService = null;
        private readonly ATMPaymentService _atmPaymentService = null;

        public AtmCommand(QueryService queryService, AtmFirstBankService atmFirstBankService, ATMPaymentService atmPaymentService)
        {
            _queryService = queryService;
            _atmFirstBankService = atmFirstBankService;
            _atmPaymentService = atmPaymentService;
        }

        /// <summary>
        /// 第一銀行取消轉帳儲值
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        public BaseResult FirstBankCancelTopUp(string bankCode, string virtualAccount)
        {
            TradeInfo tradeInfo = _queryService.GetTradeInfoByVirtualAccount(virtualAccount);
            if (tradeInfo == null)
            {
                return new BaseResult { RtnCode = 0, RtnMsg = "查無ATM轉帳儲值資料" };
            }

            TradeInfoAtm tradeInfoAtm = _queryService.GetAtmTradeInfo(tradeInfo.MerchantID, tradeInfo.MerchantTradeNo);
            if (tradeInfoAtm == null)
            {
                return new BaseResult { RtnCode = 0, RtnMsg = "查無ATM轉帳交易資料" };
            }

            if (!bankCode.Equals(tradeInfoAtm.BankCode))
            {
                return new BaseResult { RtnCode = 0, RtnMsg = "ATM銀行代碼錯誤" };
            }

            NotifyBankResult<CardPayRegRes> notifyBankResult = _atmFirstBankService.NotifyBank(tradeInfoAtm) as NotifyBankResult<CardPayRegRes>;
            if (notifyBankResult.RtnCode != 1)
            {
                return new BaseResult { RtnCode = notifyBankResult.RtnCode, RtnMsg = notifyBankResult.RtnMsg };    // Ap to Ap 失敗
            }

            //// 更新 PaymentCenter 的銀行通知狀態
            BaseResult updateCenterNotifyResult = _atmPaymentService.UpdateNotifyBankStatus(notifyBankResult.RtnData.InAccountNo, (notifyBankResult.IsSuccess ? 3 : 4), notifyBankResult.RtnData.TxnDateTime);
            if (updateCenterNotifyResult.RtnCode != 1)
            {
                return new BaseResult() { RtnCode = 0, RtnMsg = "更新 PaymentCenter 銀行通知狀態失敗" };
            }

            //// 更新 Payment 的銀行通知狀態
            BaseResult updatePaymentNotifyResult = _atmPaymentService.UpdatePaymentNotifyBankStatus(notifyBankResult.RtnData.InAccountNo, (notifyBankResult.IsSuccess ? 3 : 4), notifyBankResult.RtnData.TxnDateTime);
            if (updatePaymentNotifyResult.RtnCode != 1)
            {
                return new BaseResult() { RtnCode = 0, RtnMsg = "更新 Payment 銀行通知狀態失敗" };
            }

            // 1. 寫 log
            // 2. 寄 mail

            return updatePaymentNotifyResult;
        }
    }
}
