using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserQuery.Services
{
    using Infrastructure.Core.Models;
    using Library.Models.MemberModels;
    using Models;
    using Library.Models.ManageBank.FirstBank;
    using Infrastructure.Core.Extensions;
    using Library.Repositories.MemberRepositories;

    public class MemberBankService: Library.Services.MemberServices.LibMemberBankService
    {
        public MemberBankService(
            MemberBankRepository memberBankRepository
            ): base(memberBankRepository)
        {

        }

        /// <summary>
        /// 更新銀行帳戶狀態, 同意升2類會員
        /// </summary>
        /// <param name="TransferID">提領記錄編號</param>
        /// <param name="AgreeLevelUp">同意升級為二類會員</param>
        /// <param name="b2bResult">查詢結果</param>
        /// <returns></returns>
        public BaseResult UpdateMemberBankAccountStatus(BankTransferQueryModel query, XML<TxResult<B2B002Result>> b2bResult)
        {
            var result = new BaseResult();
            result.SetError();

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

            var model = new UpdateBankAccountStatusModel
            {
                MID = query.MID,
                Category = 0,
                BankCode = query.BankCode,
                BankAccount = query.BankAccount,
                AgreeLevelUp = query.AgreeLevelUp
            };

            if (StatusCode == "20")
            {
                model.AccountStatus = 1;
            }
            else
            {
                model.AccountStatus = 2;
            }

            return UpdateMemberBankAccountStatus(model);
        }
    }
}