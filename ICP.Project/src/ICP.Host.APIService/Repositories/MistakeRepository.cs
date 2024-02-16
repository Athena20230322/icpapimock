using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ICP.Host.APIService.Models;
using ICP.Host.Middleware.SMS.Models;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Repositories.SystemRepositories;

namespace ICP.Host.APIService.Repositories
{
    public class MistakeRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly ConfigKeyValueRepository _configKeyValueRepository = null;

        public MistakeRepository(IDbConnectionPool dbConnectionPool,
            ConfigKeyValueRepository configKeyValueRepository)
        {
            _configKeyValueRepository = configKeyValueRepository;
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得待發送簡訊清單
        /// </summary>
        /// <param name="States"></param>
        /// <param name="ChangeStates"></param>
        /// <returns></returns>
        public List<MistakeTemp> ListMistakeTemp(byte States, byte ChangeStates)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);
            string sql = "EXEC ausp_SMS_UpdateMistakeSMSStatus_SU";

            var args = new
            {
                States,
                ChangeStates
            };

            sql += db.GenerateParameter(args);

            return db.Query<MistakeTemp>(sql, args).ToList();
        }

        /// <summary>
        /// 更新三竹簡訊發送狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="RtnCode"></param>
        /// <param name="RtnMsg"></param>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        public BaseResult UpdateReceiveSMS(long AutoID, string RtnCode, string RtnMsg, string MessageId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_UpdateReceiveSMSByMistake_U";

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
        /// 接收三竹簡訊發送結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddMistakeRtnInfo(MistakeRtnModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_SMS);

            string sql = "EXEC ausp_SMS_AddSMSDataByMistake_I";

            var args = new
            {
                model.MessageId,
                model.dstaddr,
                model.dlvtime,
                model.donetime,
                model.statuscode,
                model.statusstr,
                model.StatusFlag
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 產生三竹簡訊UrlBody
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="GUID"></param>
        /// <param name="MsgData"></param>
        /// <returns></returns>
        public string MistakeUrlBody(string Phone, string GUID, string MsgData)
        {
            StringBuilder builder = new StringBuilder();
            List<string> urlList = new List<string>();

            builder.Append(_configKeyValueRepository.MTSMS_submiturl);
            builder.Append("username=").Append(_configKeyValueRepository.MTSMS_username).Append('&');
            builder.Append("password=").Append(_configKeyValueRepository.MTSMS_password).Append('&');
            builder.Append("dstaddr=").Append(HttpUtility.UrlEncode(Phone)).Append('&');
            builder.Append("smbody=").Append(HttpUtility.UrlEncode(MsgData, UTF8Encoding.UTF8)).Append('&');
            builder.Append("response=").Append(_configKeyValueRepository.MTSMS_responseurl).Append('&');
            builder.Append("clientid=").Append(GUID).Append('&');
            builder.Append("CharsetURL=").Append("UTF8");

            return builder.ToString();
        }

        /// <summary>
        /// 把三竹回傳資料更新至簡訊狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public BaseResult StrToModel(long AutoID, string data)
        {
            string outStr = string.Empty;
            string MessageId = string.Empty;
            string RtnCode = string.Empty;
            int Duplicate = 0;

            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>(
                                             data.Split(new Char[2] { (char)13, (char)10 })
                                            .Where(s => s.Contains('='))
                                            .Select(s => s.Split('='))
                                            .ToDictionary(key => key[0].Trim(), value => value[1].Trim()));
            //是否是重複簡訊(Duplicate)
            if ((dict.TryGetValue("Duplicate", out outStr) ? dict["Duplicate"] : null) == "Y")
            {
                Duplicate = 1;
            }
            else
            {
                Duplicate = 0;
            }

            MessageId = dict.TryGetValue("msgid", out outStr) ? dict["msgid"] : null;
            RtnCode = dict.TryGetValue("statuscode", out outStr) ? dict["statuscode"] : null;

            return UpdateReceiveSMS(AutoID, RtnCode, data, MessageId);
        }


    }
}