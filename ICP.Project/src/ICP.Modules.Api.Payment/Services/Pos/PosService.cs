using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Payment.Models.Pos;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.Pos
{
    public class PosService
    {
        private readonly PosRepository _posRepository = null;
        private readonly ILogger _logger = null;

        public PosService(
            PosRepository posRepository,
            ILogger<PaymentService> logger
        )
        {
            _posRepository = posRepository;
            _logger = logger;
        }

        /// <summary>
        /// 由條碼取得會員條碼資料，若queryType為1或2則會驗證條碼資訊是否正確，正確才回傳
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="queryType">1: 交易中確認驗證扣款條碼是否正確及回傳條碼　2: 交易中確認驗證儲值條碼是否正確及回傳條碼　3: 僅查詢條碼資訊</param>
        /// <returns></returns>
        public DataResult<BarcodeInfoDbRes> GetBarcodeInfo(string barcode, int queryType, decimal amount)
        {
            DataResult<BarcodeInfoDbRes> result = new DataResult<BarcodeInfoDbRes>();

            BarcodeInfoDbRes barcodeInfoDbRes = _posRepository.GetBarcodeInfo(barcode, queryType, amount);

            if (barcodeInfoDbRes == null)
            {
                result.SetCode(2071); //取得條碼資訊發生錯誤
            }
            else
            {
                if(!barcodeInfoDbRes.IsSuccess)
                {
                    result.SetError(barcodeInfoDbRes);
                }
                else
                {
                    result.SetSuccess(barcodeInfoDbRes);
                }
            }

            return result;
        }

        /// <summary>
        /// 檢核request參數
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="requestModel"></param>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public DataResult<T1> ModelValidate<T1, T2>(T2 requestModel, long merchantID)
            where T1 : BaseAuthorizationApiResult
            where T2 : BasePosReq
        {
            DataResult<T1> result = new DataResult<T1>() { RtnCode = 1 };
            int rtnCode = 1;

            if (requestModel.PlatformID > 0 && requestModel.PlatformID != merchantID)
            {
                rtnCode = 2075; //平臺商編號錯誤
            }
            else if (requestModel.PlatformID == 0 && requestModel.MerchantID != merchantID)
            {
                rtnCode = 2061; //廠商編號錯誤
            }
            else if (!requestModel.IsValid())
            {
                result.SetFormatError(requestModel.GetFirstErrorMessage());
            }

            if (result.RtnCode == 1 && rtnCode != 1)
            {
                result.SetCode(rtnCode);
            }

            return result;
        }
        /// <summary>
        /// 由儲值條碼分析取得付款類別代碼與子代碼
        /// </summary>
        /// <param name="payID"></param>
        /// <param name="paymentType"></param>
        /// <param name="paymentSubType"></param>
        public void GetPaymentTypeIDByTopUpBarcode(string barcode, out int paymentType, out int paymentSubType)
        {
            //### 付款類別代碼
            paymentType = 0;
            //### 付款子類別代碼
            paymentSubType = 1;

            switch(barcode.Substring(0, 3))
            {
                case "881":
                    paymentType = 4;
                    break;
                case "882":
                    paymentType = 5;
                    break;
            }
        }
    }
}