using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Modules.Api.PaymentCenter.Interface.ATM;
using ICP.Modules.Api.PaymentCenter.Models;
using Newtonsoft.Json;
using System;
using System.Xml;

namespace ICP.Modules.Api.PaymentCenter.Repositories.ATM
{
    public class FirstATMRepository : IATMService
    {
        /// <summary>
        /// 通知銀行(AP to AP)
        /// </summary>
        /// <param name="atmNotifyModel"></param>
        /// <returns></returns>
        public BaseResult NotifyBank(AtmNotifyModel atmNotifyModel)
        {
            // 第一銀行 AP TO AP 交易
            var cardPayRegReq = new CardPayRegReq()
            {
                SendSeqNo = atmNotifyModel.TradeNo,
                MID = "一銀給",
                FunCode = "一銀給",
                Apply = atmNotifyModel.ApplyType,
                DueDate = atmNotifyModel.ExpireDate.ToString("yyyyMMdd"),
                InAccountNo = atmNotifyModel.VirtualAccount,
                Amount = Convert.ToInt32(atmNotifyModel.Amount).ToString(),
                RsURL = string.Empty
            };

            CardPayRegRes cardPayRegRes = CardPayReg(cardPayRegReq);

            return new NotifyBankResult<CardPayRegRes>()
            {
                RtnData = cardPayRegRes,
                RtnCode = cardPayRegRes.RC.Equals("0") ? 1 : Convert.ToInt32(cardPayRegRes.RC),
                RtnMsg = cardPayRegRes.MSG
            };
        }

        /// <summary>
        /// AP to AP 交易
        /// </summary>
        /// <param name="cardPayRegReq"></param>
        /// <returns></returns>
        private CardPayRegRes CardPayReg(CardPayRegReq cardPayRegReq)
        {
            //var postUrl = string.Format("https://{0}.firstbank.com.tw/acq/cardpayreg", "teatm");
            var postUrl = "";
            var ap2apResponse = string.Empty;
            var cardPayRegRes = new CardPayRegRes();

            try
            {
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(cardPayRegReq), "CardPayRegRq");
                string ap2apData = xmlDocument.OuterXml.ToBase64();

                #region 判斷是否測試用
                if (GlobalConfigUtil.Environment.Equals("prod"))
                {
                    ap2apResponse = new NetworkHelper().DoRequest(postUrl, ap2apData, "application/x-www-form-urlencoded", 300, null, null);
                }
                else
                {
                    var testCardPayRegRes = new CardPayRegRes()
                    {
                        RC = "0",
                        MSG = "交易成功",
                        SendSeqNo = cardPayRegReq.SendSeqNo,
                        MID = cardPayRegReq.MID,
                        Apply = cardPayRegReq.Apply,
                        DueDate = cardPayRegReq.DueDate,
                        InAccountNo = cardPayRegReq.InAccountNo,
                        Amount = cardPayRegReq.Amount,
                        TxnDate = "20190603",
                        TxnTime = "185134"
                    };
                    testCardPayRegRes.MAC = testCardPayRegRes.ComputeMacValue();

                    XmlDocument testXmlDocument = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(testCardPayRegRes), "CardPayRegRs");
                    ap2apResponse = testXmlDocument.OuterXml;
                }
                #endregion

                if (!string.IsNullOrEmpty(ap2apResponse))
                {
                    var responseXmlDocument = new XmlDocument();
                    responseXmlDocument.LoadXml(ap2apResponse);

                    XmlNode responseXmlNode = responseXmlDocument.SelectSingleNode("/CardPayRegRs");
                    string xmlNodeJsonText = JsonConvert.SerializeXmlNode(responseXmlNode);
                    XmlTranserToCardPayRegRes xmlTranserToCardPayRegRes = JsonConvert.DeserializeObject<XmlTranserToCardPayRegRes>(xmlNodeJsonText);
                    cardPayRegRes = xmlTranserToCardPayRegRes.CardPayRegRs;
                }
            }
            catch (Exception ex)
            {
                cardPayRegRes.RC = "7550";
                cardPayRegRes.MSG = ex.Message;
                return cardPayRegRes;
            }

            return cardPayRegRes;
        }
    }
}
