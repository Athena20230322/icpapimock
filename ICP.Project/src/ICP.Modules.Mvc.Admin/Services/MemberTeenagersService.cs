using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models.MemberModels;
    using Models.ViewModels;
    using ICP.Infrastructure.Core.Web.Helpers;
    using System.Web;
    using System.Linq.Expressions;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Abstractions.Logging;

    public class MemberTeenagersService
    {
        MemberTeenagersRepository _memberTeenagersRepository;
        SaveFileHelper _saveFileHelper;
        ILogger<MemberTeenagersService> _logger;

        public MemberTeenagersService(
            MemberTeenagersRepository memberTeenagersRepository,
            ILogger<MemberTeenagersService> logger
            )
        {
            _memberTeenagersRepository = memberTeenagersRepository;
            _saveFileHelper = new SaveFileHelper();
            _logger = logger;
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
            #region 查詢驗證狀態
            if (queryAuthStatus == 1)  // 1: 代理人確認中
            {
                query.Stage = 0;
                query.LPAgree = 0;
            }
            else if (queryAuthStatus == 2)  // 2: 等待審核中
            {
                query.Stage = 2;
                query.LPAuth = 0;
            }
            else if (queryAuthStatus == 3)  // 3: 代理人審核通過
            {
                query.Stage = 3;
                query.IDNOStatus = 0;
            }
            else if (queryAuthStatus == 4)  // 4: 代理人未審核通過
            {
                query.Stage = 2;
                query.LPAuth = 2;
            }
            else if (queryAuthStatus == 5)  // 5: 身分驗證未通過
            {
                query.Stage = 3;
                query.IDNOStatus = 2;
            }
            #endregion

            return _memberTeenagersRepository.ListTeenagers(query);
        }

        /// <summary>
        /// 修改未成年審核備註
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="Note">審核備註</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagerNote(long MID, string Modifier, string Note, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberTeenagersRepository.UpdateTeenagerNote(MID, Modifier, Note, RealIP, ProxyIP);
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
            return _memberTeenagersRepository.UpdateTeenagersLPAuthStatus(MID, Modifier, LPAuthStatus, RealIP, ProxyIP);
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
        public BaseResult UpdateTeenagersIDNOStatus(long MID, string Modifier, byte IDNOStatus, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberTeenagersRepository.UpdateTeenagersIDNOStatus(MID, Modifier, IDNOStatus, RealIP, ProxyIP);
        }

        /// <summary>
        /// 儲存法定代理人圖片
        /// </summary>
        /// <param name="UploadType">上傳類別</param>
        /// <param name="file">上傳檔案</param>
        /// <param name="urlDir">網址根目錄</param>
        /// <param name="saveDir">實體根目錄</param>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public BaseResult SaveTeenagersImages(byte UploadType, HttpPostedFileBase file, string urlDir, string saveDir, UpdateTeenagersLegalDetailModel model)
        {
            var result = new BaseResult();
            result.SetError();

            Expression<Func<UpdateTeenagersLegalDetailModel, string>> memberLamda1 = null;

            switch (UploadType)
            {
                case 3: memberLamda1 = t => t.IDNOFile1; break;
                case 4: memberLamda1 = t => t.IDNOFile2; break;
                case 5: memberLamda1 = t => t.FilePath1; break;
                case 6: memberLamda1 = t => t.FilePath2; break;
                case 7: memberLamda1 = t => t.FilePath3; break;
                case 8: memberLamda1 = t => t.FilePath4; break;
                case 9: memberLamda1 = t => t.FilePath5; break;
                case 10: memberLamda1 = t => t.FilePath6; break;
                default:
                    result.SetError();
                    return result;
            }

            string extension = System.IO.Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";

            string fileName = $"{Guid.NewGuid()}{extension}";

            try
            {
                _saveFileHelper.SaveFileToModel(urlDir, saveDir, fileName, file, model, memberLamda1);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "儲存法定代理人圖片");
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新法定代理人資料
        /// </summary>
        /// <param name="MID">法定代理人會員編號</param>
        /// <param name="TeenagerMID">未成年會員編號</param>
        /// <param name="model">更新資料</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLegalDetail(long MID, long TeenagerMID, UpdateTeenagersLegalDetailModel model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberTeenagersRepository.UpdateTeenagersLegalDetail(MID, TeenagerMID, model, Modifier, RealIP, ProxyIP);
        }
    }
}
