using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using ICP.Library.Models.MemberModels;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Services.AdminServices;
    using Library.Services.MemberServices;
    using Models.MemberModels;
    using Models.ViewModels;
    using Services;
    using System.Web;

    public class MemberTeenagersCommand
    {
        LibMemberTeenagersService _libMemberTeenagersService;
        MemberTeenagersService _memberTeenagersService;
        MemberAuthService _memberAuthService;
        LibMemberInfoService _libMemberInfoService;
        LibAdminService _libAdminService;
        LibPersonalAuthService _libPersonalAuthService;
        LibMemberAuthService _libMemberAuthService;

        public MemberTeenagersCommand(
            LibMemberTeenagersService libMemberTeenagersService,
            MemberTeenagersService memberTeenagersService,
            MemberAuthService memberAuthService,
            LibMemberInfoService libMemberInfoService,
            LibAdminService libAdminService,
            LibPersonalAuthService libPersonalAuthService,
            LibMemberAuthService libMemberAuthService
            )
        {
            _libMemberTeenagersService = libMemberTeenagersService;
            _memberTeenagersService = memberTeenagersService;
            _memberAuthService = memberAuthService;
            _libMemberInfoService = libMemberInfoService;
            _libAdminService = libAdminService;
            _libPersonalAuthService = libPersonalAuthService;
            _libMemberAuthService = libMemberAuthService;
        }

        /// <summary>
        /// 查詢未成年審核資料
        /// </summary>
        /// <param name="query">查詢條件</param>
        /// <param name="queryAuthStatus">
        /// 查詢驗證狀態
        /// 0: 全部
        /// 1: 代理人確認中
        /// 2: 等待審核中
        /// 3: 代理人審核通過
        /// 4: 代理人未審核通過
        /// 5: 身分驗證未通過
        /// </param>
        /// <returns></returns>
        public List<MemberTeenagersQueryResult> ListTeenagers(MemberTeenagersQuery query, byte queryAuthStatus = 0)
        {
            return _memberTeenagersService.ListTeenagers(query, queryAuthStatus);
        }

        /// <summary>
        /// 瀏覽未成年審核資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public DataResult<UpdateTeenagerViewModel> ViewTeenager(long MID)
        {
            var result = new DataResult<UpdateTeenagerViewModel>();
            result.SetError();

            var getTeenagerResult = _libMemberTeenagersService.GetTeenager(MID);
            if (!getTeenagerResult.IsSuccess)
            {
                result.SetError(getTeenagerResult);
                return result;
            }

            var listLegalResult = _libMemberTeenagersService.ListTeenagersLegalDetailByTeenMID(MID);
            if (!listLegalResult.IsSuccess)
            {
                result.SetError(listLegalResult);
                return result;
            }

            var getAuthIDNOResult = _memberAuthService.GetAuthIDNO(MID);
            if (!getAuthIDNOResult.IsSuccess)
            {
                result.SetError(getAuthIDNOResult);
                return result;
            }

            var getMemberDataResult = _libMemberInfoService.GetMemberData(MID);
            var basic = getMemberDataResult.basic;

            var rtnData = new UpdateTeenagerViewModel();
            rtnData.MID = MID;
            rtnData.LegalDetails = listLegalResult.RtnData;
            rtnData.CName = basic.CName;
            rtnData.AuthIDNO = AutoMapper.Mapper.Map<UpdateMemberAuthIDNO>(getAuthIDNOResult.RtnData);
            rtnData.Note = getTeenagerResult.RtnData.Note;

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 更新審核備註
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="Note">審核備註</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagerNote(long MID, string Modifier, string Note, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            var updateNoteResult = _memberTeenagersService.UpdateTeenagerNote(MID, Modifier, Note, RealIP, ProxyIP);
            if (!updateNoteResult.IsSuccess)
            {
                result.SetError(updateNoteResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 修改未成年審核資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="idnoModel">身份證資料</param>
        /// <param name="uniformIDModel">居留證資料</param>
        /// <param name="Note">審核備註</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenager(long MID, string Modifier, UpdateTeenagerModel model, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            if (!model.IsValid())
            {
                result.SetFormatError(model.GetFirstErrorMessage());
                return result;
            }

            string Note = model.Note;
            UpdateMemberAuthIDNO idnoModel = model.AuthIDNO;

            if (idnoModel != null)
            {
                var verifyIssueDateResult = _libPersonalAuthService.VerifyIssueDate(idnoModel.IssueDate.Value);
                if (!verifyIssueDateResult.IsSuccess)
                {
                    result.SetError(verifyIssueDateResult);
                    return result;
                }

                var updateAuthIDNOResult = _memberAuthService.UpdateAuthIDNO(MID, idnoModel, Modifier, RealIP, ProxyIP);
                if (!updateAuthIDNOResult.IsSuccess)
                {
                    result.SetError(updateAuthIDNOResult);
                    return result;
                }
            }

            var updateNoteResult = _memberTeenagersService.UpdateTeenagerNote(MID, Modifier, Note, RealIP, ProxyIP);
            if (!updateNoteResult.IsSuccess)
            {
                result.SetError(updateNoteResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新法定代理人圖檔資料
        /// </summary>
        /// <param name="MID">法定代理人會員編號</param>
        /// <param name="TeenagerMID">未成年會員編號</param>
        /// <param name="UploadType">上傳類別</param>
        /// <param name="file">上傳檔案</param>
        /// <param name="urlDir">網址根目錄</param>
        /// <param name="saveDir">實體根目錄</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagerLegalDetailFile(long MID, long TeenagerMID, byte UploadType, HttpPostedFileBase file, string urlDir, string saveDir, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            var legalModel = new UpdateTeenagersLegalDetailModel();
            var saveFileResult = _memberTeenagersService.SaveTeenagersImages(UploadType, file, urlDir, saveDir, legalModel);
            if (!saveFileResult.IsSuccess)
            {
                result.SetError(saveFileResult);
                return result;
            }

            var updatelegalResult = _memberTeenagersService.UpdateTeenagersLegalDetail(MID, TeenagerMID, legalModel, Modifier, RealIP, ProxyIP);
            if (!updatelegalResult.IsSuccess)
            {
                result.SetError(updatelegalResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新未成年身份證圖檔資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="idnoModel">身份證資料</param>
        /// <param name="uniformIDModel">居留證資料</param>
        /// <param name="Note">審核備註</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagerAuthIDNOFile(long MID, byte UploadType, HttpPostedFileBase file, string urlDir, string saveDir, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            var idnoModel = new UpdateMemberAuthIDNO();
            var saveFileResult = _memberAuthService.SaveAuthIdnoImages(UploadType, file, urlDir, saveDir, idnoModel);
            if (!saveFileResult.IsSuccess)
            {
                result.SetError(saveFileResult);
                return result;
            }

            var updateAuthIDNOResult = _memberAuthService.UpdateAuthIDNO(MID, idnoModel, Modifier, RealIP, ProxyIP);
            if (!updateAuthIDNOResult.IsSuccess)
            {
                result.SetError(updateAuthIDNOResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 審核未成年法代資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="LPAuthStatus">法代資料是否審過 0: 待審 1:審核通過 2: 審核失敗</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLPAuthStatus(long MID, string Modifier, byte LPAuthStatus, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberTeenagersService.UpdateTeenagersLPAuthStatus(MID, Modifier, LPAuthStatus, RealIP, ProxyIP);
        }

        /// <summary>
        /// 審核未成年身份驗證
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="IDNOStatus">身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersIDNOStatus(long MID, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            #region 檢查資料
            var getAuthIDNOResult = _memberAuthService.GetAuthIDNO(MID);
            if (!getAuthIDNOResult.IsSuccess)
            {
                result.SetError(getAuthIDNOResult);
                return result;
            }

            var authIDNO = getAuthIDNOResult.RtnData;
            if (authIDNO.AuthStatus == 1)
            {
                result.SetSuccess();
                return result;
            }
            #endregion

            #region 查功能開關
            string appName = "member";
            var appFunctionSwitchs = _libAdminService.ListAppFunctionSwitch(appName);
            var query = appFunctionSwitchs.Where(t => (t.FunctionID == 1 || t.FunctionID == 2) && t.Status == 0).ToList();
            if (query.Any())
            {
                result.RtnCode = 0;
                result.RtnMsg = "聯徵功能尚未開啟，請聯絡客服";
                return result;
            }
            #endregion

            #region P33 驗證
            var p33Auth = new P33Auth
            {
                MID = MID,
                IDNO = authIDNO.IDNO,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };
            var p33Result = _libPersonalAuthService.AddAuthP33(p33Auth);
            if (!p33Result.IsSuccess)
            {
                result.SetError(p33Result);
                return result;
            }
            #endregion

            #region P11 驗證
            var p11Auth = new P11Auth
            {
                MID = MID,
                IDNO = authIDNO.IDNO,
                IssueDate = authIDNO.IssueDate.Value,
                ObtainType = authIDNO.ObtainType,
                IsPicture = authIDNO.IsPicture,
                BirthDay = authIDNO.BirthDay,
                IssueLocationID = authIDNO.IssueLocationID,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };
            var p11Result = _libPersonalAuthService.AddAuthP11(p11Auth);
            if (!p11Result.IsSuccess)
            {
                result.SetError(p11Result);
                return result;
            }
            #endregion

            #region 更新未成年身份驗證狀態
            byte IDNOStatus;
            if (p11Result.AuthStatus == 1)
            {
                IDNOStatus = 1;
            }
            else
            {
                IDNOStatus = 2;
            }

            var updateResult = _memberTeenagersService.UpdateTeenagersIDNOStatus(MID, Modifier, IDNOStatus, RealIP, ProxyIP);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }
            #endregion

            #region 綁定 OP 帳號
            if (p11Result.AuthStatus == 1 && updateResult.IsSuccess)
            {
                var memberData = _libMemberInfoService.GetMemberData(MID);
                var getAppTokenResult = _libMemberInfoService.GetAppTokenByMID(MID);
                if (getAppTokenResult.IsSuccess)
                {
                    _libMemberAuthService.CheckOPBind(memberData, getAppTokenResult.RtnData, Source: 1, RealIP: RealIP, ProxyIP: ProxyIP);
                }
            }
            #endregion

            result.SetSuccess();
            return result;
        }
    }
}
