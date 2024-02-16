using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Member.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Repositories
{
    public class MemberEditRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberEditRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        #region 登入密碼
        /// <summary>
        /// 更新登入密碼
        /// </summary>
        /// <param name="model">LoginPassword Model</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="email">回傳的Email</param>
        /// <returns></returns>
        //public bool UpdateLoginPassword(LoginPasswordRequest model, long mid, long realIP, long proxyIP, ref string email)
        //{
        //    bool bResult = false;

        //    var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

        //    string sql = "ausp_Member_UpdateLoginPassword_U";

        //    string oriLoginPassword = model.OriLoginPassword;
        //    string newLoginPassword = model.NewLoginPassword;

        //    var args = new
        //    {
        //        mid,
        //        oriLoginPassword = string.IsNullOrWhiteSpace(oriLoginPassword) ? Convert.DBNull : oriLoginPassword,
        //        newLoginPassword = string.IsNullOrWhiteSpace(newLoginPassword) ? Convert.DBNull : newLoginPassword,
        //        realIP,
        //        proxyIP
        //    };

        //    sql += db.GenerateParameter(args);

        //    DataTable dataTable = db.QuerySingleOrDefault<LoginPasswordResult>(sql, args);

        //    if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["RtnCode"].ToString() == "1")
        //    {
        //        email = Convert.ToString(dataTable.Rows[0]["Email"]);
        //        bResult = true;
        //    }

        //    return bResult;
            
        //}

        /// <summary>
        /// 檢查登入密碼是否正確
        /// </summary>
        /// <param name="loginPassword">登入密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckLoginPassword(string loginPassword, long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);


            bool bResult = false;

            string sql = "ausp_Member_MemberSecurity_CheckLoginPassword_S";

            var args = new
            {
                loginPassword,
                mid
            };

            int rtnCode = db.Execute(sql, args);

            if (rtnCode == 1)
            {
                bResult = true;
            }

            return bResult;
        }

        /// <summary>
        /// 檢查登入密碼是否正確
        /// </summary>
        /// <param name="loginPassword">登入密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckOldLoginPasswordSame(string loginPassword, long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);


            bool bResult = false;

            string sql = "ausp_Member_MemberSecurity_CheckOldLoginPasswordSame_S";

            var args = new
            {
                loginPassword,
                mid
            };

            int rtnCode = db.Execute(sql, args);

            if (rtnCode == 1)
            {
                bResult = true;
            }

            return bResult;
        }

        //##  更新略過修改密碼日期
        public BaseResult UpdatePwdIgnorDate(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            BaseResult RtnModel = new BaseResult();

            string sql = "ausp_Member_MemberSecurity_UpdatePwdIgnorDate_U";

            var args = new
            {
                MID
            };

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        #endregion
    }
}
