using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.AccountLinkApi;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Api.Member.Models.ACLink;
using ICP.Modules.Api.Member.Models.MemberInfo;
using ICP.Modules.Api.Member.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Commands
{
    public class MemberBankCommand : ACLinkApiCommand
    {
        LibMemberBankService _libMemberBankService;
        LibMemberInfoService _libMemberInfoService;
        MemberInfoService _memberInfoService;
        ACLinkService _accountLinkService;

        public MemberBankCommand(
            ACLinkService accountLinkService,
            ACLinkCommonService aCLinkCommonService,
            ILogger<ACLinkApiCommand> aclLinklogger,
            LibMemberBankService libMemberBankService,
            LibMemberInfoService libMemberInfoService,
            MemberInfoService memberInfoService
            ) : base(accountLinkService, aCLinkCommonService, aclLinklogger)
        {
            _libMemberBankService = libMemberBankService;
            _libMemberInfoService = libMemberInfoService;
            _memberInfoService = memberInfoService;
            _accountLinkService = accountLinkService;
        }

        #region M0027 銀行帳戶驗證
        /// <summary>
        /// 銀行帳戶驗證
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<BankAccountAuthResult> BankAccountAuth(long MID, BankAccountAuthRequest request)
        {
            var result = new DataResult<BankAccountAuthResult>();

            if (request.BankType == 1)
            {
                return AclBankAccountAuth(MID, request);
            }
            else
            {
                return BankTransferAuth(MID, request);
            }
        }

        /// <summary>
        /// 合作銀行驗證
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private DataResult<BankAccountAuthResult> AclBankAccountAuth(long MID, BankAccountAuthRequest request)
        {
            var result = new DataResult<BankAccountAuthResult>();

            var memberBankAccount = new MemberBankAccount
            {
                MID = MID,
                Category = 1,
                AuthCategory = 3,
                BankCode = request.BankCode,
                INDTAccount = string.Empty,
                BankBranchCode = string.IsNullOrWhiteSpace(request.BranchCode) ? string.Empty : request.BranchCode
            };

            if (!string.IsNullOrWhiteSpace(request.BankAccount))
            {
                string bankAccount = string.Empty;
                var validateResult = _libMemberBankService.IsValidateBankAccount(request.BankAccount, out bankAccount);
                if (!validateResult.IsSuccess)
                {
                    result.SetError(validateResult);
                    return result;
                }
                memberBankAccount.BankAccount = bankAccount;
            }

            var addResult = _libMemberBankService.AddMemberBankAccount(memberBankAccount);
            if (addResult.RtnCode != 1)
            {
                result.SetError(addResult);
                return result;
            }

            if (request.BankCode == "999")
            {
                var apiRtnData = new BankAccountAuthResult
                {
                    URL = "http://mobile-internal.ecpay.com.tw/icash/testAccountLink.html?bankCode=999",
                    ViewType = 0
                };

                result.SetSuccess(apiRtnData);
                return result;
            }
            else if (request.BankCode == "998")
            {
                var apiRtnData = new BankAccountAuthResult
                {
                    URL = "http://mobile-internal.ecpay.com.tw/icash/testAccountLink.html?bankCode=999",
                    ViewType = 1
                };

                result.SetSuccess(apiRtnData);
                return result;
            }

            var memberData = _libMemberInfoService.GetMemberData(MID);

            string idno = string.IsNullOrWhiteSpace(memberData.detail.IDNO) ? memberData.detail.UniformID : memberData.detail.IDNO;

            var authResult = new BankAccountAuthResult();

            var aclModel = new ACLinkBindReq
            {
                MID = MID,
                IDNO = idno,
                BankCode = request.BankCode,
                BankAccount = request.BankAccount
            };

            var aclAuthResult = ACLinkBind(aclModel);
            if (!aclAuthResult.IsSuccess)
            {
                result.SetError(aclAuthResult);
                return result;
            }

            var bankSetting = _accountLinkService.GetACLinkBankSetting(request.BankCode);
            var rtnData = new BankAccountAuthResult
            {
                URL = aclAuthResult.RtnData.URL,
                ViewType = bankSetting.AppBindMode
            };

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 非合作銀行驗證
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private DataResult<BankAccountAuthResult> BankTransferAuth(long MID, BankAccountAuthRequest request)
        {
            var result = new DataResult<BankAccountAuthResult>();

            var memberBankAccount = new MemberBankAccount
            {
                MID = MID,
                Category = 0,
                AuthCategory = 0,
                BankCode = request.BankCode,
                INDTAccount = string.Empty,
                BankBranchCode = string.IsNullOrWhiteSpace(request.BranchCode) ? string.Empty : request.BranchCode
            };

            string bankAccount = string.Empty;
            var validateResult = _libMemberBankService.IsValidateBankAccount(request.BankAccount, out bankAccount);
            if (!validateResult.IsSuccess)
            {
                result.SetError(validateResult);
                return result;
            }
            memberBankAccount.BankAccount = bankAccount;

            var addBankTransferResult = _libMemberBankService.AddBankTransferOnBankAccount(memberBankAccount);
            if (!addBankTransferResult.IsSuccess)
            {
                result.SetError(addBankTransferResult);
                return result;
            }

            var addResult = _libMemberBankService.AddMemberBankAccount(memberBankAccount);
            if (addResult.RtnCode != 1)
            {
                result.SetError(addResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }
        #endregion

        #region M0035 刪除綁定銀行帳戶
        /// <summary>
        /// 刪除綁定銀行帳戶
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="request">刪除帳號資料</param>
        /// <returns></returns>
        public DataResult<BaseResult> DelAccountBind(long MID, DelAccountBindRequest request)
        {
            var result = new DataResult<BaseResult>();

            byte category = 0;

            if (request.AccountType == 1)
            {
                category = 1;

                var cancelResult = AclDelAccountBind(MID, request);
                if (!cancelResult.IsSuccess)
                {
                    result.SetError(cancelResult);
                    return result;
                }
            }

            var delResult = _memberInfoService.DeleteBankAccount(MID, category, request.BankCode, request.AccountID);

            if (!delResult.IsSuccess)
            {
                result.RtnMsg = "失敗";
                return result;
            }

            result.RtnMsg = "成功";
            result.SetSuccess();

            return result;
        }

        /// <summary>
        /// ACL 解綁
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="request">刪除帳號資料</param>
        /// <returns></returns>
        private DataResult<BaseResult> AclDelAccountBind(long MID, DelAccountBindRequest request)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var memberData = _libMemberInfoService.GetMemberData(MID);

            string idno = string.IsNullOrWhiteSpace(memberData.detail.IDNO) ? memberData.detail.UniformID : memberData.detail.IDNO;

            var aclModel = new ACLinkCancelBindReq
            {
                MID = MID,
                IDNO = idno,
                BankCode = request.BankCode,
                AccountID = request.AccountID
            };

            var cancelResult = ACLinkCancel(aclModel);
            if (!cancelResult.IsSuccess)
            {
                result.SetError(cancelResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }
        #endregion

        /// <summary>
        /// 餘額提領
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<AddWithdrawBalanceResult> AddWithdrawBalance(long MID, AddWithdrawBalanceRequest request, long RealIP, long ProxyIP)
        {
            var result = new DataResult<AddWithdrawBalanceResult>();
            result.SetError();

            var bankInfo = _libMemberBankService.GetBankAccount(request.AccountID);
            if (bankInfo == null)
            {
                result.RtnMsg = "查無銀行帳號";
                return result;
            }

            int handlingCharge = 15;
            if (bankInfo.Category == 1)
            {
                handlingCharge = 0;
            }

            var withdrawBalance = new WithdrawBalance
            {
                MID = MID,
                MerchantID = 0,
                BankCode = request.BankCode,
                BankAccount = bankInfo.BankAccount,
                AMTransferType = 1,
                Amount = request.Amount + handlingCharge,
                TransferType = 1,
                AgreeLevelUp = request.AgreeLevelUp
            };

            var addResult = _libMemberBankService.AddWithdrawBalance(withdrawBalance);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            if (bankInfo.Category == 1)
            {
                var memberData = _libMemberInfoService.GetMemberData(MID);
                var aclWithdrawReq = new ACLinkWithdrawalReq
                {
                    MID = MID,
                    IDNO = string.IsNullOrEmpty(memberData.detail.IDNO) ? memberData.detail.UniformID : memberData.detail.IDNO,
                    BankCode = request.BankCode,
                    INDTAccount = bankInfo.INDTAccount,
                    Amount = request.Amount,
                    TradeNo = addResult.TradeNo
                };

                //var aclResult = ACLinkWithdrawal(aclWithdrawReq);
                var aclResult = new BaseResult();
                aclResult.SetSuccess();
                if (!aclResult.IsSuccess)
                {
                    var rtnResult = _libMemberBankService.RtnWithdrawBalance(addResult.TradeID, RealIP, ProxyIP);
                    result.SetError(aclResult);
                    return result;
                }
                else
                {
                    byte payStatus = 1;
                    _libMemberBankService.UpdateBankTransferQuery(addResult.TradeID, payStatus);
                }
            }

            var userCoinsBalance = _libMemberBankService.GetUserCoinsBalance(MID);

            var bankDetail = _libMemberBankService.GetBankDetail(request.BankCode);

            int status = addResult.RtnCode;
            if (status == 1 && bankInfo.Category == 0)
            {
                status = 2;
            }

            result.SetSuccess(new AddWithdrawBalanceResult
            {
                TopUpCash = Convert.ToInt32(userCoinsBalance.TotalBalance),
                Amount = request.Amount,
                TradeNo = addResult.TradeNo,
                HandlingCharge = handlingCharge,
                TotalAmount = request.Amount + handlingCharge,
                BankCode = request.BankCode,
                BankAccount = bankInfo.BankAccount,
                AccountLast5No = _memberInfoService.ConcealBankAccount(bankInfo.BankAccount),
                BankName = bankDetail.BankName,
                CreateDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                Status = status
            });
            return result;
        }
    }
}
