using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using ICP.Infrastructure.Abstractions.Logging;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using ICP.Infrastructure.Core.Web.Helpers;
    using Models.MemberModels;
    using Repositories;
    using System.Linq.Expressions;
    using System.Web;

    public class MemberAuthService
    {
        MemberAuthRepository _memberAuthRepository;
        SaveFileHelper _saveFileHelper;
        ILogger<MemberAuthService> _logger;

        public MemberAuthService(
            MemberAuthRepository memberAuthRepository,
            ILogger<MemberAuthService> logger
            )
        {
            _memberAuthRepository = memberAuthRepository;
            _saveFileHelper = new SaveFileHelper();
            _logger = logger;
        }

        /// <summary>
        /// 更新身份證換補發資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="model">換補發資料</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateAuthIDNO(long MID, UpdateMemberAuthIDNO model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberAuthRepository.UpdateAuthIDNO(MID, model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 更新居留證資料
        /// </summary>
        /// <param name="model">居留證資料</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateUniformID(UpdateMemberAuthUniformID model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberAuthRepository.UpdateUniformID(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得會員身份證資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public DataResult<MemberAuthIDNOModel> GetAuthIDNO(long MID)
        {
            var result = new DataResult<MemberAuthIDNOModel>();
            result.SetError();

            var rtnData = _memberAuthRepository.GetAuthIDNO(MID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 儲存身分驗證圖片
        /// </summary>
        /// <param name="UploadType">上傳類別</param>
        /// <param name="file">上傳檔案</param>
        /// <param name="urlDir">網址根目錄</param>
        /// <param name="saveDir">實體根目錄</param>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public BaseResult SaveAuthIdnoImages(byte UploadType, HttpPostedFileBase file, string urlDir, string saveDir, UpdateMemberAuthIDNO model)
        {
            var result = new BaseResult();
            result.SetError();

            Expression<Func<UpdateMemberAuthIDNO, string>> memberLamda1 = null;

            switch (UploadType)
            {
                case 1: memberLamda1 = t => t.FilePath1; break;
                case 2: memberLamda1 = t => t.FilePath2; break;
                default:
                    result.SetError();
                    return result;
            }

            string[] contentTypeArray = new string[] { "image/jpeg", "image/gif", "image/pjpeg", "image/png" };
            if (!contentTypeArray.Contains(file.ContentType))
            {
                result.RtnMsg = "檔案格式錯誤，僅接受JPG、PNG圖片檔";
                return result;
            }

            int fileSize = 1024 * 1024 * 5;
            if (file.ContentLength > fileSize)
            {
                result.RtnMsg = "超過檔案限制大小，請重新上傳小於5MB之檔案";
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
                _logger.Error(ex, "儲存身分驗證圖片");
            }

            result.SetSuccess();
            return result;
        }
    }
}
