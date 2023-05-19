using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.AccountLinkApi;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Api.Member.Models.ACLink;
using ICP.Modules.Api.Member.Models.MemberInfo;
using ICP.Modules.Api.Member.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Commands
{
    public class AdminCommand : ACLinkApiCommand
    {
        MemberInfoService _memberInfoService;
        LibMemberBankService _libMemberBankService;
        LibMemberInfoService _libMemberInfoService;
        LibMemberAuthService _libMemberAuthService;

        public AdminCommand(
            ACLinkService accountLinkService,
            ACLinkCommonService aCLinkCommonService,
            ILogger<ACLinkApiCommand> aCLLogger,
            MemberInfoService memberInfoService,
            LibMemberBankService libMemberBankService,
            LibMemberInfoService libMemberInfoService,
            LibMemberAuthService libMemberAuthService
            ) : base(accountLinkService, aCLinkCommonService, aCLLogger)
        {
            _memberInfoService = memberInfoService;
            _libMemberBankService = libMemberBankService;
            _libMemberInfoService = libMemberInfoService;
            _libMemberAuthService = libMemberAuthService;
        }

        #region M0046 取得帳戶狀態資訊
        /// <summary>
        /// M0046 取得帳戶狀態資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public DataResult<GetAccountStatusInfoResult> GetAccountStatusInfo(long MID)
        {
            var result = new DataResult<GetAccountStatusInfoResult>();
            result.SetError();

            var rtnData = GetAccountStatus(MID);

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion

        #region M0047 帳戶結清
        /// <summary>
        /// M0047 帳戶結清
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Source">來源 : 0: App 1: 電支後台 2:排程</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <param name="Modifier">修改人</param>
        /// <returns></returns>
        public DataResult<GetAccountStatusInfoResult> CloseMemberAccount(long MID, byte Source, long RealIP, long ProxyIP, string Modifier = null)
        {
            var result = new DataResult<GetAccountStatusInfoResult>();
            result.SetError();

            var rtnData = GetAccountStatus(MID);
            result.RtnData = rtnData;

            if (result.RtnData.AccountStatus)
            {
                result.RtnMsg = "您尚有帳戶餘額未提領，請先將您的帳戶餘額提領後再進行帳戶結清";
                return result;
            }

            if (result.RtnData.WithdrawStatus)
            {
                result.RtnMsg = "您的帳戶餘額正在提領中，須待提領作業完成後再進行帳戶結清";
                return result;
            }

            if (result.RtnData.AllocateStatus)
            {
                result.RtnMsg = "您的帳戶款項正在撥款中，請於待撥款項匯入您的帳戶後先完成餘額提領再進行帳戶結清";
                return result;
            }

            /*
            todo 待收帳
             */

            var memberData = _libMemberInfoService.GetMemberData(MID);
            string Idno = string.IsNullOrWhiteSpace(memberData.detail.IDNO) ? memberData.detail.UniformID : memberData.detail.IDNO;

            var bankAccounts = _libMemberBankService.ListMemberBankInfo(MID);
            var aclBankAccounts = bankAccounts.Where(t => t.Category == 1).ToList();
            foreach (var item in aclBankAccounts)
            {
                var aclModel = new ACLinkCancelBindReq
                {
                    MID = MID,
                    IDNO = Idno,
                    BankCode = item.BankCode,
                    AccountID = item.AccountID
                };

                var cancelResult = ACLinkCancel(aclModel);
                if (!cancelResult.IsSuccess)
                {
                    result.RtnMsg = "銀行快付解除綁定失敗";
                    return result;
                }
            }

            var getAppTokenResult = _libMemberInfoService.GetAppTokenByMID(MID);
            if (!getAppTokenResult.IsSuccess)
            {
                result.SetError(getAppTokenResult);
                return result;
            }
            //var OPUnBindResult = _libMemberAuthService.CheckOPUnBind(memberData, getAppTokenResult.RtnData, Source, RealIP, ProxyIP);
            //if (!OPUnBindResult.IsSuccess)
            //{
            //    result.SetError(OPUnBindResult);
            //    return result;
            //}

            var closeResult = _memberInfoService.CloseMemberAccount(MID, Source, Modifier, RealIP, ProxyIP);
            if (!closeResult.IsSuccess)
            {
                result.SetError(closeResult);
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion

        /// <summary>
        /// 確認會員是否可結清
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetAccountStatusInfoResult GetAccountStatus(long MID)
        {
            var result = new GetAccountStatusInfoResult();
            var userCoins = _memberInfoService.GetUserCoins(MID);
            if (userCoins.TopUpCash > 0)
            {
                result.AccountStatus = true;
            }

            var hasTransfer = _memberInfoService.CheckBankTransfer(MID);
            if (hasTransfer)
            {
                result.WithdrawStatus = true;
            }

            var hasAllocateTrade = _memberInfoService.CheckUnAllocateTrade(MID);
            if (hasAllocateTrade)
            {
                result.AllocateStatus = true;
            }

            /*
            todo 待收帳
             */

            return result;
        }
    }
}
