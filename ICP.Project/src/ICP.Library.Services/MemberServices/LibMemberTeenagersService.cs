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
    public class LibMemberTeenagersService
    {
        MemberConfigCyptRepository _configCyptRepository;
        MemberTeenagersRepository _memberTeenagersRepository;

        public LibMemberTeenagersService(
            MemberConfigCyptRepository configCyptRepository,
            MemberTeenagersRepository memberTeenagersRepository
            )
        {
            _configCyptRepository = configCyptRepository;
            _memberTeenagersRepository = memberTeenagersRepository;
        }

        /// <summary>
        /// 檢查是否有未成年待同意申請資料
        /// </summary>
        /// <param name="UserCode">登入帳號</param>
        /// <returns></returns>
        public BaseResult CheckTeenagersLegalDetail(string UserCode)
        {
            string Account = _configCyptRepository.Encrypt_UserCode(UserCode);

            return _memberTeenagersRepository.CheckTeenagersLegalDetail(Account);
        }

        /// <summary>
        /// 取得未成年會員法定代理人資料
        /// </summary>
        /// <param name="MID">法定代理人 會員編號</param>
        /// <param name="TeenagersMID">未成年 會員編號</param>
        /// <returns></returns>
        public DataResult<MemberTeenagersLegalDetail> GetTeenagersLegalDetail(long MID, long TeenagersMID)
        {
            var result = new DataResult<MemberTeenagersLegalDetail>();
            result.SetError();

            var rtnData = _memberTeenagersRepository.GetTeenagersLegalDetail(MID, TeenagersMID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得未成年法定代理人資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Status">狀態 0: 待同意, 1:已同意</param>
        /// <returns></returns>
        public DataResult<List<MemberTeenagersLegalDetail>> ListTeenagersLegalDetail(long MID, byte Status = 0)
        {
            var result = new DataResult<List<MemberTeenagersLegalDetail>>();
            result.SetError();

            var list = _memberTeenagersRepository.ListTeenagersLegalDetail(MID);
            if (list.Count == 0)
            {
                return result;
            }

            result.SetSuccess(list);
            return result;
        }

        /// <summary>
        /// 取得未成年法定代理人資料
        /// </summary>
        /// <param name="TeenagersMID">未成年會員編號</param>
        /// <param name="Status">狀態 0: 待同意, 1:已同意</param>
        /// <returns></returns>
        public DataResult<List<MemberTeenagersLegalDetail>> ListTeenagersLegalDetailByTeenMID(long TeenagersMID, byte Status = 1)
        {
            var result = new DataResult<List<MemberTeenagersLegalDetail>>();
            result.SetError();

            var list = _memberTeenagersRepository.ListTeenagersLegalDetailByTeenMID(TeenagersMID, Status);
            if (list.Count == 0)
            {
                return result;
            }

            result.SetSuccess(list);
            return result;
        }

        /// <summary>
        /// 取得會員未成年申請資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public DataResult<MemberTeenager> GetTeenager(long MID)
        {
            var result = new DataResult<MemberTeenager>();
            result.SetError();

            var rtnData = _memberTeenagersRepository.GetTeenager(MID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
    }
}
