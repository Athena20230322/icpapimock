using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Modules.Api.TinyURL.Repositories;

namespace ICP.Modules.API.TinyURL.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;

    public class TinyURLRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public TinyURLRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// 產生短網址
        /// </summary>
        /// <param name="WebSiteURL">原網址</param>
        /// <returns>短網址</returns>
        [EnableDbProxy]
        public virtual string AddTinyURL(string WebSiteURL)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);

            string sql = "ausp_Share_TinyURL_AddTinyURLOnGenerate_I";

            var args = new
            {
                WebSiteURL
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<string>(sql, args);
        }

        /// 取得短網址
        /// </summary>
        /// <param name="TinyURL">短網址</param>
        /// <returns>原網址</returns>
        [EnableDbProxy]
        public virtual string GetWebSiteURL(string TinyURLID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);

            string sql = "ausp_Share_TinyURL_GetWebSiteURL_S";

            var args = new
            {
                TinyURLID
            };

            sql += db.GenerateParameter(args);

            string URL = db.QuerySingleOrDefault<string>(sql, args);

            return URL;
        }
    }
}
