using Castle.Components.DictionaryAdapter.Xml;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using  ICP.Library.Models.AppRssLibrary;
namespace ICP.Library.Repositories.AppRssLibrary
{
    using Infrastructure.Abstractions.DbUtil;
    using Models.AppRssLibrary;

    public class AppRssRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public AppRssRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        /// <summary>
        /// 新增推播訊息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddAppRssContent(AppRssContent model)
        {
            var sql = "EXEC ausp_Share_Member_AppRss_AddAppRss_I";

            var args = new
            {
                model.MID,
                model.ExpireTime,
                model.Functionid,
                model.Hyper_link,
                model.NotifyMessageID,
                model.OPMID,
                model.Param,
                model.Priority,
                model.Subject,
                model.Title,
                model.Identifier
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }



    }
}