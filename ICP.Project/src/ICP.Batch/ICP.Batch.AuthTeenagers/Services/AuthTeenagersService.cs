using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AuthTeenagers.Services
{
    using ICP.Infrastructure.Core.Models;
    using Models;
    using Repositories;

    public class AuthTeenagersService
    {
        AuthTeenagersRepository _authTeenagersRepository;

        public AuthTeenagersService(AuthTeenagersRepository authTeenagersRepository)
        {
            _authTeenagersRepository = authTeenagersRepository;
        }

        /// <summary>
        /// 取得已通過法定代理人驗證會員資料
        /// </summary>
        /// <returns></returns>
        public List<AuthTeenagersData> ListAuthTeenagersData()
        {
            return _authTeenagersRepository.ListAuthTeenagersData();
        }

        /// <summary>
        /// 刪除已到期審查資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public BaseResult DeleteAuthTeenagers(long MID)
        {
            string modifier = "System";

            return _authTeenagersRepository.DeleteAuthTeenagers(MID, modifier);
        }

        /// <summary>
        /// 更新未成年會員為已成年會員
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public BaseResult UpdateMemberType(long MID)
        {
            return _authTeenagersRepository.UpdateMemberType(MID);
        }
    }
}
