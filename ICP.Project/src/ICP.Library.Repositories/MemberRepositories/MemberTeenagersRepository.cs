using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    /// <summary>
    /// 未成年
    /// </summary>
    public class MemberTeenagersRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberTeenagersRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 檢查是否有未成年待同意申請資料
        /// </summary>
        /// <param name="Account">登入帳號</param>
        /// <returns></returns>
        public BaseResult CheckTeenagersLegalDetail(string Account)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_CheckTeenagersLegalDetail_S";

            var args = new
            {
                Account
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }



        /// <summary>
        /// 取得未成年會員法定代理人資料
        /// </summary>
        /// <param name="MID">法定代理人 會員編號</param>
        /// <param name="TeenagersMID">未成年 會員編號</param>
        /// <returns></returns>
        public MemberTeenagersLegalDetail GetTeenagersLegalDetail(long MID, long TeenagersMID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_GetTeenagersLegalDetail_S";

            var args = new
            {
                MID,
                TeenagersMID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MemberTeenagersLegalDetail>(sql, args);
        }

        /// <summary>
        /// 取得未成年法定代理人資料
        /// </summary>
        /// <param name="MID">法定代理人會員編號</param>
        /// <param name="Status">狀態 0: 待同意, 1:已同意</param>
        /// <returns></returns>
        public List<MemberTeenagersLegalDetail> ListTeenagersLegalDetail(long MID, byte Status = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_ListTeenagersLegalDetail_S";

            var args = new
            {
                MID,
                Status
            };

            sql += db.GenerateParameter(args);
            return db.Query<MemberTeenagersLegalDetail>(sql, args);
        }

        /// <summary>
        /// 取得未成年法定代理人資料
        /// </summary>
        /// <param name="TeenagersMID">未成年會員編號</param>
        /// <param name="Status">狀態 0: 待同意, 1:已同意</param>
        /// <returns></returns>
        public List<MemberTeenagersLegalDetail> ListTeenagersLegalDetailByTeenMID(long TeenagersMID, byte Status = 1)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_ListTeenagersLegalDetailByTeenMID_S";

            var args = new
            {
                TeenagersMID,
                Status
            };

            sql += db.GenerateParameter(args);
            return db.Query<MemberTeenagersLegalDetail>(sql, args);
        }

        /// <summary>
        /// 取得會員未成年申請資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public MemberTeenager GetTeenager(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_GetTeenager_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MemberTeenager>(sql, args);
        }

        /// <summary>
        /// 同意未成年註冊
        /// </summary>
        /// <param name="model">同意資料</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLegalAgree(MemberTeenagersLegalDetail model, long RealIP = 0, long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_UpdateTeenagersLegalAgree_S";

            var args = new
            {
                model.MID,
                model.TeenagersMID,
                model.FilePath1,
                model.FilePath2,
                model.FilePath3,
                model.FilePath4,
                model.FilePath5,
                model.FilePath6,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
