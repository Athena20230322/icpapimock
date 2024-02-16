using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserQuery.Services
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
        /// 取得已送驗轉帳資訊 轉帳資料編號, FXML訊息編號
        /// </summary>
        /// <returns></returns>
        public DataResult<List<BankTransferQueryModel>> ListBankTransferQuery()
        {
            var result = new DataResult<List<BankTransferQueryModel>>();
            result.SetError();

            var list = _paymentRepository.ListBankTransferQuery();
            if (list.Count == 0)
            {
                return result;
            }

            result.SetSuccess(list);
            return result;
        }

        public B2B002 CreateB2B002Model(BankTransferQueryModel data, BankTransferSettingModel setting)
        {
            var model = new B2B002();
            model.SvcRqID = data.SvcRqId;
            return model;
        }

        /// <summary>
        /// 更新已送驗轉帳資訊 查詢結果
        /// </summary>
        /// <param name="TransferID">提領記錄編號</param>
        /// <param name="b2bResult">查詢結果</param>
        /// <returns></returns>
        public BaseResult UpdateBankTransferQuery(long TransferID, XML<TxResult<B2B002Result>> b2bResult)
        {
            var result = new BaseResult();
            result.SetError();

            byte PayStatus = 0;
            string ErrorCode = null;
            string ErrorMsg = null;

            /*
             * StatusCode 錯誤代碼
             * 04:付款明細檢核有誤
             * 00:處理中
             * 20:處理完成
             */
            var StatusCode = b2bResult.Tx.TxRs.StatusCode;
            if (StatusCode == "00")
            {
                result.SetSuccess();
                return result;
            }

            if (StatusCode == "20")
            {
                PayStatus = 1;  // 付款狀態 1:已完成轉帳
            }
            else
            {
                PayStatus = 2;  // 付款狀態 2: 轉帳失敗
                ErrorCode = StatusCode;
                ErrorMsg = b2bResult.Tx.TxRs.StatusDesc;
            }

            // 更新提領記錄
            return _paymentRepository.UpdateBankTransferQuery(TransferID, PayStatus, ErrorCode, ErrorMsg);
        }
    }
}
