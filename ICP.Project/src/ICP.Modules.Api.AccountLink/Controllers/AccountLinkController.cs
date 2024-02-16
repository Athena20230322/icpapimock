using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Attributes;
using ICP.Modules.Api.AccountLink.Commands;
using ICP.Modules.Api.AccountLink.Models.Cathay;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Api.AccountLink.Controllers
{
    /// <summary>
    /// AccountLink
    /// </summary>
    public class AccountLinkController : BaseAccountLinkController
    {
        private ACLinkCommand _acLinkCommand = null;
        private readonly ILogger _logger = null;

        public AccountLinkController(
            ACLinkCommand acLinkCommand,
            ILogger<AccountLinkController> logger
            )
        {
            _acLinkCommand = acLinkCommand;
            _logger = logger;
        }

        [HttpPost]
        public ContentResult TestApi(string code, int type = 0, string IDNO = "", string INDTAccount = "", string SerMsgNo = "", int Mid = 1234567890, string RefundTradeNo = "",string RtnData = "")
        {
            string encData = "";
            var json = "";


            try
            {
                #region 各功能測試

                switch (type)
                {
                    case 0://Bind
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",Timestamp:\\\"{3}\\\"}}\",BankType:\"{4}\"}}",
                            Mid,
                            IDNO,
                            "",
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            code);//test
                        break;
                    case 1: //cancel
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",Timestamp:\\\"{3}\\\"}}\",BankType:\"{4}\"}}",
                           Mid,
                           IDNO,
                           INDTAccount,//INDTAccount
                           System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                           code);//test
                        break;
                    case 2:  //bindQuery
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",SerMsgNo:\\\"{5}\\\",Timestamp:\\\"{3}\\\"}}\",BankType:\"{4}\"}}",
                           Mid,
                           IDNO,
                           INDTAccount,
                           System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                           code,
                           SerMsgNo);//SerMsgNo
                        break;
                    case 3: //pay
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",Timestamp:\\\"{3}\\\",TradeNo:\\\"{5}\\\",TradeTime:\\\"{6}\\\",TradeNote:\\\"{7}\\\",TradeAmt:\\\"{8}\\\",TradeID:\\\"{9}\\\"}}\",BankType:\"{4}\"}}",
                           Mid,
                           IDNO,
                           INDTAccount,
                           System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                           code,
                           System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + new System.Random().Next(999).ToString().PadLeft(3,'0'),//TradeNo
                           System.DateTime.Now.ToString("yyyyMMddHHmmss"),//TradeTime
                           "ICash扣",//TradeNote
                           "20",//TradeAmt
                           "123456");//TradeID
                        break;
                    case 4:  //payQuery
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",SerMsgNo:\\\"{5}\\\",Timestamp:\\\"{3}\\\"}}\",BankType:\"{4}\"}}",
                          Mid,
                          IDNO,
                          INDTAccount,
                          System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                          code,
                          SerMsgNo);//SerMsgNo
                        break;
                    case 5: //Deposit
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",Timestamp:\\\"{3}\\\",TradeNo:\\\"{5}\\\",TradeTime:\\\"{6}\\\",TradeNote:\\\"{7}\\\",TradeAmt:\\\"{8}\\\",TradeID:\\\"{9}\\\"}}\",BankType:\"{4}\"}}",
                         Mid,
                         IDNO,
                         INDTAccount,
                         System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                         code,
                         System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + new System.Random().Next(999).ToString().PadLeft(3,'0'),//TradeNo
                         System.DateTime.Now.ToString("yyyyMMddHHmmss"),//TradeTime
                         "ICash儲值",//TradeNote
                         "30",//TradeAmt
                         "123456");//TradeID
                        break;
                    case 6://Refund
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",TradeNo:\\\"{5}\\\",RefundTime:\\\"{6}\\\",RefundNote:\\\"{7}\\\",RefundAmt:\\\"{8}\\\",Timestamp:\\\"{3}\\\"}}\",BankType:\"{4}\"}}",
                       Mid,
                       IDNO,
                       INDTAccount,
                       System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                       code,
                       RefundTradeNo,//TradeNo
                       System.DateTime.Now.ToString("yyyyMMddHHmmss"),//RefundTime
                       "ICash退款",//RefundNote
                       "20");//RefundAmt
                        break;
                    case 7: //Withdrawal
                        json = string.Format("{{Json:\"{{MID:{0},IDNO:\\\"{1}\\\",INDTAccount:\\\"{2}\\\",Timestamp:\\\"{3}\\\",TradeNo:\\\"{5}\\\",Amount:\\\"{6}\\\"}}\",BankType:\"{4}\"}}",
                       Mid,
                       IDNO,
                       INDTAccount,
                       System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                       code,
                       System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + new System.Random().Next(999).ToString().PadLeft(3,'0'),//TradeNo
                       "40");//Amount
                        break;
                }

                #endregion

                _logger.Trace($"[TestApi]json : {json}");

                Infrastructure.Core.Helpers.AesCryptoHelper aesCryptoHelper = new Infrastructure.Core.Helpers.AesCryptoHelper
                {
                    Key = Services.CommonConfigService.ACLinkHashKey,
                    Iv = Services.CommonConfigService.ACLinkHashIV
                };


                if (type == 10)
                {
                    encData = aesCryptoHelper.Decrypt(RtnData);
                }
                else
                {
                    encData = aesCryptoHelper.Encrypt(json);
                }

                _logger.Trace($"[TestApi]encData : {json}");
            }
            catch (System.Exception ex)
            {
                var rtnResult = new BaseResult();
                rtnResult.SetError();

                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return Content(encData);
        }

        /// <summary>
        /// 測試用
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object Test(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.Test(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 綁定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkBind(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkBind(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 綁定申請
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkApply(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkApply(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 綁定結果通知(背景)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public object ACLinkBindResult()
        {
            string code = Request.QueryString["bankCode"];
            _logger.Trace($"ACLinkBindResult: {code}");
            string jsonData = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Request.InputStream))
            {
                jsonData = sr.ReadToEnd();
            }

            if (!int.TryParse(code, out int bankcode))
            {
                _logger.Trace($"未帶銀行代碼(code): {jsonData}");

                return JsonConvert.SerializeObject(new BaseResult().SetCode(7414));
            }

            _acLinkCommand.CreateBank((BankType)bankcode);

            _logger.Trace($"ACLinkBindResult: {jsonData}");
            var result = _acLinkCommand.ACLinkBindResult(jsonData);

            return result;
        }

        /// <summary>
        /// 綁定結果通知(背景)(國泰世華)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public object CathayBindResult(BankBindReplyReq model)
        {
            string jsonData = JsonConvert.SerializeObject(model);

            _acLinkCommand.CreateBank(BankType.Cathay);

            var result = _acLinkCommand.ACLinkBindResult(jsonData);

            //需再回給銀行端
            return result;
        }

        //綁定結果通知(國泰世華) Mock
        public object CathayBindResultMock(string txnseq, string fuseID, string mbrActNo, string mbrIdno)
        {
            string _reqTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string _bnkActNo = $"013{System.DateTime.Now.ToString("yMMddHHmmss")}";
            string _checkData = $"{txnseq}{mbrActNo}{_bnkActNo}{_reqTime}";

            var model = new BankBindReplyReq
            {
                header = new BankHeaderModel
                {
                    msgid = "ALSN001BINDING",
                    sourcechannel = "",
                    returncode = "0000",
                    returndesc = "交易成功(test)",
                    txnseq = txnseq,
                    fuseID = fuseID
                },
                sendMsgTime = _reqTime,
                cooPerAtor = "P003",
                mbrActNo = mbrActNo,
                mbrIdno = mbrIdno,
                bnkActNo = _bnkActNo,
                digestHash = new Infrastructure.Core.Helpers.HashCryptoHelper().HashSha256(_checkData).ToLower()
            };

            string jsonData = JsonConvert.SerializeObject(model);

            _acLinkCommand.CreateBank(BankType.Cathay);

            var result = _acLinkCommand.ACLinkBindResult(jsonData);

            string status = model.header.returncode == "0000" ? "0000" : model.header.returndesc;
            string url = $"{Services.CommonConfigService.BindWebResultUrl}?bankCode={(int)BankType.Cathay:000}&t={status}";

            return Redirect(url);
        }

        /// <summary>
        /// 綁定結果通知(前台)(國泰世華)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object CathayBindWebResult(BankBindReplyWebReq model)
        {
            string status = "綁定異常，請洽客服協助。";

            try
            {
                status = model.returncode == "0000" ? "0000" : model.returnmsg;
            }
            catch { }

            string url = $"{Services.CommonConfigService.BindWebResultUrl}?bankCode={(int)BankType.Cathay:000}&t={status}";

            return Redirect(url);
        }

        /// <summary>
        /// 取消綁定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkCancel(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkCancel(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 綁定狀態查詢
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkQuery(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkBindQuery(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 交易扣款
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkPay(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkPay(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 交易儲值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkDeposit(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkDeposit(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 交易退款
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkRefund(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkRefund(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 交易查詢
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkPayQuery(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkPayQuery(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        /// <summary>
        /// 交易提領
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ACLinkValidate]
        public object ACLinkWithdrawal(string token)
        {
            _acLinkCommand.CreateBank(ACModel.BankType);

            var jsonResult = _acLinkCommand.ACLinkWithdrawal(ACModel.Json);

            if (!jsonResult.TryParseJsonToObj(out DataResult result))
            {
                _logger.Trace($"轉型失敗: {jsonResult}");
                result.SetError();
            }

            result.RtnData = new
            {
                ACModel.BankType,
                Json = jsonResult
            };

            return Json(result);
        }

        #region 中國信託
        /// <summary>
        /// 中國信託端取消綁定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChinaTrustCancelBind()
        {
            string jsonData = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Request.InputStream))
            {
                jsonData = sr.ReadToEnd();
            }
            _logger.Trace($"中國信託端取消綁定: {jsonData}");
            jsonData = HttpUtility.UrlDecode(jsonData);

            _acLinkCommand.CreateBank(BankType.ChinaTrust);

            var jsonResult = _acLinkCommand.ChinaTrustCancelBind(jsonData);

            return Content(jsonResult);
        }
        #endregion
    }
}
