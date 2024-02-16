using System;
using System.Collections.Generic;

namespace ICP.Infrastructure.Abstractions.Authorization
{
    public interface IUserManager
    {
        /// <summary>
        /// 是否已登入
        /// </summary>
        bool IsLogin { get; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        long MID { get; }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetData<T>(string name);

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userData"></param>
        void Login(IDictionary<string, object> userData, DateTime? Expired = null);

        /// <summary>
        /// 登出
        /// </summary>
        void Logout();
    }
}
