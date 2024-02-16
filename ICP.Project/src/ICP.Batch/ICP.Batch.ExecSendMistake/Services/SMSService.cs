using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Batch.ExecSendMistake.Services
{
    using Infrastructure.Core.Helpers;
    using MistakeSMS;
    using Models;
    using System.Collections.Concurrent;

    public class SMSService
    {
        /// <summary>
        /// 獲取待發送簡訊清單
        /// </summary>
        /// <returns></returns>
        public List<MistakeTemp> ListMistakeTemp()
        {
            byte states = 2;
            byte changeStates = 3;

            var client = new MistakeSMSSoapClient();
            return client.ListMistakeTemp(states, changeStates).ToList();
        }

        /// <summary>
        /// 發送三竹簡訊
        /// </summary>
        /// <param name="temps"></param>
        public void ShortSmsSubmitMistake(List<MistakeTemp> temps)
        {
            var client = new MistakeSMSSoapClient();

            foreach (var item in temps)
            {
                string urlBody = client.MistakeUrlBody(HttpUtility.UrlEncode(item.Phone), item.GUID, item.MsgData);
                string receiveData = DoRequest(urlBody);
                if (receiveData != "Error")
                {
                    client.StrToModel(item.AutoID,receiveData);
                }
            }
        }

        /// <summary>
        /// GET資料至三竹簡訊
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string DoRequest(string url)
        {
            NetworkHelper networkHelper = new NetworkHelper
            {
                DefaultTimeout = ConfigService.WebRequestTimeout
            };
            return networkHelper.DoRequestWithGet(url, null, null);
        }

    }
}
