using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    public class MemberBankRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberBankRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public BaseResult AddMemberBankAccount(MemberBankAccount model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_AddBankAccount_SI";

            var args = new
            {
                model.MID,
                model.Category,
                model.BankCode,
                model.BankBranchCode,
                model.BankAccount,
                model.INDTAccount,
                model.isDefault,
                model.AuthCategory,
                model.AuthType,
                model.PaperAuthStatus,
                model.FilePath1,
                model.FilePath2,
                model.CreateUser,
                model.Source
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateMemberBankAccountStatus(UpdateBankAccountStatusModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateBankAccountStatus_IU";

            var args = new
            {
                model.MID,
                model.Category,
                model.BankCode,
                model.BankAccount,
                model.AccountStatus,
                model.INDTAccount,
                model.AgreeLevelUp
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddBankTransferOnBankAccount(MemberBankAccount model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_AddBankTransferOnBankAccount_I";

            var args = new
            {
                model.MID,
                model.BankCode,
                model.BankAccount
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public List<MemberBankInfo> ListMemberBankInfo(long MID, byte? Category = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberBankInfo_S";

            var args = new
            {
                MID,
                Category
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberBankInfo>(sql, args);
        }

        public UserCoinsBalance GetUserCoinsBalance(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_GetUserCoinsBalance_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UserCoinsBalance>(sql, args);
        }

        public WithdrawBalanceResult AddWithdrawBalance(WithdrawBalance model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_AddWithdrawBalance_SIU";

            var args = new
            {
                model.MID,
                model.MerchantID,
                model.BankCode,
                model.BankAccount,
                model.AMTransferType,
                model.Amount,
                model.TransferType,
                model.AgreeLevelUp
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<WithdrawBalanceResult>(sql, args);
        }

        public BankDetail GetBankDetail(string BankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetBankDetail_S";

            var args = new
            {
                BankCode
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BankDetail>(sql, args);
        }

        public BaseResult RtnWithdrawBalance(long TransferID, long RealIP = 0, long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_RtnWithdrawBalance_SIU";

            var args = new
            {
                TransferID,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual AuthFinancialResult GetAuthFinancial(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAuthFinancial_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<AuthFinancialResult>(sql, args);
        }

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <returns></returns>
        public List<MemberBankDetail> ListBankDetail()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberBankDetail_S";

            return db.Query<MemberBankDetail>(sql).ToList();
        }

        /// <summary>
        /// 取得銀行種類
        /// </summary>
        /// <returns></returns>
        public List<BankTypeModel> ListBankType()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_BankAccount_ListBankType_S";
            return db.Query<BankTypeModel>(sql).ToList();
        }

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <param name="BankTypeID">銀行種類</param>
        /// <returns></returns>
        public List<BankCodeModel> ListBankCode(byte BankTypeID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_BankAccount_ListBankCode_S";
            var args = new
            {
                BankTypeID
            };
            sql += db.GenerateParameter(args);
            return db.Query<BankCodeModel>(sql, args).ToList();
        }

        /// <summary>
        /// 取得銀行分行清單
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public List<BankBranchCodeModel> ListBankBranchCode(string BankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_MemberInfo_ListMemberBankCode_S";
            var args = new
            {
                BankCode
            };
            sql += db.GenerateParameter(args);
            return db.Query<BankBranchCodeModel>(sql, args).ToList();
        }

        /// <summary>
        /// 取得會員銀行帳號資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public MemberBankAccount GetBankAccount(long AccountID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "Exec ausp_Admin_MemberInfo_GetBankAccount_S";

            var args = new
            {
                AccountID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberBankAccount>(sql, args);
        }

        /// <summary>
        /// 更新已送驗轉帳資訊
        /// </summary>
        /// <returns></returns>
        public BaseResult UpdateBankTransferQuery(long TransferID, byte PayStatus, string ErrorCode, string ErrorMsg)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_UpdateBankTransferQuery_U";

            var args = new
            {
                TransferID,
                PayStatus,
                ErrorCode,
                ErrorMsg
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
