using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserSend.Repositories
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
        /// 取得待轉帳資訊發動轉帳指示
        /// </summary>
        /// <returns></returns>
        public List<BankTransferSendModel> ListBankTransferSend()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_ListBankTransferSend_S";

            return db.Query<BankTransferSendModel>(sql);
        }

        /// <summary>
        /// 更新待轉帳資訊發動轉帳指示為已送驗
        /// </summary>
        /// <param name="TransferID">提領記錄編號</param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult UpdateBankTransferSend(long TransferID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC Payment_ManageBank_UpdateBankTransferSend_U";

            var args = new
            {
                TransferID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
