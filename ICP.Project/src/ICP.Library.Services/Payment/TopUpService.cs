using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.Payment;
using ICP.Library.Models.TopUp;
using ICP.Library.Repositories.Payment;
using Newtonsoft.Json;
using System;

namespace ICP.Library.Services.Payment
{
    public class TopUpService
    {
        private readonly TopUpRepository _topUpRepository = null;
        private readonly PaymentRepository _paymentRepository = null;
        private readonly ILogger _logger = null;

        public TopUpService(
            TopUpRepository topUpRepository,
            PaymentRepository paymentRepository,
            ILogger<TopUpService> logger)
        {
            _topUpRepository = topUpRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        #region 取得會員帳戶相關資訊
        /// <summary>
        /// 取得會員儲值帳戶資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public TopUpLimitModel GetMemberTopUpInfo(long mid)
        {
            return _topUpRepository.GetTopUpLimit(mid);
        }

        /// <summary>
        /// 檢查儲值金額是否已超過儲值上限
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="tradeAmount"></param>
        /// <returns></returns>
        public DataResult<object> CheckTopUpLimit(long mid, int tradeAmount)
        {
            var result = new DataResult<object>();
            result.SetError();

            TopUpLimitModel model = this.GetMemberTopUpInfo(mid);

            if (model != null)
            {
                if (tradeAmount > model.AvailableTopUp)
                {
                    result.SetCode(2080, model);
                    _logger.Trace($"超過儲值上限: mid={mid},tradeAmount={tradeAmount},AvaliableCash={model.AvailableTopUp}");
                    return result;
                }
            }
            result.SetSuccess();

            return result;
        }
        #endregion

        #region 儲值滿額記錄
        /// <summary>
        /// 儲值訂單付款後，於入儲值前檢查此筆金額是否會超過可儲值額度
        /// 超過可儲值額度則寫入超額記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<object> CheckTopUpLimitAfterPaid(CheckTopUpLimitModel model)
        {
            var result = new DataResult<object>();
            result.SetError();

            var checkResult = this.CheckTopUpLimit(model.MID, model.Amount);
            if (!checkResult.IsSuccess)
            {
                _logger.Trace($"超過儲值上限，準備寫入儲值超額記錄: {JsonConvert.SerializeObject(model)}");

                BaseResult addResult = this.AddTopUpOverLimitRecords(model);
                if (addResult.IsSuccess)
                {
                    result.SetCode(2);
                    _logger.Trace($"儲值超額記錄寫入成功: result:{JsonConvert.SerializeObject(result)}, {JsonConvert.SerializeObject(model)}");
                }
                else
                {
                    result.SetCode(1001);
                    _logger.Trace($"儲值超額記錄寫入失敗: result:{JsonConvert.SerializeObject(result)}, {JsonConvert.SerializeObject(model)}");
                }
            }
            else
            {
                result.SetSuccess();
            }

            return result;
        }

        /// <summary>
        /// 寫入儲值滿額記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddTopUpOverLimitRecords(CheckTopUpLimitModel model)
        {
            OverLimitDbReq dbReq = new OverLimitDbReq();
            dbReq.MID = model.MID;
            dbReq.TradeID = model.TradeID;
            dbReq.PaymentTypeID = model.PaymentTypeID;
            dbReq.PaymentSubTypeID = model.PaymentSubTypeID;
            dbReq.Amount = model.Amount;

            return _topUpRepository.AddTopUpOverLimitRecords(dbReq);
        }
        #endregion

        #region 取得會員儲值條碼
        /// <summary>
        /// 取得會員儲值條碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<string> GetTopUpBarCode(GetTopUpBarcodeReq model)
        {
            var result = new DataResult<string>();
            result.SetError();

            _logger.Trace($"準備取得儲值條碼: {JsonConvert.SerializeObject(model)}");

            AddBarcodeDBReq addModel = new AddBarcodeDBReq
            {
                MID = model.MID,
                BarcodeType = "88",     // 儲值條碼
                PaymentType = model.PaymentType,
                Amount = model.Amount,
                Timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            };

            AddBarcodeDBRes rtnMode = _paymentRepository.AddBarcode(addModel);

            if (rtnMode.IsSuccess)
            {
                result.SetSuccess(rtnMode.Barcode);
            }

            _logger.Trace($"儲值條碼取得結果: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        #endregion
    }
}
