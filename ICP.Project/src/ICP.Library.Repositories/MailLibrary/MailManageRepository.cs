using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Library.Repositories.MailLibrary
{
    using Infrastructure.Abstractions.DbUtil;
    using Models.MailLibrary;

    public class MailManageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public MailManageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        #region MailContent        
        //GET
        public MailContent GetMailContent(long MailID)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_GetContent_S";

            var args = new
            {
                MailID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MailContent>(sql, args);
        }

        //GET
        public MailContent GetMailContentByKey(string MailKey)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_GetContent_S";

            var args = new
            {
                MailID = 0,
                MailKey
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MailContent>(sql, args);
        }
        #endregion
    }
}