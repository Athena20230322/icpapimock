using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Commands
{
    public class ReceiveAtmServerReturnCommand
    {
        private readonly AtmService _atmService = null;

        public ReceiveAtmServerReturnCommand(AtmService atmService)
        {
            _atmService = atmService;
        }

        /// <summary>
        /// 處理從 PaymentCenter 接收的資料，並更新 Payment 的訂單
        /// </summary>
        /// <param name="paymentCenterServerReturn"></param>
        /// <returns></returns>
        public BaseResult ProcessReceiveData(PaymentCenterServerReturn paymentCenterServerReturn)
        {
            // todo → 寫接收 log

            TradeInfoAtm tradeInfoAtm = _atmService.GetPaymentCenterServerReturnObjFromJsonData<TradeInfoAtm>(paymentCenterServerReturn);
            if (tradeInfoAtm == null)
            {
                return new BaseResult();
            }

            // todo → 寫還原為物件後的 log


            // 再重新查詢 PaymentCenter的訂單資料，double check 訂單狀態
            TradeInfoAtm tradeInfoAtmFromPaymentCenter = _atmService.GetPaymentCenterAtmTradeInfo(tradeInfoAtm);

            // 更新訂單以重新查詢訂單的資料為主
            BaseResult updateResult = _atmService.UpdateAtmPaymentTrade(tradeInfoAtmFromPaymentCenter);

            return updateResult;
        }

        /// <summary>
        /// 處理從 PaymentCenter 傳來的資料，並更新銀行通知狀態
        /// </summary>
        /// <param name="paymentCenterServerReturn"></param>
        /// <returns></returns>
        public BaseResult ProcessNotifyBankData(PaymentCenterServerReturn paymentCenterServerReturn)
        {
            NotifyBankModel notifyBankModel = _atmService.GetPaymentCenterServerReturnObjFromJsonData<NotifyBankModel>(paymentCenterServerReturn);
            if (notifyBankModel == null)
            {
                return new BaseResult { RtnCode = 0, RtnMsg = "取得 NotifyBankModel 時發生錯誤" };
            }

            return _atmService.UpdateNotifyBankStatus(notifyBankModel);
        }
    }
}
