using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Modules.Mvc.Admin.Models.MailLibrary;

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
        //ADD
        public DataResult<long> AddMailContent(MailContentModel model)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_AddContent_I";

            var args = new
            {
                model.MailKey,
                model.Title,
                model.Body,
                model.Description,
                model.Creator,
                model.LayoutKey
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        //UPDATE
        public BaseResult UpdateMailContent(MailContentModel model)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_UpdateContent_U";

            var args = new
            {
                model.MailID,
                model.MailKey,
                model.Title,
                model.Body,
                model.Description,
                model.Modifier,
                model.LayoutKey
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        //DELETE
        public BaseResult DeleteMailContent(long MailID)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_DeleteContent_D";

            var args = new
            {
                MailID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region MailTag
        //LIST
        public List<MailTag> ListMailTag(long MailID)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_ListTag_S";

            var args = new
            {
                MailID
            };

            sql += db.GenerateParameter(args);
            return db.Query<MailTag>(sql, args);
        }
        //ADD
        public DataResult<long> AddMailTag(MailTag model)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_AddTag_I";

            var args = new
            {
                model.MailID,
                model.TagKey,
                model.Name
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
        //UPDATE
        public BaseResult UpdateMailTag(MailTag model)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_UpdateTag_U";

            var args = new
            {
                model.TagID,
                model.TagKey,
                model.Name
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        //DELETE
        public BaseResult DeleteMailTag(long TagID)
        {
            string sql = "EXEC ausp_Share_Admin_Mail_DeleteTag_D";

            var args = new
            {
                TagID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion
    }
}
