using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ICP.Host.APIService.Api
{
  
    using Commands;
    using CommonServiceLocator;
    using ICP.Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;

    /// <summary>
    ///SMS 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class FETSMS : WebService
    {
        private readonly FETCommand _fetCommand = null;

        public FETSMS()
        {
            _fetCommand = ServiceLocator.Current.GetInstance<FETCommand>();
        }

        /// <summary>
        /// 取得待發送簡訊
        /// </summary>
        /// <param name="States"></param>
        /// <param name="ChangeStates"></param>
        /// <returns></returns>
        [WebMethod]
        public List<FETTemp> ListFetTemp(byte States, byte ChangeStates)
        {
            return _fetCommand.ListFetTemp(States, ChangeStates);
        }

        /// <summary>
        /// 更新簡訊發送狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="RtnCode"></param>
        /// <param name="RtnMsg"></param>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult UpdateReceiveSMS(long AutoID, string RtnCode, string RtnMsg, string MessageId)
        {
            return _fetCommand.UpdateReceiveSMS(AutoID, RtnCode, RtnMsg, MessageId);
        }
    }
}
