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

    public class PersonalAuthRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;


        public PersonalAuthRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 查詢聯徵P11紀錄查詢
        /// </summary>
        /// <param name="model">查詢model</param>
        /// <returns>清單</returns>
        public List<P11AuthHistoryQuery> QueryP11AuthLog(P11AuthHistory model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_Member_ListP11AuthLog_S";

            var args = new
            {
                model.MID,
                model.ICPMID,
                model.StartDate,
                model.EndDate,
                model.IDNO,
                model.IsPass,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);
            return db.Query<P11AuthHistoryQuery>(sql, args);
        }
        /// <summary>
        /// 查詢聯徵P33紀錄查詢
        /// </summary>
        /// <param name="model">查詢model</param>
        /// <returns>清單</returns>
        public List<P33AuthHistoryQuery> QueryP33AuthLog(P33AuthHistory model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_Member_ListP33AuthLog_S";

            var args = new
            {
                model.MID,
                model.ICPMID,
                model.StartDate,
                model.EndDate,
                model.IDNO,
                model.IsPass,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);
            return db.Query<P33AuthHistoryQuery>(sql, args);
        }
    }
}
