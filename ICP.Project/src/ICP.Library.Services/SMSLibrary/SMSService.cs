using ICP.Library.Services.SMS;

namespace ICP.Library.Services.SMSLibrary
{
    public class SMSService
    {
        SMSSoapClient _sMSSoapClient;

        public SMSService(SMSSoapClient sMsSoapClient)
        {
            _sMSSoapClient = sMsSoapClient;
        }

        /// <summary>
        /// 發送簡訊
        /// </summary>
        /// <param name="Phone">手機號碼</param>
        /// <param name="MsgData">簡訊訊息</param>
        /// <param name="SmsType">0:國內; 1:國外</param>
        /// <param name="Sender">發送者</param>
        /// <returns></returns>
        public BaseResult SendSMS(string Phone, string MsgData, byte SmsType, string Sender = null)
        {
            var sendResult = _sMSSoapClient.SendSMS(Phone, MsgData, SmsType, Sender);

            return new BaseResult
            {
                RtnCode = sendResult.RtnCode,
                RtnMsg = sendResult.RtnMsg
            };
        }
    }
}