using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Services.AccountLinkApi;
using ICP.Modules.Api.Member.Models.ACLink;
using ICP.Modules.Api.Member.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICP.Modules.Api.Member.Services
{
    public class ACLinkService
    {
        private ACLinkRepository _accountLinkRepository = null;
        private ACLinkCommonService _commonService = null;
        private readonly ILogger _logger = null;

        public ACLinkService(
            ACLinkRepository accountLinkRepository,
            ACLinkCommonService aCLinkCommonService,
            ILogger<ACLinkService> logger)
        {
            _accountLinkRepository = accountLinkRepository;
            _commonService = aCLinkCommonService;
            _logger = logger;
        }

        #region 綁定AccountLink帳號
        /// <summary>
        /// 驗證綁定AccountLink欄位邏輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindRes> ValidateACLinkBind(ACLinkBindModel model)
        {
            var result = new DataResult<ACLinkBindRes>();

            // 驗證銀行帳號: 中國信託
            if (model.BankCode == "822")
            {
                if (model.BindFlag == "Y")
                {
                    if (string.IsNullOrWhiteSpace(model.BankAccount))
                    {
                        result.SetCode(7405, model);
                        _logger.Trace($"欄位驗證不通過: BankAccount={model.BankAccount}");
                        return result;
                    }

                    // 驗證該會員是否已綁過同組銀行帳號
                    var oACLinkList = _accountLinkRepository.ListBindACLink(model.MID);
                    string bankCode = model.BankCode;
                    string bankAccount = model.BankAccount;
                    int bindingCount = oACLinkList.Where(x => x.BankCode == bankCode && x.BankAccount == bankAccount).Count();
                    if (bindingCount > 0)
                    {
                        result.SetCode(7441, model);
                        _logger.Trace($"此銀行帳號已綁定: BankAccount={model.BankAccount}");
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.Birth))
                    {
                        result.SetCode(7405, model);
                        _logger.Trace($"欄位驗證不通過: Birth={model.Birth}");
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.AuthId))
                    {
                        result.SetCode(7405, model);
                        _logger.Trace($"欄位驗證不通過: AuthId={model.AuthId}");
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.Otp))
                    {
                        result.SetCode(7405, model);
                        _logger.Trace($"欄位驗證不通過: Otp={model.Otp}");
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(model.AgreeTime))
                    {
                        result.SetCode(7405, model);
                        _logger.Trace($"欄位驗證不通過: AgreeTime={model.AgreeTime}");
                        return result;
                    }
                    result.SetSuccess();
                }
                else
                {
                    // 取得中國信託申請綁定網址
                    string bindUrl = $"{GlobalConfigUtil.Host_Member_Domain}" + "/ACLink/ChinaTrustTermsPage";
                    ACLinkBindRes rtnModel = new ACLinkBindRes { URL = bindUrl };
                    result.SetSuccess(rtnModel);
                }
            }
            else
            {
                result.SetSuccess();
            }

            return result;
        }

        /// <summary>
        /// 取得連結帳號綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkBindPostData(ACLinkBindModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            model.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var json = new
            {
                model.MID,
                model.IDNO,
                model.BankAccount,
                model.Birth,
                model.TimeStamp,
                model.AuthId,
                model.Otp
            };
            var postData = new { Json = JsonConvert.SerializeObject(json), BankType = model.BankCode };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(postData)}");

            result.SetSuccess(postData);

            return result;
        }

        /// <summary>
        /// 取得連結帳號綁定回傳資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindRes> ACLinkBindReturnData(object obj)
        {
            var result = new DataResult<ACLinkBindRes>();
            result.SetError();

            _logger.Trace($"準備組成連結帳號綁定回傳資料: {JsonConvert.SerializeObject(obj)}");

            dynamic iData = obj;
            string json = iData.Json;
            DataResult<ACLinkBindRes> rtnData = JsonConvert.DeserializeObject<DataResult<ACLinkBindRes>>(json);
            result.RtnCode = rtnData.RtnCode;
            result.RtnMsg = rtnData.RtnMsg;
            if (rtnData.RtnData != null)
            {
                result.RtnData = new ACLinkBindRes { URL = rtnData.RtnData.URL, ServiceCode = rtnData.RtnData.ServiceCode };
            }

            _logger.Trace($"成功組成回傳資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 申請綁定AccountLink帳號
        /// <summary>
        /// 驗證申請綁定AccountLink欄位邏輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ValidateACLinkApply(ACLinkBindModel model)
        {
            var result = new DataResult<object>();

            // 驗證銀行帳號: 中國信託
            if (model.BankCode == "822")
            {
                if (string.IsNullOrWhiteSpace(model.BankAccount))
                {
                    result.SetCode(7405, model);
                    _logger.Trace($"欄位驗證不通過: BankAccount={model.BankAccount}");
                    return result;
                }

                // 驗證該會員是否已綁過同組銀行帳號
                var oACLinkList = _accountLinkRepository.ListBindACLink(model.MID);
                string bankCode = model.BankCode;
                string bankAccount = model.BankAccount;
                int bindingCount = oACLinkList.Where(x => x.BankCode == bankCode && x.BankAccount == bankAccount).Count();
                if (bindingCount > 0)
                {
                    result.SetCode(7441, model);
                    _logger.Trace($"此銀行帳號已綁定: BankAccount={model.BankAccount}");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(model.Birth))
                {
                    result.SetCode(7405, model);
                    _logger.Trace($"欄位驗證不通過: Birth={model.Birth}");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(model.AgreeTime))
                {
                    result.SetCode(7405, model);
                    _logger.Trace($"欄位驗證不通過: AgreeTime={model.AgreeTime}");
                    return result;
                }
            }

            result.SetSuccess();

            return result;
        }

        /// <summary>
        /// 取得申請連結帳號綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkApplyPostData(ACLinkBindModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成申請連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            model.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var json = new
            {
                model.MID,
                model.IDNO,
                model.BankAccount,
                model.Birth,
                model.TimeStamp,
                model.AgreeTime
            };
            var postData = new { Json = JsonConvert.SerializeObject(json), BankType = model.BankCode };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(postData)}");

            result.SetSuccess(postData);

            return result;
        }

        /// <summary>
        /// 取得申請連結帳號綁定回傳資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<ACLinkApplyRes> ACLinkApplyReturnData(object obj)
        {
            var result = new DataResult<ACLinkApplyRes>();
            result.SetError();

            _logger.Trace($"準備組成申請連結帳號綁定回傳資料: {JsonConvert.SerializeObject(obj)}");

            dynamic iData = obj;
            string json = iData.Json;
            DataResult<ACLinkApplyRes> rtnData = JsonConvert.DeserializeObject<DataResult<ACLinkApplyRes>>(json);
            result.RtnCode = rtnData.RtnCode;
            result.RtnMsg = rtnData.RtnMsg;
            result.RtnData = new ACLinkApplyRes
            {
                AuthId = rtnData.RtnData.AuthId,
                ServiceCode = rtnData.RtnData.ServiceCode,
                ServiceMessage = rtnData.RtnData.ServiceMessage
            };

            _logger.Trace($"成功組成回傳資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 取消綁定AccountLink帳號
        /// <summary>
        /// 取得連結帳號綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkCancelPostData(ACLinkBindModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成取消連結帳號送出資料: {JsonConvert.SerializeObject(model)}");

            // 取得連結帳號資訊
            ACLinkModel acLinkInfo = _accountLinkRepository.GetACLinkInfo(model.MID, model.AccountID);
            if (acLinkInfo == null || string.IsNullOrWhiteSpace(acLinkInfo.BankAccount))
            {
                result.SetCode(7404);
                _logger.Trace($"取得銀行帳號失敗");
                return result;
            }

            _logger.Trace($"取得連結帳號資訊: {JsonConvert.SerializeObject(acLinkInfo)}");

            model.INDTAccount = acLinkInfo.INDTAccount;
            model.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var json = new
            {
                model.MID,
                model.IDNO,
                model.INDTAccount,
                model.TimeStamp
            };
            var postData = new { json = JsonConvert.SerializeObject(json), BankType = model.BankCode };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(postData)}");

            result.SetSuccess(postData);

            return result;
        }

        /// <summary>
        /// 取得取消連結帳號回傳資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BaseResult ACLinkCancelReturnData(object obj)
        {
            var result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備組成取消連結帳號回傳資料: {JsonConvert.SerializeObject(obj)}");

            dynamic iData = obj;
            string json = iData.Json;
            BaseResult rtnData = JsonConvert.DeserializeObject<BaseResult>(json);
            result.RtnCode = rtnData.RtnCode;
            result.RtnMsg = rtnData.RtnMsg;

            _logger.Trace($"成功組成回傳資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 提領電支帳戶金額至約定銀行帳戶
        /// <summary>
        /// 提領電支帳戶金額至約定銀行帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkWithdrawalPostData(ACLinkWithdrawalReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成提領至連結帳戶送出資料: {JsonConvert.SerializeObject(model)}");

            var json = new
            {
                model.MID,
                model.IDNO,
                model.INDTAccount,
                model.Amount,
                model.TradeNo,
                TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        };
            var postData = new { json = JsonConvert.SerializeObject(json), BankType = model.BankCode };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(postData)}");

            result.SetSuccess(postData);

            return result;
        }

        /// <summary>
        /// 取得提領回傳資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BaseResult ACLinkWithdrawalReturnData(object obj)
        {
            var result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備組成提領回傳資料: {JsonConvert.SerializeObject(obj)}");

            dynamic iData = obj;
            string json = iData.Json;
            BaseResult rtnData = JsonConvert.DeserializeObject<BaseResult>(json);
            result.RtnCode = rtnData.RtnCode;
            result.RtnMsg = rtnData.RtnMsg;

            _logger.Trace($"成功組成回傳資料: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region 取得銀行快附設定
        /// <summary>
        /// 取得銀行快附設定
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public ACLinkBankSetting GetACLinkBankSetting(string BankCode)
        {
            return _accountLinkRepository.GetACLinkBankSetting(BankCode);
        }
        #endregion

        #region 共用
        /// <summary>
        /// Json轉Model
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public DataResult<T> ParseToModel<T>(string jsonData)
        {
            _logger.Trace($"反序列化-Before: {nameof(jsonData)}，長度 = {jsonData?.Length}");

            var result = new DataResult<T>();

            T data = jsonData.TryParseJsonToObj(out T obj) ? obj : default(T);

            if (data != null)
            {
                result.SetSuccess(data);
            }
            else
            {
                result.SetCode(7400);//資料轉換失敗
            }

            _logger.Trace($"反序列化-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 驗證欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">傳入需驗證的物件</param>
        /// <returns></returns>
        public BaseResult ValidateField<T>(T model)
        {
            _logger.Trace($"驗證欄位-Before: {JsonConvert.SerializeObject(model)}");

            var result = new BaseResult();

            StringBuilder errorMsg = new StringBuilder();
            List<string> errList = new List<string>();

            errList.AddRange(ServerValidator.Validate(model));

            foreach (var item in errList)
            {
                errorMsg.Append(item.ToString() + " ");
            }

            if (string.IsNullOrWhiteSpace(errorMsg.ToString()))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7403, errorMsg);//xxx欄位驗證失敗
            }

            _logger.Trace($"驗證欄位-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得銀行帳號(只顯示後5碼，前面隱碼)
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="indtAccount"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public string GetLinkAccount(long mid, long accountId)
        {
            string result = string.Empty;

            result = _commonService.GetBankAccountMask(this.GetBankAccount(mid, accountId));

            return result;
        }

        /// <summary>
        /// 取得銀行帳號
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="indtAccount"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public string GetBankAccount(long mid, long accountId)
        {
            string result = string.Empty;

            // 取得銀行帳號
            ACLinkModel acLinkInfo = _accountLinkRepository.GetACLinkInfo(mid, accountId);
            if (acLinkInfo == null || string.IsNullOrWhiteSpace(acLinkInfo.BankAccount))
            {
                _logger.Trace($"取得銀行帳號失敗");
                return result;
            }

            result = acLinkInfo.BankAccount;

            return result;
        }
        #endregion
    }
}
