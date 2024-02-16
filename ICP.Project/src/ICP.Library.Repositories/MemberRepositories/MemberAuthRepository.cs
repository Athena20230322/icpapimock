using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Library.Models.MemberModels;
    using Infrastructure.Core.Frameworks.DbUtil;

    public class MemberAuthRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public MemberAuthRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        [EnableDbProxy]
        public virtual SMSAuthResult AddAuthSMS(SMSAuth model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthSMS_IU";

            var args = new
            {
                model.AuthType,
                model.CellPhone,
                model.MID,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<SMSAuthResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual SMSAuthVerifyResult UpdateAuthSMS(SMSAuthVerify model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_UpdateAuthSMS_U";

            var args = new
            {
                model.CellPhone,
                model.AuthCode,
                model.AuthType
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<SMSAuthVerifyResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual SMSAuthResult AddAuthCellPhone(SMSAuth model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthCellPhone_IU";

            var args = new
            {
                model.CellPhone,
                model.MID,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<SMSAuthResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual SMSAuthVerifyResult UpdateAuthCellPhoneStatus(SMSAuthVerify model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_UpdateAuthCellPhoneStatus_IU";

            var args = new
            {
                model.CellPhone,
                model.MID,
                model.AuthCode
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<SMSAuthVerifyResult>(sql, args);
        }

        public BaseResult CheckAuthSMSCount(SMSAuth model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);

            string sql = "EXEC ausp_Statistics_MemberAuth_CheckAuthSMSCount_SIU";

            var args = new
            {
                model.CellPhone,
                model.MID,
                model.RealIP,
                model.ProxyIP

                //預設 AddCount = 0 不會異動資料, 純查詢
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddAuthP33(P33AuthResult model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthP33ByLog_IU";

            var args = new
            {
                model.MID,
                model.IDNO,
                model.AuthStatus
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddAuthIDNO(AuthIDNO model, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthIDNO_SIU";

            var args = new
            {
                model.MID,
                model.IDNO,
                model.IssueDate,
                model.ObtainType,
                model.IssueLocationID,
                model.AuthType,
                model.FilePath1,
                model.FilePath2,
                model.Source,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateAuthIDNOStatus(long MID, byte AuthStatus, string Modifier = null, long RealIP = 0, long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_UpdateAuthIDNOStatus_U";

            var args = new
            {
                MID,
                AuthStatus,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public CheckIdnoRepeatModel CheckIdnoRepeat(string IDNO, long MID, bool IsOversea)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_CheckIdnoRepeat_S";

            var args = new
            {
                IDNO,
                IsOversea,
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<CheckIdnoRepeatModel>(sql, args);
        }

        /// <summary>
        /// 綁定/解綁 AppToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="AppTokenID">綁定編號</param>
        /// <param name="Type">綁定類型 0: 綁定 1:解綁</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual DataResult<long> BindAppToken(long MID, long AppTokenID, byte Type)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OP_BindAppToken_IU";

            var args = new
            {
                MID,
                AppTokenID,
                Type
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 綁定/解綁 OP 帳號
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual DataResult<long> BindOPAccount(BindOPAccountModel model, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OP_BindOPAccount_IU";

            var args = new
            {
                model.MID,
                model.OPMID,
                model.Type,
                model.Source,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 取得身份證換補發縣市清單
        /// </summary>
        /// <returns></returns>
        public List<MemberAuthIssueLocation> ListIssueLocation()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_MemberAuth_ListIssueLocation_S";
            return db.Query<MemberAuthIssueLocation>(sql);
        }
    }
}
