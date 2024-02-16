using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class ConfigRepository
    {
        private NameValueCollection AppSettings
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }

        public string AdminLoginUserPwdKey
        {
            get
            {
                return AppSettings["AdminLoginUserPwdKey"];
            }
        }

        public string AdminLoginUserPwdIV
        {
            get
            {
                return AppSettings["AdminLoginUserPwdIV"];
            }
        }

        public string APPXMLPath
        {
            get
            {
                return AppSettings["APPXMLPath"];
            }
        }

        /// <summary>
        /// 內部後台預設密碼
        /// </summary>
        public string DefaultPwd
        {
            get
            {
                return AppSettings["DefaultPwd"];
            }
        }

        /// <summary>
        /// 愛金卡公司名稱
        /// </summary>
        public string IcashCompanyName
        {
            get
            {
                return AppSettings["IcashCompanyName"];
            }
        }

        /// <summary>
        /// 寄件者
        /// </summary>
        public string AdminEmail
        {
            get
            {
                return AppSettings["AdminEmail"];
            }
        }

        /// <summary>
        /// 內部後台Domain
        /// </summary>
        public string AdminDomain
        {
            get
            {
                return AppSettings["AdminDomain"];
            }
        }

        /// <summary>
        /// Member Domain
        /// </summary>
        public string MemberDomain
        {
            get
            {
                return AppSettings["Host_Member_Domain"];
            }
        }

        /// <summary>
        /// 正式環境
        /// 0:否 1:是
        /// </summary>
        public bool ProductMode => AppSettings["ProductionMode"] == "1";
    }
}
