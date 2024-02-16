using System.Collections.Generic;
using System.Linq;

namespace ICP.Host.APIService.Repositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Models;

    public class SMSRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public SMSRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得主要閘道
        /// </summary>
        /// <returns></returns>
        public int GetPriorityGateway()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_GetPriorityGateway_S";

            return db.QuerySingleOrDefault<int>(sql);
        }

        /// <summary>
        /// 新增簡訊內容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddSMS(AddSMS model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_AddSMS_I";

            var args = new
            {
                model.Phone,
                model.MsgData,
                model.SmsType,
                model.Gateway,
                model.Sender
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}