using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ICP.Host.APIService.Commands;

namespace ICP.Host.Middleware.SMS.Api
{
    using CommonServiceLocator;
    using Infrastructure.Core.Models;
    using Models;

    /// <summary>
    ///MistakeSMS 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class MistakeSMS : WebService
    {
        private readonly MistakeCommand _mistakecommand;
        public MistakeSMS()
        {
            _mistakecommand = ServiceLocator.Current.GetInstance<MistakeCommand>();
        }

        /// <summary>
        /// 取得待發送簡訊
        /// </summary>
        /// <param name="States"></param>
        /// <param name="ChangeStates"></param>
        /// <returns></returns>
        [WebMethod]
        public List<MistakeTemp> ListMistakeTemp(byte States, byte ChangeStates)
        {
            return _mistakecommand.ListMistakeTemp(States, ChangeStates);
        }

        /// <summary>
        /// 更新三竹簡訊發送狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="RtnCode"></param>
        /// <param name="RtnMsg"></param>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult UpdateReceiveSMS(long AutoID, string RtnCode, string RtnMsg, string MessageId)
        {
            return _mistakecommand.UpdateReceiveSMS(AutoID, RtnCode, RtnMsg, MessageId);
        }

        /// <summary>
        /// 產生三竹簡訊UrlBody
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="GUID"></param>
        /// <param name="MsgData"></param>
        /// <returns></returns>
        [WebMethod]
        public string MistakeUrlBody(string Phone, string GUID, string MsgData)
        {
            return _mistakecommand.MistakeUrlBody(Phone, GUID, MsgData);
        }

        /// <summary>
        /// 把三竹回傳資料更新至簡訊狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult StrToModel(long AutoID, string data)
        {
            return _mistakecommand.StrToModel(AutoID, data);
        }
    }
}
