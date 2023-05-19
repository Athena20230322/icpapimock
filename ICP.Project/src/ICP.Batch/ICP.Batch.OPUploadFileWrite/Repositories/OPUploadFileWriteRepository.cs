using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileWrite.Repositories
{
    using Models;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Infrastructure.Abstractions.DbUtil;

    public class OPUploadFileWriteRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public OPUploadFileWriteRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得綁定帳號重送記錄
        /// </summary>
        /// <param name="Source">產生重送來源 2:API重送排程 3: SFTP 通知排程</param>
        /// <param name="DataDate">重送資料日期</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual List<BindOPAccountNotifyRecord> ListBindOPAccountNotify_ReSendRecord(byte Source, DateTime DataDate)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OP_ListBindOPAccountNotify_ReSendRecord_SIU";

            var args = new
            {
                Source,
                DataDate
            };

            sql += db.GenerateParameter(args);

            return db.Query<BindOPAccountNotifyRecord>(sql, args);
        }
    }
}
