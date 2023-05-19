using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Authorization.Utils
{
    public static class ConfigUtil
    {
        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string Authorization_Domain
        {
            get
            {
                return GetAppSetting("Authorization_Domain");
            }
        }
    }
}
