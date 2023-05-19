using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Banner;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class BannerRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public BannerRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 廣告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListBannerRes> ListBanner(ListBannerReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_ListBanner_S";

            sql += db.GenerateParameter(req);

            return db.Query<ListBannerRes>(sql, req);
        }

        /// <summary>
        /// 取得廣告位置(依BannerID)
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public List<BannerSiteDbRes> GetBannerSite(int bannerID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_GetBannerSite_S";

            var args = new
            {
                BannerID = bannerID
            };

            sql += db.GenerateParameter(args);

            return db.Query<BannerSiteDbRes>(sql, args);
        }

        /// <summary>
        /// 取得廣告位置清單
        /// </summary>
        /// <returns></returns>
        public List<BannerSiteDbRes> ListBannerSiteData()
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_ListBannerSiteData_S";

            return db.Query<BannerSiteDbRes>(sql);
        }

        /// <summary>
        /// 新增廣告&更新排序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<int> AddBanner(AddBannerReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_AddBanner_IU";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<DataResult<int>>(sql, req);
        }

        /// <summary>
        /// 設定廣告位置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult SetBannerSite(object req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_AddBannerSite_ID";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 新增廣告內頁圖片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddImage(AddImageReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_AddImage_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public GetBannerRes GetBanner(int bannerID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_GetBanner_S";

            var args = new
            {
                BannerID = bannerID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<GetBannerRes>(sql, args);
        }

        /// <summary>
        /// 更新/刪除/審核 廣告
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult EditBanner(EditBannerReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_UpdateBanner_U";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 檢查廣告排序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult CheckBannerOrderID(object req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_Banner_CheckBannerOrderID_S";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }
    }
}
