using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserSend.Services
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Library.Models.ManageBank.FirstBank;
    using Models;
    using Repositories;
    using Library.Models.Payment;

    public class PaymentService
    {
        PaymentRepository _paymentRepository;

        public PaymentService(
            PaymentRepository paymentRepository
            )
        {
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// 取得待轉帳資訊發動轉帳指示
        /// </summary>
        /// <returns></returns>
        public DataResult<List<BankTransferSendModel>> ListBankTransferSend()
        {
            var result = new DataResult<List<BankTransferSendModel>>();
            result.SetError();

            var list = _paymentRepository.ListBankTransferSend();
            if (list.Count == 0)
            {
                return result;
            }

            result.SetSuccess(list);
            return result;
        }

        public B2B001 CreateB2B001Model(BankTransferSendModel data, BankTransferSettingModel setting)
        {
            var model = new B2B001();
            var item = new B2B001.RecordModel();
            var payeeInfo = new PayeeInfoModel();

            payeeInfo.PayeeAccountId = data.TransAccount;
            payeeInfo.PayeeName = data.AccountName;
            payeeInfo.PayeeBankId = data.BankCode;
            payeeInfo.PayeeAccount = data.BankAccount.PadLeft(14, '0');

            item.RecordSeqNo = 1;
            item.PmtRemitRefId = data.TransferID.ToString();
            item.PayAccountId = setting.PayAccountId;
            item.PayAccountName = setting.PayAccountName;
            item.PayAccount = setting.PayAccount;
            item.PayAmount = data.Amount;
            item.ChargeRegulation = setting.ChargeRegulation;
            item.PayDate = DateTime.Today.ToString("yyyy-MM-dd");
            item.SettlePath = "2";
            item.PayeeInfo = payeeInfo;

            model.Record = new List<B2B001.RecordModel>();
            model.Record.Add(item);
            model.ChksumRecord = model.Record.Count;
            return model;
        }

        /// <summary>
        /// 更新待轉帳資訊發動轉帳指示為已送驗
        /// </summary>
        /// <param name="TransferID">提領記錄編號</param>
        /// <returns></returns>
        public BaseResult UpdateBankTransferSend(long TransferID, XML<TxResult<B2BResult>> b2bResult)
        {
            return _paymentRepository.UpdateBankTransferSend(TransferID);
        }
    }
}
