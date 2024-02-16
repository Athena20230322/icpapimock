using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Repositories.Payment;
using ICP.Library.Services.MemberApi;
using ICP.Modules.Api.Authorization.Services;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeOnline;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services
{
    public class ChargeOnlineService
    {
        private readonly CertificateMemberApiService _certificateMemberApiService = null;
        private readonly IdentifyService _identifyService = null;
        private readonly PaymentTradeRepository _paymentTradeRepository = null;
        private readonly PaymentTypeRepository _paymentTypeRepository = null;
        private readonly CommonService _commonService = null;
        private readonly ILogger _logger = null;

        public ChargeOnlineService(
            CertificateMemberApiService certificateMemberApiService,
            IdentifyService identifyService,
            PaymentTradeRepository paymentTradeRepository,
            PaymentTypeRepository paymentTypeRepository,
            CommonService commonService,
            ILogger<ChargeOnlineService> logger
        )
        {
            _certificateMemberApiService = certificateMemberApiService;
            _identifyService = identifyService;
            _paymentTradeRepository = paymentTradeRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _commonService = commonService;
            _logger = logger;
        }

        /// <summary>
        /// 取得暫存的訂單資訊
        /// </summary>
        /// <param name="chargeOnlineRequest"></param>
        /// <returns></returns>
        public DataResult<CashierReq> GetCashierReq(ChargeOnlineRequest chargeOnlineRequest)
        {
            DataResult<CashierReq> result = new DataResult<CashierReq>();
            result.SetError();

            chargeOnlineRequest.AppData.TryParseJsonToObj<AppDataModel>(out AppDataModel appSignInfo);

            if (appSignInfo == null)
            {
                return result;
            }

            #region Dev TEST測試需先mark
            var getClientAesCertResult = _certificateMemberApiService.GetClientAesCert(appSignInfo.EncKeyID);

            if (!getClientAesCertResult.IsSuccess)
            {
                result.SetError(getClientAesCertResult);
                return result;
            }

            // 取得 Rsa 金鑰
            var clientAesCert = getClientAesCertResult.RtnData;

            // 解密密文
            DataResult<EncDataModel> decryptResult = _identifyService.DecryptClientAesData<EncDataModel>(clientAesCert.AES_Key, clientAesCert.AES_IV, appSignInfo.EncData);
            #endregion

            //### TEST Start
            //DataResult<EncDataModel> decryptResult = new DataResult<EncDataModel>();
            //var data = appSignInfo.EncData.TryParseJsonToObj<EncDataModel>(out EncDataModel encdata);
            //decryptResult.SetSuccess(encdata);
            //### TEST End

            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            //### Temp Table取得訂單資訊
            PaymentTempTradeDbRes paymentTemp = _paymentTradeRepository.GetTempTradeNoInfo(decryptResult.RtnData);

            if (!paymentTemp.IsSuccess)
            {
                result.SetCode(paymentTemp.RtnCode);
                return result;
            }           

            //### Temp Table取得訂單明細
            List<ItemModel> items = _paymentTradeRepository.GetTempTradeItemsDetailInfo(decryptResult.RtnData);

            if (items != null && items.Count() > 0)
            {
                paymentTemp.ItemList = JsonConvert.SerializeObject(items);
            }

            _commonService.GetPaymentTypeIDByPayID(paymentTemp.PayID, out int paymentType, out int paymentSubType, out long accountID);

            result.SetSuccess(new CashierReq()
            {
                MerchantID = paymentTemp.MerchantID,
                PlatformID = paymentTemp.PlatformID,
                MerchantTradeNo = paymentTemp.MerchantTradeNo,
                MerchantTradeDate = paymentTemp.MerchantTradeDate,
                Amount = paymentTemp.Amount / 100,
                BonusAmt = paymentTemp.BonusAmt / 100,
                ItemAmt = paymentTemp.ItemAmt / 100,
                UtilityAmt = paymentTemp.UtilityAmt / 100,
                CommAmt = paymentTemp.CommAmt / 100,
                ExceptAmt1 = paymentTemp.ExceptAmt1 / 100,
                ExceptAmt2 = paymentTemp.ExceptAmt2 / 100,
                RedeemFlag = paymentTemp.RedeemFlag,
                DebitPoint = paymentTemp.DebitPoint / 100,
                NonRedeemAmt = paymentTemp.NonRedeemAmt / 100,
                NonPointAmt = paymentTemp.NonPointAmt / 100,
                StoreID = paymentTemp.StoreId,
                StoreName = paymentTemp.StoreName,
                CarrierType = paymentTemp.CarrierType,
                WalletID = paymentTemp.WalletId,
                MID = paymentTemp.MID,
                TradeModeID = (int)eTradeMode.Transaction, //### 交易
                PaymentTypeID = paymentType,
                PaymentSubTypeID = paymentSubType,
                AccountID = accountID,
                PayID = paymentTemp.PayID,
                OWSubmitDate = decryptResult.RtnData.TimeStamp,
                Remark = paymentTemp.Remark,
                ItemList = paymentTemp.ItemList,
            });

            return result;
        }
    }
}
