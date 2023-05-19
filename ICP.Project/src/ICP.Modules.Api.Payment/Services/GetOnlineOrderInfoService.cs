using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.MemberModels;
using ICP.Library.Repositories.MerchantRepositories;
using ICP.Library.Services.MemberServices;
using ICP.Library.Services.MerchantServices;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo;
using ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using MemberInfoRepository = ICP.Library.Repositories.MemberRepositories.MemberInfoRepository;

namespace ICP.Modules.Api.Payment.Services
{
    public class GetOnlineOrderInfoService
    {
        private readonly PaymentTradeRepository _paymentTradeRepository = null;
        private readonly MemberPaymentInfoRepository _memberPaymentInfoRepository = null;
        private readonly MemberInfoRepository _libMemberInfoRepository = null;
        private readonly MerchantInfoRepository _libMerchantInfoRepository = null;
        private readonly ILogger _logger = null;

        public GetOnlineOrderInfoService(
            PaymentTradeRepository paymentTradeRepository,
            MemberPaymentInfoRepository memberPaymentInfoRepository,
            MemberInfoRepository libMemberInfoRepository,
            MerchantInfoRepository libMerchantInfoRepository,
            ILogger<GetOnlineOrderInfoService> logger
        )
        {
            _paymentTradeRepository = paymentTradeRepository;
            _memberPaymentInfoRepository = memberPaymentInfoRepository;
            _libMemberInfoRepository = libMemberInfoRepository;
            _libMerchantInfoRepository = libMerchantInfoRepository;
            _logger = logger;
        }

        /// <summary>
        /// 建立Temp訂單
        /// </summary>
        /// <param name="addTempTrade"></param>
        /// <returns></returns>
        public AddTempTradeDbRes AddTempTrade(AddTempTradeDbReq addTempTrade)
        {
            AddTempTradeDbRes addTempTradeTradeResult = _paymentTradeRepository.AddTempTrade(addTempTrade);

            addTempTradeTradeResult.SetCode(addTempTradeTradeResult.RtnCode);

            return addTempTradeTradeResult;
        }

        /// <summary>
        /// 檢核取得會員全付款方式參數
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mid"></param>
        /// <param name="memberData"></param>
        /// <param name="memberAccountInfo"></param>
        /// <returns></returns>
        public BaseResult ValidateGetOnlineOrderInfo(GetOnlineOrderInfoReq request, long mid, ref MemberDataModel memberData, ref AccountLinkRes memberAccountInfo, ref MerchantDataModel merchantData)
        {
            _logger.Trace($"取得會員全付款方式參數驗證開始");

            BaseResult result = new BaseResult();
            result.SetError();

            try
            {                
                if (request.MerchantID == 0)
                {
                    result.SetCode(2001);
                    return result;
                }

                //### 會員不存在
                if (mid == 0)
                {
                    result.SetCode(2042);
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.MerchantTradeNo))
                {
                    result.SetCode(2002);
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.PayID))
                {
                    result.SetCode(2043);
                    return result;
                }

                //### 驗證平台商
                if (request.PlatformID > 0)
                {
                    var plarformInfo = _libMerchantInfoRepository.GetMerchantData(request.PlatformID);

                    if(plarformInfo == null || plarformInfo.States != 1)
                    {
                        //### 平台商不存在
                        result.SetCode(2060);
                        return result;
                    }
                }

                //### 取得廠商資料
                var merchantInfo = _libMerchantInfoRepository.GetMerchantData(request.MerchantID);

                if (merchantInfo == null || merchantInfo.States != 1)
                {
                    //### 廠商編號不存在
                    result.SetCode(2041);
                    return result;
                }

                //### 取得廠商會員資料
                var merchantMemberData = _libMemberInfoRepository.GetMemberData(request.MerchantID);

                if (merchantMemberData == null || merchantMemberData.basic == null)
                {
                    //### 廠商編號不存在
                    result.SetCode(2041);
                    return result;
                }

                merchantData = new MerchantDataModel()
                {
                    MerchantID = request.MerchantID.ToString(),
                    MerchantIcpMID = merchantMemberData.basic.ICPMID,
                    MerchantName = !string.IsNullOrWhiteSpace(merchantInfo.MerchantName) ?
                                        merchantInfo.MerchantName :
                                        (
                                            !string.IsNullOrWhiteSpace(merchantInfo.WebSiteName) ?
                                                merchantInfo.WebSiteName :
                                                (
                                                    !string.IsNullOrWhiteSpace(merchantMemberData.basic.CName) ? merchantMemberData.basic.CName : request.MerchantID.ToString()
                                                )
                                        )
                };

                //### 付款方式            
                int.TryParse(request.PayID.Substring(0, 1), out int paymentType);

                if (!Enum.IsDefined(typeof(ePaymentType), paymentType))
                {
                    result.SetCode(2047);
                    return result;
                }

                memberData = _libMemberInfoRepository.GetMemberData(mid);

                //### 會員不存在
                if (memberData == null || memberData.basic == null)
                {
                    result.SetCode(2042);
                    return result;
                }

                //### 付款方式為iCash的檢核
                if (ePaymentType.ACCOUNTLINK == (ePaymentType)paymentType)
                {
                    string checkiCashAccount = request.PayID.Substring(1);

                    //### 傳入參數會員選擇的付款方式格式錯誤
                    if (string.IsNullOrWhiteSpace(memberData.basic.ICPMID) || !memberData.basic.ICPMID.Equals(checkiCashAccount))
                    {
                        result.SetCode(2047);
                        return result;
                    }
                }

                //### 付款方式為AccountLink的檢核
                if (ePaymentType.ACCOUNTLINK == (ePaymentType)paymentType)
                {
                    string indtAccount = request.PayID.Length > 4 ? request.PayID.Substring(3) : "";

                    var acclinks = _memberPaymentInfoRepository.ListAccountLinkInfo(mid);

                    //### 傳入參數會員選擇的付款方式格式錯誤
                    if (acclinks == null || acclinks.Count() == 0 || acclinks.Where(x => x.INDTAccount == indtAccount).Count() == 0)
                    {
                        result.SetCode(2047);
                        return result;
                    }

                    var account = acclinks.FirstOrDefault(x => x.INDTAccount == indtAccount);

                    memberAccountInfo = new AccountLinkRes()
                    {
                        INDTAccount = account.INDTAccount,
                        BankCode = account.BankCode,
                        BankName = account.BankName,
                        BankShortName = account.BankShortName,
                        LinkAccount = account.BankAccount,
                        AccountLastNo = account.BankAccount.Length > 5 ? account.BankAccount.Substring(account.BankAccount.Length - 5, 5) : account.BankAccount,
                        IsDefaultBank = account.IsDefault.ToString()
                    };
                }

                result.SetSuccess();
            }catch(Exception ex)
            {
                _logger.Error($"取得會員全付款方式參數驗證發生未預期錯誤,錯誤訊息為：{ex.ToString()}");
                throw ex;
            }
            finally
            {
                _logger.Error($"取得會員全付款方式參數驗證結束, 回傳代碼={result.RtnCode}");
            }

            return result;
        }

        /// <summary>
        /// 向OW發出Request取得訂單資訊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<AddTempTradeDbReq> RedirectToOWGetTempTrade(GetOnlineOrderInfoReq request)
        {
            DataResult<AddTempTradeDbReq> result = new DataResult<AddTempTradeDbReq>();

            //### Test Start
            //var jsonitemlist =new List<ItemModel>()
            //    {
            //        new ItemModel(){
            //            ItemName = "ItemName",
            //            Quantity = "1",
            //        },
            //        new ItemModel(){
            //            ItemName = "ItemName2",
            //            Quantity = "2",
            //        }
            //    };

            //var tepxEle = new XElement("ItemList",
            //            from items in jsonitemlist
            //            select new XElement("Item",
            //                         new XElement("ItemName", items.ItemName),
            //                           new XElement("Quantity", items.Quantity)
            //                       ));

            //result.SetSuccess(new AddTempTradeDbReq()
            //{
            //    MerchantID = request.MerchantID,
            //    WalletId = "123456",
            //    Amount = 100,
            //    MerchantTradeNo = request.MerchantTradeNo, //$"TEST{DateTime.Now.ToString("yyyyMMddHHmmssfff")}",
            //    MerchantTradeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
            //    ts = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(),
            //    PayID = request.PayID,
            //    ItemList = tepxEle.ToString(),
            //});

            //return result;
            //### Test End

            //### 利用訂單編號至OW端撈取定單資料
            long realMerchantID = request.PlatformID > 0 ? request.PlatformID : request.MerchantID;
            var merchantCertInfo = _libMerchantInfoRepository.GetMerchantCertificateData(realMerchantID);

            AesCryptoHelper _aesCryptoHelper = new AesCryptoHelper()
            {
                Key = merchantCertInfo.AES_Key,
                Iv = merchantCertInfo.AES_IV
            };

            RsaCryptoHelper _rsaCryptoHelper = new RsaCryptoHelper();

            string encData = _aesCryptoHelper.Encrypt(JsonConvert.SerializeObject(
                    new
                    {
                        PlatformID = request.PlatformID > 0 ? request.PlatformID.ToString() : "",
                        MerchantID = request.MerchantID.ToString(),
                        MerchantTradeNo = request.MerchantTradeNo
                    }
                ));

            _rsaCryptoHelper.ImportPemPrivateKey(merchantCertInfo.PrivateCert /* ClientPrivateKey */);
            string signature = _rsaCryptoHelper.SignDataWithSha256(encData);

            IDictionary<string, string> form = new Dictionary<string, string>();
            form.Add("EncData", encData);

            var content = new FormUrlEncodedContent(form);

            content.Headers.Add("X-iCP-EncKeyID", merchantCertInfo.ClientCertId.ToString());
            content.Headers.Add("X-iCP-Signature", signature);

            string stringResult;
            string resultSignature;

            //### OW線上交易訂單查詢網址
            string url = $"{GlobalConfigUtil.Host_OW_Domain}/Mock/GetOnlineOrderInfo";

            _logger.Trace($"OW線上交易訂單查詢網址開始");

            using (HttpClient _httpClient = new HttpClient())
            {
                var postResult = _httpClient.PostAsync(url, content).Result;
                stringResult = postResult.Content.ReadAsStringAsync().Result;

                _logger.Trace($"OW線上交易訂單查詢網址結果String Result：{ stringResult }");

                var headerSignature = postResult.Headers.Where(x => x.Key == "X-iCP-Signature").FirstOrDefault();
                resultSignature = headerSignature.Value?.FirstOrDefault();

                _logger.Trace($"OW線上交易訂單查詢網址結果Signature：{ resultSignature }");
            }

            _rsaCryptoHelper.ImportPemPublicKey(merchantCertInfo.ClientPublicCert /* ServerPubCert */);

            if (!string.IsNullOrEmpty(resultSignature))
            {
                bool isValid = _rsaCryptoHelper.VerifySignDataWithSha256(stringResult, resultSignature);
                if (!isValid)
                {
                    throw new Exception("簽章驗證失敗");
                }
            }

            JToken jToken = JToken.Parse(stringResult);
            int rtnCode = jToken["StatusCode"].Value<int>();
            string rtnMsg = jToken["StatusMessage"].Value<string>();
            string decryptContent;

            if (rtnCode == 1)
            {
                decryptContent = _aesCryptoHelper.Decrypt(jToken["EncData"].Value<string>());

                _logger.Trace($"OW回傳EncData Decrypt：{ decryptContent }");

                AddTempTradeDbReq addTempTrade = JsonConvert.DeserializeObject<AddTempTradeDbReq>(decryptContent);

                //### ItemList Json轉成XML再塞DB
                addTempTrade.ItemList.TryParseJsonToObj<List<ItemModel>>(out List<ItemModel> model);

                if (model != null && model.Count() > 0)
                {
                    var xEle = new XElement("ItemList",
                        from items in model
                        select new XElement("Item",
                                        new XElement("ItemName", items.ItemName),
                                        new XElement("Quantity", items.Quantity),
                                        new XElement("Remark", items.Remark)
                                   ));

                    addTempTrade.ItemList = xEle.ToString();
                }

                result.SetSuccess(addTempTrade);
            }
            else
            {
                result.SetError(new BaseResult()
                {
                    RtnCode = rtnCode,
                    RtnMsg = rtnMsg
                });
            }

            return result;
        }
    }
}