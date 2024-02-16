using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomSendApi
{
    public class BaseCustomSendApiRequest
    {
        private string _TimeStamp;

        /// <summary>
        /// 呼叫時間
        /// </summary>
        public string TimeStamp
        {
            get
            {
                if (_TimeStamp == null)
                {
                    _TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                }
                return _TimeStamp;
            }
        }
    }
}
