using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Helpers;
using ICP.Host.APIService.Repositories;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.DTO;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Repositories.SystemRepositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICP.Host.APIService.Services
{
    public class EinvoiceService
    {
        private readonly ConfigService _configService;
        private ILogger _logger;
        private readonly ConfigKeyValueRepository _configKeyValueRepository;


        public EinvoiceService(
            ConfigService configService,
            ILogger<EinvoiceService> logger,
            ConfigKeyValueRepository configKeyValueRepository)
        {
            _configService = configService;
            _logger = logger;
            _configKeyValueRepository = configKeyValueRepository;
        }

        #region 構建函數類

        private string Uuid => Guid.NewGuid().ToString();

        private string Serial => DateTime.Now.ToString("ddhhmmssff");

        private string Timestamp =>
            (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds.ToString("0");

        private string ExpTimeStamp =>
            (DateTime.UtcNow.AddMinutes(1) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds.ToString("0");

        #endregion

        #region 手機條碼綁定金融帳戶或電子支付帳戶 

        /// <summary>
        /// 手機條碼綁定金融帳戶或電子支付帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BankAccountResultDTO SetBankAccount(BankAccountDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["accountNo"] = model.AccountNo,
                ["action"] = "generalCarrierBank",
                ["appID"] = _configService.EinvoiceAppID,
                ["bankNo"] = model.BankNo,
                ["cardEncrypt"] = model.CardEncrypt,
                ["cardNo"] = model.CardNo,
                ["cardType"] = "3J0002",
                ["enableRemit"] = "Y",
                ["expTimeStamp"] = ExpTimeStamp,
                ["rocID"] = model.RocID,
                ["serial"] = Serial,
                ["timeStamp"] = Timestamp,
                ["updateAcc"] = "Y",
                ["userIdType"] = model.UserIdType,
                ["uuid"] = Uuid,
                ["version"] = "1.0",
                ["winnerName"] = model.WinnerName,
                ["winnerPhone"] = model.WinnerPhone
            };
            return ExecuteApi<BankAccountResultDTO>(_configService.Einvoice_Url_SetBankAccount, param, model.Timeout,
                Serial);
        }

        #endregion

        #region 載具發票表頭查詢

        /// <summary>
        /// 載具發票表頭查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierInvTitleResultDTO GetCarrierInvTitle(CarrierInvTitleDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "carrierInvChk",
                ["appID"] = _configService.EinvoiceAppID,
                ["cardNo"] = model.CardNo,
                ["startDate"] = model.StartDate,
                ["endDate"] = model.EndDate,
                ["onlyWinningInv"] = model.OnlyWinningInv,
                ["expTimeStamp"] = ExpTimeStamp,
                ["timeStamp"] = Timestamp,
                ["cardType"] = "3J0002",
                ["cardEncrypt"] = model.CardEncrypt,
                ["uuid"] = Uuid,
                ["version"] = "0.5"
            };
            return ExecuteApi<CarrierInvTitleResultDTO>(_configService.Einvoice_Url_GetCarrierInvTitle, param,
                model.Timeout);
        }

        #endregion

        #region 載具發票明細查詢

        /// <summary>
        /// 載具發票明細查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierInvDetailResultDTO GetCarrierInvDetail(CarrierInvDetailDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "carrierInvDetail",
                ["appID"] = _configService.EinvoiceAppID,
                ["cardNo"] = model.CardNo,
                ["expTimeStamp"] = ExpTimeStamp,
                ["timeStamp"] = Timestamp,
                ["cardType"] = "3J0002",
                ["invDate"] = model.InvDate,
                ["invNum"] = model.InvNum,
                ["cardEncrypt"] = model.CardEncrypt,
                ["uuid"] = Uuid,
                ["version"] = "0.5"
            };


            return ExecuteApi<CarrierInvDetailResultDTO>(_configService.Einvoice_Url_GetCarrierInvDetail, param,
                model.Timeout);
        }

        #endregion

        #region 變更手機條碼驗證碼

        /// <summary>
        /// 變更手機條碼驗證碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ChangeCarrierPwdResultDTO ChangeCarrierPwd(ChangeCarrierPwdDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "changeVer",
                ["appID"] = _configService.EinvoiceAppID,
                ["cardNo"] = model.CardNo,
                ["newVerify"] = model.NewVerify,
                ["oldVerify"] = model.OldVerify,
                ["serial"] = Serial,
                ["timeStamp"] = Timestamp,
                ["uuid"] = Uuid,
                ["version"] = "1.0"
            };

            return ExecuteApi<ChangeCarrierPwdResultDTO>(_configService.Einvoice_Url_ChangeCarrierPwd, param,
                model.Timeout, Serial);
        }

        #endregion

        #region 忘記手機條碼驗證碼

        /// <summary>
        /// 忘記手機條碼驗證碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ForgotCarrierPwdResultDTO ForgotCarrierPwd(ForgotCarrierPwdDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "forgetVer",
                ["appID"] = _configService.EinvoiceAppID,
                ["email"] = model.Email,
                ["phoneNo"] = model.PhoneNo,
                ["serial"] = Serial,
                ["timeStamp"] = Timestamp,
                ["uuid"] = Uuid,
                ["version"] = "1.0"
            };

            return ExecuteApi<ForgotCarrierPwdResultDTO>(_configService.Einvoice_Url_ForgotCarrierPwd, param,
                model.Timeout, Serial);
        }

        #endregion

        #region 手機條碼歸戶載具查詢

        /// <summary>
        /// 手機條碼歸戶載具查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierUnderTypeResultDTO GetCarrierUnderType(CarrierUnderTypeDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["version"] = "1.0",
                ["serial"] = Serial,
                ["action"] = "qryCarrierAgg",
                ["cardType"] = "3J0002",
                ["cardNo"] = model.CardNo,
                ["cardEncrypt"] = model.CardEncrypt,
                ["appID"] = _configService.EinvoiceAppID,
                ["timeStamp"] = Timestamp,
                ["uuid"] = Uuid
            };

            return ExecuteApi<CarrierUnderTypeResultDTO>(_configService.Einvoice_Url_GetCarrierUnderType, param,
                model.Timeout, Serial);
        }

        #endregion

        #region 查詢手機條碼

        /// <summary>
        /// 查詢手機條碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierBarcodeResultDTO GetCarrierBarcode(CarrierBarcodeDTO model)
        {
            #region 此段為測試用 改為載具發票表頭查詢 反驗手機條碼
            if (ConfigService.CarrierInvMockVerification == "1")
            {
                CarrierInvTitleDTO carrierInvTitle = new CarrierInvTitleDTO
                {
                    CardNo = model.CardNo,
                    StartDate = ConvertHelper.ToSimpleTaiwanDate(DateTime.Now).Substring(0, 5),//只取當月
                    EndDate = ConvertHelper.ToSimpleTaiwanDate(DateTime.Now).Substring(0, 5),//只取當月
                    OnlyWinningInv = "N",
                    CardEncrypt = model.VerificationCode
                };

                CarrierInvTitleResultDTO carrierInvTitleResult = GetCarrierInvTitle(carrierInvTitle);

                CarrierBarcodeResultDTO carrierBarcodeResult = new CarrierBarcodeResultDTO();
                //如果錯誤就把驗證碼壓為空值
                if (carrierInvTitleResult.Code != "200")
                {
                    carrierBarcodeResult.VerificationCode = "";
                    carrierBarcodeResult.CardNo = model.CardNo;
                    carrierBarcodeResult.Code = carrierInvTitleResult.Code;
                    carrierBarcodeResult.PhoneNo = model.PhoneNo;
                    
                }
                if (carrierInvTitleResult.Code == "200")
                {
                    carrierBarcodeResult.VerificationCode = model.VerificationCode;
                    carrierBarcodeResult.CardNo = model.CardNo;
                    carrierBarcodeResult.Code = carrierInvTitleResult.Code;
                    carrierBarcodeResult.PhoneNo = model.PhoneNo;
                }

                return carrierBarcodeResult;
            }
            #endregion

            #region 正式區塊
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "getBarcode",
                ["appID"] = _configService.EinvoiceAppID,
                ["phoneNo"] = model.PhoneNo,
                ["timeStamp"] = Timestamp,
                ["uuid"] = Uuid,
                ["verificationCode"] = model.VerificationCode,
                ["version"] = "1.0"
            };

            return ExecuteApi<CarrierBarcodeResultDTO>(_configService.Einvoice_Url_GetCarrierBarcode, param,
                model.Timeout);
            #endregion
            
        }

        #endregion

        #region 手機條碼及驗證碼註冊

        /// <summary>
        /// 手機條碼及驗證碼註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RegisterCarrierResultDTO RegisterCarrier(RegisterCarrierDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["action"] = "pubCarVerReg",
                ["appID"] = _configService.EinvoiceAppID,
                ["email"] = model.Email,
                ["phoneNo"] = model.PhoneNo,
                ["isVerification"] = "Y",
                ["serial"] = Serial,
                ["timeStamp"] = Timestamp,
                ["uuid"] = Uuid,
                ["verify"] = model.Verify,
                ["version"] = "1.0"
            };

            return ExecuteApi<RegisterCarrierResultDTO>(_configService.Einvoice_Url_RegisterCarrier, param,
                model.Timeout, Serial);
        }

        #endregion

        #region 查詢發票明細

        /// <summary>
        /// 查詢發票明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvDetailResultDTO GetInvDetail(InvDetailDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["version"] = "0.5",
                ["action"] = "qryInvDetail",
                ["invTerm"] = model.InvTerm,
                ["UUID"] = Uuid,
                ["type"] = model.Type,
                ["invNum"] = model.InvNum,
                ["generation"] = "V2",
                ["invTerm"] = model.InvTerm,
                ["invDate"] = model.InvDate,
                ["encrypt"] = model.Encrypt,
                ["sellerID"] = model.SellerID,
                ["randomNumber"] = model.RandomNumber,
                ["appID"] = _configService.EinvoiceAppID
            };


            return ExecuteApi<InvDetailResultDTO>(_configService.Einvoice_Url_GetInvDetail, param, model.Timeout);
        }

        #endregion

        #region 查詢發票表頭

        /// <summary>
        /// 查詢發票表頭
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvTitleResultDTO GetInvTitle(InvTitleDTO model)
        {
            var param = new SortedDictionary<string, string>
            {
                ["version"] = "0.5",
                ["action"] = "qryInvHeader",
                ["UUID"] = Uuid,
                ["type"] = model.Type,
                ["invNum"] = model.InvNum,
                ["generation"] = "V2",
                ["invDate"] = model.InvDate,
                ["appID"] = _configService.EinvoiceAppID
            };


            return ExecuteApi<InvTitleResultDTO>(_configService.Einvoice_Url_GetInvTitle, param, model.Timeout);
        }

        #endregion

        
        #region 私有函數

        /// <summary>
        /// 呼叫 Api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        private T ExecuteApi<T>(string url, SortedDictionary<string, string> data, int timeout, string serial = null)
            where T : BaseResultDTO
        {
            string guid = Guid.NewGuid().ToString();
            string queryString = ParamterHelper.DictionaryToParamter(data, false);

            // 加密簽章
            string signature = HashString(queryString);
            data.Add("signature", signature);


            //url=url.Replace("https://wwwtest.einvoice.nat.gov.tw/", "https://api.einvoice.nat.gov.tw/");


            string apiUrl;
            IDictionary<string, string> apiParam;

            if (!string.IsNullOrWhiteSpace(_configService.Einvoice_Url_InvoiceApiProxy))
            {
                apiUrl = _configService.Einvoice_Url_InvoiceApiProxy.Substring(0,
                    _configService.Einvoice_Url_InvoiceApiProxy.LastIndexOf('?'));

                apiParam = new Dictionary<string, string>
                {
                    {"RequestUrl", url},
                    {"PostData", HttpUtility.HtmlEncode(ParamterHelper.DictionaryToParamter(data))}
                };
            }
            else
            {
                apiUrl = url;
                apiParam = new Dictionary<string, string>(data);
            }

            T result;

            try
            {
                string apiResult = NetworkService(apiUrl, apiParam, timeout);

                _logger.Info(
                    $"{nameof(guid)}:{guid} {nameof(apiResult)}:{(apiResult.TryParseToJson(out JToken jToken) ? jToken : apiResult)}");


                if (string.IsNullOrWhiteSpace(apiResult))
                {
                    throw new ArgumentNullException($"{nameof(apiResult)} 回傳資料為空");
                }

                result = JsonConvert.DeserializeObject<T>(apiResult);

                // 檢查 HashSerial 避免偽造
                if (result != null && result.Code == "200" && !IsVaildSerial(serial, result.HashSerial))
                {
                    throw new Exception("HashSerial 不正確");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{nameof(guid)}:{guid}");
                result = null;
            }

            if (result == null)
            {
                result = JsonConvert.DeserializeObject<T>(BaseResultDTO.DefaultErrorString);
            }

            return result;
        }

        /// <summary>
        /// 發送API
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="apiParam"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private string NetworkService(string apiUrl, IDictionary<string, string> apiParam, int timeout)
        {
            NetworkHelper networkService = new NetworkHelper();
            timeout = (timeout > 0 ? timeout : 15) + 5;
            return networkService.DoRequestWithUrlEncode(apiUrl, apiParam, timeout, null, null);
        }

        /// <summary>
        /// 檢查 Serial 是否偽造
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="apiResultSerial"></param>
        /// <returns></returns>
        private bool IsVaildSerial(string serial, string apiResultSerial)
        {
            if (string.IsNullOrWhiteSpace(serial))
            {
                return true;
            }

            return (HashString(serial) == apiResultSerial);
        }

        /// <summary>
        /// 符合財政部 hash 規則
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string HashString(string str)
        {
            return HMACSHAHelper.HMACSHA256Base64(_configService.EinvoiceApiKey, str);
        }

        #endregion
    }
}