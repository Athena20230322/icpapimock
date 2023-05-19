using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Api.Payment.Models.Pos;
using System.Text;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class PosRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public PosRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 由付款條碼取得會員資料
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public virtual BarcodeInfoDbRes GetBarcodeInfo(string barcode, int queryType, decimal amount)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            StringBuilder sql = new StringBuilder();
            sql.Append(" EXEC ausp_Payment_Trade_GetBarcodeInfo_S ");
            sql.Append(" @Barcode, ");
            sql.Append(" @QueryType, ");
            sql.Append(" @Amount ");

            return db.QuerySingleOrDefault<BarcodeInfoDbRes>(sql.ToString(), new
            {
                Barcode = barcode,
                QueryType = queryType,
                Amount = amount
            });
        }
    }
}