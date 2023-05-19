using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.PaymentCenterApi.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Interface.ATM;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Xml;
using AutoMapper;
using ICP.Infrastructure.Core.Helpers;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class ATMPaymentService : IPaymentMethod
    {
        private PaymentSubType_ATM[] _needNotifyList;
        private ATMPaymentRepository _atmPaymentRepository;
        private IATMServiceFactory _atmServiceFactory;

        public ATMPaymentService(ATMPaymentRepository atmPaymentRepository, IATMServiceFactory atmServiceFactory)
        {
            _atmPaymentRepository = atmPaymentRepository;
            _atmServiceFactory = atmServiceFactory;

            _needNotifyList = new PaymentSubType_ATM[]
            {
                PaymentSubType_ATM.First
            };
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

            var paymentType = (PaymentSubType_ATM)tradeRequestModel.PaymentSubTypeID;
            if (paymentType < PaymentSubType_ATM.Min ||
                paymentType > PaymentSubType_ATM.Max)
            {
                result.SetCode(7029);
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
            PaymentSubType_ATM paymentSubType_ATM = (PaymentSubType_ATM)tradeRequestModel.PaymentSubTypeID;

            // 建立虛擬帳號
            var virtualAccountModel = _atmPaymentRepository.CreateVirtualAccount(tradeRequestModel);
            if (!virtualAccountModel.IsSuccess)
            {
                return result.SetError(virtualAccountModel);
            }
            result.RtnData.VirtualAccount = virtualAccountModel.VirtualAccount;
            result.RtnData.ATMTradeDate = DateTime.Now;     // ATM 交易日期
            result.RtnData.ATMExpireDate = DateTime.Today.AddDays(GetBankExpireDayNumber(paymentSubType_ATM) + 1).AddSeconds(-1);    // 儲值期限：建單指定天數後的 23:59

            // 通知銀行虛擬帳號建立資訊
            if (_needNotifyList.Contains(paymentSubType_ATM))
            {
            var atmNotifyModel = Mapper.Map<AtmNotifyModel>(tradeRequestModel);
                atmNotifyModel.VirtualAccount = result.RtnData.VirtualAccount;
                atmNotifyModel.ExpireDate = result.RtnData.ATMExpireDate;
                atmNotifyModel.ApplyType = "Y";

                var notifyResult = NotifyBank(paymentSubType_ATM, atmNotifyModel);
                if (!notifyResult.IsSuccess)
                {
                    return result.SetError(notifyResult);
                }
            }

            return result;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult AddTrade(TradeReqModel tradeRequestModel)
        {
            return _atmPaymentRepository.AddTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            return _atmPaymentRepository.UpdateTrade(tradeResponseModel.RtnData);
        }

        /// <summary>
        /// 通知銀行虛擬帳號...等資訊
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="atmNotifyModel"></param>
        /// <returns></returns>
        private BaseResult NotifyBank(PaymentSubType_ATM bankType, AtmNotifyModel atmNotifyModel)
        {
            var atm = _atmServiceFactory.Create(bankType);
            var notifyResult = atm.NotifyBank(atmNotifyModel);

            return notifyResult;
        }

        /// <summary>
        /// 更新 PaymentCenter 通知銀行狀態
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <param name="notifyBankStatus"></param>
        /// <param name="notifyBankDateTime"></param>
        /// <returns></returns>
        public BaseResult UpdateNotifyBankStatus(string virtualAccount, int notifyBankStatus, DateTime notifyBankDateTime)
        {
            return _atmPaymentRepository.UpdateNotifyBankStatus(virtualAccount, notifyBankStatus, notifyBankDateTime);
        }

        /// <summary>
        /// 更新 Payment 的通知銀行狀態
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <param name="notifyBankStatus"></param>
        /// <param name="notifyBankDateTime"></param>
        /// <returns></returns>
        public BaseResult UpdatePaymentNotifyBankStatus(string virtualAccount, int notifyBankStatus, DateTime notifyBankDateTime)
        {
            string postUrl = "http://localhost:3312/api/Payment/ReceiveAtmServerReturn/UpdateNotifyBankStatus";     // payment 的更新通知銀行狀態網址
            var postData = new
            {
                JsonData = JsonConvert.SerializeObject(new { VirtualAccount = virtualAccount, NotifyBankStatus = notifyBankStatus, NotifyBankDateTime = notifyBankDateTime.ToString("yyyy/MM/dd HH:mm:ss") })
            };
            string response = new NetworkHelper().DoRequestWithJson(postUrl, JsonConvert.SerializeObject(postData), 300, null, null);
            BaseResult updatePaymentNotifyBankStatusResult = JsonConvert.DeserializeObject<BaseResult>(response);

            return updatePaymentNotifyBankStatusResult;
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
            throw new Exception("ATM 無退款功能");
        }

        /// <summary>
        /// 退款訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryRefundTradeModel QryRefundTrade(RefundReqModel tradeRequestModel)
        {
            throw new Exception("ATM 無退款功能");
        }

        /// <summary>
        /// 新增退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddRefundTradeModel AddRefundTrade(RefundReqModel tradeRequestModel)
        {
            throw new Exception("ATM 無退款功能");
        }

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> tradeResponseModel)
        {
            throw new Exception("ATM 無退款功能");
        }

        /// <summary>
        /// 逾時退款交易執行
        /// </summary>
        /// <typeparam name="tradeRequestModel"></typeparam>
        /// <returns></returns>
        public void TimeoutRefundProcess(TradeReqModel tradeRequestModel)
        {
            throw new Exception("ATM 無退款功能");
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
            throw new Exception("ATM 無取消功能");
        }

        /// <summary>
        /// 取消訂單查詢
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public QryReversalTradeModel QryReversalTrade(ReversalReqModel tradeRequestModel)
        {
            throw new Exception("ATM 無取消功能");
        }

        /// <summary>
        /// 新增取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddReversalTradeModel AddReversalTrade(ReversalReqModel tradeRequestModel)
        {
            throw new Exception("ATM 無取消功能");
        }

        /// <summary>
        /// 更新取消訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> tradeResponseModel)
        {
            throw new Exception("ATM 無取消功能");
        }
        #endregion

        /// <summary>
        /// 取得各銀行的 ATM 轉帳期限天數
        /// </summary>
        /// <returns></returns>
        private int GetBankExpireDayNumber(PaymentSubType_ATM paymentSubTypeATM)
        {
            switch (paymentSubTypeATM)
            {
                case PaymentSubType_ATM.First:
                    return 1;
                default:
                    return 0;
    }
}
    }
}
