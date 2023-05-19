using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Core.Models;
    using Models.MailLibrary;
    using Infrastructure.Abstractions.DbUtil;

    public class NotifyManageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public NotifyManageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        #region NotifyContent
        //ADD
        public DataResult<long> AddNotifyContent(NotifyContentModel model)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_AddContent_I ";

            var args = new
            {
                model.NotifyKey,
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
        public BaseResult UpdateNotifyContent(NotifyContentModel model)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_UpdateContent_U ";

            var args = new
            {
                model.NotifyID,
                model.NotifyKey,
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
        public BaseResult DeleteNotifyContent(long NotifyID)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_DeleteContent_U ";

            var args = new
            {
                NotifyID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region NotifyTag
        //LIST
        public List<NotifyTag> ListNotifyTag(long NotifyID)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_ListTag_S ";

            var args = new
            {
                NotifyID
            };
            sql += db.GenerateParameter(args);
            return db.Query<NotifyTag>(sql, args);
        }
        //ADD
        public DataResult<long> AddNotifyTag(NotifyTag model)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_AddTag_I ";

            var args = new
            {
                NotifyID = model.NotifyID,
                model.TagKey,
                model.Name
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
        //UPDATE
        public BaseResult UpdateNotifyTag(NotifyTag model)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_UpdateTag_U ";

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
        public BaseResult DeleteNotifyTag(long TagID)
        {
            string sql = "EXEC ausp_Share_Admin_Notify_DeleteTag_D ";

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
