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
    using Newtonsoft.Json;
    using Services;
    using System.Web;

    public class PersonalAuthCommand
    {
        LibPersonalAuthService _libPersonalAuthService;
        PersonalAuthService _personalAuthService;
        public PersonalAuthCommand(LibPersonalAuthService libPersonalAuthService, PersonalAuthService personalAuthService)
        {
            _libPersonalAuthService = libPersonalAuthService;
            _personalAuthService = personalAuthService;
        }
        /// <summary>
        /// P11驗證(身分驗證)
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="model">查詢物件</param>
        /// <param name="UserName">後台帳號</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        public P11AuthQueryVM P11Auth(P11AuthVM model, string UserName, long RealIP = 0, long ProxyIP = 0, long MID = 0)
        {
            var p11Auth = new P11Auth
            {
                MID = MID,
                IDNO = model.IDNO,
                IssueDate = new DateTime(model.IssueDateYear, model.IssueDateMonth, model.IssueDateDay),
                ObtainType = model.ObtainType,
                IsPicture = model.IsPicture,
                BirthDay = new DateTime(model.BirthdayYear, model.BirthdayMonth, model.BirthdayDay),
                IssueLocationID = model.IssueLocationID,
                UserName = UserName,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            var p11AuthResult = _libPersonalAuthService.VerifyP11Auth(p11Auth);

            var result = new P11AuthQueryVM()
            {
                IsPass = p11AuthResult.IsPass,
                Answer = p11AuthResult.Answer,
                Result = p11AuthResult.Result
            };

            return result;
        }
        /// <summary>
        /// P33驗證
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="model">查詢物件</param>
        /// <param name="Modifier">修改者</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public P33AuthQueryVM P33Auth(P33AuthVM model, string Modifier, long RealIP = 0, long ProxyIP = 0, long MID = 0)
        {
            var p33Auth = new P33Auth
            {
                MID = MID,
                IDNO = (model.IDTypes == 1) ? model.UnifiedBusinessNo : model.IDNO,
                UserName = Modifier,
                Source = 2,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            var p33AuthResult = _libPersonalAuthService.VerifyP33Auth(p33Auth);

            var result = new P33AuthQueryVM()
            {
                IsPass = p33AuthResult.IsPass,
                DataCount = p33AuthResult.DataCount
            };

            return result;
        }
        /// <summary>
        /// P11驗證記錄查詢
        /// </summary>
        /// <param name="model">查詢物件</param>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public List<P11AuthHistoryQueryVM> QueryP11AuthLog(P11AuthHistoryVM model, long? MID = null)
        {
            var dbmodel = new P11AuthHistory()
            {
                EndDate = model.EndDate,
                ICPMID = model.ICPMID,
                IDNO = model.IDNO,
                IsPass = model.IsPass,
                MID = MID,
                PageNo = model.PageNo,
                PageSize = model.PageSize,
                StartDate = model.StartDate
            };

            var dbresults = _personalAuthService.QueryP11AuthLog(dbmodel);
            var results = new List<P11AuthHistoryQueryVM>();
            if (dbresults == null)
                return results;
            foreach (var dbresult in dbresults)
            {
                var result = new P11AuthHistoryQueryVM()
                {
                    Answer = dbresult.Answer,
                    BirthDate = dbresult.BirthDate,
                    CreateDate = dbresult.CreateDate,
                    ICPMID = dbresult.ICPMID,
                    IDNO = dbresult.IDNO,
                    IsPass = dbresult.IsPass,
                    IssueDate = dbresult.IssueDate,
                    IssueLoc = dbresult.IssueLoc,
                    IssueType = dbresult.IssueType,
                    PicFree = dbresult.PicFree,
                    Result = dbresult.Result,
                    Source = dbresult.Source,
                    TotalCount = dbresult.TotalCount,
                    UserName = dbresult.UserName
                };
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// P33驗證記錄查詢
        /// <summary>
        /// <param name="model">查詢物件</param>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public List<P33AuthHistoryQueryVM> QueryP33AuthLog(P33AuthHistoryVM model, long? MID = null)
        {
            var dbmodel = new P33AuthHistory()
            {
                EndDate = model.EndDate,
                ICPMID = model.ICPMID,
                IDNO = (model.IDTypes == 1) ? model.UnifiedBusinessNo : model.IDNO,
                IsPass = model.IsPass,
                MID = MID,
                PageNo = model.PageNo,
                PageSize = model.PageSize,
                StartDate = model.StartDate
            };

            var dbresults = _personalAuthService.QueryP33AuthLog(dbmodel);
            var results = new List<P33AuthHistoryQueryVM>();
            if (dbresults == null)
                return results;
            foreach (var dbresult in dbresults)
            {
                var result = new P33AuthHistoryQueryVM()
                {
                    Answer = dbresult.Answer,
                    CreateDate = dbresult.CreateDate,
                    DataCount = dbresult.DataCount,
                    ICPMID = dbresult.ICPMID,
                    IDNO = dbresult.IDNO,
                    IsPass = dbresult.IsPass,
                    P33AuthDatas = _personalAuthService.DataListJsonStringDeserialize(dbresult.DataList),
                    Result = dbresult.Result,
                    Source = dbresult.Source,
                    TotalCount = dbresult.TotalCount,
                    UserName = dbresult.UserName
                };
                results.Add(result);
            }
            return results;
        }
    }
}
