using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.TopUp;
using System;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class TopUpRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public TopUpRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        #region 自動儲值設定
        /// <summary>
        /// 新增/更新自動儲值條件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult UpdateAutoTopUpCondition(AutoTopUpModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_AddAutoTopUpSetting_IU]";

            int Action = 1;     //會員手動設定

            var args = new
            {
                model.MID,
                model.AccountID,
                model.TopUpSwitch,
                model.TopUpMode,
                model.TopUpUnit,
                model.LimitAmount,
                model.DailyLimitAmount,
                Action
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新自動儲值開關設定
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="TopUpSwitch"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult UpdateAutoTopUpSwitch(long MID, int TopUpSwitch)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_UpdateAutoTopUpSwitch_U]";

            var args = new
            {
                MID,
                TopUpSwitch
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得自動儲值設定
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public AutoTopUpModel GetAutoTopUpInfo(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_GetAutoTopUpSetting_S]";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<AutoTopUpModel>(sql, args);
        }
        #endregion

        #region 自動儲值訂單
        /// <summary>
        /// 取得當日已自動儲值的金額
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public int GetTodayAutoTopUpAmount(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetDailyAutoTopUpAmount_S]";

            string QueryDate = DateTime.Now.ToString("yyyy/MM/dd");

            var args = new
            {
                MID,
                QueryDate
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }
        #endregion

        #region 新增電支帳戶金額
        /// <summary>
        /// 新增電支帳戶金額
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddUserCoins(AddUserCoinsDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);
            string sql = "[ICP_Coins].[dbo].[EXEC ausp_Coins_UserCoin_AddUserCoins_SI]";

            var args = new
            {
                model.TradeNo,
                model.MID,
                model.MerchantID,
                model.TradeModeID,
                model.PaymentTypeID,
                model.PaymentSubTypeID,
                model.TradeRealCash,
                model.TradeTopUpCash,
                model.Currency,
                model.Notes
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion
    }
}
