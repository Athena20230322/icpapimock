using ICP.Batch.AccountLink.Models;
using ICP.Batch.AccountLink.Repositories;
using ICP.Library.Models.AccountLinkApi.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ICP.Batch.AccountLink.Services
{
    public class ConfigService
    {
        protected BankType _bankType;
        protected ACLinkRepository _acLinkRepository = null;

        /// <summary>
        /// 取出AccountLink相關設定
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetACLinkSettingValue(string key)
        {
            string sBankCode = ((int)_bankType).ToString("000");
            List<ACLinkSettingDbRes> list = _acLinkRepository.ListAccountLinkSetting(sBankCode);

            ACLinkSettingDbRes oModel = list.Find(t => t.ACLinkKey == key);
            var value = oModel == null ? "" : oModel.ACLinkValue;

            return value.ToString();
        }

        /// <summary>
        ///  資料下載路徑
        /// </summary>
        public string DownloadPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["DownloadPath"] ?? "";
                return string.Format(sPath, _bankType.ToString());
            }
        }

        /// <summary>
        /// 資料上傳路徑
        /// </summary>
        public string UploadPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["UploadPath"] ?? "";
                return string.Format(sPath, DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 搬移路徑(匯入的壓縮檔)
        /// </summary>
        public string ZipPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["ZipPath"] ?? "";
                return string.Format(sPath, DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 搬移路徑(執行進行中)
        /// </summary>
        public string ProcessPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["ProcessPath"] ?? "";
                return string.Format(sPath, _bankType.ToString(), DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 搬移路徑(執行成功)
        /// </summary>
        public string SuccessPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["SuccessPath"] ?? "";
                return string.Format(sPath, _bankType.ToString(), DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 搬移路徑(執行失敗)
        /// </summary>
        public string FailPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["FailPath"] ?? "";
                return string.Format(sPath, _bankType.ToString(), DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// Log路徑
        /// </summary>
        public string LogPath
        {
            get
            {
                string sPath = ConfigurationManager.AppSettings["LogPath"] ?? "";
                return string.Format(sPath, _bankType.ToString(), DateTime.Now.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 平台代碼
        /// </summary>
        protected string ACLinkECID
        {
            get
            {
                return GetACLinkSettingValue("ACLinkECID");
            }
        }
    }
}
