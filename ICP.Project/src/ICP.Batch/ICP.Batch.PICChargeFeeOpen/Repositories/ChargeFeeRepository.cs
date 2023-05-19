using ICP.Batch.PICChargeFeeOpen.Models;
using ICP.Batch.PICChargeFeeOpen.Models.Enums;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen.Repositories
{
   public class ChargeFeeRepository 
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        public ChargeFeeRepository(IDbConnectionPool dbConnectionPool) 
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得待開立手續費清單
        /// </summary>
        [EnableDbProxy]
        public List<T> ListChargeFeeInvoiceIssue<T>(Issue_StatusEnum Issue_Status = Issue_StatusEnum.Default
            , StateEnums State = StateEnums.Default)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            var sql = "EXEC ausp_Member_ChargeFee_ListInvoiceIssueByStatus_S ";

            var args = new
            {
                Issue_Status,
                State
            };

            sql += db.GenerateParameter(args);

            return db.Query<T>(sql, args);
        }

        /// <summary>
        /// 取得待開立手續費商品清單
        /// </summary>
        [EnableDbProxy]
        public List<T> ListChargeFeeInvoiceIssue_ProductItem<T>(string InvoiceIDs)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            var sql = "EXEC ausp_Member_ChargeFee_ListProductItem_S";

            var args = new
            {
                InvoiceIDs
            };

            sql += db.GenerateParameter(args);

            return db.Query<T>(sql, args);
        }

        /// <summary>
        /// 更新 PIC 回傳結果 到資料庫
        /// </summary>
        [EnableDbProxy]
        public void UpdateInvoiceIssueByPicResult(CF_InvoiceIssue_UpdateStatusModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            var sql = "EXEC ausp_Member_ChargeFee_UpdateIssueOpenOrderResult_U ";

            var args = new
            {
                model.InvoiceID,
                model.InvoiceNo,
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
