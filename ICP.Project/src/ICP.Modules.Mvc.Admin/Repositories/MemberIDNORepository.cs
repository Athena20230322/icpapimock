using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class MemberIDNORepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public MemberIDNORepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        #region 取得身分驗證列表
        /// <summary>
        /// 取得身分驗證列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberAuthIDNO> ListAuthMemberIDNO(QueryMemberIDNO model)
        {
            string sql = "Exec ausp_Admin_MemberAuth_ListAuthMemberIDNO_S";

            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.AuthStatus,
                model.PaperAuthStatus,
                model.CName,
                model.IDNO,
                model.ICPMID,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberAuthIDNO>(sql, args).ToList();
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
            string sql = "Exec ausp_Admin_MemberAuth_UpdateCName_U";

            var args = new
            {
                MID,
                CName,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得會員身分驗證資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public MemberAuthIDNO GetAuthIDNO(long MID)
        {
            string sql = "Exec ausp_Member_MemberAuth_GetAuthIDNO_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAuthIDNO>(sql, args);
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
            string sql = "Exec ausp_Member_MemberAuth_UpdateAuthIDNOPaperAuthStatus_U";

            var args = new
            {
                MID,
                PaperAuthStatus,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
