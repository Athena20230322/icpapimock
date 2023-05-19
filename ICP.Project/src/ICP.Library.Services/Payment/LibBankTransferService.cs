using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.Payment
{
    using Library.Models.ManageBank.FirstBank;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Repositories.Payment;

    public class LibBankTransferService
    {
        PaymentRepository _paymentRepository;

        public LibBankTransferService(
            PaymentRepository paymentRepository
            )
        {
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// 取得待轉帳資訊發動轉帳指示設定
        /// </summary>
        /// <returns></returns>
        public DataResult<BankTransferSettingModel> GetBankTransferSetting()
        {
            var result = new DataResult<BankTransferSettingModel>();
            result.SetError();

            var rtnData = _paymentRepository.GetBankTransferSetting();
            if (rtnData == null)
            {
                result.RtnMsg = "提領設定為空";
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
    }
}
