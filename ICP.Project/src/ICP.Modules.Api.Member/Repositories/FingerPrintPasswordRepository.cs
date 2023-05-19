using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Member.Models;
using ICP.Modules.Api.Member.Models.MemberInfo;

namespace ICP.Modules.Api.Member.Repositories
{
    public class FingerPrintPasswordRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public FingerPrintPasswordRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        #region 

        /// <summary>
        /// 取得指紋密碼狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public BaseResult GetFingerPrintPasswordStatus(long mid)
        {

            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_GetFingerPrintLockStatus_S";

            var args = new
            {
                mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }

        /// <summary>
        /// 更新指紋密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateFingerPrintPassword(FingerPrintPasswordModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_UpdateFingerPrintPassword_SIU";

            sql += db.GenerateParameter(model);
            //var args = new
            //{
            //    model
            //};

            //sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, model);

        }

        /// <summary>
        /// 檢查指紋密碼是否正確
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult CheckFingerPrintPassword(FingerPrintPasswordModel model)
        {

            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_CheckFingerPrintPassword_SIU";

            var args = new
            {
                model.MID,
                model.Password,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }

        /// <summary>
        /// 取得當前指紋密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public BaseResult GetFingerPrintPassword(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_GetFingerPrintPassword_S";

            var args = new
            {
                mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }

        #endregion





    }
}
