using ICP.Batch.PICChargeFeeResult.Models;
using ICP.Batch.PICChargeFeeResult.Models.Enums;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeResult.Repositories
{
   public class ChargeFeeRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public ChargeFeeRepository(IDbConnectionPool dbConnectionPool) 
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 更新 PIC 回傳結果 到資料庫
        /// </summary>
        [EnableDbProxy]
        public void UpdateInvoiceIssueByPicResult(CF_InvoiceIssue_UpdateModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            var sql = "EXEC ausp_Member_ChargeFee_UpdateIssueOpenResult_U";

            var args = new
            {
                model.InvoiceNo,
                model.InvoiceNumber,
                model.InvoiceDate,
                model.Issue_Status,
                model.State,
                model.RtnCode,
                model.RtnMsg,
                model.Modifier
            };

            sql += db.GenerateParameter(args);

            db.QuerySingleOrDefault<int>(sql, args);
        }

    }
}
