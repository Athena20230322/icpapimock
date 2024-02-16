using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    public class MemberNotifyMessageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private readonly IDbConnection db = null;

        public MemberNotifyMessageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
        }

        /// <summary>
        /// 查詢訊息中心
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="PageIndex">頁索引</param>
        /// <param name="PageSize">頁筆數</param>
        /// <returns></returns>
        public MemberNotifyMessageResult ListNotifyMessage(long MID, int PageIndex, int PageSize, byte? Status = 1)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_ListNotifyMessage_S";

            var args = new
            {
                MID,
                PageIndex,
                PageSize,
                Status
            };

            sql += db.GenerateParameter(args);

            var types = new Type[]
            {
                typeof(MemberNotifyMessage),
                typeof(int),
                typeof(int)
            };

            var results = db.QueryMultiple(types, sql, args);

            var result = new MemberNotifyMessageResult();
            result.Items = results[0].Cast<MemberNotifyMessage>().ToList();
            result.TotalCount = results[1].Cast<int>().FirstOrDefault();
            result.PageCount = results[1].Cast<int>().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 取得訊息清單
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="MsgID">指定取得的訊息ID(包含此ID)</param>
        /// <param name="Type">取得指定取得ID的較新或較舊訊息 0：較舊 1：較新</param>
        /// <param name="Count">撈取筆數</param>
        /// <returns></returns>
        public List<MemberNotifyMessage> ListNotifyMessageByID(long MID, long MsgID, int Type, int Count)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_ListNotifyMessageByID_S";

            var args = new
            {
                MID,
                MsgID,
                Type,
                Count
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberNotifyMessage>(sql, args);
        }

        /// <summary>
        /// 取得訊息清單 (差異)
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ModifyDate">上次取得訊息時間</param>
        /// <returns></returns>
        public List<MemberNotifyMessage> ListSynNotifyMessageByID(long MID, DateTime ModifyDate)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_ListSynNotifyMessageByID_S";

            var args = new
            {
                MID,
                ModifyDate
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberNotifyMessage>(sql, args);
        }

        /// <summary>
        /// 刪除訊息中心
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ArrayMsgID">訊息ID字串(逗點分隔), 空值代表全部</param>
        /// <returns></returns>
        public BaseResult DeleteNotifyMessage(long MID, string ArrayMsgID = null)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_UpdateNotifyMessageDelete_IU";

            var args = new
            {
                MID,
                ArrayMsgID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新訊息中心已讀
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ArrayMsgID">訊息ID字串(逗點分隔), 空值代表全部</param>
        /// <returns></returns>
        public BaseResult UpdateNotifyMessageRead(long MID, string ArrayMsgID = null)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_UpdateNotifyMessageRead_IU";

            var args = new
            {
                MID,
                ArrayMsgID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得未讀數量
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public MemberNotifyMessageUnread GetNotifyMessageUnReadCount(long MID)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_GetNotifyMessageUnReadCount_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberNotifyMessageUnread>(sql, args);
        }

        /// <summary>
        /// 取得訊息清單內容
        /// </summary>
        /// <param name="NotifyMessageID">訊息編號</param>
        /// <param name="MID">檢查 MID</param>
        /// <returns></returns>
        public MemberNotifyMessageDetail GetNotifyMessage(long NotifyMessageID, long? MID = null, byte? Status = 1)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_GetNotifyMessage_S";

            var args = new
            {
                NotifyMessageID,
                MID,
                Status
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberNotifyMessageDetail>(sql, args);
        }
        
        /// <summary>
        /// 新增訊息中心
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="categoryId"></param>
        /// <param name="push">推送開關 0推送 1不推送</param>
        /// <returns></returns>
        public BaseResult AddNotifyMessage(long mid,string subject,string body,int categoryId=0,int push=0)
        {
            string sql = "EXEC ausp_Share_NotifyMessage_AddNotifyMessage_I";

            var args = new
            {
                MID=mid,
                Subject=subject,
                Body=body,
                CategoryID=categoryId,
                Push=push
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
