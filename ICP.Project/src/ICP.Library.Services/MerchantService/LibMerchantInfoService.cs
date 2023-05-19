using ICP.Library.Models.MerchantModels;
using ICP.Library.Repositories.MerchantRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MerchantServices
{    
    public class LibMerchantInfoService
    {
        private readonly MerchantInfoRepository _merchantInfoRepository = null;

        public LibMerchantInfoService
        (
            MerchantInfoRepository merchantInfoRepository
        )
        {
            _merchantInfoRepository = merchantInfoRepository;
        }

        /// <summary>
        /// 取得廠商資訊
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public MerchantDataModel GetMerchantData(long merchantID)
        {
            var result = _merchantInfoRepository.GetMerchantData(merchantID);

            return result;
        }

        /// <summary>
        /// 取得廠商金鑰資訊
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public MerchantCertificateModel GetMerchantCertificateData(long merchantID)
        {
            var result = _merchantInfoRepository.GetMerchantCertificateData(merchantID);

            return result;
        }
    }
}
