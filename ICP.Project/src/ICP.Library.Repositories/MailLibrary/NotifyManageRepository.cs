using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICP.Library.Repositories.MailLibrary
{
    using Infrastructure.Abstractions.DbUtil;
    using Models.MailLibrary;

    public class NotifyManageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public NotifyManageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        //GET
        public NotifyContent GetNotifyContent(long NotifyID)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_GetContent_S ";

            var args = new
            {
                NotifyID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<NotifyContent>(sql, args);
        }

        //GET
        public NotifyContent GetNotifyContentByKey(string NotifyKey)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_GetContent_S ";

            var args = new
            {
                NotifyID = 0,
                NotifyKey
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<NotifyContent>(sql, args);
        }
    }
}
