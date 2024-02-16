using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Payment.Models.Event.Banner;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Repositories.Event
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
        /// <param name="siteID"></param>
        /// <returns></returns>
        public List<ListBannerRes> ListBanner(int siteID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Banner_ListBanner_S";

            var args = new
            {
                SiteID = siteID
            };

            sql += db.GenerateParameter(args);

            return db.Query<ListBannerRes>(sql, args);
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public GetBannerRes GetBanner(int bannerID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Banner_GetBanner_S";

            var args = new
            {
                BannerID = bannerID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<GetBannerRes>(sql, args);
        }
    }
}
