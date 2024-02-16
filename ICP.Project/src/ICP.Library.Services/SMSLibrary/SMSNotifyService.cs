using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.SMS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ICP.Library.Services.SMSLibrary
{
    using BaseResult = Infrastructure.Core.Models.BaseResult;

    public class SMSNotifyService
    {
        SMSSoapClient _sMSSoapClient;

        public SMSNotifyService(SMSSoapClient sMSSoapClient)
        {
            _sMSSoapClient = sMSSoapClient;
        }


        /// <summary>
        /// 接收遠傳回傳XML
        /// </summary>
        /// <param name="xml"></param>
        public void AddFETRtnInfo(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.Replace("xml=", ""));

            XmlNodeList itemDataList = xmlDoc.SelectNodes("SmsPushDrNreq/Receipt");
            string sysId = xmlDoc.SelectSingleNode("SmsPushDrNreq/SysId")?.InnerText;

            foreach (XmlNode node in itemDataList)
            {
                var smsModel = new FETRtnModel
                {
                    SysId = sysId,
                    MessageId = node["MessageId"] == null ? "" : node["MessageId"].InnerText,
                    DestAddress = node["DestAddress"] == null ? "" : node["DestAddress"].InnerText,
                    DeliveryStatus = node["DeliveryStatus"] == null ? "" : node["DeliveryStatus"].InnerText,
                    ErrorCode = node["ErrorCode"] == null ? "" : node["ErrorCode"].InnerText,
                    SubmitDate = node["SubmitDate"] == null ? "" : node["SubmitDate"].InnerText,
                    DoneDate = node["DoneDate"] == null ? "" : node["DoneDate"].InnerText,
                    Seq = node["Seq"] == null ? "" : node["Seq"].InnerText
                };
                _sMSSoapClient.AddFETRtnInfo(smsModel);
            }
        }

        /// <summary>
        /// 接收三竹回傳字串
        /// </summary>
        /// <param name="data"></param>
        public void AddMistakeRtnInfo(string data)
        {
            string outStr = string.Empty;
            int outInt;

            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>(
                data.Split('&')
                    .Where(s => s.Contains('='))
                    .Select(s => s.Split('='))
                    .ToDictionary(key => key[0].Trim(), value => value[1].Trim()));

            string _statuscode = dict.TryGetValue("statuscode", out outStr) ? dict["statuscode"] : null;
            string _statusflag = dict.TryGetValue("StatusFlag", out outStr) ? dict["StatusFlag"] : null;

            var smsModel = new MistakeRtnModel
            {
                MessageId = dict.TryGetValue("msgid", out outStr) ? dict["msgid"] : null,
                DstAddr = dict.TryGetValue("dstaddr", out outStr) ? dict["dstaddr"] : null,
                Dlvtime = dict.TryGetValue("donetime", out outStr) ? dict["donetime"] : null,
                StatusCode = Int32.TryParse(_statuscode, out outInt) ? Int32.Parse(_statuscode) : 0,
                StatusStr = dict.TryGetValue("statusstr", out outStr) ? dict["statusstr"] : null,
                StatusFlag = Int32.TryParse(_statusflag, out outInt) ? Int32.Parse(_statusflag) : 0,
            };

            _sMSSoapClient.AddMistakeInfo(smsModel);
        }
    }
}