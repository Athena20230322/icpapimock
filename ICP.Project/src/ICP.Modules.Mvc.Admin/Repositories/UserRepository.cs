using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using ICP.Infrastructure.Core.Frameworks.DbUtil;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;

    public class UserRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public UserRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        [EnableDbProxy]
        public virtual DataResult<int> AddUser(User model, string AuthCode, long IP, string Pwd)
        {
            string sql = "EXEC ausp_Admin_AddUser_I";
            var args = new
            {
                model.Account,
                model.CName,
                model.IsManager,
                model.DeptID,
                model.UserEmail,
                model.UserStatus,
                model.EID,
                AuthCode,
                IP,
                Pwd
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, args);
        }

        public BaseResult AuthUser(string AuthCode, string Pwd)
        {
            string sql = "EXEC ausp_Admin_AuthUser_U";
            var args = new
            {
                AuthCode,
                Pwd
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult CheckUserPwd(int UserID, string Pwd)
        {
            string sql = "EXEC ausp_Admin_CheckUserPwd_S";
            var args = new
            {
                UserID,
                Pwd
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult ChangeUserPwd(int UserID, string Pwd, long IP)
        {
            string sql = "EXEC ausp_Admin_ChangeUserPwd_U";
            var args = new
            {
                UserID,
                Pwd,
                IP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public CheckUserForgetTokenResult CheckUserForgetToken(string Token)
        {
            string sql = "EXEC ausp_Admin_CheckUserForgetToken_S";
            var args = new
            {
                Token
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<CheckUserForgetTokenResult>(sql, args);
        }

        public UserLoginResult CheckUserLogin(string Account, string Pwd)
        {
            string sql = "EXEC ausp_Admin_CheckUserLogin_SIU";
            var args = new
            {
                Account,
                Pwd
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UserLoginResult>(sql, args);            
        }

        public UpdateUserLoginTokenResult UpdateUserLoginToken(int UserID, string LoginToken)
        {
            string sql = "EXEC ausp_Admin_UpdateUserLoginToken_SU";
            var args = new
            {
                UserID,
                LoginToken
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UpdateUserLoginTokenResult>(sql, args);
        }

        public BaseResult CheckUserLoginToken(int UserID, string LoginToken)
        {
            string sql = "EXEC ausp_Admin_CheckUserLoginToken_S";
            var args = new
            {
                UserID,
                LoginToken
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UpdateUserLoginTokenResult>(sql, args);
        }

        public User GetUser(int UserID)
        {
            string sql = "EXEC ausp_Admin_GetUser_S";
            var args = new
            {
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<User>(sql, args);
        }

        public List<UserQueryResult> ListUser(UserQuery query)
        {
            string sql = "EXEC ausp_Admin_ListUser_S";
            sql += db.GenerateParameter(query);
            return db.Query<UserQueryResult>(sql, query);
        }

        public BaseResult ResetUserPwd(string Token, string Pwd, long IP)
        {
            string sql = "EXEC ausp_Admin_ResetUserPwd_U";
            var args = new
            {
                Token,
                Pwd,
                IP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateUser(int UserID, User model, long IP)
        {
            string sql = "EXEC ausp_Admin_UpdateUser_U";
            var args = new
            {
                UserID,
                model.Account,
                model.CName,
                model.DeptID,
                model.IsManager,
                model.UserEmail,
                model.UserStatus,
                model.EID,
                IP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateUserForgetToken(string Account, string UserEmail, string Token)
        {
            string sql = "EXEC ausp_Admin_UpdateUserForgetToken_U";
            var args = new
            {
                Account,
                UserEmail,
                Token
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateUserPwd(int UserID, string Pwd, long IP)
        {
            string sql = "EXEC ausp_Admin_UpdateUserPwd_U";
            var args = new
            {
                UserID,
                Pwd,
                IP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateUserStatus(int UserID, byte UserStatus)
        {
            string sql = "EXEC ausp_Admin_UpdateUserStatus_U";
            var args = new
            {
                UserID,
                UserStatus
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult DeleteUser(int UserID)
        {
            string sql = "EXEC ausp_Admin_DeleteUser_D";
            var args = new
            {
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public List<UserQueryResult> ListUserByGroup(int UserGroupID)
        {
            string sql = "EXEC ausp_Admin_ListUserByGroup_S";
            var args = new
            {
                UserGroupID
            };
            sql += db.GenerateParameter(args);
            return db.Query<UserQueryResult>(sql, args);
        }

        public UserSecurity GetUserSecurity(int UserID)
        {
            string sql = "EXEC ausp_Admin_GetUserSecurity_S";
            var args = new
            {
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UserSecurity>(sql, args);
        }

        public DataResult<int> CheckUserAuthCode(string AuthCode)
        {
            string sql = "EXEC ausp_Admin_CheckUserAuthCode_S";
            var args = new
            {
                AuthCode
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, args);
        }

        public User GetUserByAccount(string Account)
        {
            string sql = "EXEC ausp_Admin_GetUserByAccount_S";
            var args = new
            {
                Account
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<User>(sql, args);
        }

        /// <summary>
        /// 取得業務人員
        /// </summary>
        /// <returns></returns>
        public List<User> ListSales()
        {
            string sql = "EXEC ausp_Admin_Users_ListSales_S";
            return db.Query<User>(sql);
        }
    }
}
