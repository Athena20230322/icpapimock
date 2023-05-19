using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Models.PaymentCenterApi.Enums;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;
using ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class AccountLinkPaymentService : IPaymentMethod
    {
        private AccountLinkPaymentRepository _AccountLinkPaymentRepository;

        private string _ACLPAYURL;
        private string _ACLQRYURL;
        private string _ACLRFNURL;
        private string _ACLTUPURL;
        private string _ACLKey;
        private string _ACLIV;

        public AccountLinkPaymentService(AccountLinkPaymentRepository accountLinkPaymentRepository)
        {
            _AccountLinkPaymentRepository = accountLinkPaymentRepository;

            _ACLPAYURL = $"{GlobalConfigUtil.Host_Middleware_AccountLink_Domain}/AccountLink/ACLinkPay";
            _ACLQRYURL = $"{GlobalConfigUtil.Host_Middleware_AccountLink_Domain}/AccountLink/ACLinkPayQuery";
            _ACLRFNURL = $"{GlobalConfigUtil.Host_Middleware_AccountLink_Domain}/AccountLink/ACLinkRefund";
            _ACLTUPURL = $"{GlobalConfigUtil.Host_Middleware_AccountLink_Domain}/AccountLink/ACLinkDeposit";
            _ACLKey = GlobalConfigUtil.ACLink_HashKey;
            _ACLIV = GlobalConfigUtil.ACLink_HashIV;
        }

        #region 交易
        /// <summary>
        /// 交易資料驗證
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<TradeResModel> Validate(TradeReqModel tradeRequestModel)
        {
            var result = new DataResult<TradeResModel>();
            result.SetSuccess();

            var paymentType = (PaymentSubType_AccountLink)tradeRequestModel.PaymentSubTypeID;
            if (paymentType < PaymentSubType_AccountLink.Min ||
                paymentType > PaymentSubType_AccountLink.Max)
            {
                result.SetCode(7029);
            }

            long accountID = 0;
            if (!long.TryParse(tradeRequestModel.AccountID, out accountID))
            {
                result.SetCode(7032);
            }

            return result;
        }

        /// <summary>
        /// 交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<TradeResModel> Process(TradeReqModel tradeRequestModel)
        {
            var result = new DataResult<TradeResModel>();
            result.SetSuccess(new TradeResModel() { PaymentCenterTradeID = tradeRequestModel.TradeID });

            // 1.建立虛擬帳號
            //var virtualAccount = _AccountLinkPaymentRepository.CreateVirtualAccount(tradeRequestModel);
            //if (!virtualAccount.IsSuccess)
            //{
            //    return result.SetError(virtualAccount);
            //}
            //result.RtnData.VirtualAccount = virtualAccount.VirtualAccount;

            // 2.取得會員身分證號/居留證號,AccountLink帳戶
            long accountID = long.Parse(tradeRequestModel.AccountID);

            var getResult = _GetAccountLinkInfo(tradeRequestModel.MID, accountID);
            if (!getResult.IsSuccess)
            {
                return result.SetError(getResult);
            }

            // 3.送至銀行授權AccountLink付款
            var payResult = _Pay(tradeRequestModel, getResult);
            if (!payResult.IsSuccess)
            {
                result.RtnData.PayRtnCode = payResult.RtnCode.ToString();
                result.RtnData.PayRtnMsg = payResult.RtnMsg;
                return result.SetError(payResult);
            }

            // 4.送至銀行驗證交易結果
            var verifyResult = _PayedVerify(tradeRequestModel, getResult, payResult.RtnData);
            if (!verifyResult.IsSuccess)
            {
                result.RtnData.VerifyRtnCode = verifyResult.RtnCode.ToString();
                result.RtnData.VerifyRtnMsg = verifyResult.RtnMsg;
                return result.SetError(verifyResult);
            }

            return _TransTradeResponseModel(tradeRequestModel.TradeID, payResult, verifyResult);
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult AddTrade(TradeReqModel tradeRequestModel)
        {
            string bankCode = ((int)_GetBankType(tradeRequestModel.PaymentSubTypeID)).ToString("D3");
            return _AccountLinkPaymentRepository.AddTrade(tradeRequestModel, bankCode);
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            return _AccountLinkPaymentRepository.UpdateTrade(tradeResponseModel.RtnData);
        }
        #endregion

        #region 退款
        /// <summary>
        /// 退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<RefundResModel> RefundProcess(QryRefundTradeModel tradeRequestModel)
        {
            // 退款賣家依撥款負向處理,不先扣帳戶
            //var deduResult = _AccountLinkPaymentRepository.ReduceICash(new ICashIncDecModel()
            //{
            //    TradeNo = tradeRequestModel.TradeNo,
            //    MID = tradeRequestModel.MerchantID,
            //    TradeModeID = tradeRequestModel.TradeModeID,
            //    PaymentTypeID = tradeRequestModel.PaymentTypeID,
            //    PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
            //    Notes = $"{tradeRequestModel.TradeModeID}_{tradeRequestModel.PaymentTypeID}_{tradeRequestModel.PaymentSubTypeID}_交易退款扣款",
            //    Amount = tradeRequestModel.RefundAmount
            //});

            var result = new DataResult<RefundResModel>()
                        .SetSuccess(new RefundResModel());

            //if (!deduResult.IsSuccess)
            //{
            //    result.SetError(deduResult);
            //    return result;
            //}

            var addResult = _AccountLinkPaymentRepository.AddICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = 1,
                PaymentTypeID = 1,
                PaymentSubTypeID = 2,
                Notes = $"{tradeRequestModel.TradeModeID}_{tradeRequestModel.PaymentTypeID}_{tradeRequestModel.PaymentSubTypeID}_交易退款入款",
                Amount = tradeRequestModel.RefundAmount
            });
            result.SetError(addResult);
            return result;
        }

        /// <summary>
        /// 退款訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryRefundTradeModel QryRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _AccountLinkPaymentRepository.QryRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddRefundTradeModel AddRefundTrade(RefundReqModel tradeRequestModel)
        {
            return _AccountLinkPaymentRepository.AddRefundTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> tradeResponseModel)
        {
            return _AccountLinkPaymentRepository.UpdateRefundTrade(tradeID, tradeResponseModel);
        }

        /// <summary>
        /// 逾時退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public void TimeoutRefundProcess(TradeReqModel tradeRequestModel)
        {
            // 取得會員身分證號/居留證號,AccountLink帳戶
            long accountID = long.Parse(tradeRequestModel.AccountID);

            var getResult = _GetAccountLinkInfo(tradeRequestModel.MID, accountID);
            if (!getResult.IsSuccess)
            {
                return;
            }

            // 取得銀行交易結果
            var payedResult = _GetPayedResultInfo(tradeRequestModel);

            // 交易為成功,需進行退款
            if (!string.IsNullOrEmpty(payedResult.PayRtnCode) &&
                payedResult.PayRtnCode.Equals("0000"))
            {
                var refundResult = _Refund(tradeRequestModel, getResult, new ACLinkPayRes() { BankTradeNo = payedResult.BankTradeNo });
                
                // 退款失敗,需通知
                if(!refundResult.IsSuccess)
                {

                }
            }
        }
        #endregion

        #region 取消
        /// <summary>
        /// 取消交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public DataResult<ReversalResModel> ReversalProcess(QryReversalTradeModel tradeRequestModel)
        {
            var deduResult = _AccountLinkPaymentRepository.ReduceICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MerchantID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = $"{tradeRequestModel.TradeModeID}_{tradeRequestModel.PaymentTypeID}_{tradeRequestModel.PaymentSubTypeID}_交易取消扣款",
                Amount = tradeRequestModel.Amount
            });

            var result = new DataResult<ReversalResModel>()
                        .SetSuccess(new ReversalResModel());

            if (!deduResult.IsSuccess)
            {
                result.SetError(deduResult);
                return result;
            }

            var addResult = _AccountLinkPaymentRepository.AddICash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                TradeModeID = 1,
                PaymentTypeID = 1,
                PaymentSubTypeID = 2,
                Notes = $"{tradeRequestModel.TradeModeID}_{tradeRequestModel.PaymentTypeID}_{tradeRequestModel.PaymentSubTypeID}_交易取消入款",
                Amount = tradeRequestModel.Amount
            });
            result.SetError(addResult);
            return result;
        }

        /// <summary>
        /// 取消訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryReversalTradeModel QryReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _AccountLinkPaymentRepository.QryReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 新增取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddReversalTradeModel AddReversalTrade(ReversalReqModel tradeRequestModel)
        {
            return _AccountLinkPaymentRepository.AddReversalTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> tradeResponseModel)
        {
            return _AccountLinkPaymentRepository.UpdateReversalTrade(tradeID, tradeResponseModel);
        }
        #endregion

        #region 資料處理
        /// <summary>
        /// 新增儲值金
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public DataResult<TradeResModel> AddTopupCash(TradeReqModel tradeRequestModel)
        {
            var deduResult = _AccountLinkPaymentRepository.AddTopupCash(new ICashIncDecModel()
            {
                TradeNo = tradeRequestModel.TradeNo,
                MID = tradeRequestModel.MID,
                MerchantID = tradeRequestModel.MerchantID,
                TradeModeID = tradeRequestModel.TradeModeID,
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Notes = $"{tradeRequestModel.TradeModeID}_{tradeRequestModel.PaymentTypeID}_{tradeRequestModel.PaymentSubTypeID}_儲值交易入款(AccountLink)",
                Amount = tradeRequestModel.Amount
            });
            var result = new DataResult<TradeResModel>()
                        .SetSuccess(new TradeResModel() { PaymentCenterTradeID = tradeRequestModel.TradeID })
                        .SetError(deduResult);

            return result;
        }

        /// <summary>
        /// 取得會員身分證號/居留證號
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        private AccountLinkInfoModel _GetAccountLinkInfo(long MID, long accountID)
        {
            return _AccountLinkPaymentRepository._GetAccountLinkInfo(MID, accountID);
        }

        /// <summary>
        /// 送至銀行授權AccountLink付款
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        private DataResult<ACLinkPayRes> _Pay(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, VirtualAccountModel virtualAccountModel = null)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper(_ACLKey, _ACLIV);

            var postModel = new ACLinkDecryptModel
            {
                Json = _GetACLinkModelData(tradeRequestModel, memberInfoModel, virtualAccountModel),
                BankType = _GetBankType(tradeRequestModel.PaymentSubTypeID)
            };

            string postData = $"token={HttpUtility.UrlEncode(aesCryptoHelper.Encrypt(JsonConvert.SerializeObject(postModel)))}";

            // 送至Middleware進行授權
            string payURL = tradeRequestModel.TradeModeID.Equals((int)eTradeMode.Topup) ? _ACLTUPURL : _ACLPAYURL;
            var recvData = _AccountLinkPaymentRepository.Pay(payURL, postData);

            var result = new DataResult<ACLinkPayRes>().SetSuccess(new ACLinkPayRes());

            if(recvData.Contains("作業逾時"))
            {
                result.SetCode(7006);
                return result;
            }

            BaseResult errorResult = null;
            if (recvData.TryParseJsonToObj(out errorResult))
            {
                result.SetCode(errorResult.RtnCode);
                return result;
            }

            var payResultObj = JsonConvert.DeserializeObject(recvData);
            var payResult = aesCryptoHelper.Decrypt(payResultObj as string);

            var rtnACLinkDecryptModel = new ACLinkDecryptModel();
            if (!payResult.TryParseJsonToObj(out rtnACLinkDecryptModel))
            {
                result.SetCode(7030);
                return result;
            }
            
            if (!rtnACLinkDecryptModel.Json.TryParseJsonToObj(out result))
            {
                result.SetCode(7031);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 送至銀行驗證AccountLink付款結果
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        private DataResult<ACLinkPayRes> _PayedVerify(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, ACLinkPayRes payResultModel)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper(_ACLKey, _ACLIV);

            var postModel = new
            {
                Json = _GetACLinkQryModelData(tradeRequestModel, memberInfoModel, payResultModel),
                BankType = _GetBankType(tradeRequestModel.PaymentSubTypeID)
            };

            string postData = $"token={HttpUtility.UrlEncode(aesCryptoHelper.Encrypt(JsonConvert.SerializeObject(postModel)))}";

            // 送至Middleware進行查詢
            var recvData = _AccountLinkPaymentRepository.PayedVerify(_ACLQRYURL, postData);

            var result = new DataResult<ACLinkPayRes>().SetSuccess(new ACLinkPayRes());
            BaseResult errorResult = null;
            if (recvData.TryParseJsonToObj(out errorResult))
            {
                result.SetCode(errorResult.RtnCode);
                return result;
            }

            var payResultObj = JsonConvert.DeserializeObject(recvData);
            var payResult = aesCryptoHelper.Decrypt(payResultObj as string);

            var rtnACLinkDecryptModel = new ACLinkDecryptModel();
            if (!payResult.TryParseJsonToObj(out rtnACLinkDecryptModel))
            {
                result.SetCode(7030);
                return result;
            }
            
            if (!rtnACLinkDecryptModel.Json.TryParseJsonToObj(out result))
            {
                result.SetCode(7031);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 送至銀行驗證AccountLink付款結果
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        private DataResult<ACLinkPayRes> _Refund(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, ACLinkPayRes payResultModel)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper(_ACLKey, _ACLIV);

            var postModel = new
            {
                Json = _GetACLinkRefundModelData(tradeRequestModel, memberInfoModel, payResultModel),
                BankType = _GetBankType(tradeRequestModel.PaymentSubTypeID)
            };

            string postData = $"token={HttpUtility.UrlEncode(aesCryptoHelper.Encrypt(JsonConvert.SerializeObject(postModel)))}";

            // 送至Middleware進行查詢
            var recvData = _AccountLinkPaymentRepository.Refund(_ACLRFNURL, postData);

            var result = new DataResult<ACLinkPayRes>().SetSuccess(new ACLinkPayRes());
            BaseResult errorResult = null;
            if (recvData.TryParseJsonToObj(out errorResult))
            {
                result.SetCode(errorResult.RtnCode);
                return result;
            }

            var payResultObj = JsonConvert.DeserializeObject(recvData);
            var payResult = aesCryptoHelper.Decrypt(payResultObj as string);

            var rtnACLinkDecryptModel = new ACLinkDecryptModel();
            if (!payResult.TryParseJsonToObj(out rtnACLinkDecryptModel))
            {
                result.SetCode(7030);
                return result;
            }

            if (!rtnACLinkDecryptModel.Json.TryParseJsonToObj(out result))
            {
                result.SetCode(7031);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 查詢AccountLink付款結果
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        private TradeResModel _GetPayedResultInfo(TradeReqModel tradeRequestModel)
        {
            return  _AccountLinkPaymentRepository._GetPayedResultInfo(tradeRequestModel, _GetBankType(tradeRequestModel.PaymentSubTypeID));
        }

        private DataResult<TradeResModel> _TransTradeResponseModel
            (
                long tradeID,
                DataResult<ACLinkPayRes> payResultModel,
                DataResult<ACLinkPayRes> verifyResultModel,
                VirtualAccountModel virtualAccountModel = null
            )
        {
            var tradeResponseModel = new DataResult<TradeResModel>();
            tradeResponseModel.SetSuccess(new TradeResModel() { PaymentCenterTradeID = tradeID, PaymentDate = DateTime.Now });
            tradeResponseModel.RtnData.BankTradeNo = payResultModel.RtnData.BankTradeNo;
            tradeResponseModel.RtnData.PayRtnCode = payResultModel.RtnCode.ToString();
            tradeResponseModel.RtnData.PayRtnMsg = payResultModel.RtnMsg;
            tradeResponseModel.RtnData.VerifyRtnCode = verifyResultModel.RtnCode.ToString();
            tradeResponseModel.RtnData.VerifyRtnMsg = verifyResultModel.RtnMsg;

            return tradeResponseModel;
        }

        private string _GetACLinkModelData(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, VirtualAccountModel virtualAccountModel = null)
        {
            var result = string.Empty;

            BankType bankType = _GetBankType(tradeRequestModel.PaymentSubTypeID);
            switch (bankType)
            {
                case BankType.First:
                    result = JsonConvert.SerializeObject(new FirstACLinkPayModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        TradeAmt = (int)tradeRequestModel.Amount,
                        TimeStamp = DateTime.Now.ToString(),
                        TradeTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        BankType = BankType.First,
                        IDNO = memberInfoModel.IDNO,
                        INDTAccount = memberInfoModel.INDTAccount,
                        //PayeeAccount = virtualAccountModel.VirtualAccount,
                        TradeNote = "1_2_1_交易AccountLink付款"
                    });
                    break;
                case BankType.ChinaTrust:
                    result = JsonConvert.SerializeObject(new ChinaTrustACLinkPayModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        TradeAmt = (int)tradeRequestModel.Amount,
                        TimeStamp = DateTime.Now.ToString(),
                        BankType = BankType.ChinaTrust,
                        IDNO = memberInfoModel.IDNO,
                        INDTAccount = memberInfoModel.INDTAccount,
                        TradeModeID = tradeRequestModel.TradeModeID,
                        TradeNote = "1_2_1_交易AccountLin付款"
                    });
                    break;
                case BankType.Cathay:
                    result = JsonConvert.SerializeObject(new CathayACLinkPayModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        TradeAmt = (int)tradeRequestModel.Amount,
                        TimeStamp = DateTime.Now.ToString(),
                        BankType = BankType.Cathay,
                        IDNO = memberInfoModel.IDNO,
                        INDTAccount = tradeRequestModel.AccountID
                    });
                    break;
                default:
                    break;
            }

            return result;
        }

        private string _GetACLinkQryModelData(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, ACLinkPayRes payResultModel)
        {
            var result = string.Empty;

            BankType bankType = _GetBankType(tradeRequestModel.PaymentSubTypeID);
            switch (bankType)
            {
                case BankType.First:
                    result = JsonConvert.SerializeObject(new FisrtACLinkPayQryModel()
                    {
                        MID = tradeRequestModel.MID,
                        IDNO = memberInfoModel.IDNO,
                        SerMsgNo = payResultModel.BankTradeNo,
                        TimeStamp = DateTime.Now.ToString(),
                    });
                    break;
                case BankType.ChinaTrust:
                    result = JsonConvert.SerializeObject(new ChinaTrustACLinkPayQryModel()
                    {
                        MID = tradeRequestModel.MID,
                        IDNO = memberInfoModel.IDNO,
                        BankTradeNo = payResultModel.BankTradeNo,
                        TimeStamp = DateTime.Now.ToString(),
                    });
                    break;
                case BankType.Cathay:
                    break;
                default:
                    break;
            }

            return result;
        }

        private string _GetACLinkRefundModelData(TradeReqModel tradeRequestModel, AccountLinkInfoModel memberInfoModel, ACLinkPayRes payResultModel)
        {
            var result = string.Empty;

            BankType bankType = _GetBankType(tradeRequestModel.PaymentSubTypeID);
            switch (bankType)
            {
                case BankType.First:
                    result = JsonConvert.SerializeObject(new FisrtACLinkRefundModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        IDNO = memberInfoModel.IDNO,
                        INDTAccount = memberInfoModel.INDTAccount,
                        RefundTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        RefundAmt = (int)tradeRequestModel.Amount,
                        BankType = BankType.First,
                        TimeStamp = DateTime.Now.ToString(),
                        RefundNote = "逾時交易退款"
                    });
                    break;
                case BankType.ChinaTrust:
                    result = JsonConvert.SerializeObject(new ChinaTrustACLinkRefundModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        IDNO = memberInfoModel.IDNO,
                        BankTradeNo = payResultModel.BankTradeNo,
                        INDTAccount = memberInfoModel.INDTAccount,
                        RefundTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        RefundAmt = (int)tradeRequestModel.Amount,
                        TradeSource = tradeRequestModel.TradeModeID.Equals((int)eTradeMode.Topup) ? 3 : 1,
                        TimeStamp = DateTime.Now.ToString(),
                        TradeNote = "逾時交易退款"
                    });
                    break;
                case BankType.Cathay:
                    result = JsonConvert.SerializeObject(new CathayACLinkRefundModel()
                    {
                        MID = tradeRequestModel.MID,
                        TradeNo = tradeRequestModel.TradeNo,
                        IDNO = memberInfoModel.IDNO,
                        MsgNo = payResultModel.BankTradeNo,
                        INDTAccount = memberInfoModel.INDTAccount,
                        BankAccount = memberInfoModel.INDTAccount,
                        RefundTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        RefundAmt = (int)tradeRequestModel.Amount,
                        TimeStamp = DateTime.Now.ToString()
                    });
                    break;
                default:
                    break;
            }

            return result;
        }

        private BankType _GetBankType(int PaymentSubTypeID)
        {
            BankType bankType = BankType.First;
            switch (PaymentSubTypeID)
            {
                case 1: bankType = BankType.First; break;
                case 2: bankType = BankType.ChinaTrust; break;
                case 3: bankType = BankType.Cathay; break;
            }
            return bankType;
        }
        #endregion
    }
}
