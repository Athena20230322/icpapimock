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

    public class MemberAuthRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public MemberAuthRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        /// <summary>
        /// 更新身份證換補發資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="model">換補發資料</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateAuthIDNO(long MID, UpdateMemberAuthIDNO model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_MemberAuth_UpdateAuthIDNO_U";
            var args = new
            {
                MID,
                Modifier,

                model.IDNO,
                model.IssueDate,
                model.ObtainType,
                model.IssueLocationID,
                model.IsPicture,
                model.FilePath1,
                model.FilePath2,
                model.Birthday,

                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新身份證換補發資料
        /// </summary>
        /// <param name="model">居留證資料</param>
        /// <param name="Modifier">修改人</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateUniformID(UpdateMemberAuthUniformID model, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            string sql = "EXEC ausp_Member_Admin_MemberAuth_UpdateAuthIDNO_U";
            var args = new
            {
                model.MID,
                Modifier,

                model.UniformID,
                model.UniformIssueDate,
                model.UniformExpireDate,
                model.UniformNumber,
                model.FilePath1,
                model.FilePath2,
                model.Birthday,

                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, args);
        }

        /// <summary>
        /// 取得會員身份證資料
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public MemberAuthIDNOModel GetAuthIDNO(long MID)
        {
            string sql = "EXEC ausp_Member_Admin_MemberAuth_GetAuthIDNO_S";
            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MemberAuthIDNOModel>(sql, args);
        }
    }
}
