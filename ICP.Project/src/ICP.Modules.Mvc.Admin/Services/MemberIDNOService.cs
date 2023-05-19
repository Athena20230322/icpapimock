using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class MemberIDNOService
    {
        MemberIDNORepository _memberIDNORepository;
        Library.Repositories.MemberRepositories.MemberAuthRepository _memberAuthRepository;

        public MemberIDNOService(
            MemberIDNORepository memberIDNORepository,
            Library.Repositories.MemberRepositories.MemberAuthRepository memberAuthRepository
            )
        {
            _memberIDNORepository = memberIDNORepository;
            _memberAuthRepository = memberAuthRepository;
        }

        #region 取得身分驗證列表
        /// <summary>
        /// 取得身分驗證列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberAuthIDNO> ListAuthMemberIDNO(QueryMemberIDNO model)
        {
            return _memberIDNORepository.ListAuthMemberIDNO(model);
        }
        #endregion

        /// <summary>
        /// 修改會員姓名
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="CName"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateCName(long MID, string CName, string Modifier, long RealIP, long ProxyIP)
        {
            return _memberIDNORepository.UpdateCName(MID, CName, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得會員身分驗證資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public MemberAuthIDNO GetAuthIDNO(long MID)
        {
            return _memberIDNORepository.GetAuthIDNO(MID);
        }

        /// <summary>
        /// 更新文件審核狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="PaperAuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdatePaperAuthStatus(long MID, byte PaperAuthStatus, string Modifier, long RealIP, long ProxyIP)
        {
            return _memberIDNORepository.UpdatePaperAuthStatus(MID, PaperAuthStatus, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 更新身分驗證狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="AuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateAuthIDNOStatus(long MID, byte AuthStatus, string Modifier, long RealIP, long ProxyIP)
        {
            return _memberAuthRepository.UpdateAuthIDNOStatus(MID, AuthStatus, Modifier, RealIP, ProxyIP);
        }
    }
}
