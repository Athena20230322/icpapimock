using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Internal;

namespace ICP.Batch.AppRssPush.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Models;
    public class AppRssPushRepositories
    {
        private readonly IDbConnectionPool _dbConnectionPool;

        public AppRssPushRepositories(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }


        /// <summary>
        /// 取得並更新待推播清單
        /// </summary>
        /// <returns></returns>
        public virtual List<AppRssPushContent> GetAppRssPushList()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);

            var sql = "EXEC ausp_Share_Member_AppRss_PushAppRssList_SU";

            return db.Query<AppRssPushContent>(sql);
        }

        /// <summary>
        /// 更新推播狀態
        /// </summary>
        /// <returns></returns>
        public virtual BaseResult UpdateAppRssPushList(string traceid, int Status, string message)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);

            var sql = "EXEC ausp_Share_Member_AppRss_UpdatePushAppRssList_U";

            var args = new
            {
                traceid,
                Status,
                message
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得AppRssRequestModel
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AppRssRequestContent GetAppRssRequest(AppRssPushContent content, string sourceName)
        {
            AppRssRequestContent request = new AppRssRequestContent
            {
                traceid = content.Traceid,
                source_name = sourceName,
                mid = content.OPMID.IsNullOrEmpty()?"":content.OPMID,
                alert = content.Subject.IsNullOrEmpty()?"":content.Subject,
                hyper_link = content.Hyper_link.IsNullOrEmpty()?"":content.Hyper_link,
                functionid = content.Functionid.IsNullOrEmpty()?"":content.Functionid,
                param = content.Param.IsNullOrEmpty()?"":content.Param
            };
            return request;

        }
    }
}