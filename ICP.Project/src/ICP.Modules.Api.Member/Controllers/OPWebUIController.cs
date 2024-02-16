using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    using Infrastructure.Abstractions.Authorization;
    using Infrastructure.Abstractions.FilterProxy;
    using Infrastructure.Core.Web.Attributes;
    using Infrastructure.Core.Web.Controllers;
    using Library.Models.OpenWalletApi.WebUIApi;
    using Library.Repositories.MemberRepositories;
    using Repositories;
    using Commands;
    using ICP.Infrastructure.Core.Models;
    //Z:\綠界專案開發部\專案相關\2019愛金卡專案\開發用\外部服務串接規格\OPEN POINT\愛金卡_官網API介面一覽_20190329.xlsx
    [ActionFilterProxy(ProxyType = ProxyType.OPWebUIApi)]
    [LogRequest(LogTextResponse = true)]
    public class OPWebUIController : BaseApiController
    {
        private readonly IUserManager _userManager = null;
        OPWebUICommand _oPWebUICommand;
        MemberConfigRepository _memberConfigRepository;

        public OPWebUIController(
            IAuthorizationFactory authorizationFactory,
            OPWebUICommand oPWebUICommand,
            ConfigRepository configRepository,
            MemberConfigRepository memberConfigRepository
            )
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _oPWebUICommand = oPWebUICommand;
            _memberConfigRepository = memberConfigRepository;
        }

        protected override ActionResult AppResult(DataResult obj)
        {
            var rtnData = (BaseWebUIApiResult)obj.RtnData;

            if (rtnData == null)
            {
                rtnData = new BaseWebUIApiResult();
            }

            if (string.IsNullOrWhiteSpace(rtnData.StatusCode))
            {
                if (obj.IsSuccess)
                {
                    rtnData.StatusCode = "00";
                }
                else
                {
                    rtnData.StatusCode = obj.RtnCode.ToString().PadLeft(4, '0');
                }

                rtnData.StatusMessage = obj.RtnMsg;
            }

            return Json(rtnData);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginWebUIRequest request)
        {
            var result = _oPWebUICommand.Login(request, RealIP, ProxyIP);

            return AppResult(result);
        }

        [HttpPost]
        public ActionResult GetUserData(GetUserDataWebUIRequest request)
        {
            long MID = _userManager.MID;

            var result = _oPWebUICommand.GetUserData(MID);

            return AppResult(result);
        }

        [HttpPost]
        public ActionResult AgreeRegister(AgreeRegisterWebUIRequest request)
        {
            long MID = _userManager.MID;

            string urlDir = _memberConfigRepository.Path_TeenagersLegalDetail.TrimEnd('/') + $"/{DateTime.Today.ToString("yyyyMM")}";

            string saveDir = Server.MapPath(urlDir);

            var result = _oPWebUICommand.AgreeRegister(MID, request, urlDir, saveDir, RealIP, ProxyIP);

            return AppResult(result);
        }
    }
}
