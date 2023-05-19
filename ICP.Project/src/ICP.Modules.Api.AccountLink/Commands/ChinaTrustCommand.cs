using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models.ChinaTrust;
using ICP.Modules.Api.AccountLink.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Commands
{
    /// <summary>
    /// 中國信託 AccountLink
    /// </summary>
    class ChinaTrustCommand : BaseACLinkCommand
    {
        private readonly ChinaTrustService _chinaTrustService = null;
        private readonly ILogger _logger = null;

        public ChinaTrustCommand(
            ChinaTrustService chinaTrustService,
            ILogger<ChinaTrustCommand> logger
            )
        {
            _chinaTrustService = chinaTrustService;
            _logger = logger;
        }

        #region 申請連結帳號綁定
        /// <summary>
        /// 申請連結帳號綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkApply(string json)
        {
            _logger.Info($"[ACLinkApply] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkBindApply;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkApplyReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                ACLinkApplyReturnModel mockData = new ACLinkApplyReturnModel
                {
                    UserNo = parseRequestModel.RtnData.MID.ToString(),
                    ReturnCode = "0000",
                    ServiceCode = "0000",
                    ServiceMessage = "Success",
                    AuthId = "MOCK"
                };
                var mockResult = _chinaTrustService.GetACLinkApplyResult(mockData);
                result = JsonConvert.SerializeObject(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkApplyPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行申請綁定結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkApplyReturnData(resultData.RtnData);

            if (!aclinkReturnData.IsSuccess)
            {
                result = GetResult(aclinkReturnData);
                return result;
            }

            // 組成回傳資料
            var aclinkResult = _chinaTrustService.GetACLinkApplyResult(aclinkReturnData.RtnData);
            result = JsonConvert.SerializeObject(aclinkResult);

            #endregion

            return result;
        }
        #endregion

        #region 連結帳號綁定
        /// <summary>
        /// 連結帳號綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBind(string json)
        {
            _logger.Info($"[ACLink] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkBind;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkBindReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                BankType bankType = BankType.ChinaTrust;
                ACLinkReturnModel mockData = new ACLinkReturnModel
                {
                    UserNo = parseRequestModel.RtnData.MID.ToString(),
                    DebitAccount = parseRequestModel.RtnData.BankAccount,
                    ReturnCode = "0000",
                    ServiceCode = "0000",
                    ServiceMessage = "Success",
                    TrxNo = _chinaTrustService.GetMsgNo(bankType)
                };

                // 新增綁定資料
                var mockResult = _chinaTrustService.AddACLink(mockData);
                result = JsonConvert.SerializeObject(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行
            // 組成送出資料
            var postData = _chinaTrustService.ACLinkPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行綁定結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkReturnData(resultData.RtnData);
            if (!aclinkReturnData.IsSuccess)
            {
                result = GetResult(aclinkReturnData);
                return result;
            }

            #region 新增綁定資料

            // 新增綁定資料
            var addResult = _chinaTrustService.AddACLink(aclinkReturnData.RtnData);

            // 發送綁定成功通知
            if (addResult.IsSuccess)
            {
                
            }

            #endregion

            // 回傳資料
            result = JsonConvert.SerializeObject(addResult);

            #endregion

            return result;
        }
        #endregion

        #region 取消連結帳戶綁定
        /// <summary>
        /// 取消連結帳戶綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkCancel(string json)
        {
            _logger.Info($"[ACLinkCancel] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkCancel;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkCancelReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                // 更新綁定資料(取消綁定)
                var mockResult = _chinaTrustService.ACLinkCancelBind(parseRequestModel.RtnData.MID, parseRequestModel.RtnData.INDTAccount);
                result = JsonConvert.SerializeObject(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkCancelPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行取消綁定結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkCancelReturnData(resultData.RtnData);
            if (!aclinkReturnData.IsSuccess)
            {
                result = GetResult(aclinkReturnData);
                return result;
            }

            #region 更新綁定資料

            // 更新綁定資料(取消綁定)
            long mid = parseRequestModel.RtnData.MID;
            string indtAccount = parseRequestModel.RtnData.INDTAccount;
            var updateResult = _chinaTrustService.ACLinkCancelBind(mid, indtAccount);

            // 發送解除綁定通知
            if (updateResult.IsSuccess)
            {
                // ====> 放共用 (原本在member處理 改放acmiddleware一起處理)

            }

            #endregion

            // 回傳資料
            result = JsonConvert.SerializeObject(updateResult);

            #endregion

            return result;
        }
        #endregion

        #region 銀行端取消連結帳戶綁定
        /// <summary>
        /// 銀行端取消連結帳戶綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ChinaTrustCancelBind(string json)
        {
            _logger.Info($"[ChinaTrustCancelBind] {json}");

            // 參數定義
            string result = String.Empty;

            // 取得回傳資料
            var aclinkData = _chinaTrustService.GetACLinkCancelReturnData(json);
            if (!aclinkData.IsSuccess)
            {
                // 回傳中信處理結果 (0001:參數錯誤)
                result = JsonConvert.SerializeObject(new { ReturnCode = "0001" });
                _logger.Info($"[ChinaTrustCancelBind] 傳入參數錯誤:{json}");
                return result;
            }

            // 更新綁定資料(銀行端取消綁定)
            var updateResult = _chinaTrustService.ACLinkBankCancelBind(aclinkData.RtnData);

            // 回傳中信處理結果
            result = JsonConvert.SerializeObject(updateResult.RtnData);

            _logger.Info($"[ChinaTrustCancelBind] 回傳中信處理結果:{result}");

            return result;
        }
        #endregion

        #region 連結帳戶交易扣款
        /// <summary>
        /// 連結帳戶交易扣款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPay(string json)
        {
            _logger.Info($"[ACLinkPay] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkPay;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkPayReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 取得入帳虛擬帳號

            // 取得Payment的TradeID
            var getResult = _chinaTrustService.GetTradeID(parseRequestModel.RtnData.TradeNo);
            if (!getResult.IsSuccess)
            {
                result = GetResult(getResult);
                return result;
            }
            long tradeId = getResult.RtnData;

            // 取得入帳虛擬帳號
            var vAccountResult = _chinaTrustService.GetVirtualAccount(tradeId, parseRequestModel.RtnData.TradeAmt);
            if (!vAccountResult.IsSuccess)
            {
                result = GetResult(vAccountResult);
                return result;
            }
            string vAccount = vAccountResult.RtnData;

            // 將取得的虛擬帳號更新至Payment訂單
            var updateTradeResult = _chinaTrustService.UpdateTradeVirtualAccount(tradeId, vAccount);
            if (!updateTradeResult.IsSuccess)
            {
                result = GetResult(updateTradeResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if(_chinaTrustService.isMockBank())
            {
                var mockResult = _chinaTrustService.MockACLinkPayReturnData();
                result = GetResult(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkPayPostData(parseRequestModel.RtnData, vAccount);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行扣款] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行扣款結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkPayReturnData(resultData.RtnData, vAccount);

            // 回傳資料
            result = GetResult(aclinkReturnData);

            #endregion

            return result;
        }
        #endregion

        #region 連結帳戶儲值
        /// <summary>
        /// 連結帳戶儲值 (轉至ACLinkPay，中信儲值與扣款同一支API)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkDeposit(string json)
        {
            _logger.Info($"[ACLinkDeposit] {json}");

            return this.ACLinkPay(json);
        }
        #endregion

        #region 連結帳戶交易退款
        /// <summary>
        /// 連結帳戶交易退款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkRefund(string json)
        {
            _logger.Info($"[ACLinkRefund] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkRefund;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkRefundReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                var mockResult = _chinaTrustService.MockACLinkRefundReturnData();
                result = GetResult(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkRefundPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行退款結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkRefundReturnData(resultData.RtnData);

            // 回傳資料
            result = GetResult(aclinkReturnData);

            #endregion

            return result;
        }
        #endregion

        #region 提領金額至連結帳戶
        /// <summary>
        /// 提領金額至連結帳戶
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkWithdrawal(string json)
        {
            _logger.Info($"[ACLinkWithdrawal] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkWithdrawal;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkWithdrawalReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                var mockResult = new BaseResult { RtnCode = 1, RtnMsg = "Success" };
                result = GetResult(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkWithdrawalPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行提領] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行提領結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkWithdrawalReturnData(resultData.RtnData);

            // 回傳資料
            result = GetResult(aclinkReturnData);

            #endregion

            return result;
        }
        #endregion

        #region 銀行交易結果查詢
        /// <summary>
        /// 銀行交易結果查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPayQuery(string json)
        {
            _logger.Info($"[ACLinkPayQuery] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkPayQuery;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkPayQryReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行
            if (_chinaTrustService.isMockBank())
            {
                var mockResult = _chinaTrustService.MockACLinkPayReturnData();
                result = GetResult(mockResult);
                return result;
            }

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkPayQueryPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行扣款結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkPayReturnData(resultData.RtnData);

            // 回傳資料
            result = GetResult(aclinkReturnData);

            #endregion

            return result;
        }
        #endregion

        #region 連結綁定狀態查詢
        /// <summary>
        /// 連結綁定狀態查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBindQuery(string json)
        {
            _logger.Info($"[ACLinkQuery] {json}");

            // 參數定義
            ApiType apiType = ApiType.ACLinkBindQuery;     //api類型
            string result = String.Empty;

            #region 處理傳入資料

            // 取得傳入參數
            var parseRequestModel = _chinaTrustService.ParseToModel<ACLinkBindQryReq>(json);
            if (!parseRequestModel.IsSuccess)
            {
                result = GetResult(parseRequestModel);
                return result;
            }

            // 驗證參數
            var validateResult = _chinaTrustService.ValidateField(parseRequestModel.RtnData);
            if (!validateResult.IsSuccess)
            {
                result = GetResult(validateResult);
                return result;
            }

            #endregion

            #region 模擬銀行 (測試用)

            // 模擬銀行

            #endregion

            #region 送至銀行

            // 組成送出資料
            var postData = _chinaTrustService.ACLinkQueryPostData(parseRequestModel.RtnData);
            if (!postData.IsSuccess)
            {
                result = GetResult(postData);
                return result;
            }

            _logger.Info($"[送至銀行申請綁定] postData:{JsonConvert.SerializeObject(postData.RtnData)}");

            // 送出資料
            var postResult = PostToChinaTrust(apiType, postData.RtnData);
            if (!postResult.IsSuccess)
            {
                result = GetResult(postResult);
                return result;
            }

            _logger.Info($"[接收銀行綁定狀態查詢結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 回傳資料驗證 (解密及驗章)
            var resultData = _chinaTrustService.ValidateBankReturnData(apiType, postResult.RtnData);
            if (!resultData.IsSuccess)
            {
                result = GetResult(resultData);
                return result;
            }

            // 取得回傳資料
            var aclinkReturnData = _chinaTrustService.GetACLinkQueryReturnData(resultData.RtnData);

            // 回傳資料
            result = GetResult(aclinkReturnData);

            #endregion

            return result;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 送至中國信託
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public DataResult<ACLinkRes> PostToChinaTrust(ApiType apiType, object dataObj)
        {
            var result = new DataResult<ACLinkRes>();
            var rtnData = new ACLinkRes
            {
                ResMsgSign = "",
                ResAesKey = "",
                ResSecData = ""
            };
            result.SetSuccess(rtnData);

            _logger.Trace($"[PostToChinaTrust]開始準備送至中國信託: Data:{JsonConvert.SerializeObject(dataObj)}, apiType:{apiType.ToString()}");

            // 記錄送出資訊
            _chinaTrustService.AddChinaTrustSendLog(apiType, dataObj);

            #region 組成送出資訊

            // 取得AES key
            string aesKey = _chinaTrustService.GetRandomKey();

            // 取得簽章，並將簽章後的資料做AES加密
            var signResult = _chinaTrustService.GetSign(dataObj, aesKey);
            if (!signResult.IsSuccess)
            {
                result.SetCode(10019, signResult);
                _logger.Info($"簽章失敗: signResult:{JsonConvert.SerializeObject(signResult)}, dataObj:{JsonConvert.SerializeObject(dataObj)}, aesKey:{aesKey}");
                return result;
            }
            string reqMsgSign = signResult.RtnData;

            // AES key做RSA加密
            var rsaResult = _chinaTrustService.EncryptRSA(aesKey);
            if (!rsaResult.IsSuccess)
            {
                result.SetCode(10019, rsaResult);
                _logger.Info($"RSA加密失敗: rsaResult:{JsonConvert.SerializeObject(rsaResult)}, dataObj:{JsonConvert.SerializeObject(dataObj)}, aesKey:{aesKey}");
                return result;
            }
            string reqAesKey = rsaResult.RtnData;

            string reqMerchantId = _chinaTrustService.ACLinkMerchantID;
            string keyId = _chinaTrustService.ACLinkKeyId;
            string data = string.Empty;
            string requestUrl = _chinaTrustService.GetPostUrl(apiType);

            #endregion 

            string resMsgSign = string.Empty;
            string resAesKey = string.Empty;
            string resSecData = string.Empty;
            string receiveData = string.Empty;
            string currentDomain = string.Empty;

            // 建立HttpWebRequest物件
            HttpWebRequest httpWebRequest = null;
            Encoding encoding = Encoding.GetEncoding(65001);
            string contentType = "application/json";

            if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }

            try
            {
                httpWebRequest.CookieContainer = new CookieContainer();
                HttpCookieCollection httpCookies = System.Web.HttpContext.Current.Request.Cookies;
            }
            catch { }

            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; NeosBrowser; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpWebRequest.Accept = "text/html";
            httpWebRequest.Referer = currentDomain;
            httpWebRequest.ContentType = contentType;

            // 設定Header
            httpWebRequest.Headers.Set("Req-Msg-Sign", reqMsgSign);
            httpWebRequest.Headers.Set("Req-AESkey", reqAesKey);
            httpWebRequest.Headers.Set("Req-MerchantId", reqMerchantId);
            httpWebRequest.Headers.Set("KeyId", keyId);

            _logger.Trace($"[PostToChinaTrust] 送出資訊: requestUrl:{requestUrl}, Req-Msg-Sign:{reqMsgSign}, Req-AESkey:{reqAesKey}, Req-MerchantId:{reqMerchantId}, KeyId:{keyId}");

            try
            {
                using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sw.Write(data);
                    sw.Flush();
                    sw.Close();
                }

                // 取得Response結果
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                resMsgSign = httpWebResponse.GetResponseHeader("Res-Msg-Sign");
                resAesKey = httpWebResponse.GetResponseHeader("Res-AESKey");
                resSecData = httpWebResponse.GetResponseHeader("Res-SecData");
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                {
                    receiveData = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception exception)
            {
                receiveData = exception.ToString();
                result.SetError();
            }
            finally
            {
                _logger.Trace($"[PostToChinaTrust] 中國信託回傳結果: Res-Msg-Sign:{resMsgSign}, Res-AESkey:{resAesKey}, Req-SecData:{resSecData}, receiveData:{receiveData}");
            }

            rtnData = new ACLinkRes
            {
                ResMsgSign = resMsgSign,
                ResAesKey = resAesKey,
                ResSecData = resSecData
            };

            return result;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// 設定回傳RtnCode RtnMsg
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetResult(BaseResult result)
        {
            string jsonStr = JsonConvert.SerializeObject(result);
            return jsonStr;
        }

        /// <summary>
        /// 設定回傳json字串
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetResult(DataResult result)
        {
            string jsonStr = JsonConvert.SerializeObject(result);
            return jsonStr;
        }

        public override string ACLinkBindResult(string json)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
