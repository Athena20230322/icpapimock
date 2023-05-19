using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.ChinaTrust;
using ICP.Modules.Api.AccountLink.Repositories;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 中國信託 AccountLink
    /// </summary>
    class ChinaTrustService : ACLinkService
    {
        private readonly string _merchantAccount = "12345678901234";
        private readonly bool isMockSign = false;
        private readonly bool isMockRSA = true;
        private readonly ChinaTrustACLinkRepository _chinaTrustACLinkRepository = null;
        private readonly LibMemberBankService _libMemberBankService = null;

        public ChinaTrustService(
            ACLinkRepository acLinkRepository,
            ChinaTrustACLinkRepository chinaTrustACLinkRepository,
            LibMemberBankService libMemberBankService,
            ILogger<ChinaTrustService> logger
            )
        {
            _bankType = BankType.ChinaTrust;
            _acLinkRepository = acLinkRepository;
            _chinaTrustACLinkRepository = chinaTrustACLinkRepository;
            _libMemberBankService = libMemberBankService;
            _logger = logger;
        }

        #region ACLinkApply - 申請連結帳號綁定
        /// <summary>
        /// 取得申請連結帳號綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkApplyPostData(ACLinkApplyReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成申請連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkApplyModel aclinkApplyModel = new ACLinkApplyModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                LinkType = ACLinkType,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                HolderRelationship = ACLinkHolderRelationship,
                Birth = model.Birth,
                DebitAccount = model.BankAccount,
                AgreeTime = model.AgreeTime,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkApplyModel)}");

            result.SetSuccess(aclinkApplyModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳綁定申請結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<ACLinkApplyReturnModel> GetACLinkApplyReturnData(object obj)
        {
            var result = new DataResult<ACLinkApplyReturnModel>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳綁定申請結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkApplyReturnModel rtnModel = new ACLinkApplyReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    LinkType = iReturnData.LinkType,
                    UserNo = iReturnData.UserNo,
                    UserId = iReturnData.UserId,
                    HolderRelationship = iReturnData.HolderRelationship,
                    Birth = iReturnData.Birth,
                    DebitAccount = iReturnData.DebitAccount,
                    AuthId = iReturnData.AuthId,
                    TrxNo = iReturnData.TrxNo,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得綁定申請結果: {JsonConvert.SerializeObject(rtnModel)}");

                //if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000")
                //{
                //    result.SetCode(10019, rtnModel);
                //    _logger.Trace($"綁定申請失敗: ReturnCode:{iResponse.ReturnCode}, ReturnMessage:{iResponse.ReturnMessage}, " +
                //        $"ServiceCode:{iServiceResponse.ServiceCode}, ServiceMessage:{iServiceResponse.ServiceMessage}");
                //    return result;
                //}

                result.SetSuccess(rtnModel);
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 組成綁定申請結果回傳資訊
        /// </summary>
        /// <param name="aclinkModel"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkApplyResult(ACLinkApplyReturnModel aclinkModel)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成綁定申請結果: {JsonConvert.SerializeObject(aclinkModel)}");

            //string data = JsonConvert.SerializeObject(obj);
            //ACLinkApplyReturnModel aclinkModel = JsonConvert.DeserializeObject<ACLinkApplyReturnModel>(data);

            Int64.TryParse(aclinkModel.UserNo, out long mid);
            int rtnCode = (aclinkModel.ReturnCode == "0000" && aclinkModel.ServiceCode == "0000" && mid > 0) ? 1 : 7423;
            string rtnMsg = rtnCode == 1 ? "Success" : "申請失敗";

            var rtnModel = new
            {
                RtnCode = rtnCode,
                RtnMsg = rtnMsg,
                aclinkModel.ServiceCode,
                aclinkModel.ServiceMessage,
                aclinkModel.AuthId
            };
            result.RtnCode = rtnCode;
            result.RtnMsg = rtnMsg;
            result.RtnData = rtnModel;

            _logger.Trace($"綁定申請結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region ACLink - 連結帳號綁定(OTP驗證)
        /// <summary>
        /// 取得連結帳號綁定(OTP驗證)送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkPostData(ACLinkBindReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成連結帳號綁定送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkModel aclinkModel = new ACLinkModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                LinkType = ACLinkType,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                HolderRelationship = ACLinkHolderRelationship,
                DebitAccount = model.BankAccount,
                AuthId = model.AuthId,
                Otp = model.Otp,
                Birth = model.Birth,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳綁定結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkReturnData(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳綁定結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkReturnModel rtnModel = new ACLinkReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    LinkType = iReturnData.LinkType,
                    UserNo = iReturnData.UserNo,
                    UserId = iReturnData.UserId,
                    HolderRelationship = iReturnData.HolderRelationship,
                    Birth = iReturnData.Birth,
                    DebitAccount = iReturnData.DebitAccount,
                    TrxNo = iReturnData.TrxNo,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得綁定結果: {JsonConvert.SerializeObject(rtnModel)}");

                Int64.TryParse(rtnModel.UserNo, out long mid);
                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, rtnModel);
                    _logger.Trace($"銀行回傳綁定結果: 綁定失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess(rtnModel);
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 新增AccountLink綁定資料
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> AddACLink(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備新增AccountLink綁定資料: {JsonConvert.SerializeObject(obj)}");

            string data = JsonConvert.SerializeObject(obj);
            ACLinkReturnModel aclinkModel = JsonConvert.DeserializeObject<ACLinkReturnModel>(data);

            Int64.TryParse(aclinkModel.UserNo, out long mid);
            if (mid == 0)
            {
                result.SetCode(10019, aclinkModel);
                _logger.Trace($"新增AccountLink綁定資料: MID錯誤, {JsonConvert.SerializeObject(obj)}");
            }

            // 寫入綁定資料
            string INDTAccount = GetINDTAccount(_bankType);
            var addModel = new ACLinkAddDbReq
            {
                MID = mid,
                INDTAccount = INDTAccount,
                BankCode = string.Format("{0:000}", (int)_bankType),
                BankAccount = aclinkModel.DebitAccount,
                MsgNo = aclinkModel.TrxNo,
                Status = (aclinkModel.ReturnCode == "0000" && aclinkModel.ServiceCode == "0000") ? 1 : 3
            };

            BaseResult rtnModel = _acLinkRepository.AddAccountLink(addModel);
            result.RtnCode = Convert.ToInt32(rtnModel.RtnCode);
            result.RtnMsg = rtnModel.RtnMsg;
            result.RtnData = aclinkModel;

            _logger.Trace($"綁定資料新增結果: {JsonConvert.SerializeObject(rtnModel)}");

            // 更新會員銀行帳號驗證狀態
            var bankAccountModel = new UpdateBankAccountStatusModel
            {
                MID = mid,
                Category = 1,
                BankCode = string.Format("{0:000}", (int)_bankType),
                BankAccount = aclinkModel.DebitAccount,
                AccountStatus = (byte)(result.RtnCode == 1 ? 1 : 2),
                INDTAccount = INDTAccount
            };

            _logger.Trace($"準備更新會員銀行帳號驗證狀態: {JsonConvert.SerializeObject(bankAccountModel)}");

            var updateResult = _libMemberBankService.UpdateMemberBankAccountStatus(bankAccountModel);
            result.RtnCode = updateResult.RtnCode;
            result.RtnMsg = updateResult.RtnMsg;

            _logger.Trace($"會員銀行帳號驗證狀態更新結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region ACLinkCancel - 取消連結帳戶綁定
        /// <summary>
        /// 取得取消連結帳戶綁定送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkCancelPostData(ACLinkCancelReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成取消連結帳戶綁定送出資料: {JsonConvert.SerializeObject(model)}");

            string bankAccount = this.GetBankAccount(model.MID, model.INDTAccount, string.Format("{0:000}", (int)_bankType));
            bankAccount = "12345600001111";
            if (string.IsNullOrWhiteSpace(bankAccount))
            {
                result.SetCode(10019);
                _logger.Info($"取得銀行帳號失敗: mid:{model.MID}, indtAccount:{model.INDTAccount}, bankCode:{string.Format("{0:000}", (int)_bankType)}");
                return result;
            }

            ACLinkCancelModel aclinkModel = new ACLinkCancelModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                DebitAccount = bankAccount,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳取消綁定結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<ACLinkReturnModel> GetACLinkCancelReturnData(object obj)
        {
            var result = new DataResult<ACLinkReturnModel>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳取消綁定結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkReturnModel rtnModel = new ACLinkReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    UserNo = iReturnData.UserNo,
                    UserId = iReturnData.UserId,
                    TrxNo = iReturnData.TrxNo,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得取消綁定結果: {JsonConvert.SerializeObject(rtnModel)}");

                Int64.TryParse(rtnModel.UserNo, out long mid);
                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, rtnModel);
                    _logger.Trace($"銀行回傳取消綁定結果: 取消綁定失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess(rtnModel);
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 更新AccountLink綁定資料(取消綁定)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkCancelBind(long MID, string INDTAccount)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備更新AccountLink綁定資料(取消綁定): MID:{MID}, INDTAccount:{INDTAccount}");

            // 更新綁定資料 - 取消綁定
            var updateModel = new ACLinkCancelDbReq
            {
                MID = MID,
                INDTAccount = INDTAccount,
                BankCode = string.Format("{0:000}", (int)_bankType)
            };

            BaseResult rtnModel = _acLinkRepository.CancelAccountLink(updateModel);
            result.RtnCode = Convert.ToInt32(rtnModel.RtnCode);
            result.RtnMsg = rtnModel.RtnMsg;

            _logger.Trace($"取消綁定資料更新結果: {JsonConvert.SerializeObject(rtnModel)}");

            return result;
        }
        #endregion

        #region BankACLinkCancel - 銀行端取消連結帳戶綁定
        /// <summary>
        /// 銀行端取消連結帳戶綁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkBankCancelBind(ACLinkReturnModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備更新AccountLink綁定資料(銀行端取消綁定): {JsonConvert.SerializeObject(model)}");

            long mid = Convert.ToInt64(model.UserNo);
            string indtAccount = GetBankAccount(mid, model.DebitAccount, string.Format("{0:000}", (int)_bankType));

            _logger.Trace($"更新綁定狀態: mid:{mid}, indtAccount:{indtAccount} ");

            // 更新綁定資料 - 取消綁定
            var updateModel = new ACLinkCancelDbReq
            {
                MID = mid,
                INDTAccount = indtAccount,
                BankCode = string.Format("{0:000}", (int)_bankType)
            };

            BaseResult rtnModel = _acLinkRepository.CancelAccountLink(updateModel);
            _logger.Trace($"取消綁定資料更新結果: {JsonConvert.SerializeObject(rtnModel)}");

            result.RtnCode = Convert.ToInt32(rtnModel.RtnCode);
            result.RtnMsg = rtnModel.RtnMsg;
            var bankReturnCode = result.RtnCode == 1 ? "0000" : "0001";
            result.RtnData = new { ReturnCode = bankReturnCode };

            _logger.Trace($"更新AccountLink綁定資料(銀行端取消綁定)完成: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion

        #region ACLinkPay - 連結帳戶交易扣款
        /// <summary>
        /// 取得連結帳戶交易扣款送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkPayPostData(ACLinkPayReq model, string vAccount)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成交易扣款送出資料: {JsonConvert.SerializeObject(model)}, vAccount:{vAccount}");

            string bankAccount = this.GetBankAccount(model.MID, model.INDTAccount, string.Format("{0:000}", (int)_bankType));
            if (string.IsNullOrWhiteSpace(bankAccount))
            {
                result.SetCode(10019);
                _logger.Info($"取得銀行帳號失敗: mid:{model.MID}, indtAccount:{model.INDTAccount}, bankCode:{string.Format("{0:000}", (int)_bankType)}");
                return result;
            }
            _logger.Trace($"成功取得銀行帳號: bankAccount:{bankAccount}");

            ACLinkPayModel aclinkModel = new ACLinkPayModel
            {
                OrderNo = model.TradeNo,
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                TrxType = (model.TradeModeID == 2) ? "2" : "0",
                PayerAccount = bankAccount,
                PayeeAccount = vAccount,    
                TrxAmt = model.TradeAmt,
                TrxShopName = model.TradeNote,
                OriginalReferenceNo = "",
                TrxNo = GetMsgNo(_bankType),
                TrxTime = model.TradeTime
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳扣款結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkPayReturnData(object obj, string vAccount = "")
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳扣款結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkReturnModel payResult = new ACLinkReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    TrxNo = iReturnData.TrxNo,
                    TrxTime = iReturnData.TrxTime,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage,
                    VirtualAccount = vAccount
                };

                _logger.Trace($"成功取得扣款結果: {JsonConvert.SerializeObject(payResult)}");

                // 帶入回傳資料
                Int64.TryParse(payResult.UserNo, out long mid);
                result.RtnData = new { BankTradeNo = payResult.TrxNo };

                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(7419, payResult);
                    _logger.Trace($"銀行回傳扣款結果: 扣款失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(7419);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 模擬銀行回傳扣款結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> MockACLinkPayReturnData()
        {
            var result = new DataResult<object>();
            result.SetError();

            ACLinkResultModel payResult = new ACLinkResultModel
            {
                BankTradeNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成模擬銀行回傳扣款結果: {JsonConvert.SerializeObject(payResult)}");

            result.SetSuccess(payResult);

            return result;
        }
        #endregion

        #region ACLinkWithdrawal - 提領金額至連結帳戶
        /// <summary>
        /// 取得提領送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkWithdrawalPostData(ACLinkWithdrawalReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成提領送出資料: {JsonConvert.SerializeObject(model)}");

            string bankAccount = this.GetBankAccount(model.MID, model.INDTAccount, string.Format("{0:000}", (int)_bankType));
            if (string.IsNullOrWhiteSpace(bankAccount))
            {
                result.SetCode(10019);
                _logger.Info($"取得銀行帳號失敗: mid:{model.MID}, indtAccount:{model.INDTAccount}, bankCode:{string.Format("{0:000}", (int)_bankType)}");
                return result;
            }

            ACLinkPayModel aclinkModel = new ACLinkPayModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                TrxType = "4",      // 提領
                PayerAccount = _merchantAccount,
                PayeeAccount = bankAccount,
                TrxAmt = model.Amount,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳提領結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkWithdrawalReturnData(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳提領結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkReturnModel payResult = new ACLinkReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantId = iReturnData.MerchantId,
                    TrxNo = iReturnData.TrxNo,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得提領結果: {JsonConvert.SerializeObject(payResult)}");

                // 帶入回傳資料
                Int64.TryParse(payResult.UserNo, out long mid);
                result.RtnData = new { MID = mid, MsgNo = payResult.TrxNo };

                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, payResult);
                    _logger.Trace($"銀行回傳提領結果: 提領失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }
        #endregion

        #region ACLinkRefund - 連結帳戶交易退款
        /// <summary>
        /// 取得連結帳戶交易退款送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkRefundPostData(ACLinkRefundReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成交易退款送出資料: {JsonConvert.SerializeObject(model)}");

            string bankAccount = this.GetBankAccount(model.MID, model.INDTAccount, string.Format("{0:000}", (int)_bankType));
            if (string.IsNullOrWhiteSpace(bankAccount))
            {
                result.SetCode(10019);
                _logger.Info($"取得銀行帳號失敗: mid:{model.MID}, indtAccount:{model.INDTAccount}, bankCode:{string.Format("{0:000}", (int)_bankType)}");
                return result;
            }

            ACLinkPayModel aclinkModel = new ACLinkPayModel
            {
                OrderNo = model.TradeNo,
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                TrxType = "9",
                PayerAccount = _merchantAccount,
                PayeeAccount = bankAccount,
                TrxAmt = model.RefundAmt,
                TrxShopName = "",
                OriginalReferenceNo = model.BankTradeNo,
                TrxNo = GetMsgNo(_bankType),
                TrxTime = model.RefundTime
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳退款結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkRefundReturnData(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳退款結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkReturnModel refundResult = new ACLinkReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    TrxNo = iReturnData.TrxNo,
                    TrxTime = iReturnData.TrxTime,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得退款結果: {JsonConvert.SerializeObject(refundResult)}");

                // 帶入回傳資料
                Int64.TryParse(refundResult.UserNo, out long mid);
                result.RtnData = new { BankTradeNo = refundResult.TrxNo };

                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, refundResult);
                    _logger.Trace($"銀行回傳退款結果: 扣款失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 模擬銀行回傳退款結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> MockACLinkRefundReturnData()
        {
            var result = new DataResult<object>();
            result.SetError();

            ACLinkResultModel refundResult = new ACLinkResultModel
            {
                BankTradeNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成模擬銀行回傳扣款結果: {JsonConvert.SerializeObject(refundResult)}");

            result.SetSuccess(refundResult);

            return result;
        }
        #endregion

        #region ACLinkQuery - 連結綁定狀態查詢
        /// <summary>
        /// 取得連結綁定狀態查詢送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkQueryPostData(ACLinkBindQryReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成連結綁定狀態查詢送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkModel aclinkModel = new ACLinkModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行回傳綁定狀態查詢結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkQueryReturnData(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行回傳綁定狀態查詢結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkQueryReturnModel queryResult = new ACLinkQueryReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    UserId = iReturnData.UserId,
                    UserNo = iReturnData.UserNo,
                    DebitAccount = iReturnData.DebitAccount,
                    BindingStatus = iReturnData.BindingStatus,
                    TrxDate = iReturnData.TrxDate,
                    TrxTime = iReturnData.TrxTime,
                    TrxNo = iReturnData.TrxNo,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得綁定狀態查詢結果: {JsonConvert.SerializeObject(queryResult)}");

                // 帶入回傳資料
                Int64.TryParse(queryResult.UserNo, out long mid);
                result.RtnData = new { MID = mid, MsgNo = queryResult.TrxNo, Status = queryResult.BindingStatus };

                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, queryResult);
                    _logger.Trace($"銀行回傳綁定狀態查詢結果: 查詢失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }
        #endregion

        #region ACLinkPayQuery - 銀行交易結果查詢
        /// <summary>
        /// 取得銀行交易結果查詢送出資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ACLinkPayQueryPostData(ACLinkPayQryReq model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備組成銀行交易結果查詢送出資料: {JsonConvert.SerializeObject(model)}");

            ACLinkPayModel aclinkModel = new ACLinkPayModel
            {
                MerchantType = ACLinkMerchantType,
                MerchantId = ACLinkMerchantID,
                UserNo = model.MID.ToString(),
                UserId = model.IDNO,
                OriginalReferenceNo = model.BankTradeNo,
                TrxNo = GetMsgNo(_bankType)
            };

            _logger.Trace($"成功組成送出資料: {JsonConvert.SerializeObject(aclinkModel)}");

            result.SetSuccess(aclinkModel);

            return result;
        }

        /// <summary>
        /// 取得銀行交易查詢結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<object> GetACLinkPayQueryReturnData(object obj)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"準備取得銀行交易查詢結果: {JsonConvert.SerializeObject(obj)}");

            // 解析回傳資訊
            try
            {
                dynamic iData = obj;
                dynamic iResponse = iData.Response;
                dynamic iReturnData = iResponse.ReturnData;
                dynamic iServiceResponse = iResponse.ServiceResponse;

                ACLinkQueryPayReturnModel payResult = new ACLinkQueryPayReturnModel
                {
                    TransactionId = iReturnData.TransactionId,
                    MerchantType = iReturnData.MerchantType,
                    MerchantId = iReturnData.MerchantId,
                    UserId = iReturnData.UserId,
                    UserNo = iReturnData.UserNo,
                    TrxType = iReturnData.TrxType,
                    PayerAccount = iReturnData.PayerAccount,
                    PayeeAccount = iReturnData.PayeeAccount,
                    TrxAmt = iReturnData.TrxAmt,
                    TrxResult = iReturnData.TrxResult,
                    OriginalReferenceNo = iReturnData.OriginalReferenceNo,
                    TrxNo = iReturnData.TrxNo,
                    TrxTime = iReturnData.TrxTime,
                    ReturnCode = iResponse.ReturnCode,
                    ReturnMessage = iResponse.ReturnMessage,
                    ServiceCode = iServiceResponse.ServiceCode,
                    ServiceMessage = iServiceResponse.ServiceMessage
                };

                _logger.Trace($"成功取得銀行交易查詢結果: {JsonConvert.SerializeObject(payResult)}");

                // 帶入回傳資料
                Int64.TryParse(payResult.UserNo, out long mid);
                result.RtnData = new { MID = mid, MsgNo = payResult.TrxNo };

                if (iResponse.ReturnCode != "0000" || iServiceResponse.ServiceCode != "0000" || mid == 0)
                {
                    result.SetCode(10019, payResult);
                    _logger.Trace($"銀行回傳扣款結果: 扣款失敗 , {JsonConvert.SerializeObject(obj)}");
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 是否模擬銀行
        /// </summary>
        /// <returns></returns>
        public bool isMockBank()
        {
            return MockBank;
        }

        /// <summary>
        /// 取得簽章
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<string> GetSign(object obj, string aesKey)
        {
            var result = new DataResult<string>();
            result.SetError();

            ChinaTrustSignatureService signService = new ChinaTrustSignatureService();
            _logger.Trace($"開始準備簽章: {JsonConvert.SerializeObject(obj)}");

            // 簽章
            var signResult = new DataResult<string> { RtnCode = 1, RtnMsg = "模擬簽章成功", RtnData = "testSign1234" };
            if (!isMockSign)
            {
                string data = JsonConvert.SerializeObject(obj);
                signResult = Sign(data);
                if (!signResult.IsSuccess)
                {
                    result.SetCode(signResult.RtnCode);
                    return result;
                }

                //string signStr = signService.SignatureData(data);
                //if (!string.IsNullOrWhiteSpace(signStr))
                //{
                //    signResult.SetSuccess(signStr);
                //}
            }
            _logger.Trace($"簽章完成: {signResult.RtnData}");

            // 簽章AES加密
            string rsaResult = this.EncryptAesECB(signResult.RtnData, aesKey);
            if (string.IsNullOrWhiteSpace(rsaResult))
            {
                result.SetCode(7425);
                return result;
            }
            result.SetSuccess(rsaResult);

            _logger.Trace($"簽章加密完成: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 驗證銀行回傳的資料 (解密及驗章)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> ValidateBankReturnData(ApiType apiType, ACLinkRes model)
        {
            var result = new DataResult<object>();
            result.SetError();

            _logger.Trace($"開始準備解密: {JsonConvert.SerializeObject(model)}");

            // 取得AES Key
            var rsaResult = DecryptRSA(model.ResAesKey);
            if (!rsaResult.IsSuccess)
            {
                result.SetCode(rsaResult.RtnCode);
                return result;
            }

            // 解密
            string source = DecryptAesECB(model.ResSecData, rsaResult.RtnData);
            if (string.IsNullOrWhiteSpace(source))
            {
                result.SetCode(10019, model);
                _logger.Info($"取得原文失敗: {JsonConvert.SerializeObject(model)}");
            }

            // 驗章
            var validateResult = ValidateSign(source);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode);
                _logger.Info($"驗章失敗: {JsonConvert.SerializeObject(validateResult)}");
                return result;
            }

            // 驗章成功，取得參數內容
            result.RtnData = JsonConvert.DeserializeObject(source);

            // 記錄接收資訊
            AddChinaTrustReceiveLog(apiType, result.RtnData);

            _logger.Trace($"驗證完成: {JsonConvert.SerializeObject(model)}");

            return result;
        }

        /// <summary>
        /// AES加密 (ECB/PKCS5Padding)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string EncryptAesECB(string source, string key)
        {
            _logger.Trace($"開始準備AES加密");

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(source);
            string result = string.Empty;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            result = Convert.ToBase64String(resultArray);
            aes.Clear();

            _logger.Trace($"AES加密完成");

            return result;
        }

        /// <summary>
        /// AES解密 (ECB/PKCS5Padding)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DecryptAesECB(string source, string key)
        {
            _logger.Trace($"開始準備AES解密");

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(source);
            string result = string.Empty;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            result = Convert.ToBase64String(resultArray);
            aes.Clear();

            _logger.Trace($"AES解密完成");

            return result;
        }

        /// <summary>
        /// 隨機取得AES Key
        /// </summary>
        /// <returns></returns>
        public string GetRandomKey()
        {
            _logger.Trace($"開始準備取得AES Key");

            string key = String.Empty;

            // 隨機取得16碼亂數
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            int len = 16;
            char[] chars = new char[len];
            Random rd = new Random();

            for (int i = 0; i < len; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            key = new string(chars);

            _logger.Trace($"成功取得AES Key");

            return key;
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DataResult<string> EncryptRSA(string source)
        {
            var result = new DataResult<string>();
            result.SetError();

            _logger.Trace($"開始準備RSA加密");

            ChinaTrustSignatureService signService = new ChinaTrustSignatureService();

            string rsaStr = string.Empty;
            //rsaStr = signService.SignatureData(source);
            //result.SetSuccess(rsaStr);

            string filePath = $@"D:\ecpay\ICP.Project\src\ICP.Infrastructure.Host.KeyApi\App_Data\icashCert\PublicKey-822.txt";
            string key = System.IO.File.ReadAllText(filePath);

            if (isMockRSA)
            {
                rsaStr = "testEncrypt123456";
                result.SetSuccess(rsaStr);
                _logger.Trace($"模擬RSA加密");
                return result;
            }

            _logger.Trace($"取得公鑰");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            
            try
            {
                rsa.FromXmlString(key);
                byte[] data = Encoding.UTF8.GetBytes(source);
                byte[] encryptData = rsa.Encrypt(data, false);
                rsaStr = Convert.ToBase64String(encryptData);
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Info($"RSA加密失敗: source:{source}, Error:{ex}");
                return result;
            }

            _logger.Trace($"RSA加密完成");

            if (!string.IsNullOrWhiteSpace(rsaStr))
            {
                result.SetSuccess(rsaStr);
            }
            else
            {
                result.SetCode(10019);
                _logger.Info($"RSA加密失敗: source:{source}, Error: rsaStr is null.");
            }

            return result;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DataResult<string> DecryptRSA(string source)
        {
            var result = new DataResult<string>();
            result.SetError();

            _logger.Trace($"開始準備RSA解密");

            string filePath = $@"D:\ecpay\ICP.Project\src\ICP.Infrastructure.Host.KeyApi\App_Data\icashCert\PrivateKey-822.txt";
            string key = System.IO.File.ReadAllText(filePath);
            string decryptStr = string.Empty;

            _logger.Trace($"取得私鑰");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            try
            {
                rsa.FromXmlString(key);
                byte[] data = Convert.FromBase64String(source);
                byte[] decryptData = rsa.Decrypt(data, false);
                decryptStr = Encoding.UTF8.GetString(decryptData);
            }
            catch (Exception ex)
            {
                result.SetCode(10019);
                _logger.Info($"RSA解密失敗: source:{source}, Error:{ex}");
                return result;
            }

            _logger.Trace($"RSA解密完成");

            if (!string.IsNullOrWhiteSpace(decryptStr))
            {
                result.SetSuccess(decryptStr);
            }
            else
            {
                result.SetCode(10019);
                _logger.Info($"RSA解密失敗: source:{source}, Error: decryptStr is null.");
            }

            return result;
        }
        #endregion

        #region 資料處理
        /// <summary>
        /// 記錄送出資料DB Log
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="signObj"></param>
        public void AddChinaTrustSendLog(ApiType apiType, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            ACLinkSendLogModel sendModel = JsonConvert.DeserializeObject<ACLinkSendLogModel>(data);

            _chinaTrustACLinkRepository.AddChinaTrustSendLog(sendModel);
        }

        /// <summary>
        /// 記錄接收資料DB Log
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="signObj"></param>
        public void AddChinaTrustReceiveLog(ApiType apiType, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            ACLinkReceiveLogModel receiveModel = JsonConvert.DeserializeObject<ACLinkReceiveLogModel>(data);

            _chinaTrustACLinkRepository.AddChinaTrustReceiveLog(receiveModel);
        }

        /// <summary>
        /// 取得銀行帳號
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="indtAccount"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public string GetBankAccount(long mid, string indtAccount, string bankCode)
        {
            string result = string.Empty;

            ACLinkInfoModel acLinkInfo = this.GetAccountLinkInfo(mid, indtAccount, bankCode);
            if (acLinkInfo == null || string.IsNullOrWhiteSpace(acLinkInfo.BankAccount))
            {
                _logger.Trace($"取得銀行帳號失敗 mid:{mid}, indtAccount:{indtAccount}, bankCode:{bankCode}");
                return result;
            }

            result = acLinkInfo.BankAccount;

            return result;
        }

        /// <summary>
        /// 取得會員指定銀行的AccountLink綁定資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="indtAccount"></param>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public ACLinkInfoModel GetAccountLinkInfo(long mid, string indtAccount, string bankCode)
        {
            ACLinkInfoDbReq model = new ACLinkInfoDbReq
            {
                MID = mid,
                INDTAccount = indtAccount,
                BankCode = bankCode
            };

            ACLinkInfoDbRes acLinkInfo = _acLinkRepository.GetAccountLinkInfo(model);
            ACLinkInfoModel rtnModel = Mapper.Map<ACLinkInfoModel>(acLinkInfo);

            return rtnModel;
        }

        /// <summary>
        /// 新增中國信託AccountLink入帳用虛擬帳號
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="tradeId"></param>
        /// <returns></returns>
        public ACLinkTradeModel AddChinaTrustVirtualAccount(int amount, long tradeId)
        {
            int paymentTypeID = 2;          // AccountLink
            int paymentSubTypeID = 2;       // ChinaTrust

            ACLinkVAccountDbReq model = new ACLinkVAccountDbReq
            {
                PaymentTypeID = paymentTypeID,
                PaymentSubTypeID = paymentSubTypeID,
                Amount = amount,
                TradeID = tradeId
            };

            ACLinkVAccountDbRes addResult = _chinaTrustACLinkRepository.AddChinaTrustVirtualAccount(model);
            ACLinkTradeModel rtnModel = Mapper.Map<ACLinkTradeModel>(addResult);

            return rtnModel;
        }

        /// <summary>
        /// 取得Payment的TradeID
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        public DataResult<long> GetTradeID(string tradeNo)
        {
            var result = new DataResult<long>();
            result.SetError();

            _logger.Trace($"準備取得Payment的TradeID: TradeNo:{tradeNo}");

            // 取得Payment的TradeID
            long tradeId = _acLinkRepository.GetTradeID(tradeNo);
            
            if (tradeId == 0)
            {
                result.SetCode(7416);
                _logger.Info($"取得Payment的TradeID失敗: TradeNo:{tradeNo}");
                return result;
            }
                
            _logger.Trace($"取得Payment的TradeID: tradeId:{tradeId}");

            result.SetSuccess(tradeId);

            return result;
        }

        /// <summary>
        /// 取得入帳用虛擬帳號
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public DataResult<string> GetVirtualAccount(long tradeId, int amount)
        {
            var result = new DataResult<string>();
            result.SetError();

            _logger.Trace($"準備取得虛擬帳號: tradeId:{tradeId}");

            // 取得入帳虛擬帳號
            ACLinkTradeModel vAccountResult = this.AddChinaTrustVirtualAccount(amount, tradeId);
            if (!vAccountResult.IsSuccess || string.IsNullOrWhiteSpace(vAccountResult.VirtualAccount))
            {
                result.SetCode(7413);
                _logger.Info($"取得入帳虛擬帳號失敗: amount:{amount}, tradeId:{tradeId}");
                return result;
            }
            result.SetSuccess(vAccountResult.VirtualAccount);

            _logger.Trace($"成功取得入帳虛擬帳號: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 更新Payment訂單的VirtualAccount
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        public BaseResult UpdateTradeVirtualAccount(long tradeId, string virtualAccount)
        {
            var result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備更新Payment訂單: tradeId:{tradeId}, virtualAccount:{virtualAccount}");

            result = _acLinkRepository.UpdateTradeVirtualAccount(tradeId, virtualAccount);

            _logger.Trace($"更新Payment結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion
    }
}
