using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Repositories.MemberRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    public class LibMemberBankService
    {
        MemberBankRepository _memberBankRepository;

        public LibMemberBankService(MemberBankRepository memberBankRepository)
        {
            _memberBankRepository = memberBankRepository;
        }

        /// <summary>
        /// 新增會員銀行帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddMemberBankAccount(MemberBankAccount model)
        {
            return _memberBankRepository.AddMemberBankAccount(model);
        }

        /// <summary>
        /// 更新會員銀行帳戶驗證結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateMemberBankAccountStatus(UpdateBankAccountStatusModel model)
        {
            return _memberBankRepository.UpdateMemberBankAccountStatus(model);
        }

        /// <summary>
        /// 新增一元驗證資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddBankTransferOnBankAccount(MemberBankAccount model)
        {
            return _memberBankRepository.AddBankTransferOnBankAccount(model);
        }

        /// <summary>
        /// 取得會員的銀行帳號資料
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<MemberBankInfo> ListMemberBankInfo(long MID, byte? Category = null)
        {
            return _memberBankRepository.ListMemberBankInfo(MID, Category);
        }

        /// <summary>
        /// 查詢待提領帳戶餘額
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UserCoinsBalance GetUserCoinsBalance(long MID)
        {
            return _memberBankRepository.GetUserCoinsBalance(MID);
        }

        /// <summary>
        /// 新增提領
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WithdrawBalanceResult AddWithdrawBalance(WithdrawBalance model)
        {
            return _memberBankRepository.AddWithdrawBalance(model);
        }

        /// <summary>
        /// 取得單筆銀行明細
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public BankDetail GetBankDetail(string BankCode)
        {
            return _memberBankRepository.GetBankDetail(BankCode);
        }

        /// <summary>
        /// 提領失敗退款
        /// </summary>
        /// <param name="TransferID"></param>
        /// <returns></returns>
        public BaseResult RtnWithdrawBalance(long TransferID, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberBankRepository.RtnWithdrawBalance(TransferID, RealIP, ProxyIP);
        }

        public BaseResult IsValidateBankAccount(string bankAccount, out string rtnBankAccount)
        {
            var result = new BaseResult
            {
                RtnCode = 0,
                RtnMsg = "銀行帳號 格式錯誤"
            };

            rtnBankAccount = string.Empty;

            if (string.IsNullOrWhiteSpace(bankAccount))
            {
                return result;
            }

            rtnBankAccount = bankAccount;
            while (rtnBankAccount.Length < 14)
            {
                rtnBankAccount = "0" + rtnBankAccount;
            }

            result.SetSuccess();
            result.RtnMsg = string.Empty;

            return result;
        }

        public AuthFinancialResult GetAuthFinancial(long MID)
        {
            return _memberBankRepository.GetAuthFinancial(MID);
        }

        /// <summary>
        /// 取得會員銀行帳號資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public MemberBankAccount GetBankAccount(long AccountID)
        {
            return _memberBankRepository.GetBankAccount(AccountID);
        }

        /// <summary>
        /// 更新已送驗轉帳資訊
        /// </summary>
        /// <returns></returns>
        public BaseResult UpdateBankTransferQuery(long TransferID, byte PayStatus, string ErrorCode = null, string ErrorMsg = null)
        {
            return _memberBankRepository.UpdateBankTransferQuery(TransferID, PayStatus, ErrorCode, ErrorMsg);
        }

        /// <summary>
        /// 取得銀行種類
        /// </summary>
        /// <returns></returns>
        public List<BankTypeModel> ListBankType() => _memberBankRepository.ListBankType();

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <param name="BankTypeID">銀行種類</param>
        /// <returns></returns>
        public List<BankCodeModel> ListBankCode(byte BankTypeID) => _memberBankRepository.ListBankCode(BankTypeID);

        /// <summary>
        /// 取得銀行分行清單
        /// </summary>
        /// <returns></returns>
        public List<BankBranchCodeModel> ListBankBranchCode(string BankCode) => _memberBankRepository.ListBankBranchCode(BankCode);
    }
}
