using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ICP.Host.Middleware.JCIC.Api
{
    using Commands;
    using CommonServiceLocator;
    using Models;

    /// <summary>
    ///PersonalAuth 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class PersonalAuth : System.Web.Services.WebService
    {
        private readonly PersonalAuthCommand _personalAuthCommand = null;

        public PersonalAuth()
        {
            _personalAuthCommand = ServiceLocator.Current.GetInstance<PersonalAuthCommand>();
        }

        [WebMethod]
        public P33AuthResult P33Auth(P33Auth model)
        {
            var authResult = _personalAuthCommand.ExecP33Auth(model);

            return new P33AuthResult
            {
                RtnCode = authResult.Return_Code,
                RtnMsg = authResult.Return_Msg,
                IsPass = authResult.IsPass,
                DataCount = authResult.DataCount,
                DataList = authResult.DataList
            };
        }

        [WebMethod]
        public P11AuthResult P11Auth(P11Auth model)
        {
            var authResult = _personalAuthCommand.ExecP11Auth(model);

            return new P11AuthResult
            {
                RtnCode = authResult.Return_Code,
                RtnMsg = authResult.Return_Msg,
                IsPass = authResult.IsPass,
                Answer = authResult.Answer,
                Result = authResult.Result
            };
        }
    }
}
