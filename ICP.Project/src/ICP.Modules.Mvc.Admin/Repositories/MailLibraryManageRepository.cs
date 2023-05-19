using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Modules.Mvc.Admin.Models.MailLibrary;

    public class MailLibraryManageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public MailLibraryManageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        //LIST
        public List<ContentQueryResult> ListContent(ContentQueryModel query)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_ListContent_S";

            var args = new
            {
                query.PageSize,
                query.PageNo,
                query.MailKey,
                query.Description
            };

            sql += db.GenerateParameter(args);
            return db.Query<ContentQueryResult>(sql, args);
        }
    }
}
