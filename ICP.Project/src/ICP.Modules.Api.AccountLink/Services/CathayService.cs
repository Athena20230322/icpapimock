using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models.Cathay;
using ICP.Modules.Api.AccountLink.Repositories;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 國泰世華 AccountLink
    /// </summary>
    class CathayService : ACLinkService
    {
        private readonly CathayACLinkRepository _cathayACLinkRepository = null;

        public CathayService(
            ACLinkRepository acLinkRepository,
            CathayACLinkRepository cathayACLinkRepository,
            ILogger<CathayService> logger
            )
        {
            _bankType = BankType.Cathay;
            _acLinkRepository = acLinkRepository;
            _cathayACLinkRepository = cathayACLinkRepository;
            _logger = logger;
        }

        #region 建立請求Model

        /// <summary>
        /// 建立綁定Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateBindModel")]
        public DataResult<BankBindReq> CreateBindModel(ApiType apiType, ACLinkBindModel model)
        {
            _logger.Trace($"建立綁定資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankBindReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)

            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.IDNO;//會員身份證字號(加密前) (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            //string sReplyApiURL = HttpUtility.UrlEncode(GetPostUrl(ApiType.BindApiResult));//綁定結果通知
            //string ReplyWebURL = HttpUtility.UrlEncode(GetPostUrl(ApiType.BindWebResult));//綁定後導回頁
            string sReplyApiURL = GetPostUrl(ApiType.BindApiResult);//綁定結果通知
            string ReplyWebURL = GetPostUrl(ApiType.BindWebResult);//綁定後導回頁

            #endregion

            BankBindReq oReqModel = new BankBindReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                sign = sSign,
                replyApiURL = sReplyApiURL,
                replyWebURL = ReplyWebURL
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立綁定資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立綁定結果Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateBindResultModel")]
        public DataResult<BankBindReplyRes> CreateBindResultModel(ApiType apiType, BankBindReplyReq model)
        {
            _logger.Trace($"建立綁定結果資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankBindReplyRes>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sReturncode = model.header.returncode;//回覆值
            string sReturndesc = model.header.returndesc;//回覆說明
            string sTxnseq = model.header.txnseq;//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            string sFuseId = model.header.fuseID;//CUB交易序號

            #endregion

            BankBindReplyRes oReqModel = new BankBindReplyRes
            {
                Header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = sReturncode,
                    returndesc = sReturndesc,
                    txnseq = sTxnseq,
                    fuseID = sFuseId
                }
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立綁定結果資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立解綁Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateUnBindModel")]
        public DataResult<BankUnbindReq> CreateUnbindModel(ApiType apiType, ACLinkCancelModel model)
        {
            _logger.Trace($"建立解綁資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankUnbindReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//取消綁定之銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.IDNO;//會員身份證字號(加密前) (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankUnbindReq oReqModel = new BankUnbindReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立解綁資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立付款Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreatePayModel")]
        public DataResult<BankPayReq> CreatePayModel(ApiType apiType, ACLinkPayModel model)
        {
            _logger.Trace($"建立付款資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankPayReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sOrderNo = model.TradeNo;//訂單號碼
            int iTxnAmt = model.TradeAmt;//交易金額
            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.BankAccount + sOrderNo + iTxnAmt;//銀行帳號(加密前)+訂單號碼+交易金額 (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankPayReq oReqModel = new BankPayReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                orderNo = sOrderNo,
                txnAmt = iTxnAmt,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立付款資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立儲值Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateDepositModel")]
        public DataResult<BankDepositReq> CreateDepositModel(ApiType apiType, ACLinkDepositModel model)
        {
            _logger.Trace($"建立儲值資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankDepositReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sOrderNo = model.TradeNo;//訂單號碼
            int iTxnAmt = model.TradeAmt;//交易金額
            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.BankAccount + sOrderNo + iTxnAmt;//銀行帳號(加密前)+訂單號碼+交易金額 (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankDepositReq oReqModel = new BankDepositReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                orderNo = sOrderNo,
                txnAmt = iTxnAmt,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立儲值資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立退款Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateRefundModel")]
        public DataResult<BankRefundReq> CreateRefundModel(ApiType apiType, ACLinkRefundModel model)
        {
            _logger.Trace($"建立退款資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankRefundReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sOrderNo = model.TradeNo;//訂單號碼
            string sOrgOrderNo = model.TradeNo;//原訂單號碼
            int iTxnAmt = model.RefundAmt;//退款金額
            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.BankAccount + sOrderNo + iTxnAmt;//銀行帳號(加密前)+訂單號碼+退款金額 (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankRefundReq oReqModel = new BankRefundReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                orderNo = sOrderNo,
                org_OrderNo = sOrgOrderNo,
                txnAmt = iTxnAmt,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立退款資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立提領Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateRestoreModel")]
        public DataResult<BankRestoreReq> CreateRestoreModel(ApiType apiType, ACLinkWithdrawalModel model)
        {
            _logger.Trace($"建立提領資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankRestoreReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            int iTxnAmt = model.Amount;//提領金額
            string sOrderNo = model.TradeNo;//訂單號碼
            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.BankAccount + sOrderNo + iTxnAmt;//銀行帳號(加密前)+訂單號碼+提領金額 (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankRestoreReq oReqModel = new BankRestoreReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                txnAmt = iTxnAmt,
                orderNo = sOrderNo,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立提領資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立綁定查詢Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreateBindQryModel")]
        public DataResult<BankBindQryReq> CreateBindQryModel(ApiType apiType, ACLinkBindQryModel model)
        {
            _logger.Trace($"建立綁定查詢資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankBindQryReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.IDNO;//會員身份證字號(加密前) (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankBindQryReq oReqModel = new BankBindQryReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立綁定查詢資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        /// <summary>
        /// 建立交易查詢Model
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [LogRequest(LogTextResponse = true, Name = "CreatePayQryModel")]
        public DataResult<BankPayQryReq> CreatePayQryModel(ApiType apiType, ACLinkPayQryModel model)
        {
            _logger.Trace($"建立交易查詢資料-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<BankPayQryReq>();

            #region 物件資料

            string sMsgid = GetMsgid(apiType);//交易代號
            string sSourcechannel = "";//交易來源(空)

            string sTxnseq = GetMsgNo(_bankType);//業者交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            if (string.IsNullOrWhiteSpace(sTxnseq))
            {
                result.SetCode(7406);//取號失敗
                return result;
            }

            string sCooPerAtor = ACLinkCooPerAtor;//合作業者代號
            string sMbrActNo = model.MID.ToString();//會員編號

            string sMbrIdno = TripleDESEncrypt(model.IDNO);//會員身份證字號(需加密)
            if (string.IsNullOrWhiteSpace(sMbrIdno))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sBnkActNo = TripleDESEncrypt(model.BankAccount);//銀行帳號(需加密)
            if (string.IsNullOrWhiteSpace(sBnkActNo))
            {
                result.SetCode(7407);//加密失敗
                return result;
            }

            string sQryTxnSeq = model.SerMsgNo;//欲查詢之交易序號(業者代碼(4)+交易日期(yyyymmdd)+序號(8))
            string sSendMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss");//發送訊息時間

            string sSignSource1 = sTxnseq + sCooPerAtor + sMbrActNo;//交易序號+合作業者代號+會員編號 (各api固定都有)
            string sSignSource2 = model.IDNO;//會員身份證字號(加密前) (依不同api而不同)
            string sSignSource3 = sSendMsgTime;//發送訊息時間 (各api固定都有)

            var signResult = Sign(sSignSource1 + sSignSource2 + sSignSource3);
            if (!signResult.IsSuccess)
            {
                result.SetCode(signResult.RtnCode);
                return result;
            }
            string sSign = signResult.RtnData;//簽章值(PKCS7)

            #endregion

            BankPayQryReq oReqModel = new BankPayQryReq
            {
                header = new BankHeaderModel
                {
                    msgid = sMsgid,
                    sourcechannel = sSourcechannel,
                    returncode = "",//Request不帶值
                    returndesc = "",//Request不帶值
                    txnseq = sTxnseq,
                    fuseID = ""//Request不帶值
                },
                sendMsgTime = sSendMsgTime,
                cooPerAtor = sCooPerAtor,
                mbrActNo = sMbrActNo,
                mbrIdno = sMbrIdno,
                bnkActNo = sBnkActNo,
                qryTxnSeq = sQryTxnSeq,
                sign = sSign
            };

            result.SetSuccess(oReqModel);

            _logger.Trace($"建立交易查詢資料-After: {JsonConvert.SerializeObject(oReqModel)}");

            return result;
        }

        #endregion

        /// <summary>
        /// 記錄傳送請求資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <param name="mid"></param>
        [LogRequest(LogTextResponse = true, Name = "AddSendLog")]
        public void AddSendLog<T>(ApiType apiType, T model, long mid)
        {
            var sourceProps = model.GetType().GetProperties();

            ACLinkSendLogDbReq dbModel = new ACLinkSendLogDbReq();
            var targetProps = dbModel.GetType().GetProperties();

            foreach (var targetProp in targetProps)
            {
                foreach (var sourceProp in sourceProps)
                {
                    if (sourceProp.PropertyType.Name == "BankHeaderModel")
                    {
                        var subModel = sourceProp.GetValue(model, null);
                        var subProps = subModel.GetType().GetProperties();

                        foreach (var subProp in subProps)
                        {
                            if (targetProp.Name.ToLower() == subProp.Name.ToLower())
                            {
                                targetProp.SetValue(dbModel, subProp.GetValue(subModel, null));
                            }
                        }
                    }
                    else if (targetProp.Name.ToLower() == sourceProp.Name.ToLower())
                    {
                        targetProp.SetValue(dbModel, sourceProp.GetValue(model, null));
                    }
                }
            }

            dbModel.ApiType = apiType.ToString();
            dbModel.MID = mid;

            _cathayACLinkRepository.AddSendLog(dbModel);
        }

        /// <summary>
        /// 記錄傳送回應資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <param name="mid"></param>
        [LogRequest(LogTextResponse = true, Name = "AddReceiveLog")]
        public void AddReceiveLog<T>(ApiType apiType, T model, long mid)
        {
            var sourceProps = model.GetType().GetProperties();

            ACLinkReceiveLogDbReq dbModel = new ACLinkReceiveLogDbReq();
            var targetProps = dbModel.GetType().GetProperties();

            foreach (var targetProp in targetProps)
            {
                foreach (var sourceProp in sourceProps)
                {
                    if (sourceProp.PropertyType.Name == "BankHeaderModel")
                    {
                        var subModel = sourceProp.GetValue(model, null);
                        var subProps = subModel.GetType().GetProperties();

                        foreach (var subProp in subProps)
                        {
                            if (targetProp.Name.ToLower() == subProp.Name.ToLower())
                            {
                                targetProp.SetValue(dbModel, subProp.GetValue(subModel, null));
                            }
                        }
                    }
                    else if (targetProp.Name.ToLower() == sourceProp.Name.ToLower())
                    {
                        targetProp.SetValue(dbModel, sourceProp.GetValue(model, null));
                    }
                }
            }

            dbModel.ApiType = apiType.ToString();
            dbModel.MID = mid;

            _cathayACLinkRepository.AddReceiveLog(dbModel);
        }

        /// <summary>
        /// 取得綁定記錄
        /// </summary>
        /// <param name="model"></param>
        [LogRequest(LogTextResponse = true, Name = "GetBindLog")]
        public DataResult<ACLinkBindLogModel> GetBindLog(ACLinkBindLogQryModel model)
        {
            DataResult<ACLinkBindLogModel> result = new DataResult<ACLinkBindLogModel>();

            var dbReqModel = Mapper.Map<ACLinkBindLogDbReq>(model);

            var dbResModel = _cathayACLinkRepository.GetBindLog(dbReqModel);

            if (dbResModel != null)
            {
                result.SetSuccess(Mapper.Map<ACLinkBindLogModel>(dbResModel));
            }
            else
            {
                result.SetCode(7415);
            }

            return result;
        }

        /// <summary>
        /// 取交易代號
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        private string GetMsgid(ApiType apiType)
        {
            string sMsgid = string.Empty;

            switch (apiType)
            {
                case ApiType.ACLinkBind://綁定
                    sMsgid = "ALSM001BINDING";
                    break;
                //case ApiType.ACLinkResult://綁定結果通知(銀行發動)
                //    sMsgid = "ALSN001BINDING";
                //    break;
                case ApiType.ACLinkCancel://取消綁定
                    sMsgid = "ALSM002UNBIND";
                    break;
                case ApiType.ACLinkPay://付款
                    sMsgid = "ALSD001PAYMENT";
                    break;
                case ApiType.ACLinkDeposit://儲值
                    sMsgid = "ALSD002DEPOSIT";
                    break;
                case ApiType.ACLinkRefund://退款
                    sMsgid = "ALSC001REFUND";
                    break;
                case ApiType.ACLinkWithdrawal://提領
                    sMsgid = "ALSC002RESTORE";
                    break;
                case ApiType.ACLinkBindQuery://綁定查詢
                    sMsgid = "ALSQ001BINDING";
                    break;
                case ApiType.ACLinkPayQuery://交易查詢
                    sMsgid = "ALSQ002TRANSACTION";
                    break;
                default:
                    break;
            }

            return sMsgid;
        }

    }
}
