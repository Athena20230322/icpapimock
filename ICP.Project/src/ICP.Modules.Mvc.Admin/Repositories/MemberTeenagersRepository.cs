using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models.MemberModels;
    using Models.ViewModels;

    public class MemberTeenagersRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db = null;

        public MemberTeenagersRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        /// <summary>
        /// 查詢未成年審核資料
        /// </summary>
        /// <param name="query">查詢條件</param>
        /// <returns></returns>
        public List<MemberTeenagersQueryResult> ListTeenagers(MemberTeenagersQuery query)
        {
            string sql = "EXEC ausp_Member_Admin_Teenagers_ListTeenagers_S";
            sql += db.GenerateParameter(query);
            return db.Query<MemberTeenagersQueryResult>(sql, query);
        }

        /// <summary>
        /// 修改未成年審核備註
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="Note">審核備註</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagerNote(long MID, string Modifier, string Note, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_Teenagers_UpdateTeenagerNote_U";
            var args = new
            {
                MID,
                Modifier,
                Note,
                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 審核未成年法代資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="LPAuthStatus">法代資料是否審過 0: 待審 1:審核通過 2: 審核失敗</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLPAuthStatus(long MID, string Modifier, byte LPAuthStatus, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_Teenagers_UpdateTeenagersLPAuthStatus_U";
            var args = new
            {
                MID,
                Modifier,
                LPAuthStatus,
                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 審核未成年身份驗證
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="IDNOStatus">身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersIDNOStatus(long MID, string Modifier, byte IDNOStatus, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_Teenagers_UpdateTeenagersIDNOStatus_U";
            var args = new
            {
                MID,
                Modifier,
                IDNOStatus,
                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新法定代理人資料
        /// </summary>
        /// <param name="MID">法定代理人會員編號</param>
        /// <param name="TeenagerMID">未成年會員編號</param>
        /// <param name="model">更新資料</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLegalDetail(long MID, long TeenagerMID, UpdateTeenagersLegalDetailModel model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_Teenagers_TeenagersLegalDetail_U";
            var args = new
            {
                MID,
                TeenagersMID = TeenagerMID,
                model.IDNOFile1,
                model.IDNOFile2,
                model.FilePath1,
                model.FilePath2,
                model.FilePath3,
                model.FilePath4,
                model.FilePath5,
                model.FilePath6,
                Modifier,
                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
