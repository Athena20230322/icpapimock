using System.Collections.Generic;
using System.Linq;

namespace ICP.Host.APIService.Repositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Models;

    public class FETRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public FETRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得待發送簡訊
        /// </summary>
        /// <param name="States"></param>
        /// <param name="ChangeStates"></param>
        /// <returns></returns>
        public List<FETTemp> ListFetTemp(byte States, byte ChangeStates)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_UpdateSMSStatus_SU";

            var args = new
            {
                States,
                ChangeStates
            };

            sql += db.GenerateParameter(args);

            return db.Query<FETTemp>(sql, args).ToList();
        }

        /// <summary>
        /// 更新簡訊發送狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="RtnCode"></param>
        /// <param name="RtnMsg"></param>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        public BaseResult UpdateReceiveSMS(long AutoID, string RtnCode, string RtnMsg, string MessageId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_UpdateReceiveSMSByFET_U";

            var args = new
            {
                AutoID,
                RtnCode,
                RtnMsg,
                MessageId
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 接收簡訊發送結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddFETRtnInfo(FETRtnModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_AddSMSDataByFET_I";

            var args = new
            {
                model.SysId,
                model.MessageId,
                model.DestAddress,
                model.DeliveryStatus,
                model.ErrorCode,
                model.SubmitDate,
                model.DoneDate,
                model.Seq
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}