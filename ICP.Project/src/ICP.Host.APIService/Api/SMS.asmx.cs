using System.Web.Services;

namespace ICP.Host.APIService.Api
{
    using Commands;
    using CommonServiceLocator;
    using ICP.Infrastructure.Core.Models;
    using Models;

    /// <summary>
    ///SendSMS 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class SMS : System.Web.Services.WebService
    {
        private readonly SMSCommand _smsCommand = null;
        private readonly FETCommand _fetCommand = null;
        private readonly MistakeCommand _mistakecommand = null;

        public SMS()
        {
            _smsCommand = ServiceLocator.Current.GetInstance<SMSCommand>();
            _fetCommand = ServiceLocator.Current.GetInstance<FETCommand>();
            _mistakecommand = ServiceLocator.Current.GetInstance<MistakeCommand>();
        }

        [WebMethod]
        public BaseResult SendSMS(string Phone, string MsgData, byte SMSType, string Sender = null)
        {
            return _smsCommand.SendSMS(Phone, MsgData, SMSType, Sender);
        }

        /// <summary>
        /// 接收簡訊發送結果(遠傳)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult AddFETRtnInfo(FETRtnModel model)
        {
            return _fetCommand.AddFETRtnInfo(model);
        }

        /// <summary>
        /// 接收簡訊發送結果(三竹)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult AddMistakeInfo(MistakeRtnModel model)
        {
            return _mistakecommand.AddMistakeRtnInfo(model);
        }
    }
}