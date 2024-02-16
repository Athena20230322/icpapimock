using System;
using System.Configuration;
using ICP.Library.Repositories.SystemRepositories;

namespace ICP.Batch.AppRssPush.Services
{

    public class ConfigService
    {
        private readonly ConfigKeyValueRepository _configKeyValueRepository;

        public ConfigService(
            ConfigKeyValueRepository configKeyValueRepository
        )
        {
            _configKeyValueRepository = configKeyValueRepository;
        }

        /// <summary>
        ///發動來源 
        /// </summary>
        public static string SourceName
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["source_name"];
                return retVal ?? "";
            }
        }

        /// <summary>
        /// WebRequest Timeout設定值
        /// </summary>
        public static int WebRequestTimeout
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["WebRequestTimeout"];
                return retVal == null ? 0 : Convert.ToInt32(retVal);
            }
        }

        /// <summary>
        /// AppRssPush發送位置
        /// </summary>
        public string Url => _configKeyValueRepository.AppRss_Push_Url;

    }
}