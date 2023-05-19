using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Repositories;
using System.Collections.Generic;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// AccountLink 相關config設定
    /// </summary>
    class ACLinkConfigService
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
            CacheService cacheService = new CacheService();

            string cacheName = "AccountLinkSetting_" + _bankType;

            List<ACLinkSettingDbRes> list = cacheService.Get<List<ACLinkSettingDbRes>>(cacheName);

            if (list == null)
            {
                string sBankCode = ((int)_bankType).ToString("000");
                list = _acLinkRepository.ListAccountLinkSetting(sBankCode);
                cacheService.Set(list, cacheName, 1200);
            }

            ACLinkSettingDbRes oModel = list.Find(t => t.ACLinkKey == key);
            var value = oModel == null ? "" : oModel.ACLinkValue;

            return value.ToString();
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

        #region HSM

        /// <summary>
        /// 簽章的Key標籤
        /// </summary>
        protected string HSMKeyLabel
        {
            get
            {
                return GetACLinkSettingValue("HSMKeyLabel");
            }
        }

        /// <summary>
        /// 3DES的Key標籤
        /// </summary>
        protected string HSM3DESKeyLabel
        {
            get
            {
                return GetACLinkSettingValue("HSM3DESKeyLabel");
            }
        }

        /// <summary>
        /// 簽章的憑證編號
        /// </summary>
        protected string HSMCertId
        {
            get
            {
                return GetACLinkSettingValue("HSMCertId");
            }
        }

        /// <summary>
        /// 憑證序號
        /// </summary>
        protected string HSMCERTSN
        {
            get
            {
                return GetACLinkSettingValue("HSMCERTSN");
            }
        }

        #endregion

        #region 綁定

        /// <summary>
        /// 帳號綁定
        /// </summary>
        public string ACLinkBindAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkBindAddr");
            }
        }

        /// <summary>
        /// 帳戶解綁
        /// </summary>
        public string ACLinkCancelAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkCancelAddr");
            }
        }

        /// <summary>
        /// 綁定申請
        /// </summary>
        public string ACLinkBindApplyAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkBindApplyAddr");
            }
        }

        /// <summary>
        /// 綁定結果通知(後端)
        /// </summary>
        public string BindResultApiUrl
        {
            get
            {
                return GetACLinkSettingValue("BindResultApiUrl");
            }
        }

        /// <summary>
        /// 綁定結束導回(前端)
        /// </summary>
        public string BindResultWebUrl
        {
            get
            {
                return GetACLinkSettingValue("BindResultWebUrl");
            }
        }

        #endregion

        #region 交易

        /// <summary>
        /// 交易扣款
        /// </summary>
        protected string ACLinkPayAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkPayAddr");
            }
        }

        /// <summary>
        /// 交易儲值
        /// </summary>
        protected string ACLinkDepositAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkDepositAddr");
            }
        }

        /// <summary>
        /// 交易退款
        /// </summary>
        protected string ACLinkRefundAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkRefundAddr");
            }
        }

        /// <summary>
        /// 交易提領
        /// </summary>
        protected string ACLinkWithdrawalAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkWithdrawalAddr");
            }
        }

        #endregion

        #region 查詢

        /// <summary>
        /// 綁定狀態查詢
        /// </summary>
        protected string ACLinkBindQryAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkBindQryAddr");
            }
        }

        /// <summary>
        /// 交易扣款查詢
        /// </summary>
        protected string ACLinkPayQryAddr
        {
            get
            {
                return GetACLinkSettingValue("ACLinkPayQryAddr");
            }
        }

        #endregion

        #region 其它

        /// <summary>
        /// 合作業者代號
        /// </summary>
        protected string ACLinkCooPerAtor
        {
            get
            {
                return GetACLinkSettingValue("ACLinkCooPerAtor");
            }
        }

        /// <summary>
        /// http header
        /// </summary>
        public string ACLinkClientID
        {
            get
            {
                return GetACLinkSettingValue("ACLinkClientID");
            }
        }

        #endregion

        #region 中國信託
        /// <summary>
        /// 業者統編
        /// </summary>
        public string ACLinkMerchantID
        {
            get
            {
                return GetACLinkSettingValue("ACLinkMerchantID");
            }
        }

        /// <summary>
        /// 業者KeyId
        /// </summary>
        public string ACLinkKeyId
        {
            get
            {
                return GetACLinkSettingValue("ACLinkKeyId");
            }
        }

        /// <summary>
        /// 連結類別
        /// </summary>
        public string ACLinkType
        {
            get
            {
                return GetACLinkSettingValue("ACLinkType");
            }
        }

        /// <summary>
        /// 業種
        /// </summary>
        public string ACLinkMerchantType
        {
            get
            {
                return GetACLinkSettingValue("ACLinkMerchantType");
            }
        }

        /// <summary>
        /// 連結會員關係
        /// </summary>
        public string ACLinkHolderRelationship
        {
            get
            {
                return GetACLinkSettingValue("ACLinkHolderRelationship");
            }
        }

        /// <summary>
        /// 是否模擬銀行 (True:模擬)
        /// </summary>
        public bool MockBank
        {
            get
            {
                bool isMock = false;    
                string mockBank = GetACLinkSettingValue("MockBank");

                // 1:Mock
                if (!string.IsNullOrWhiteSpace(mockBank) && mockBank == "1")
                {
                    isMock = true;
                }

                return isMock;
            }
        }
        #endregion
    }
}
