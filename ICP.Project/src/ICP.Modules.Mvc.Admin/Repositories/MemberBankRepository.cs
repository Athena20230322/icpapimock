using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class MemberBankRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public MemberBankRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        /// <summary>
        /// 取得銀行帳號列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberBankAccount> ListAuthMemberBankAccount(QueryMemberBankAccount model)
        {
            string sql = "Exec ausp_Admin_MemberAuth_ListAuthMemberBankAccount_S";

            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.AccountStatus,
                model.PaperAuthStatus,
                model.CName,
                model.ICPMID,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberBankAccount>(sql, args).ToList();
        }

        /// <summary>
        /// 更新會員銀行帳號資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateBankAccount(long AccountID, UpdateMemberBankAccount model, string Modifier)
        {
            string sql = "Exec ausp_Admin_MemberInfo_UpdateBankAccount_U";

            var args = new
            {
                AccountID,
                model.BankCode,
                model.BankBranchCode,
                model.BankAccount,
                model.AuthMsg,
                model.FilePath1,
                Modifier
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新文件審核狀態
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PaperAuthStatus"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdatePaperAuthStatus(long AccountID, byte PaperAuthStatus, string Modifier)
        {
            string sql = "Exec ausp_Admin_MemberInfo_UpdateBankAccountPaperAuthStatus_U";

            var args = new
            {
                AccountID,
                PaperAuthStatus,
                Modifier
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
