using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Member.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Services
{
    public class MemberEditService
    {
        private readonly MemberEditRepository _memberEditRepository = null;
        private readonly ILogger _logger = null;

        public MemberEditService(
            MemberEditRepository memberEditRepository,
            ILogger<MemberGraphicLockService> logger)
        {
            _memberEditRepository = memberEditRepository;
            _logger = logger;
        }

        /// <summary>
        /// 取得會員登入資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        //public MemberLoginInfo GetMemberLoginInfo(long MID)
        //{
        //    MemberInfoDao MemberLoginInfoDao = new MemberInfoDao();
        //    return MemberLoginInfoDao.GetMemberLoginInfo(MID);
        //}

    }
}
