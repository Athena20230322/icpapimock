using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.First;
using ICP.Modules.Api.AccountLink.Services;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Commands
{
    /// <summary>
    /// 第一銀行 AccountLink
    /// </summary>
    class FirstCommand : BaseACLinkCommand
    {
        private FirstService _firstService = null;
        private readonly ILogger _logger = null;

        public FirstCommand(
            FirstService firstService,
            ILogger<FirstCommand> logger
            )
        {
            _firstService = firstService;
            _logger = logger;
        }

        #region 帳號綁定&接收綁定結果
        /// <summary>
        /// 帳號綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBind(string json)
        {
            _logger.Info($"[ACLinkBind] {json}");

            ApiType apiType = ApiType.ACLinkBind;    //api類型
            ACLinkBindModel sourceModel = new ACLinkBindModel();
            ACLinkBindReq postReqModel = new ACLinkBindReq();
            ACLinkBindRes1 postResModel = new ACLinkBindRes1();
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseSourceResult = _firstService.ParseToModel<ACLinkBindModel>(json);
                if (!parseSourceResult.IsSuccess)
                {
                    return GetJsonResult(parseSourceResult);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseSourceResult.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseSourceResult.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkBindReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行綁定] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行綁定結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    // 取得傳入參數
                    var parseResData = _firstService.ParseToModel<ACLinkBindRes1>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    postResModel.S_KEY = "testS_KEY";
                    postResModel.LINK_URL = postReqModel.SUCC_URL;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                #region mock 新增綁定資料
                if (_firstService.isMockBank())
                {
                    ACLinkBindRes2 mockBindRes2 = new ACLinkBindRes2() {
                        MSG_NO = postResModel.MSG_NO,
                        RTN_CODE = postResModel.RTN_CODE,
                        RTN_MSG = postResModel.RTN_MSG,
                        EC_ID = postResModel.EC_ID,
                        EC_USER = postResModel.EC_USER,
                        INDT_ACNT = _firstService.GetINDTAccount(Library.Models.AccountLinkApi.Enums.BankType.First).Substring(2),
                        LINK_ACNT = _firstService.GetINDTAccount(Library.Models.AccountLinkApi.Enums.BankType.First).Substring(2),
                        LINK_GRAD = "A"
                    };

                    var addResult = _firstService.AddACLink(mockBindRes2);
                    if (!addResult.IsSuccess)
                    {
                        return GetJsonResult(addResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess();
                rtnResult.RtnData = new { URL = postResModel.LINK_URL };
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 處理銀行綁定結果回傳
        /// </summary>
        /// <param name="json"></param>
        public override string ACLinkBindResult(string json)
        {
            _logger.Info($"[ACLinkBindResult] {json}");

            ApiType apiType = ApiType.BindApiResult;    //api類型
            ACLinkBindRes2 postResModel = new ACLinkBindRes2();
            DataResult rtnResult = new DataResult();//最後回傳結果
            json = HttpUtility.UrlDecode(json);

            try
            {
                #region 處理傳入資料(postResModel)
                // 取得傳入參數
                var parseResModel = _firstService.ParseToModel<ACLinkBindRes2>(json);
                if (!parseResModel.IsSuccess)
                {
                    return GetJsonResult(parseResModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseResModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postResModel = parseResModel.RtnData;
                #endregion

                if (_firstService.isMockBank())
                {
                    #region mock postResModel
                    postResModel.INDT_ACNT = _firstService.GetINDTAccount(Library.Models.AccountLinkApi.Enums.BankType.First).Substring(2);
                    postResModel.LINK_ACNT = _firstService.GetINDTAccount(Library.Models.AccountLinkApi.Enums.BankType.First).Substring(2);
                    string _sRtn = _firstService.GetDigestStr(ApiType.BindApiResult, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);


                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }

                // 新增綁定資料
                var addResult = _firstService.AddACLink(postResModel);
                if (!addResult.IsSuccess)
                {
                    return GetJsonResult(addResult);
                }

                rtnResult.SetSuccess();
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 帳號取消綁定
        /// <summary>
        /// 帳號取消綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkCancel(string json)
        {
            _logger.Info($"[ACLinkCancel] {json}");

            ApiType apiType = ApiType.ACLinkCancel;     //api類型
            ACLinkCancelModel sourceModel = new ACLinkCancelModel();
            ACLinkCancelReq postReqModel = new ACLinkCancelReq();
            ACLinkCancelRes postResModel = new ACLinkCancelRes();
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkCancelModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkCancelReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行取消綁定] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行取消綁定結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    // 取得傳入參數
                    var parseResData = _firstService.ParseToModel<ACLinkCancelRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                // 更新綁定資料(取消綁定)
                var updateResult = _firstService.ACLinkCancelBind(sourceModel.MID, sourceModel.INDTAccount);
                if (!updateResult.IsSuccess)
                {
                    return GetJsonResult(updateResult);
                }

                rtnResult.SetSuccess();
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 綁定狀態查詢
        /// <summary>
        /// 綁定狀態查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBindQuery(string json)
        {
            _logger.Info($"[ACLinkBindQuery] {json}");

            ApiType apiType = ApiType.ACLinkBindQuery;     //api類型
            ACLinkBindQryModel sourceModel = new ACLinkBindQryModel();
            ACLinkBindQryReq postReqModel = new ACLinkBindQryReq();
            ACLinkBindQryRes postResModel = new ACLinkBindQryRes();
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkBindQryModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkBindQryReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行綁定狀態查詢] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行綁定狀態查詢結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkBindQryRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    postResModel.INDT_ACNT = parseRequestModel.RtnData.INDTAccount;
                    postResModel.LINK_ACNT = "123456";//parseRequestModel.RtnData.BankAccount;
                    postResModel.LINK_GRAD = "A";
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess();
                rtnResult.RtnData = JsonConvert.SerializeObject(new
                {
                    INDTAccount = postResModel.INDT_ACNT,
                    LINKAccount = postResModel.LINK_ACNT
                });
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 交易扣款
        /// <summary>
        /// 交易扣款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPay(string json)
        {
            _logger.Info($"[ACLinkPay] {json}");

            ApiType apiType = ApiType.ACLinkPay;     //api類型
            ACLinkPayModel sourceModel = new ACLinkPayModel();
            ACLinkPayReq postReqModel = new ACLinkPayReq();
            ACLinkPayRes postResModel = new ACLinkPayRes();
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkPayModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkPayReq(1, sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行扣款] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行扣款結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkPayRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess(new ACLinkResultModel() { BankTradeNo = postResModel.MSG_NO });

            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 交易儲值
        /// <summary>
        /// 交易儲值
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkDeposit(string json)
        {
            _logger.Info($"[ACLinkDeposit] {json}");

            ApiType apiType = ApiType.ACLinkDeposit;     //api類型
            ACLinkPayModel sourceModel = new ACLinkPayModel();
            ACLinkPayReq postReqModel = new ACLinkPayReq();
            ACLinkPayRes postResModel = new ACLinkPayRes();
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkPayModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkPayReq(3, sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行儲值扣款] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行儲值扣款結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkPayRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess(new ACLinkResultModel() { BankTradeNo = postResModel.MSG_NO });
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 交易退款
        /// <summary>
        /// 交易退款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkRefund(string json)
        {
            _logger.Info($"[ACLinkRefund] {json}");

            ApiType apiType = ApiType.ACLinkRefund;     //api類型
            ACLinkRefundModel sourceModel = new ACLinkRefundModel();
            ACLinkRefundReq postReqModel = new ACLinkRefundReq();
            ACLinkRefundRes postResModel = new ACLinkRefundRes();
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkRefundModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkRefundReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行退款] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行退款結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkRefundRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess(new ACLinkResultModel() { BankTradeNo = postResModel.MSG_NO });
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 交易提領
        /// <summary>
        /// 交易提領
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkWithdrawal(string json)
        {
            _logger.Info($"[ACLinkWithdrawal] {json}");

            ApiType apiType = ApiType.ACLinkWithdrawal;     //api類型
            ACLinkWithdrawModel sourceModel = new ACLinkWithdrawModel();
            ACLinkWithdrawReq postReqModel = new ACLinkWithdrawReq();
            ACLinkWithdrawRes postResModel = new ACLinkWithdrawRes();
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkWithdrawModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkWithdrawReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行提領] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    //post異常需反查交易是否成功
                    ACLinkPayQryModel qryModel = new ACLinkPayQryModel()
                    {
                        SerMsgNo = postReqModel.MSG_NO,
                        MID = sourceModel.MID,
                        IDNO = sourceModel.IDNO
                    };

                    var qryResult = PayQuery(qryModel);
                    if (qryResult.IsSuccess)
                    {
                        return GetJsonResult(qryResult);
                    }

                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行提領結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkWithdrawRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess(new ACLinkResultModel() { BankTradeNo = postResModel.MSG_NO });

            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        #region 交易查詢
        /// <summary>
        /// 交易查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPayQuery(string json)
        {
            _logger.Info($"[ACLinkPayQuery] {json}");

            ApiType apiType = ApiType.ACLinkPayQuery;     //api類型
            ACLinkPayQryModel sourceModel = new ACLinkPayQryModel();
            ACLinkPayQryReq postReqModel = new ACLinkPayQryReq();
            ACLinkPayQryRes postResModel = new ACLinkPayQryRes();
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 處理傳入資料(sourceModel)
                // 取得傳入參數
                var parseRequestModel = _firstService.ParseToModel<ACLinkPayQryModel>(json);
                if (!parseRequestModel.IsSuccess)
                {
                    return GetJsonResult(parseRequestModel);
                }

                // 驗證傳入參數
                var validateResult = _firstService.ValidateField(parseRequestModel.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                sourceModel = parseRequestModel.RtnData;
                #endregion

                #region 檢查api執行是否逾時
                var chkTimeoutResult = _firstService.CheckTimeout(sourceModel.Timestamp);
                if (!chkTimeoutResult.IsSuccess)
                {
                    return GetJsonResult(chkTimeoutResult);
                }
                #endregion

                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkPayQryReq(sourceModel);
                if (!postReqData.IsSuccess)
                {
                    return GetJsonResult(postReqData);
                }

                // 驗證送出資料
                validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行交易查詢] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    return GetJsonResult(postResult);
                }

                _logger.Info($"[接收銀行交易查詢結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkPayQryRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        return GetJsonResult(parseResData);
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return GetJsonResult(validateResult);
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return GetJsonResult(validateResult);
                    }
                }
                #endregion

                rtnResult.SetSuccess();
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);
                _logger.Warning(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
        #endregion

        /// <summary>
        /// 設定回傳RtnCode RtnMsg
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetJsonResult(BaseResult result)
        {
            string jsonStr = JsonConvert.SerializeObject(new BaseResult
            {
                RtnCode = result.RtnCode,
                RtnMsg = result.RtnMsg
            });

            return jsonStr;
        }

        /// <summary>
        /// 反查交易
        /// </summary>
        /// <param name="sourceModel"></param>
        /// <returns></returns>
        private BaseResult PayQuery(ACLinkPayQryModel model)
        {
            ApiType apiType = ApiType.ACLinkPayQuery;     //api類型
            ACLinkPayQryReq postReqModel = new ACLinkPayQryReq();
            ACLinkPayQryRes postResModel = new ACLinkPayQryRes();
            var result = new BaseResult();

            try
            {
                #region 處理送出資料(postReqModel)
                // 組成送出資料
                var postReqData = _firstService.ACLinkPayQryReq(model);
                if (!postReqData.IsSuccess)
                {
                    result.RtnCode = postReqData.RtnCode;
                    result.RtnMsg = postReqData.RtnMsg;
                    return result;
                }

                // 驗證送出資料
                var validateResult = _firstService.ValidateField(postReqData.RtnData);
                if (!validateResult.IsSuccess)
                {
                    return validateResult;
                }

                postReqModel = postReqData.RtnData;
                #endregion

                // 記錄送出資料
                _firstService.AddFirstSendLog(apiType, postReqModel);

                #region POST
                _logger.Info($"[送至銀行交易查詢] postData:{JsonConvert.SerializeObject(postReqModel)}");

                // 送出資料
                var postResult = _firstService.PostToBank(apiType, postReqModel, 950);
                if (!postResult.IsSuccess)
                {
                    result.RtnCode = postResult.RtnCode;
                    result.RtnMsg = postResult.RtnMsg;
                    return result;
                }

                _logger.Info($"[接收銀行交易查詢結果] {JsonConvert.SerializeObject(postResult)}");
                #endregion

                #region 處理回傳資料(postResModel)
                if (!_firstService.isMockBank())
                {
                    var parseResData = _firstService.ParseToModel<ACLinkPayQryRes>(postResult.RtnData);
                    if (!parseResData.IsSuccess)
                    {
                        result.RtnCode = parseResData.RtnCode;
                        result.RtnMsg = parseResData.RtnMsg;
                        return result;
                    }
                    postResModel = parseResData.RtnData;
                }
                else
                {
                    #region mock postResModel
                    postResModel.MSG_NO = postReqModel.MSG_NO;
                    postResModel.RTN_CODE = "0000";
                    postResModel.RTN_MSG = "執行成功, 無錯誤發生(test)";
                    postResModel.EC_ID = postReqModel.EC_ID;
                    postResModel.EC_USER = postReqModel.EC_USER;
                    string _sRtn = _firstService.GetDigestStr(apiType, postResModel);
                    postResModel.RTN_DIGEST = Infrastructure.Core.Utils.CryptUtil.sha256(_sRtn).ToLower();
                    #endregion
                }
                #endregion

                // 記錄接收資訊
                _firstService.AddFirstReceiveLog(apiType, postResModel);

                #region 驗證回傳資料(postResModel)
                validateResult = _firstService.ValidateField(postResModel);
                if (!validateResult.IsSuccess)
                {
                    return validateResult;
                }

                //驗證第一銀行回傳值資料
                if (!_firstService.isMockBank())
                {
                    validateResult = _firstService.ValidateBankResModel(apiType, postResModel);
                    if (!validateResult.IsSuccess)
                    {
                        return validateResult;
                    }
                }
                #endregion

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(7499);
                _logger.Warning(ex, result.RtnMsg, result);
            }

            return result;
        }
    }
}
