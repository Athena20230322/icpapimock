using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    using Infrastructure.Core.Models;
    using Models.MemberModels;
    using Repositories.MemberRepositories;

    public class LibMemberRegService
    {
        MemberConfigCyptRepository _configCyptRepository;
        MemberRegRepository _memberRegRepository;

        public LibMemberRegService(
            MemberConfigCyptRepository configCyptRepository,
            MemberRegRepository memberRegRepository)
        {
            _configCyptRepository = configCyptRepository;
            _memberRegRepository = memberRegRepository;
        }

        public DataResult<long> CheckReferrerCode(string ReferrerCode)
        {
            return _memberRegRepository.CheckReferrerCode(ReferrerCode);
        }

        public DataResult<long> AddTempRegisterData(AddTempRegisterDataModel model)
        {
            model.Account = _configCyptRepository.Encrypt_UserCode(model.Account);
            model.Pwd = _configCyptRepository.Hash_UserPwd(model.Pwd);

            return _memberRegRepository.AddTempRegisterData(model);
        }

        public BaseResult CheckMemberTemp(long TempMID)
        {
            return _memberRegRepository.CheckMemberTemp(TempMID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempMID"></param>
        /// <param name="MemberType">會員類型(&運算) 1:一類會員(個人) 2:二類會員(個人) 8:紙本特店 128:新註冊會員</param>
        /// <returns></returns>
        public DataResult<long> TempRegisterDataToMember(long TempMID, short MemberType)
        {
            return _memberRegRepository.TempRegisterDataToMember(TempMID, MemberType);
        }
    }
}
