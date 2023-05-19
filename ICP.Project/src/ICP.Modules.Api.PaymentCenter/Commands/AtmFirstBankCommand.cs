using ICP.Infrastructure.Abstractions.EmailSender;
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
    public class AtmFirstBankCommand
    {
        private readonly AtmFirstBankService _atmFirstBankService = null;
        private readonly IEmailSender _emailSender;

        public AtmFirstBankCommand(AtmFirstBankService atmFirstBankService, IEmailSender emailSender)
        {
            _atmFirstBankService = atmFirstBankService;
            _emailSender = emailSender;
        }

        /// <summary>
        /// 銷帳處理
        /// </summary>
        /// <param name="atmFirstBankWriteOffDataReq"></param>
        /// <returns></returns>
        public BaseResult WriteOff(AtmFirstBankWriteOffDataReq atmFirstBankWriteOffDataReq)
        {
            var baseResult = new BaseResult() { RtnCode = 0 };
            string firstRtnCode = string.Empty, realRtnCode = string.Empty, errorMessage = string.Empty;

            List<string> writeOffContents = _atmFirstBankService.GetWriteOffContents(atmFirstBankWriteOffDataReq);
            foreach (string lineContent in writeOffContents)
            {
                if (lineContent.Length == 91)
                {
                    AtmFirstBankWriteOffData atmFirstBankWriteOffData = _atmFirstBankService.TransferToWriteOffData(lineContent);
                    if (atmFirstBankWriteOffData.VirtualAccount.Equals(new string('0', 16)))
                    {
                        // 實體帳號做提款或存款的交易，不做任何處理，但仍需於回覆正常訊息代碼 0000
                        firstRtnCode = "0000";
                    }
                    else
                    {
                        TradeInfoAtm tradeInfoAtm = _atmFirstBankService.UpdatePaymentCenterAtmTrade(atmFirstBankWriteOffData);
                        if (tradeInfoAtm == null || tradeInfoAtm.RtnCode != 1)  // 回傳商店前，銷帳失敗之處理
                        {
                            TradeInfo tradeInfo = _atmFirstBankService.GetTradeInfoByWriteOffData(atmFirstBankWriteOffData);
                            if (tradeInfo != null)
                            {
                                tradeInfo.RtnCode = tradeInfoAtm.RtnCode;
                                tradeInfo.RtnMsg = tradeInfoAtm.RtnMsg;
                                _atmFirstBankService.AddWriteOffErrorLog(tradeInfo);
                            }
                        }

                        if (tradeInfoAtm != null)
                        {
                            baseResult = _atmFirstBankService.UpdatePaymentAtmTrade(tradeInfoAtm);
                        }

                        // todo 寫 log


                        // 要回傳給第一銀行的訊息
                        firstRtnCode = (baseResult.RtnCode == 1 ? "0000" : baseResult.RtnCode.ToString());

                        // 記錄銷帳錯誤的虛擬帳號
                        if (!firstRtnCode.Equals("0000"))
                        {
                            errorMessage += $"\r\n[ATM][FIRST][{DateTime.Now:yyyyMMdd}] 虛擬帳號 {atmFirstBankWriteOffData.VirtualAccount} 銷帳失敗。";
                        }
                    }
                }
                else
                {
                    firstRtnCode = "9999";
                }

                // 記錄第一筆 RtnCode
                realRtnCode = string.IsNullOrEmpty(firstRtnCode) ? realRtnCode : firstRtnCode;
            }

            // 當有錯誤發送郵件
            if (!string.IsNullOrEmpty(errorMessage))
            {
                //_emailSender.SendMail("愛金卡錯誤信收件人", "[ATM][FIRST]虛擬帳號銷帳失敗通知", errorMessage);
            }

            // 僅回傳第一筆 RtnCode ，其餘郵件通知
            return new BaseResult() { RtnMsg = realRtnCode };
        }
    }
}
