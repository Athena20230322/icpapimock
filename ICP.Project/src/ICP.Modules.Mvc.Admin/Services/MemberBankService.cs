using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Helpers;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class MemberBankService
    {
        MemberBankRepository _memberBankRepository;
        Library.Repositories.MemberRepositories.MemberBankRepository _libMemberBankRepository;
        SaveFileHelper _saveFileHelper;
        ILogger<MemberBankService> _logger;

        public MemberBankService(
            MemberBankRepository memberBankRepository,
            Library.Repositories.MemberRepositories.MemberBankRepository libMemberBankRepository,
            ILogger<MemberBankService> logger
            )
        {
            _memberBankRepository = memberBankRepository;
            _libMemberBankRepository = libMemberBankRepository;
            _saveFileHelper = new SaveFileHelper();
            _logger = logger;
        }

        /// <summary>
        /// 取得銀行帳號列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberBankAccount> ListAuthMemberBankAccount(QueryMemberBankAccount model)
        {
            return _memberBankRepository.ListAuthMemberBankAccount(model);
        }

        /// <summary>
        /// 更新會員銀行帳號資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateBankAccount(long AccountID, UpdateMemberBankAccount model, string Modifier)
        {
            return _memberBankRepository.UpdateBankAccount(AccountID, model, Modifier);
        }

        /// <summary>
        /// 儲存銀行存摺封面
        /// </summary>
        /// <param name="UploadType">上傳類別</param>
        /// <param name="file">上傳檔案</param>
        /// <param name="urlDir">網址根目錄</param>
        /// <param name="saveDir">實體根目錄</param>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public BaseResult SaveBankAccountImages(byte UploadType, HttpPostedFileBase file, string urlDir, string saveDir, UpdateMemberBankAccount model)
        {
            var result = new BaseResult();
            result.SetError();

            Expression<Func<UpdateMemberBankAccount, string>> memberLamda1 = null;

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
                _logger.Error(ex, "儲存銀行存摺封面");
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新文件審核狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="PaperAuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdatePaperAuthStatus(long AccountID, byte PaperAuthStatus, string Modifier)
        {
            return _memberBankRepository.UpdatePaperAuthStatus(AccountID, PaperAuthStatus, Modifier);
        }

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <returns></returns>
        public List<Library.Models.MemberModels.MemberBankDetail> ListBankDetail()
        {
            return _libMemberBankRepository.ListBankDetail();
        }
    }
}
