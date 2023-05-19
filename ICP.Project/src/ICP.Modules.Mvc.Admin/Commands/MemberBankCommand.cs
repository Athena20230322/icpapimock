using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class MemberBankCommand
    {
        MemberBankService _memberBankService;
        LibMemberBankService _libMemberBankService;

        public MemberBankCommand(
            MemberBankService memberBankService,
            LibMemberBankService libMemberBankService
            )
        {
            _memberBankService = memberBankService;
            _libMemberBankService = libMemberBankService;

        }

        #region 取得銀行帳號列表
        /// <summary>
        /// 取得銀行帳號列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberBankAccount> ListAuthMemberBankAccount(QueryMemberBankAccountVM model)
        {
            DateTime startDate = Convert.ToDateTime(model.StartDate);
            DateTime endDate = Convert.ToDateTime(model.EndDate);

            byte? accountStatus = null;
            byte? paperAuthStatus = null;
            switch (model.Status)
            {
                case AuthStatusType.NoneAuth:
                    accountStatus = 0;
                    paperAuthStatus = 0;
                    break;
                case AuthStatusType.PaperPass:
                    paperAuthStatus = 1;
                    break;
                case AuthStatusType.PaperFail:
                    paperAuthStatus = 2;
                    break;
                case AuthStatusType.AuthPass:
                    accountStatus = 1;
                    break;
                case AuthStatusType.AuthFail:
                    accountStatus = 2;
                    break;
                case AuthStatusType.All:
                default:
                    break;
            }

            var query = new QueryMemberBankAccount
            {
                StartDate = startDate,
                EndDate = endDate,
                AccountStatus = accountStatus,
                PaperAuthStatus = paperAuthStatus,
                CName = model.CName,
                ICPMID = model.ICPMID,
                PageNo = model.PageNo,
                PageSize = model.PageSize
            };

            return _memberBankService.ListAuthMemberBankAccount(query);
        }
        #endregion

        /// <summary>
        /// 取得會員銀行帳號
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public EditBankAccountVM GetBankAccount(long AccountID)
        {
            var bankAccount = _libMemberBankService.GetBankAccount(AccountID);

            var model = new UpdateMemberBankAccount
            {
                BankCode = bankAccount.BankCode,
                BankBranchCode = bankAccount.BankBranchCode,
                BankAccount = bankAccount.BankAccount,
                FilePath1 = bankAccount.FilePath1
            };

            return new EditBankAccountVM
            {
                MemberBankAccount = model
            };
        }

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <returns></returns>
        public List<Library.Models.MemberModels.MemberBankDetail> ListBankDetail()
        {
            return _memberBankService.ListBankDetail();
        }

        /// <summary>
        /// 取得銀行分行清單
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public List<Library.Models.MemberModels.BankBranchCodeModel> ListBankCode(string BankCode) => _libMemberBankService.ListBankBranchCode(BankCode);
        

        /// <summary>
        /// 更新會員銀行帳號資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateBankAccount(long AccountID, EditBankAccountVM model, string urlDir, string saveDir, string Modifier)
        {
            var result = new BaseResult();
            var memberBankAccount = model.MemberBankAccount;

            string bankAccount = string.Empty;
            var verifyResult = _libMemberBankService.IsValidateBankAccount(memberBankAccount.BankAccount, out bankAccount);
            if (!verifyResult.IsSuccess)
            {
                result.SetError(verifyResult);
                return result;
            }
            memberBankAccount.BankAccount = bankAccount;

            if (model.FileUpload1 != null)
            {
                var saveFileResult = _memberBankService.SaveBankAccountImages(1, model.FileUpload1, urlDir, saveDir, memberBankAccount);
                if (!saveFileResult.IsSuccess)
                {
                    result.SetError(saveFileResult);
                    return result;
                }
            }

            return _memberBankService.UpdateBankAccount(AccountID, memberBankAccount, Modifier);
        }

        /// <summary>
        /// 更新文件審核狀態
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PaperAuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdatePaperAuthStatus(long AccountID, byte PaperAuthStatus, string Modifier)
        {
            return _memberBankService.UpdatePaperAuthStatus(AccountID, PaperAuthStatus, Modifier);
        }

        /// <summary>
        /// 銀行帳號驗證(一元驗證)
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="BankCode"></param>
        /// <param name="BankAccount"></param>
        /// <returns></returns>
        public BaseResult AuthBankAccount(long MID, string BankCode, string BankAccount)
        {
            var model = new Library.Models.MemberModels.MemberBankAccount
            {
                MID = MID,
                BankCode = BankCode,
                BankAccount = BankAccount
            };

            return _libMemberBankService.AddBankTransferOnBankAccount(model);
        }
    }
}
