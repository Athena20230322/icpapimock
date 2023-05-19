using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Library.Models.Payment;
using ICP.Library.Repositories.Payment;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Library.Services.Payment
{
    public class PaymentCommonService
    {
        private readonly ILogger _logger = null;
        private readonly PaymentRepository _paymentTradeRepository = null;

        public PaymentCommonService(
            ILogger<PaymentCommonService> Logger,
            PaymentRepository paymentTradeRepository
        )
        {
            _logger = Logger;
            _paymentTradeRepository = paymentTradeRepository;
        }

        /// <summary>
        /// 取得CheckMacValue檢核碼
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="hashKey"></param>
        /// <param name="hashIV"></param>
        /// <returns></returns>
        public string GenerateCheckMacValue(object sourceObject, string hashKey, string hashIV)
        {
            var dict = sourceObject.GetType()
                                   .GetProperties()
                                   .Where(x => x.CanRead)
                                   .ToDictionary(x => x.Name, x => x.GetValue(sourceObject, null)?.ToString());

            return generateCheckMacValue(dict, hashKey, hashIV);
        }

        /// <summary>
        /// 取得CheckMacValue檢核碼
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="hashKey"></param>
        /// <param name="hashIV"></param>
        /// <returns></returns>
        public string GenerateCheckMacValue(NameValueCollection requestData, string hashKey, string hashIV)
        {
            if (requestData != null)
            {
                var formData = requestData.ToDictionary();

                return generateCheckMacValue(formData, hashKey, hashIV);
            }

            return "";
        }

        private string generateCheckMacValue(IDictionary<string, string> dict, string hashKey, string hashIV)
        {
            //### 將傳遞參數依照第一個英文字母，由A到Z的順序來排序(遇到第一個英名字母相同時，以第二個英名字母來比較，以此類推)，並且以&方式將所有參數串連。
            var sortDict = dict.OrderBy(x => x.Key);
            var list = sortDict.Where(x => !x.Key.Equals("CHECKMACVALUE", StringComparison.OrdinalIgnoreCase)).Select(x => $"{x.Key}={x.Value}");

            //### 參數最前面加上HashKey、最後面加上HashIV
            string queryString = ($"Hashkey={hashKey}&{string.Join("&", list)}&HashIV={hashIV}");

            _logger.Trace($"QueryString(參數最前面加上HashKey、最後面加上HashIV)：{queryString}");

            //### 將整串字串進行URL encode
            queryString = HttpUtility.UrlEncode(queryString, Encoding.UTF8);

            _logger.Trace($"QueryString(將整串字串進行URL encode)：{queryString}");

            //### 轉為小寫
            queryString = queryString.ToLower();

            _logger.Trace($"QueryString(轉為小寫)：{queryString}");

            HashCryptoHelper hashCrypto = new HashCryptoHelper();
            
            string checkMacValue = hashCrypto.HashSha256(queryString).ToUpper();

            _logger.Trace($"CheckMacValue：{checkMacValue}");

            //### 以SHA256加密方式來產生雜凑值
            //### 再轉大寫產生CheckMacValue
            return checkMacValue;
        }

        /// <summary>
        /// 撈取訂單資訊 
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public GetTradeInfoDbRes GetTradeInfo(GetTradeInfoDbReq getTradeInfoDbReq)
        {
            return _paymentTradeRepository.GetTradeInfo(getTradeInfoDbReq);
        }
    }
}
