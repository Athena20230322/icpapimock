using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.AccountLinkApi;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.First;
using ICP.Modules.Api.AccountLink.Repositories;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Services
{
    class FirstService : ACLinkService
    {
        private readonly FirstACLinkRepository _firstACLinkRepository = null;
        private readonly ACLinkCommonService _aCLinkCommonService = null;
        private readonly LibMemberBankService _libMemberBankService = null;

        public FirstService(
            ACLinkRepository acLinkRepository,
            FirstACLinkRepository firstACLinkRepository,
            ACLinkCommonService aCLinkCommonService,
            LibMemberBankService libMemberBankService,
            ILogger<FirstService> logger
            )
        {
            _bankType = BankType.First;
            _acLinkRepository = acLinkRepository;
            _firstACLinkRepository = firstACLinkRepository;
            _aCLinkCommonService = aCLinkCommonService;
            _libMemberBankService = libMemberBankService;
            _logger = logger;
        }

        #region ACLinkBind - 帳號綁定
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindReq> ACLinkBindReq(ACLinkBindModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkBindReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            #endregion

            _logger.Trace($"準備組成連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkBindReq bindReq = new ACLinkBindReq
            {
                MSG_NO = msgNo,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                SUCC_URL = HttpUtility.UrlEncode(BindResultWebUrl),
                FAIL_URL = HttpUtility.UrlEncode(BindResultWebUrl),
                RSLT_URL = HttpUtility.UrlEncode(BindResultApiUrl),
                EC_ID = ACLinkECID,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(bindReq)}");

            result.SetSuccess(bindReq);
            return result;
        }

        /// <summary>
        /// 新增AccountLink綁定資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddACLink(ACLinkBindRes2 model)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備新增AccountLink綁定資料: {JsonConvert.SerializeObject(model)}");

            Int64.TryParse(model.EC_USER, out long mid);
            if (mid == 0)
            {
                result.SetCode(7403, new StringBuilder(model.EC_USER));
                _logger.Trace($"新增AccountLink綁定資料: MID錯誤, {JsonConvert.SerializeObject(model)}");
                return result;
            }

            //檢查訊息序號綁定申請記錄
            result = _firstACLinkRepository.CheckBindLog(ApiType.ACLinkBind.ToString(), model.MSG_NO, mid.ToString());
            if (result.RtnCode != 1)
            {
                return result;
            }

            // 寫入綁定資料
            var addModel = new ACLinkAddModel
            {
                MID = mid,
                INDTAccount = model.INDT_ACNT,
                BankCode = string.Format("{0:000}", (int)_bankType),
                BankAccount = model.LINK_ACNT,
                MsgNo = model.MSG_NO,
                Status = model.RTN_CODE == "0000" ? 1 : 3 //1:綁定 2:解綁 3:驗證失敗
            };

            result = AddACLinkInfo(addModel);

            _logger.Trace($"綁定資料新增結果: {JsonConvert.SerializeObject(result)}");

            // 更新會員銀行帳號驗證狀態
            var bankAccountModel = new UpdateBankAccountStatusModel
            {
                MID = mid,
                Category = 1,
                BankCode = string.Format("{0:000}", (int)_bankType),
                BankAccount = model.LINK_ACNT,
                AccountStatus = (byte)(result.RtnCode == 1 ? 1 : 2),
                INDTAccount = model.INDT_ACNT,
            };

            _logger.Trace($"準備更新會員銀行帳號驗證狀態: {JsonConvert.SerializeObject(bankAccountModel)}");

            var updateResult = _libMemberBankService.UpdateMemberBankAccountStatus(bankAccountModel);
            result.RtnCode = updateResult.RtnCode;
            result.RtnMsg = updateResult.RtnMsg;

            _logger.Trace($"會員銀行帳號驗證狀態更新結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region  ACLinkCancel - 帳號取消綁定
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkCancelReq> ACLinkCancelReq(ACLinkCancelModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkCancelReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+帳號識別碼+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.INDTAccount + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成取消連結帳戶綁定送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkCancelReq cancelReq = new ACLinkCancelReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                INDT_ACNT = model.INDTAccount,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(cancelReq)}");

            result.SetSuccess(cancelReq);

            return result;
        }

        /// <summary>
        /// 更新AccountLink綁定資料(取消綁定)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BaseResult ACLinkCancelBind(long MID, string INDTAccount)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備更新AccountLink綁定資料(取消綁定): MID:{MID}, INDTAccount:{INDTAccount}");

            // 更新綁定資料 - 取消綁定
            var addModel = new ACLinkCancelDbReq
            {
                MID = MID,
                INDTAccount = INDTAccount,
                BankCode = string.Format("{0:000}", (int)_bankType)
            };

            result = _acLinkRepository.CancelAccountLink(addModel);

            _logger.Trace($"取消綁定資料更新結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region ACLinkBindQuery - 綁定狀態查詢
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindQryReq> ACLinkBindQryReq(ACLinkBindQryModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkBindQryReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+查詢訊息序號+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.SerMsgNo + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成連結綁定狀態查詢送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkBindQryReq bindQryReq = new ACLinkBindQryReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                SER_MSG_NO = model.SerMsgNo,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(bindQryReq)}");

            result.SetSuccess(bindQryReq);

            return result;
        }
        #endregion

        #region ACLinkPay - 交易扣款or儲值扣款
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkPayReq> ACLinkPayReq(int tradeType, ACLinkPayModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string payeeAccount = string.Empty; //轉入虛擬帳戶
            var result = new DataResult<ACLinkPayReq>();
            result.SetError();

            #region 取得轉入虛擬帳號
            var payeeAccountResult = GetPayeeAccount(model);
            if (!payeeAccountResult.IsSuccess)
            {
                result.SetCode(7413);
                return result;
            }
            payeeAccount = payeeAccountResult.VirtualAccount;
            #endregion

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+交易時間+訂單編號+交易金額+帳號識別碼+轉入虛擬帳號+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.TradeTime + model.TradeNo + model.TradeAmt.ToString() + model.INDTAccount + payeeAccount + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成扣款送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkPayReq payReq = new ACLinkPayReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                TRNS_TIME = model.TradeTime,
                TRNS_NO = model.TradeNo,
                TRNS_NOTE = "ICash扣",
                TRNS_AMT = model.TradeAmt,
                INDT_ACNT = model.INDTAccount,
                PAYEE_ACNT = payeeAccount,
                PAY_TYPE = (tradeType == 1) ? "P" : "D",
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(payReq)}");

            result.SetSuccess(payReq);

            return result;
        }

        /// <summary>
        /// 取得轉入虛擬帳號
        /// </summary>
        /// <param name="model"></param>
        public VirtualAccountModel GetPayeeAccount(ACLinkPayModel model)
        {
            var obj = new
            {
                PaymentTypeID = 2,
                PaymentSubTypeID= 1,//,_aCLinkCommonService.GetACLinkBankInfo("007").PaymentSubTypeID,
                Amount = model.TradeAmt,
                TradeID = model.TradeID,
                VirtualAccount = ""
            };

            return _firstACLinkRepository.GetPayeeAccount(obj);
        }
        #endregion

        #region ACLinkRefund - 交易退款
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkRefundReq> ACLinkRefundReq(ACLinkRefundModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkRefundReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+退款交易時間+退款(原)訂單編號+退款金額+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.RefundTime + model.TradeNo + model.RefundAmt.ToString() + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成交易扣款送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkRefundReq refundReq = new ACLinkRefundReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                REFUND_TIME = model.RefundTime,
                TRNS_NO = model.TradeNo,
                REFUND_NOTE = "ICash退款",// model.RefundNote,
                REFUND_AMT = model.RefundAmt,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(refundReq)}");

            result.SetSuccess(refundReq);

            return result;
        }
        #endregion

        #region ACLinkWithdrawal - 交易提領
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkWithdrawReq> ACLinkWithdrawReq(ACLinkWithdrawModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkWithdrawReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號+平台代碼+平台會員代號+使用者身分證號+提領交易時間+提領金額+帳戶識別碼+簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.TradeTime + model.Amount.ToString() + model.INDTAccount + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成交易提領送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkWithdrawReq withdrawReq = new ACLinkWithdrawReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                TRNS_TIME = model.TradeTime,
                TRNS_NOTE = "ICash提領",//model.TradeNote,
                TRNS_AMT = model.Amount,
                INDT_ACNT = model.INDTAccount,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(withdrawReq)}");

            result.SetSuccess(withdrawReq);

            return result;
        }
        #endregion

        #region ACLinkPayQuery - 交易查詢
        /// <summary>
        /// 組成送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkPayQryReq> ACLinkPayQryReq(ACLinkPayQryModel model)
        {
            string msgNo = GetMsgNo(_bankType); //訊息序號
            string signTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = new DataResult<ACLinkPayQryReq>();
            result.SetError();

            #region 簽章設定
            //訊息序號 + 平台代碼 + 平台會員代號 + 使用者身分證號 + 查詢訊息序號 + 簽章日期時間
            var preSignStr = msgNo + ACLinkECID + model.MID.ToString() + model.IDNO + model.SerMsgNo + signTime;

            var signResult = Sign(preSignStr);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            };
            #endregion

            _logger.Trace($"準備組成交易查詢送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkPayQryReq payQryReq = new ACLinkPayQryReq
            {
                MSG_NO = msgNo,
                EC_ID = ACLinkECID,
                EC_USER = model.MID.ToString(),
                CUST_ID = model.IDNO,
                SER_MSG_NO = model.SerMsgNo,
                CERT_SN = HSMCERTSN,
                SIGN_TIME = signTime,
                SIGN_VALUE = GetHexadecimal(signResult.RtnData.ToString())
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(payQryReq)}");

            result.SetSuccess(payQryReq);

            return result;
        }
        #endregion

        #region 回傳資料處理
        /// <summary>
        /// 驗證第一銀行回傳值資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BaseResult ValidateBankResModel(ApiType apiType, object obj)
        {
            BaseResult result = new BaseResult();
            result.SetError();
            dynamic iData = obj;

            _logger.Trace($"驗證開始: {JsonConvert.SerializeObject(obj)}");

            string preDigestStr = GetDigestStr(apiType, obj); //取得回傳訊息雜湊值
            BaseResult validateResult = ValidateBankHash(iData.RTN_DIGEST, preDigestStr);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode);
                _logger.Trace($"銀行回傳結果: 訊息雜湊值驗證失敗 , {JsonConvert.SerializeObject(obj)}");
                return result;
            }

            if (iData.EC_ID != ACLinkECID)
            {
                result.SetCode(7403, new StringBuilder(iData.EC_ID));
                _logger.Trace($"銀行回傳結果: 失敗 , {JsonConvert.SerializeObject(obj)}");
                return result;
            }

            Int64.TryParse(iData.EC_USER, out long mid);
            if (mid == 0)
            {
                result.SetCode(7403, new StringBuilder(iData.EC_USER));
                _logger.Trace($"銀行回傳結果: 失敗 , {JsonConvert.SerializeObject(obj)}");
                return result;
            }

            if (iData.RTN_CODE != "0000")
            {
                //取得第一銀行回傳對應訊息
                result = GetBankRtnCode(apiType, iData.RTN_CODE, iData.RTN_MSG);
                _logger.Trace($"銀行回傳結果: 失敗 , {JsonConvert.SerializeObject(obj)}");
                return result;
            }

            _logger.Trace($"驗證完成: {JsonConvert.SerializeObject(obj)}");

            result.SetSuccess();

            return result;
        }

        /// <summary>
        /// 取得回傳訊息雜湊值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetDigestStr(ApiType apiType, object obj)
        {
            dynamic iData = obj;
            string preDigestStr = string.Empty;

            switch (apiType)
            {
                case ApiType.BindApiResult:
                    //訊息序號+回應代碼+平台代碼+平台會員代號+帳戶識別碼+綁定實體帳戶+身份認證等級
                    preDigestStr = string.Format("{0}{1}{2}{3}{4}{5}{6}", iData.MSG_NO, iData.RTN_CODE, iData.EC_ID, iData.EC_USER, iData.INDT_ACNT, iData.LINK_ACNT, iData.LINK_GRAD);
                    break;
                case ApiType.ACLinkBindQuery:
                    //訊息序號+回應代碼+平台代碼+平台會員代號+帳戶識別碼+綁定實體帳戶+身份認證等級
                    preDigestStr = string.Format("{0}{1}{2}{3}{4}{5}{6}", iData.MSG_NO, iData.RTN_CODE, iData.EC_ID, iData.EC_USER, iData.INDT_ACNT, iData.LINK_ACNT, iData.LINK_GRAD);
                    break;
                default:
                    //訊息序號+回應代碼+平台代碼+平台會員代號
                    preDigestStr = string.Format("{0}{1}{2}{3}", iData.MSG_NO, iData.RTN_CODE, iData.EC_ID, iData.EC_USER);
                    break;
            }

            return preDigestStr;
        }

        /// <summary>
        /// 取得第一銀行回傳對應訊息
        /// </summary>
        /// <param name="rtnCode"></param>
        /// <param name="rtnMsg"></param>
        private BaseResult GetBankRtnCode(ApiType apiType, string rtnCode, string rtnMsg)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            if (rtnCode == "5023") //連結帳戶已綁定
            {
                result.SetCode(7441);
            }
            else if (rtnCode == "5025") //查無連結帳戶綁定記錄
            {
                result.SetCode(7442);
            }
            else
            {
                result.SetCode(7440, GetApiName(apiType));
            }

            return result;
        }
        #endregion

        #region Log
        /// <summary>
        /// 記錄送出資料DB Log
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="obj"></param>
        public void AddFirstSendLog(ApiType apiType, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            ACLinkSendLogDbReq reqModel = JsonConvert.DeserializeObject<ACLinkSendLogDbReq>(data);
            reqModel.ApiType = apiType.ToString();

            _firstACLinkRepository.AddFirstSendLog(reqModel);
        }

        /// <summary>
        /// 記錄接收資料DB Log
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="obj"></param>
        public void AddFirstReceiveLog(ApiType apiType, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            ACLinkReceiveLogDbReq reqModel = JsonConvert.DeserializeObject<ACLinkReceiveLogDbReq>(data);
            reqModel.ApiType = apiType.ToString();

            _firstACLinkRepository.AddFirstReceiveLog(reqModel);
        }
        #endregion

        /// <summary>
        /// 驗證一銀RTN_DIGEST
        /// </summary>
        /// <param name="digestHash"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private BaseResult ValidateBankHash(string digestHash, string data)
        {
            _logger.Trace($"驗證Hash-Before: {data}");

            var result = new BaseResult();
            StringBuilder digestSB = new StringBuilder();
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
            byte[] source = Encoding.ASCII.GetBytes(data);//將字串轉為Byte[]
            byte[] hashbytes = sha256.ComputeHash(source);//進行SHA256加密

            foreach (var item in hashbytes)
            {
                digestSB.Append(item.ToString("X2"));
            }

            if (digestHash.ToLower().Equals(digestSB.ToString().ToLower()))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7405);//資料驗證失敗
            }

            _logger.Trace($"驗證Hash-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
