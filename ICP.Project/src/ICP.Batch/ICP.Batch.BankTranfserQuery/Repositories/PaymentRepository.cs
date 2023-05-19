using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserQuery.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Models;

    public class PaymentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public PaymentRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得已送驗轉帳資訊 轉帳資料編號, FXML訊息編號
        /// </summary>
        /// <returns></returns>
        public List<BankTransferQueryModel> ListBankTransferQuery()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_ListBankTransferQuery_S";

            return db.Query<BankTransferQueryModel>(sql);
        }

        /// <summary>
        /// 更新已送驗轉帳資訊 查詢結果
        /// </summary>
        /// <returns></returns>
        public BaseResult UpdateBankTransferQuery(long TransferID, byte PayStatus, string ErrorCode, string ErrorMsg)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_UpdateBankTransferQuery_U";

            var args = new
            {
                TransferID,
                PayStatus,
                ErrorCode,
                ErrorMsg
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
