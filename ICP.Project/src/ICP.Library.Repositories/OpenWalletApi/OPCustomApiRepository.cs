using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ICP.Library.Repositories.OpenWalletApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Abstractions.DbUtil;
    using Models.Enums;
    using Models.OpenWalletApi.Enums;
    using Models.OpenWalletApi.CustomReceiveApi;
    using Models.OpenWalletApi.CustomSendApi;
    using MemberRepositories;
    using ICP.Infrastructure.Abstractions.Logging;
    using ICP.Library.Repositories.SystemRepositories;

    /// <summary>
    /// OP客制API
    /// </summary>
    public class OPCustomApiRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        MemberConfigCyptRepository _configCyptRepository;
        ConfigKeyValueRepository _configKeyValueRepository;
        ILogger<OPCustomApiRepository> _logger;

        public OPCustomApiRepository(
            MemberConfigCyptRepository configCyptRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            IDbConnectionPool dbConnectionPool,
            ILogger<OPCustomApiRepository> logger
            )
        {
            _configCyptRepository = configCyptRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _dbConnectionPool = dbConnectionPool;
            _logger = logger;
        }

        public BaseResult AddCustomAPILog(
            TransType transType, 
            CustomApiMethodType methodType, 
            string EncData, 
            long? MID = null, 
            string StatusCode = null, 
            string StatusMessage = null, 
            long RealIP = 0, 
            long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Member_OP_AddCustomAPILog_I";

            var args = new
            {
                TransType = (byte)transType,
                Method = (byte)methodType,
                EncData,
                MID,
                StatusCode,
                StatusMessage,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 發動 api
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="customApiMethodType"></param>
        /// <returns></returns>
        private DataResult<TResult> CallApi<TResult>(
            string url, 
            object obj, 
            CustomApiMethodType customApiMethodType, 
            long? MID = null,
            long RealIP = 0,
            long ProxyIP = 0
            ) where TResult : BaseCustomSendApiResult, new()
        {
            string address = _configKeyValueRepository.op_custom_domain;

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            var genMaskResult = GenerateMask(customApiMethodType, obj);
            if (!genMaskResult.IsSuccess)
            {
                var errResult = new DataResult<TResult>();
                errResult.SetError(genMaskResult);
                return errResult;
            }
            string Mask = genMaskResult.RtnData;

            //INPUT 欄位
            JObject jb = JObject.FromObject(obj);

            //增加 Mask
            jb.Add("Mask", Mask);

            //產生 json
            string json = jb.ToString(Newtonsoft.Json.Formatting.None);

            //aes 加密
            string encData = _configCyptRepository.Encrypt_CustomOpenWalletEncData(json);

            //send log
            AddCustomAPILog(TransType.Send, customApiMethodType, encData, MID: MID, RealIP: RealIP, ProxyIP: ProxyIP);

            //產生 body
            var formData = new Dictionary<string, string>();
            formData.Add("EncData", encData);

            var content = new FormUrlEncodedContent(formData);

            string stringResult = null;
            TResult model = null;

            try
            {
                //using (var _httpClient = new HttpClient { BaseAddress = new Uri(address) })
                //{
                //    var postResult = _httpClient.PostAsync(url, content).Result;
                //    stringResult = postResult.Content.ReadAsStringAsync().Result;
                //}

                //string resJson = _configCyptRepository.Decrypt_CustomOpenWalletEncData(stringResult);
                //model = JsonConvert.DeserializeObject<TResult>(resJson);

                //測試
                stringResult = string.Empty;
                model = new TResult();
                model.StatusCode = "00";
                model.StatusMessage = "test success";

                //receive log
                AddCustomAPILog(
                    TransType.Receive, 
                    customApiMethodType, 
                    stringResult, 
                    MID: MID, 
                    StatusCode: (model != null ? model.StatusCode : null), 
                    StatusMessage: (model != null ? model.StatusMessage : null), 
                    RealIP: RealIP, 
                    ProxyIP: ProxyIP);
            }
            catch (Exception ex)
            {
                model = new TResult();
                model.StatusCode = "ex";
                _logger.Error(ex.Message);
            }

            return modelToResult(model);
        }

        /// <summary>
        /// api 結果轉成 DataResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private DataResult<T> modelToResult<T>(T model) where T : BaseCustomSendApiResult
        {
            var result = new DataResult<T>();
            result.SetSuccess(model);

            if (!string.IsNullOrEmpty(model.StatusCode) && model.StatusCode != "00")
            {
                result.SetCode(0);
            }

            return result;
        }

        /// <summary>
        /// 產生 Mask
        /// </summary>
        /// <param name="customApiMethodType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<string> GenerateMask(CustomApiMethodType customApiMethodType, object model)
        {
            var result = new DataResult<string>();
            result.SetError();

            GeneratePropertiesObject gen = null;

            if (customApiMethodType == CustomApiMethodType.BindicashAccount)
            {
                #region 綁定icash會員
                gen =
                    new GeneratePropertiesObject<BindicashAccountRequest>((BindicashAccountRequest)model)
                    .Add(t => t.OpMemberID)
                    .Add(t => t.IcashAccount)
                    .Add(t => t.ICPCarrierNum)
                    .Add(t => t.CarrierType)
                    .Add(t => t.TimeStamp)
                    ;
                #endregion
            }
            else if (customApiMethodType == CustomApiMethodType.UnBindicashAccount)
            {
                #region 解綁icash會員
                gen =
                    new GeneratePropertiesObject<UnBindicashAccountRequest>((UnBindicashAccountRequest)model)
                    .Add(t => t.OpMemberID)
                    .Add(t => t.IcashAccount)
                    .Add(t => t.TimeStamp)
                    ;
                #endregion
            }
            else if (customApiMethodType == CustomApiMethodType.NoticeMemberDelete)
            {
                #region 會員解戶通知
                gen =
                    new GeneratePropertiesObject<NoticeMemberDeleteRequest>((NoticeMemberDeleteRequest)model)
                    .Add(t => t.mid)
                    .Add(t => t.request_time)
                    ;
                #endregion
            }
            else if (customApiMethodType == CustomApiMethodType.NoticeMobileBarcode)
            {
                #region 會員手機條碼異動
                gen =
                    new GeneratePropertiesObject<NoticeMobileBarcodeRequest>((NoticeMobileBarcodeRequest)model)
                        .Add(t => t.mid)
                        .Add(t=>t.mobile_barcode)
                        .Add(t => t.request_time)
                    ;
                #endregion
            }
            else
            {
                return result;
            }

            //所有的INPUT欄位依序組合
            string valueString = gen.ToValueString();

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            string Mask = _configCyptRepository.MD5_CustomOpenWalletMask(valueString);

            if (Mask != null) Mask = Mask.ToLower();

            result.SetSuccess(Mask);
            return result;
        }

        /// <summary>
        /// 綁定icash會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<BaseCustomSendApiResult> BindicashAccount(BindicashAccountRequest model, long MID, long RealIP = 0, long ProxyIP = 0)
        {
            string url = "BindicashAccount";

            return CallApi<BaseCustomSendApiResult>(url, model, CustomApiMethodType.BindicashAccount, MID: MID, RealIP: RealIP, ProxyIP: ProxyIP);
        }

        /// <summary>
        /// 解綁icash會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<BaseCustomSendApiResult> UnBindicashAccount(UnBindicashAccountRequest model, long MID, long RealIP = 0, long ProxyIP = 0)
        {
            string url = "UnBindicashAccount";

            return CallApi<BaseCustomSendApiResult>(url, model, CustomApiMethodType.UnBindicashAccount, MID: MID, RealIP: RealIP, ProxyIP: ProxyIP);
        }
    }
}
