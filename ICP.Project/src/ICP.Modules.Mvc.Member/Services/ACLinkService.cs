using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Member.Models.ACLink;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Mvc.Member.Services
{
    public class ACLinkService
    {
        private readonly ILogger _logger = null;

        public ACLinkService(ILogger<ACLinkService> logger)
        {
            _logger = logger;
        }

        #region 取得連結帳號綁定回傳資料
        /// <summary>
        /// 取得連結帳號綁定回傳資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<ACLinkResultModel> GetACLinkResult(object obj)
        {
            var result = new DataResult<ACLinkResultModel>();
            result.SetError();

            _logger.Trace($"準備組成連結帳號綁定回傳資料: {JsonConvert.SerializeObject(obj)}");

            dynamic iData = obj;
            result.RtnCode = iData.RtnCode;
            result.RtnMsg = iData.RtnMsg;
            result.RtnData.MsgNo = iData.RtnData.MsgNo;
            result.RtnData.BankCode = iData.RtnData.BankCode;

            _logger.Trace($"成功組成回傳資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 中國信託
        /// <summary>
        /// 取得連結帳號綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindModel> ACLinkBindPostData(ACLinkBindModel model)
        {
            var result = new DataResult<ACLinkBindModel>();
            result.SetError();

            _logger.Trace($"準備組成連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            model.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            model.BindFlag = "Y";
            model.BankCode = "822";

            ACLinkBindModel rtnModel = new ACLinkBindModel
            {
                MID = model.MID,
                IDNO = model.IDNO,
                BankAccount = model.BankAccount,
                Birth = model.Birth,
                TimeStamp = model.TimeStamp,
                AuthId = model.AuthId,
                Otp = model.Otp,
                BindFlag = model.BindFlag,
                BankCode = model.BankCode,
                AgreeTime = model.AgreeTime
            };

            result.SetSuccess(rtnModel);

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得中國信託指定的回應訊息
        /// </summary>
        /// <param name="rtnCode"></param>
        /// <returns></returns>
        public string GetChinaTrustRtnMsg(string rtnCode)
        {
            string rtnMsg = string.Empty;

            switch (rtnCode)
            {
                case "0000":    //成功
                    rtnMsg = "您已綁定中國信託銀行約定連結存款帳戶,交易時可使用綁定的銀行帳戶進行付款。";
                    break;
                case "8200":    //未留存EMAIL
                    rtnMsg = "您尚未在中國信託銀行留存email，請立即登入網路銀行設定或洽客服協助。";
                    break;
                case "8201":    //未申請OTP
                    rtnMsg = "您尚未申請中國信託OTP(一次性交易密碼)功能，請持金融卡至中國信託ATM(網路ATM)或持身分證及原留印鑑親洽任一分行申請。" +
                             "提醒您：若您持有兩張(含)以上中國信託金融卡，需親洽分行申請。";
                    break;
                case "9007":    //網路銀行功能非正常可使用狀態
                    rtnMsg = "您的中國信託網路銀行非有效狀態，需重新申請，請持金融卡至中國信託ATM(網路ATM)或持身分證及原留印鑑親洽任一分行申請。";
                    break;
                case "9105":    //帳戶狀態異常
                    rtnMsg = "帳戶狀態異常，請洽中國信託銀行客服協助。";
                    break;
                case "9464":    //未申請網銀
                    rtnMsg = "您尚未申請中國信託網路銀行功能，請持金融卡至中國信託ATM(網路ATM)或持身分證及原留印鑑親洽任一分行申請。";
                    break;
                case "9639":    //外國人未於本行留存統一證號
                    rtnMsg = "您尚未於中國信託銀行留存統一證號，無法設定此功能，請您本人攜帶居留證或統一證號基資表至任一分行辦理申請，謝謝。";
                    break;
                case "9940":    //OTP狀態有誤
                    rtnMsg = "您的OTP(一次性交易密碼)功能非有效狀態，請您本人攜帶身分證及原留印鑑至中國信託任一分行辦理。";
                    break;
                case "9234":    //支存戶無法設定
                    rtnMsg = "您輸入的帳號為支票存款帳戶，無法申請此服務，請選擇以其他帳號申請。";
                    break;
                case "9560":    //暫時無法受理此客戶交易
                    rtnMsg = "目前無法受理您的交易，請洽中國信託銀行客服協助。";
                    break;
                default:
                    rtnMsg = string.Format("({0})綁定異常，請洽客服協助。", rtnCode);
                    break;
            }

            return rtnMsg;
        }
        #endregion
    }
}
