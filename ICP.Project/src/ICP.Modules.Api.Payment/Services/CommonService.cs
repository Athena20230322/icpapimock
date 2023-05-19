using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.Payment;
using ICP.Library.Repositories.Payment;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Interface;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services
{
    public class CommonService
    {
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly PaymentTypeRepository _paymentTypeRepository = null;

        public CommonService(
            PaymentCommonService paymentCommonService,
            PaymentTypeRepository paymentTypeRepository
        )
        {
            _paymentCommonService = paymentCommonService;
            _paymentTypeRepository = paymentTypeRepository;
        }

        /// <summary>
        /// 檢查驗證碼
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public bool ValidateCheckMacValue(object o, string checkMacValue, NameValueCollection queryString = null)
        {
            string checkVal = _paymentCommonService.GenerateCheckMacValue(o,
                                                GlobalConfigUtil.SYS_HashKey,
                                                GlobalConfigUtil.SYS_HashIV);

            string checkVal2 = _paymentCommonService.GenerateCheckMacValue(queryString,
                                                GlobalConfigUtil.SYS_HashKey,
                                                GlobalConfigUtil.SYS_HashIV);

            return checkVal.Equals(checkMacValue) || checkVal2.Equals(checkMacValue);

        }

        /// <summary>
        /// 設定回傳值得CheckMacValue
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public T1 SetChckMacValueResult<T1, T2>(T1 t1, T2 t2)
            where T1 : BaseResult, ICheckMacValueResult//, new()
            where T2 : BaseResult
        {
            if(t1 == null)
            {
                //t1 = new T1();
                t1 = System.Activator.CreateInstance<T1>();
            }

            if (t2 != null)
            {
                t1.SetResult(t2);
            }

            if (string.IsNullOrWhiteSpace(t1.CheckMacValue))
            {
                t1.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(t1, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);
            }

            return t1;
        }
            
        /// <summary>
        /// 由PayID分析取得付款類別代碼與子代碼
        /// </summary>
        /// <param name="payID"></param>
        /// <param name="paymentType"></param>
        /// <param name="paymentSubType"></param>
        public void GetPaymentTypeIDByPayID(string payID, out int paymentType, out int paymentSubType, out long accountID)
        {
            //### 付款類別代碼
            int.TryParse(!string.IsNullOrWhiteSpace(payID) ? payID.Substring(0, 1) : "", out paymentType);
            //### 付款子類別代碼
            paymentSubType = 1;

            int paymentTypeID = paymentType;
            string tmpAccountID = "";
            accountID = 0;

            if (ePaymentType.ACCOUNTLINK == (ePaymentType)paymentType)
            {
                //### PayID: AccountLink => 2(1碼) + BankCode (3碼) + INDTAccount (綁定識別碼)
                tmpAccountID = payID.Length > 4 ? payID.Substring(4) : "";

                long.TryParse(tmpAccountID, out accountID);                

                //### 取得PaymentSubTypeID
                //### 取得銀行代碼
                string bankCode = payID.Length > 4 ? payID.Substring(1, 3) : "";

                List<TradeTypeModel> tradeTypes = _paymentTypeRepository.ListTradeSubType(paymentType);

                if (tradeTypes != null && tradeTypes.Count() > 0 && tradeTypes.Where(x => x.PaymentTypeID == paymentTypeID && x.BankCode == bankCode).Count() > 0)
                {
                    paymentSubType = tradeTypes.Single(x => x.PaymentTypeID == paymentTypeID && x.BankCode == bankCode).PaymentSubTypeID;
                }
            }
        }
    }
}
