using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class MemberIDNOCommand
    {
        MemberIDNOService _memberIDNOService;
        MemberAuthService _memberAuthService;
        LibMemberAuthService _libMemberAuthService;
        LibPersonalAuthService _libPersonalAuthService;
        LibMemberInfoService _libMemberInfoService;

        public MemberIDNOCommand(
            MemberIDNOService memberIDNOService,
            MemberAuthService memberAuthService,
            LibMemberAuthService libMemberAuthService,
            LibPersonalAuthService libPersonalAuthService,
            LibMemberInfoService libMemberInfoService
            )
        {
            _memberIDNOService = memberIDNOService;
            _memberAuthService = memberAuthService;
            _libMemberAuthService = libMemberAuthService;
            _libPersonalAuthService = libPersonalAuthService;
            _libMemberInfoService = libMemberInfoService;
        }

        #region 取得身分驗證列表
        /// <summary>
        /// 取得身分驗證列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<List<MemberAuthIDNO>> ListAuthMemberIDNO(QueryMemberIDNOVM model)
        {
            var result = new DataResult<List<MemberAuthIDNO>>();
            result.SetError();

            DateTime startDate = Convert.ToDateTime(model.StartDate);
            DateTime endDate = Convert.ToDateTime(model.EndDate);

            if (!string.IsNullOrWhiteSpace(model.CName))
            {
                var verifyResult = _libMemberAuthService.IsValidateCName(model.CName);
                if (!verifyResult.IsSuccess)
                {
                    result.SetError(verifyResult);
                    return result;
                }
            }

            byte? authStatus = null;
            byte? paperAuthStatus = null;
            switch (model.Status)
            {
                case AuthStatusType.NoneAuth:
                    authStatus = 0;
                    paperAuthStatus = 0;
                    break;
                case AuthStatusType.PaperPass:
                    paperAuthStatus = 1;
                    break;
                case AuthStatusType.PaperFail:
                    paperAuthStatus = 2;
                    break;
                case AuthStatusType.AuthPass:
                    authStatus = 1;
                    break;
                case AuthStatusType.AuthFail:
                    authStatus = 2;
                    break;
                case AuthStatusType.All:
                default:
                    break;
            }

            var query = new QueryMemberIDNO
            {
                StartDate = startDate,
                EndDate = endDate,
                AuthStatus = authStatus,
                PaperAuthStatus = paperAuthStatus,
                CName = model.CName,
                IDNO = model.IDNO,
                ICPMID = model.ICPMID,
                PageNo = model.PageNo,
                PageSize = model.PageSize
            };

            var list = _memberIDNOService.ListAuthMemberIDNO(query);

            result.SetSuccess(list);
            return result;
        }
        #endregion

        /// <summary>
        /// 修改會員姓名
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="CName"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateCName(long MID, string CName, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();

            var cNameVerifyResult = _libMemberAuthService.IsValidateCName(CName);
            if (!cNameVerifyResult.IsSuccess)
            {
                result.SetError(cNameVerifyResult);
                return result;
            }

            result = _memberIDNOService.UpdateCName(MID, CName, Modifier, RealIP, ProxyIP);
            return result;
        }

        /// <summary>
        /// 取得會員身分驗證資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public EditAuthIDNOVM GetAuthIDNO(long MID)
        {
            var authIdno = _memberIDNOService.GetAuthIDNO(MID);

            var model = new UpdateMemberAuthIDNO
            {
                IDNO = authIdno.IDNO,
                IssueDate = authIdno.IssueDate,
                ObtainType = authIdno.ObtainType.Value,
                IssueLocationID = authIdno.IssueLocationID,
                IsPicture = authIdno.IsPicture,
                FilePath1 = authIdno.FilePath1,
                FilePath2 = authIdno.FilePath2,
                Birthday = authIdno.Birthday
            };

            return new EditAuthIDNOVM
            {
                AuthIDNO = model
            };
        }

        /// <summary>
        /// 更新會員身分驗證資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateAuthIDNO(long MID, EditAuthIDNOVM model, string urlDir, string saveDir, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            var authIdno = model.AuthIDNO;

            var verifyIssueDateResult = _libPersonalAuthService.VerifyIssueDate(authIdno.IssueDate.Value);
            if (!verifyIssueDateResult.IsSuccess)
            {
                result.SetError(verifyIssueDateResult);
                return result;
            }

            if (model.FileUpload1 != null)
            {
                var saveFileResult = _memberAuthService.SaveAuthIdnoImages(1, model.FileUpload1, urlDir, saveDir, authIdno);
                if (!saveFileResult.IsSuccess)
                {
                    result.SetError(saveFileResult);
                    return result;
                }
            }

            if (model.FileUpload2 != null)
            {
                var saveFileResult = _memberAuthService.SaveAuthIdnoImages(2, model.FileUpload2, urlDir, saveDir, authIdno);
                if (!saveFileResult.IsSuccess)
                {
                    result.SetError(saveFileResult);
                    return result;
                }
            }

            return _memberAuthService.UpdateAuthIDNO(MID, authIdno, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 更新文件審核狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="PaperAuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdatePaperAuthStatus(long MID, byte PaperAuthStatus, string Modifier, long RealIP, long ProxyIP)
        {
            return _memberIDNOService.UpdatePaperAuthStatus(MID, PaperAuthStatus, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 身分驗證
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AuthIDNO(long MID, EditAuthIDNOVM model, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            var p33Auth = new P33Auth
            {
                MID = MID,
                IDNO = model.AuthIDNO.IDNO,
                UserName = Modifier,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            var p33AuthResult = _libPersonalAuthService.VerifyP33Auth(p33Auth);
            if (!p33AuthResult.IsSuccess)
            {
                result.RtnMsg = p33AuthResult.RtnMsg;
                return result;
            }

            var p11Auth = new P11Auth
            {
                MID = MID,
                IDNO = model.AuthIDNO.IDNO,
                IssueDate = model.AuthIDNO.IssueDate.Value,
                ObtainType = model.AuthIDNO.ObtainType,
                IsPicture = model.AuthIDNO.IsPicture,
                BirthDay = model.AuthIDNO.Birthday,
                IssueLocationID = model.AuthIDNO.IssueLocationID,
                UserName = Modifier,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            var p11AuthResult = _libPersonalAuthService.VerifyP11Auth(p11Auth);
            if (!p11AuthResult.IsSuccess)
            {
                result.RtnMsg = p11AuthResult.RtnMsg;
                return result;
            }

            result.SetSuccess();
            result.RtnMsg = "驗證成功";

            return result;
        }

        /// <summary>
        /// 更新身分驗證狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="AuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateAuthIDNOStatus(long MID, byte AuthStatus, string Modifier, long RealIP, long ProxyIP)
        {
            var result = _memberIDNOService.UpdateAuthIDNOStatus(MID, AuthStatus, Modifier, RealIP, ProxyIP);

            if (AuthStatus == 1 && result.IsSuccess)
            {
                #region 綁定 OP 帳號
                var memberData = _libMemberInfoService.GetMemberData(MID);
                var getAppTokenResult = _libMemberInfoService.GetAppTokenByMID(MID);
                if (getAppTokenResult.IsSuccess)
                {
                    _libMemberAuthService.CheckOPBind(memberData, getAppTokenResult.RtnData, Source: 1, RealIP: RealIP, ProxyIP: ProxyIP);
                }
                #endregion
            }

            return result;
        }
    }
}
