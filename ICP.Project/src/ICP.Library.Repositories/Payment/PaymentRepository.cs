using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.Payment
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Models.Payment;
    using Models.ManageBank.FirstBank;

    public class PaymentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public PaymentRepository(
            IDbConnectionPool dbConnectionPool
        )
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 撈取訂單資訊 
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public GetTradeInfoDbRes GetTradeInfo(GetTradeInfoDbReq getTradeInfoDbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTradeInfo_S]";

            var args = new
            {
                MerchantTradeNo = getTradeInfoDbReq.MerchantTradeNo,
                MerchantID = getTradeInfoDbReq.MerchantID,
                PlatformID = getTradeInfoDbReq.PlatformID,
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<GetTradeInfoDbRes>(sql, args);
        }

        /// <summary>
        /// 新增條碼
        /// </summary>
        /// <param name="addBarcodeRequest"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual AddBarcodeDBRes AddBarcode(AddBarcodeDBReq addBarcodeRequest)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_AddBarcode_SIU]";

            var args = new
            {
                addBarcodeRequest.MID,
                addBarcodeRequest.BarcodeType,
                addBarcodeRequest.Timestamp,
                addBarcodeRequest.PaymentType,
                addBarcodeRequest.PayID,
                addBarcodeRequest.Amount
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<AddBarcodeDBRes>(sql, args);
        }

        /// <summary>
        /// 取得待轉帳資訊發動轉帳指示設定
        /// </summary>
        /// <returns></returns>
        public BankTransferSettingModel GetBankTransferSetting()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_GetBankTransferSetting_S";

            return db.QuerySingleOrDefault<BankTransferSettingModel>(sql);
        }
    }
}
