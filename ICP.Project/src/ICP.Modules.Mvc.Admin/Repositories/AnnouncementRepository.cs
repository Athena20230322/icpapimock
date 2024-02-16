using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Announcement;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class AnnouncementRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public AnnouncementRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 訊息公告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListAnnounceDbRes> ListContent(ListAnnounceDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_ListContent_S";

            sql += db.GenerateParameter(req);

            return db.Query<ListAnnounceDbRes>(sql, req);
        }

        /// <summary>
        /// 公告類別清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListCategoryDbRes> ListCategory(ListCategoryDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_ListCategory_S";

            sql += db.GenerateParameter(req);

            return db.Query<ListCategoryDbRes>(sql, req);
        }

        /// <summary>
        /// 新增公告
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> AddContent(AddContentDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_AddContent_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        /// <summary>
        /// 新增圖片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddImage(AddImageDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_AddImage_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 新增MID名單檔案
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> AddMidFile(AddMidFileDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_AddFileUpload_I";

            sql += db.GenerateParameter(req);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        /// <summary>
        /// 新增MID名單檔案
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddMid(AddMidDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_AddFileUploadDetail_I";

            sql += db.GenerateParameter(req);
            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 取得公告
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public GetContentDbRes GetContent(int nID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_GetContent_S";

            var args = new
            {
                NID = nID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<GetContentDbRes>(sql, args);
        }

        /// <summary>
        /// 取得MID名單檔案
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public List<GetMidFileDbRes> GetMidFile(int nID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_ListFileUpload_S";

            var args = new
            {
                NID = nID
            };

            sql += db.GenerateParameter(args);
            return db.Query<GetMidFileDbRes>(sql, args);
        }

        /// <summary>
        /// 更新/刪除/審核 訊息公告
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> EditContent(EditContentDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_UpdateContent_U";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        /// <summary>
        /// 修改MID名單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult EditMidList(EditMidDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_UpdateFileUpload_U";

            sql += db.GenerateParameter(req);
            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 新增公告類別
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> AddCategory(AddCategoryDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_AddCategory_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        /// <summary>
        /// 更新公告類別
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult EditCategory(EditCategoryDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Announce_UpdateCategory_U";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 新增訊息中心
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> AddNotifyMessage(object req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_NotifyMessage_AddNotifyMessage_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        #region Add Log
        /// <summary>
        /// AddContentLog
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddContentLog(AddContentLogDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Share_Announce_MainLog_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// Add Category Log
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddCategoryLog(AddCategoryLogDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Share_Announce_CategoryLog_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }
        #endregion

        /// <summary>
        /// 取得會員編號
        /// </summary>
        /// <param name="IDNO">身份證</param>
        /// <param name="ICPMID">電支帳號</param>
        /// <returns></returns>
        public DataResult<long> GetMID(string IDNO = null, string ICPMID = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMID_S";

            var args = new
            {
                IDNO,
                ICPMID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
    }
}
