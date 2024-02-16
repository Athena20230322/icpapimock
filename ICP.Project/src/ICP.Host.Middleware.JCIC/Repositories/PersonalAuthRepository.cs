using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Repositories
{
    using Models;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;

    public class PersonalAuthRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public PersonalAuthRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
        }

        public LogResult AddAuthP33Log(P33Auth model)
        {
            string sql = "EXEC ausp_Member_AddAuthP33Log_I";
            sql += db.GenerateParameter(model);

            return db.QuerySingleOrDefault<LogResult>(sql, model);
        }

        public LogResult AddAuthP11Log(P11Auth model)
        {
            string sql = "EXEC ausp_Member_AddAuthP11Log_I";
            sql += db.GenerateParameter(model);

            return db.QuerySingleOrDefault<LogResult>(sql, model);
        }

        public BaseResult UpdateAuthP33Log(P33AuthResponse model)
        {
            string sql = "EXEC ausp_Member_UpdateAuthP33Log_I";
            var args = new
            {
                model.LogID,
                model.Code,
                model.Msg,
                model.SessionID,
                model.IsPass,
                model.DataCount,
                model.DataList
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateAuthP11Log(P11AuthResponse model)
        {
            string sql = "EXEC ausp_Member_UpdateAuthP11Log_SIU";
            var args = new
            {
                model.LogID,
                model.Return_Code,
                model.Return_Msg,
                model.Code,
                model.Msg,
                model.SessionID,
                model.IsPass,
                model.Answer,
                model.Result
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}