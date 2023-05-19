using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ICP.Infrastructure.Core.Helpers;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Authorization.Models;
using ICP.Modules.Api.Member.Models.Certificate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.IO;


namespace ICP.Host.ApiTest.Services
{
    using Models;
    using System.Net.Http.Headers;
    using System.Web;

    public class MockService
    {
        MockSetting _setting { get; set; }

        public MockService(string env)
        {
            LoadMockSetting(env);
        }

        public long MID
        {
            get
            {
                return _setting.MID;
            }
        }

        public string UserCode
        {
            get
            {
                return _setting.UserCode;
            }
        }

        #region Call Api
        public string CallNormalApi(string env, string host, string url, string json)
        {
            int delag = 0;
            return CallNormalApi(env, host, url, json, ref delag);
        }
        public string CallNormalApi(string env, string host, string url, string json, ref int delag)
        {
            string address = getHostDomain(env, host);

            var jobj = JObject.Parse(json);
            DateTime dateTimeNow = DateTime.Now;
            jobj.Add("Timestamp", dateTimeNow.ToString("yyyy/MM/dd HH:mm:ss"));

            #region 輔助產生參數值

            if (url.IndexOf("/Pos/") > 0)
            {
                if(jobj["PlatformID"] != null && jobj["MerchantID"] != null)
                {
                    if(Convert.ToString(jobj["PlatformID"]) == "default")
                    {
                        switch (env)
                        {
                            case "dev":
                                jobj["PlatformID"] = "10020336";

                                break;
                            case "beta":
                                jobj["PlatformID"] = "10026443";

                                break;
                            case "stage":
                                jobj["PlatformID"] = "10000001";

                                break;
                        }
                    }

                    if (Convert.ToString(jobj["MerchantID"]) == "default")
                    {
                        jobj["MerchantID"] = jobj["PlatformID"];
                    }
                }

                if (url.IndexOf("/Pos/CheckOut") > 0 || url.IndexOf("/Pos/TopUp") > 0)
                {
                    if (jobj["MerchantTradeNo"] != null)
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(jobj["MerchantTradeNo"])))
                        {
                            jobj["MerchantTradeNo"] = "Mock" + dateTimeNow.ToString("yyyyMMddHHmmssfff");
                        }
                    }
                }
            }

            if (jobj["MerchantTradeDate"] != null)
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(jobj["MerchantTradeDate"])))
                {
                    jobj["MerchantTradeDate"] = dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            #endregion

            json = jobj.ToString();

            _aesCryptoHelper.Key = _setting.AES_Key;
            _aesCryptoHelper.Iv = _setting.AES_IV;
            string encData = _aesCryptoHelper.Encrypt(json);

            _rsaCryptoHelper.ImportPemPrivateKey(_setting.ClientPrivateKey);
            string signature = _rsaCryptoHelper.SignDataWithSha256(encData);

            IDictionary<string, string> form = new Dictionary<string, string>();
            form.Add("EncData", encData);

            var content = new FormUrlEncodedContent(form);
            content.Headers.Add("X-iCP-EncKeyID", _setting.AES_CertId.ToString());
            content.Headers.Add("X-iCP-Signature", signature);

            string stringResult;
            string resultSignature;

            DateTime dtStart;
            DateTime dtEnd;

            using (HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(address) })
            {
                dtStart = DateTime.Now;

                var postResult = _httpClient.PostAsync(url, content).Result;
                stringResult = postResult.Content.ReadAsStringAsync().Result;

                dtEnd = DateTime.Now;

                delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds; ;

                var headerSignature = postResult.Headers.Where(x => x.Key == "X-iCP-Signature").FirstOrDefault();
                resultSignature = headerSignature.Value?.FirstOrDefault();
            }

            _rsaCryptoHelper.ImportPemPublicKey(_setting.ServerPubCert);
            bool isValid = _rsaCryptoHelper.VerifySignDataWithSha256(stringResult, resultSignature);
            if (!isValid)
            {
                throw new Exception("簽章驗證失敗");
            }

            JToken jToken = JToken.Parse(stringResult);
            int RtnCode = 0;
            string RtnMsg = "";

            if(jToken["RtnCode"] != null)
            {
                RtnCode = jToken["RtnCode"].Value<int>();
                RtnMsg = jToken["RtnMsg"].Value<string>();
            }
            else if(jToken["StatusCode"] != null)
            {
                RtnCode = jToken["StatusCode"].Value<int>();
                RtnMsg = jToken["StatusMessage"].Value<string>();
            }
            string decryptContent;
            if (RtnCode == 1)
            {
                decryptContent = _aesCryptoHelper.Decrypt(jToken["EncData"].Value<string>());
                var model = JsonConvert.DeserializeObject<BaseAuthorizationApiResult>(decryptContent);
                checkTimestamp(model.Timestamp);
                decryptContent = "{\"RtnCode\":" + RtnCode + ",\"RtnMsg\":\"" + RtnMsg + "\",\"EncData\":" + decryptContent + "}";
            }
            else
            {
                decryptContent = JsonConvert.SerializeObject(new { RtnCode, RtnMsg });
            }

            return decryptContent;
        }

        public string CallNormalApi(string env, string host, string url, string json, ref int delag, List<File> files = null)
        {
            string address = getHostDomain(env, host);

            if (files == null || !files.Any())
            {
                return CallNormalApi(env, host, url, json, ref delag);
            }

            var jobj = JObject.Parse(json);
            jobj.Add("Timestamp", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            json = jobj.ToString();

            _aesCryptoHelper.Key = _setting.AES_Key;
            _aesCryptoHelper.Iv = _setting.AES_IV;
            string encData = _aesCryptoHelper.Encrypt(json);

            _rsaCryptoHelper.ImportPemPrivateKey(_setting.ClientPrivateKey);
            string signature = _rsaCryptoHelper.SignDataWithSha256(encData);

            IDictionary<string, string> form = new Dictionary<string, string>();
            form.Add("EncData", encData);

            string stringResult;
            string resultSignature;

            DateTime dtStart;
            DateTime dtEnd;

            using (HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(address) })
            {
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var keyValuePair in form)
                    {
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }
                    content.Headers.Add("X-iCP-EncKeyID", _setting.AES_CertId.ToString());
                    content.Headers.Add("X-iCP-Signature", signature);

                    foreach (var file in files)
                    {
                        content.Add(CreateFileContent(file.InputStream, file.UploadName, file.FileName, file.ContentType));
                    }

                    dtStart = DateTime.Now;

                    var postResult = _httpClient.PostAsync(url, content).Result;
                    stringResult = postResult.Content.ReadAsStringAsync().Result;

                    dtEnd = DateTime.Now;

                    delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;

                    var headerSignature = postResult.Headers.Where(x => x.Key == "X-iCP-Signature").FirstOrDefault();
                    resultSignature = headerSignature.Value?.FirstOrDefault(); 
                }
            }

            _rsaCryptoHelper.ImportPemPublicKey(_setting.ServerPubCert);
            bool isValid = _rsaCryptoHelper.VerifySignDataWithSha256(stringResult, resultSignature);
            if (!isValid)
            {
                throw new Exception("簽章驗證失敗");
            }

            JToken jToken = JToken.Parse(stringResult);
            int RtnCode = jToken["RtnCode"].Value<int>();
            string RtnMsg = jToken["RtnMsg"].Value<string>();
            string decryptContent = _aesCryptoHelper.Decrypt(jToken["EncData"].Value<string>());
            var model = JsonConvert.DeserializeObject<BaseAuthorizationApiResult>(decryptContent);
            checkTimestamp(model.Timestamp);
            decryptContent = "{\"RtnCode\":" + RtnCode + ",\"RtnMsg\":\"" + RtnMsg + "\",\"EncData\":" + decryptContent + "}";

            return decryptContent;
        }
        #endregion

        #region Hosts
        private string EnvHostSettingPath
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/App_Data/EnvHost.json");
            }
        }

        private string getHostDomain(string env, string host)
        {
            string json = System.IO.File.ReadAllText(EnvHostSettingPath);

            var settings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            return settings[env][host];
        }
        #endregion

        #region MockSetting
        private void LoadMockSetting(string env)
        {
            _setting = GetMockSetting(env);

            if (_setting == null)
            {
                _setting = new MockSetting();
            }

            CheckMockSetting(env);
        }

        private void CheckMockSetting(string env)
        {
            if (DateTime.Now < _setting.ExpireDate) return;

            var cert = _setting;

            GetCertificate(env, ref cert);

            SaveMockSetting(env, cert);

            _setting = cert;
        }

        private string MockSettingPath
        {
            get
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                return dir.TrimEnd('\\') + "\\ICP.Host.ApiTest.json";
            }
        }

        private MockSetting GetMockSetting(string env)
        {
            var settings = GetAllMockSettings();

            if (!settings.ContainsKey(env)) return null;

            return settings[env];
        }

        private Dictionary<string, MockSetting> GetAllMockSettings()
        {
            if (!System.IO.File.Exists(MockSettingPath)) return new Dictionary<string, MockSetting>();

            string json = System.IO.File.ReadAllText(MockSettingPath);

            return JsonConvert.DeserializeObject<Dictionary<string, MockSetting>>(json);
        }

        private bool SaveMockSetting(string env, MockSetting model)
        {
            var settings = GetAllMockSettings();

            if (settings.ContainsKey(env))
            {
                settings[env] = model;
            }
            else
            {
                settings.Add(env, model);
            }

            string json = JsonConvert.SerializeObject(settings);

            try
            {
                System.IO.File.WriteAllText(MockSettingPath, json);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void UpdateMockSetting(string env, string UserCode, long MID)
        {
            var settings = GetAllMockSettings();

            if (!settings.ContainsKey(env))
            {
                return;
            }

            var setting = settings[env];
            setting.UserCode = UserCode;
            setting.MID = MID;

            string json = JsonConvert.SerializeObject(settings);

            try
            {
                System.IO.File.WriteAllText(MockSettingPath, json);
            }
            catch
            {

            }
        }

        public void RemoveMockSetting(string env)
        {
            var settings = GetAllMockSettings();

            if (!settings.ContainsKey(env))
            {
                return;
            }

            settings.Remove(env);

            string json = JsonConvert.SerializeObject(settings);

            try
            {
                System.IO.File.WriteAllText(MockSettingPath, json);
            }
            catch
            {

            }
        }

        private void GetCertificate(string env, ref MockSetting model)
        {
            string address = getHostDomain(env, "member");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(address)
            };

            _clientPrivateKey = model.ClientPrivateKey;
            _clientPublicKey = model.ClientPublicKey;
            _serverPubCertID = model.ServerPubCertID;
            _serverPublicKey = model.ServerPubCert;

            generateAES(model.ServerPubCertID, model.ServerPubCert, model.ClientPrivateKey);

            model.ClientPrivateKey = _clientPrivateKey;
            model.ClientPublicKey = _clientPublicKey;
            model.ServerPubCertID = _serverPubCertID;
            model.ServerPubCert = _serverPublicKey;

            model.AES_CertId = _aesClientCertId;
            model.AES_Key = _aesKey;
            model.AES_IV = _aesIv;
            model.ExpireDate = DateTime.Parse(_aesExpireDate);
        }
        #endregion

        #region Certificate
        private HttpClient _httpClient;

        private readonly RsaCryptoHelper _rsaCryptoHelper = new RsaCryptoHelper();
        private readonly AesCryptoHelper _aesCryptoHelper = new AesCryptoHelper();

        private long _serverPubCertID = -1;
        private string _serverPublicKey = null;
        private string _clientPublicKey = null;
        private string _clientPrivateKey = null;
        private long _aesClientCertId = -1;
        private string _aesKey = null;
        private string _aesIv = null;
        private string _aesExpireDate = null;

        private (string Content, string Signature) callCertificateApi(string action, long certId, string serverPublicKey, string clientPrivateKey, object obj, string certHeaderName)
        {
            string json = JsonConvert.SerializeObject(obj);

            _rsaCryptoHelper.ImportPemPublicKey(serverPublicKey);
            string encData = _rsaCryptoHelper.Encrypt(json);

            _rsaCryptoHelper.ImportPemPrivateKey(clientPrivateKey);
            string signature = _rsaCryptoHelper.SignDataWithSha256(encData);

            IDictionary<string, string> form = new Dictionary<string, string>();
            form.Add("EncData", encData);

            var content = new FormUrlEncodedContent(form);
            content.Headers.Add(certHeaderName, certId.ToString());
            content.Headers.Add("X-iCP-Signature", signature);

            var postResult = _httpClient.PostAsync(action, content).Result;
            string stringResult = postResult.Content.ReadAsStringAsync().Result;

            var headerSignature = postResult.Headers.Where(x => x.Key == "X-iCP-Signature").FirstOrDefault();
            string resultSignature = headerSignature.Value?.FirstOrDefault();

            return (stringResult, resultSignature);
        }

        private void checkTimestamp(string timestamp)
        {
            if (!DateTime.TryParse(timestamp, out DateTime dt))
            {
                throw new Exception("Timestamp 有誤");
            }

            double subSec = DateTime.Now.Subtract(dt).TotalSeconds;
            if (subSec > 15 || subSec < -15)
            {
                throw new Exception("Timestamp 誤差過大");
            }
        }

        private (long CertId, string PublicKey) getDefaultPucCert()
        {
            string url = "/api/member/Certificate/GetDefaultPucCert";

            var postResult = _httpClient.PostAsync(url, null).Result;
            string stringResult = postResult.Content.ReadAsStringAsync().Result;

            Console.WriteLine($"回傳：{stringResult}");

            JObject jObj = JObject.Parse(stringResult);
            int rtnCode = jObj.Value<int>("RtnCode");

            long certId = jObj.Value<long>("DefaultPubCertID");
            string publicKey = jObj.Value<string>("DefaultPubCert");

            return (certId, publicKey);
        }

        private (ExchangePucCertResult Result, string ClientPrivateKey) exchangePucCert()
        {
            var getDefaultPucCertResult = getDefaultPucCert();

            var key = _rsaCryptoHelper.GeneratePemKey();
            var result = callCertificateApi("/api/member/Certificate/ExchangePucCert",
                                 getDefaultPucCertResult.CertId,
                                 getDefaultPucCertResult.PublicKey,
                                 key.PrivateKey,
                                 new ExchangePucCertRequest
                                 {
                                     ClientPubCert = key.PublicKey,
                                     Timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                                 },
                                 "X-iCP-DefaultPubCertID");

            var apiResult = JsonConvert.DeserializeObject<AuthorizationApiEncryptResult>(result.Content);
            if (apiResult.RtnCode != 1)
            {
                throw new Exception(apiResult.RtnMsg);
            }

            _rsaCryptoHelper.ImportPemPrivateKey(key.PrivateKey);
            string json = _rsaCryptoHelper.Decrypt(apiResult.EncData);

            var exchangePucCertResult = JsonConvert.DeserializeObject<ExchangePucCertResult>(json);

            _rsaCryptoHelper.ImportPemPublicKey(exchangePucCertResult.ServerPubCert);
            bool isValid = _rsaCryptoHelper.VerifySignDataWithSha256(result.Content, result.Signature);
            if (!isValid)
            {
                throw new Exception("簽章驗證失敗");
            }

            checkTimestamp(exchangePucCertResult.Timestamp);

            _clientPrivateKey = key.PrivateKey;
            _clientPublicKey = key.PublicKey;
            _serverPublicKey = exchangePucCertResult.ServerPubCert;
            _serverPubCertID = exchangePucCertResult.ServerPubCertID;

            return (exchangePucCertResult, key.PrivateKey);
        }

        private void generateAES(long ServerPubCertID = 0, string ServerPubCert = null, string ClientPrivateKey = null)
        {
            if (ServerPubCertID == 0)
            {
                var exchangePucCertResult = exchangePucCert();

                ServerPubCertID = exchangePucCertResult.Result.ServerPubCertID;
                ServerPubCert = exchangePucCertResult.Result.ServerPubCert;
                ClientPrivateKey = exchangePucCertResult.ClientPrivateKey;
            }

            var result = callCertificateApi("/api/member/Certificate/GenerateAES",
                                 ServerPubCertID,
                                 ServerPubCert,
                                 ClientPrivateKey,
                                 new BaseAuthorizationApiRequest
                                 {
                                     Timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                                 },
                                 "X-iCP-ServerPubCertID");

            var apiResult = JsonConvert.DeserializeObject<AuthorizationApiEncryptResult>(result.Content);
            if (apiResult.RtnCode != 1)
            {
                throw new Exception(apiResult.RtnMsg);
            }

            _rsaCryptoHelper.ImportPemPrivateKey(ClientPrivateKey);
            string json = _rsaCryptoHelper.Decrypt(apiResult.EncData);

            var generateAesResult = JsonConvert.DeserializeObject<GenerateAesResult>(json);

            _rsaCryptoHelper.ImportPemPublicKey(ServerPubCert);
            bool isValid = _rsaCryptoHelper.VerifySignDataWithSha256(result.Content, result.Signature);
            if (!isValid)
            {
                throw new Exception("簽章驗證失敗");
            }

            checkTimestamp(generateAesResult.Timestamp);

            _aesClientCertId = generateAesResult.EncKeyID;
            _aesKey = generateAesResult.AES_Key;
            _aesIv = generateAesResult.AES_IV;
            _aesExpireDate = generateAesResult.ExpireDate;
        }
        #endregion

        private StreamContent CreateFileContent(Stream stream, string name, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = string.Format("\"{0}\"", name),
                FileName = string.Format("\"{0}\"", fileName)
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        public string AppRedirect(string env, string redirectUrl)
        {
            string host = "member";

            string address = getHostDomain(env, host);

            string url = "/api/member/Certificate/AppRedirect";

            string json = JsonConvert.SerializeObject(new
            {
                Timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Url = redirectUrl
            });

            _aesCryptoHelper.Key = _setting.AES_Key;
            _aesCryptoHelper.Iv = _setting.AES_IV;
            string encData = _aesCryptoHelper.Encrypt(json);

            _rsaCryptoHelper.ImportPemPrivateKey(_setting.ClientPrivateKey);
            string signature = _rsaCryptoHelper.SignDataWithSha256(encData);

            IDictionary<string, string> form = new Dictionary<string, string>();
            form.Add("EncKeyID", _setting.AES_CertId.ToString());
            form.Add("Signature", signature);
            form.Add("EncData", encData);

            string query = string.Join("&", form.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));

            return address + url + "?" + query;
        }
    }
}