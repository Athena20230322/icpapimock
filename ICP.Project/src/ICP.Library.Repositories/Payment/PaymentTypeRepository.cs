using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Library.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.Payment
{
    public class PaymentTypeRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public PaymentTypeRepository(
            IDbConnectionPool dbConnectionPool
        )
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得特定付款方式的收單行清單
        /// </summary>
        /// <param name="paymentTypeID"></param>
        /// <returns></returns>
        public List<TradeTypeModel> ListTradeSubType(int paymentTypeID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_ListTradeSubType_S";

            var args = new
            {
                PaymentTypeID = paymentTypeID  
            };

            sql += db.GenerateParameter(args);
            return db.Query<TradeTypeModel>(sql, args);
        }  
    }
}
