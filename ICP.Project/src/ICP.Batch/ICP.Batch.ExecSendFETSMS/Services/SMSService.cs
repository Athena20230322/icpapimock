using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ICP.Batch.ExecSendFETSMS.Services
{
    using FETSMS;
    using Infrastructure.Core.Helpers;
    using Models;

    public class SMSService
    {
        /// <summary>
        /// 取得待發送簡訊
        /// </summary>
        /// <returns></returns>
        public List<FETTemp> ListFetTemp()
        {
            byte states = 2;
            byte changeStates = 3;

            var client = new FETSMSSoapClient();

            return client.ListFetTemp(states, changeStates).ToList();
        }

        /// <summary>
        /// 發送簡訊
        /// </summary>
        /// <param name="temps"></param>
        public void ShortSmsSubmitFET(List<FETTemp> temps)
        {
            var client = new FETSMSSoapClient();

            temps.ForEach(t =>
            {
                //編成 Base64 字串
                byte[] bytes = Encoding.GetEncoding(65001).GetBytes(t.MsgData);
                string smsBody = Convert.ToBase64String(bytes);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(GetTemplateXML());
                doc.SelectSingleNode("ShortSmsSubmitReq/SysId").InnerText = ConfigService.SysId;
                doc.SelectSingleNode("ShortSmsSubmitReq/SrcAddress").InnerText = ConfigService.SrcAddress;
                doc.SelectSingleNode("ShortSmsSubmitReq/DestAddress").InnerText = HttpUtility.UrlEncode(t.Phone, Encoding.UTF8);
                doc.SelectSingleNode("ShortSmsSubmitReq/SmsBody").InnerText = HttpUtility.UrlEncode(smsBody, Encoding.UTF8);
                doc.SelectSingleNode("ShortSmsSubmitReq/ExpiryMinutes").InnerText = ConfigService.ExpiryMinutes;
                doc.SelectSingleNode("ShortSmsSubmitReq/LongSmsFlag").InnerText = ConfigService.LongSmsFlag;
                doc.SelectSingleNode("ShortSmsSubmitReq/FlashFlag").InnerText = ConfigService.FlashFlag;
                doc.SelectSingleNode("ShortSmsSubmitReq/DrFlag").InnerText = ConfigService.DrFlag;
                doc.SelectSingleNode("ShortSmsSubmitReq/FirstFailFlag").InnerText = ConfigService.FirstFailFlag;

                string receiveData = DoRequest(doc.OuterXml);
                if (receiveData != "Post Data Error")
                {
                    SMSModel model = Xml2Model(receiveData);
                    model.AutoID = t.AutoID;

                    BaseResult dbResult = client.UpdateReceiveSMS(model.AutoID, model.RtnCode, model.RtnMsg, model.MessageId);
                }
            });
        }

        /// <summary>
        /// 建立XML模版
        /// </summary>
        /// <returns></returns>
        private string GetTemplateXML()
        {
            string xml = string.Empty;
            XDocument doc = new XDocument();

            doc.Declaration = new XDeclaration("1.0", "utf-8", null);

            XElement ShortSmsSubmitReq = new XElement("ShortSmsSubmitReq");

            //共有的節點
            ShortSmsSubmitReq.Add(new XElement("SysId", ""));
            ShortSmsSubmitReq.Add(new XElement("SrcAddress", ""));
            ShortSmsSubmitReq.Add(new XElement("DestAddress", ""));
            ShortSmsSubmitReq.Add(new XElement("SmsBody", ""));
            ShortSmsSubmitReq.Add(new XElement("ExpiryMinutes", ""));
            ShortSmsSubmitReq.Add(new XElement("LongSmsFlag", ""));
            ShortSmsSubmitReq.Add(new XElement("FlashFlag", ""));
            ShortSmsSubmitReq.Add(new XElement("DrFlag", ""));
            ShortSmsSubmitReq.Add(new XElement("FirstFailFlag", ""));
            doc.Add(ShortSmsSubmitReq);

            return doc.Declaration.ToString() + doc.ToString();
        }

        /// <summary>
        /// 傳送資料至遠傳
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string DoRequest(string data)
        {
            string postUrl = ConfigService.FETSmsSubmitUrl;

            NetworkHelper networkHelper = new NetworkHelper
            {
                DefaultTimeout = ConfigService.WebRequestTimeout
            };

            IDictionary<string, string> formBody = new Dictionary<string, string>();
            formBody.Add("xml", data);

            return networkHelper.DoRequestWithUrlEncode(postUrl, formBody);
        }

        /// <summary>
        /// 遠傳回傳的 XML 轉 Model
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private SMSModel Xml2Model(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            SMSModel model = new SMSModel
            {
                RtnCode = xmlDoc.SelectSingleNode("SubmitRes/ResultCode") == null ? null : HttpUtility.UrlDecode(xmlDoc.SelectSingleNode("SubmitRes//ResultCode").InnerText),
                RtnMsg = xmlDoc.SelectSingleNode("SubmitRes/ResultText") == null ? null : HttpUtility.UrlDecode(xmlDoc.SelectSingleNode("SubmitRes//ResultText").InnerText),
                MessageId = xmlDoc.SelectSingleNode("SubmitRes/MessageId") == null ? null : HttpUtility.UrlDecode(xmlDoc.SelectSingleNode("SubmitRes//MessageId").InnerText)
            };

            return model;
        }
    }
}
