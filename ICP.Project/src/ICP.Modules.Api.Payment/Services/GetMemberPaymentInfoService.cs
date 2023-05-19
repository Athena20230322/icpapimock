using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using System.Collections.Generic;
using System.Linq;
using MemberInfoRepository = ICP.Library.Repositories.MemberRepositories.MemberInfoRepository;

namespace ICP.Modules.Api.Payment.Services
{
    public class GetMemberPaymentInfoService
    {
        private readonly MemberPaymentInfoRepository _memberPaymentInfoRepository = null;
        private readonly Repositories.MemberInfoRepository _memberInfoRepository = null;
        private readonly MemberInfoRepository _libMemberInfoRepository = null;

        public GetMemberPaymentInfoService(
            MemberPaymentInfoRepository memberPaymentInfoRepository,
            Repositories.MemberInfoRepository memberInfoRepository,
            MemberInfoRepository libMemberInfoRepository
        )
        {
            _memberPaymentInfoRepository = memberPaymentInfoRepository;
            _memberInfoRepository = memberInfoRepository;
            _libMemberInfoRepository = libMemberInfoRepository;
        }

        /// <summary>
        /// 取得會員AccountLink資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public List<AccountLinkRes> GetAccountLinks(long mid)
        {
            List<AccountLinkRes> accountLinks = new List<AccountLinkRes>();

            List<AccountLinkDbRes> accounts = _memberPaymentInfoRepository.ListAccountLinkInfo(mid);

            if(accounts != null && accounts.Count() > 0)
            {
                foreach (var item in accounts)
                {
                    accountLinks.Add(new AccountLinkRes()
                    {
                        PayID = $"{(int)ePaymentType.ACCOUNTLINK}{item.BankCode}{item.AccountID.ToString().PadLeft(13, '0')}",
                        INDTAccount = item.INDTAccount,
                        BankCode = item.BankCode,
                        BankName = item.BankName,
                        BankShortName = item.BankShortName,
                        LinkAccount = item.BankAccount,
                        AccountLastNo = item.BankAccount.Length > 5 ? item.BankAccount.Substring(item.BankAccount.Length - 5, 5) : item.BankAccount,
                        IsDefaultBank = item.IsDefault.ToString()
                    });
                }
            }

            return accountLinks;
        }

        /// <summary>
        /// 取得會員愛金帳戶資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public iCashAccountInfo GetiCashAccountInfo(long mid)
        {
            //### 先取得帳戶資料
            MemberCoinsModel memberCoins = _memberInfoRepository.GetUserCoins(mid);

            //### 取得會員基本資料
            MemberDataModel memberData = _libMemberInfoRepository.GetMemberData(mid);

            return new iCashAccountInfo()
            {
                PayID = $"{(int)ePaymentType.TRANSACTION_ICASH}{(memberData != null && memberData.basic != null && !string.IsNullOrWhiteSpace(memberData.basic.ICPMID) ? memberData.basic.ICPMID : "")}",
                AvailableCash = (memberCoins != null ? memberCoins.RealCash + memberCoins.TopUpCash - memberCoins.FreezeCash : 0),
                AccountCash = (memberCoins != null ? memberCoins.RealCash + memberCoins.TopUpCash : 0)
            };
        }

        /// <summary>
        /// 檢核取得會員全付款方式參數
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public BaseResult ValidateGetMemberPaymentInfo(GetMemberPaymentInfoReq request)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            if (string.IsNullOrWhiteSpace(request.Timestamp))
            {

            }

            result.SetSuccess();

            return result;
        }
    }
}
