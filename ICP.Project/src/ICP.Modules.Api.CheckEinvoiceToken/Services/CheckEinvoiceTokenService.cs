using System;
using System.Text;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Repositories.SystemRepositories;
using ICP.Modules.Api.CheckEinvoiceToken.Repositories;
using Newtonsoft.Json;

namespace ICP.Modules.Api.CheckEinvoiceToken.Services
{
    public class CheckEinvoiceTokenService
    {
        private CheckEinvoiceTokenRepository _checkEinvoiceTokenRepository;
        private ConfigKeyValueRepository _configKeyValueRepository;
        private ILogger<CheckEinvoiceTokenService> _logger;
        public CheckEinvoiceTokenService(
            CheckEinvoiceTokenRepository checkEinvoiceTokenRepository, 
            ConfigKeyValueRepository configKeyValueRepository,
            ILogger<CheckEinvoiceTokenService> ilogger)
        {
            _checkEinvoiceTokenRepository = checkEinvoiceTokenRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _logger = ilogger;
        }

        public string checkIssueToken(InvoiceBindReturn model)
        {
            string ReturnStr = string.Empty;


            //int InvoiceID = Convert.ToInt32(Session["InvoiceID"]);
            //### 建立接收資料 Log

            // Log 檔
            if (model != null)
            {
                _logger.Info(JsonConvert.SerializeObject(model), "接收歸戶資料");
            }

            //### 驗證 token 
            bool checkValidate = ValidateToken(model);

            ReturnStr = checkValidate == true ? "Y" : "N";

            //### 建立回傳資料 Log

            if (model != null)
            {
                _logger.Info(JsonConvert.SerializeObject(model) + ReturnStr, "回送歸戶資料");
            }


            return ReturnStr;
        }
        #region  歸戶

        /// <summary>
        /// 設定 小平台傳送參數至大平台 回傳資訊
        /// </summary>
        /// <param name="RtnModel">回傳的Model</param> 
        ///<rereturns>String</rereturns>
        public string GetRrturnString(InvoiceBindReturn model)
        {
            string RtnString = string.Empty;

            var result = new
            {
                RtnCode = model.RtnCode,
                RtnMsg = model.RtnMsg,
                card_ban = model.card_ban,
                card_no1 = model.card_no1,
                card_no2 = model.card_no2,
                card_type = model.card_type,
                back_url = model.back_url,
                token = model.token,
                URL = _configKeyValueRepository.Einvoice_Url_APMEMBERVAN
            };

            RtnString = JsonConvert.SerializeObject(result);

            return RtnString;
        }

        #region 歸戶 step2

        /// <summary>
        /// 驗證 token 是否有效
        /// </summary>
        /// <param name="RtnModel">接收的Model</param> 
        ///<rereturns>String</rereturns>
        public bool ValidateToken(InvoiceBindReturn model)
        {
            //## 建立 Model & Service
            InvoiceBindReturn checkModel = new InvoiceBindReturn();

            string token = model.token;
            string cardtype = DecodeString(model.card_type);
            string card_no1 = DecodeString(model.card_no1);
            string card_no2 = DecodeString(model.card_no2);
            string card_ban = model.card_ban;

            bool checkStatus = false;
            //### 依照token 取出發票資料來比對
            checkModel = _checkEinvoiceTokenRepository.GetIssueBindDataByToken(card_no1, token);

            if (checkModel != null)
            {
                if (checkModel.card_ban == card_ban && checkModel.card_no1 == card_no1 &&
                    checkModel.card_no1 == card_no2 && cardtype == _configKeyValueRepository.Einvoice_CardType)
                {
                    checkStatus = true;
                }
            }

            return checkStatus;
        }

        #endregion

        #region 歸戶 step3

        /// <summary>
        /// 大平台接收小平台歸戶傳送Step3之參數，大平台回傳歸戶結果，回傳路徑Step1參數之"back_url"傳送相關參數
        /// </summary>
        /// <param name="RtnModel">接收的Model</param> 
        ///<rereturns>String</rereturns>
        public bool UpdateBindStatus(InvoiceBindReturn model)
        {
            //## 建立 Model & Service
            BaseResult result = new BaseResult();
            string cardtype = DecodeString(model.card_type);
            model.card_no1 = DecodeString(model.card_no1);
            model.card_no2 = DecodeString(model.card_no2);


            if (cardtype != _configKeyValueRepository.Einvoice_CardType || model.card_no1 != model.card_no2)
            {
                return false;
            }

            //### 依照token 取出發票資料來比對
            result = _checkEinvoiceTokenRepository.UpdateIssueBindStatus(model);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 新增載具歸戶Log
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddConsolidateCarrierLog(InvoiceBindLogModel model)
        {
            
            _checkEinvoiceTokenRepository.AddConsolidateCarrierLog(model);
            return;
        }

        #region 共用 base64 加解密

        public static string EncodeString(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string DecodeString(string toDecrypt)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(toDecrypt.Replace(" ", "+"));
            return Encoding.UTF8.GetString(encodedDataAsBytes);
        }

        #endregion

        #endregion
    }
}