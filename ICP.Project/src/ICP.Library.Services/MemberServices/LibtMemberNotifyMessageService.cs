using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Repositories.MemberRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    public class LibtMemberNotifyMessageService
    {
        ConvertHelper _convertHelper;
        MemberNotifyMessageRepository _memberNotifyMessageRepository = null;

        public LibtMemberNotifyMessageService(
            MemberNotifyMessageRepository memberNotifyMessageRepository
            )
        {
            _convertHelper = new ConvertHelper();
            _memberNotifyMessageRepository = memberNotifyMessageRepository;
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
            return _memberNotifyMessageRepository.ListNotifyMessage(MID, PageIndex, PageSize, Status);
        }

        /// <summary>
        /// 取得較新或較舊訊息
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="MsgID">指定取得的訊息ID(包含此ID)</param>
        /// <param name="Type">取得指定取得ID的較新或較舊訊息 0：較舊 1：較新</param>
        /// <param name="Count">撈取筆數</param>
        /// <returns></returns>
        public List<MemberNotifyMessage> ListNotifyMessageByID(long MID, long MsgID, int Type, int Count)
        {
            return _memberNotifyMessageRepository.ListNotifyMessageByID(MID, MsgID, Type, Count);
        }

        /// <summary>
        /// 取得訊息清單 (差異)
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ModifyDate">上次取得訊息時間</param>
        /// <returns></returns>
        public List<MemberNotifyMessage> ListSynNotifyMessageByID(long MID, long TimeStemp)
        {
            DateTime ModifyDate = _convertHelper.TimeSpan2Date(TimeStemp).Value;
            return _memberNotifyMessageRepository.ListSynNotifyMessageByID(MID, ModifyDate);
        }

        /// <summary>
        /// 刪除訊息
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ArrayMsgID">訊息ID陣列</param>
        /// <returns></returns>
        public BaseResult DeleteNotifyMessage(long MID, List<long> ArrayMsgID)
        {
            if (ArrayMsgID == null || ArrayMsgID.Count == 0)
            {
                var result1 = new BaseResult();
                result1.SetError();
                return result1;
            }

            string sArrayMsgID = string.Join(",", ArrayMsgID);

            return _memberNotifyMessageRepository.DeleteNotifyMessage(MID, sArrayMsgID);
        }

        /// <summary>
        /// 刪除訊息
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="MsgID">訊息ID</param>
        /// <returns></returns>
        public BaseResult DeleteNotifyMessage(long MID, long MsgID)
        {
            return _memberNotifyMessageRepository.DeleteNotifyMessage(MID, MsgID.ToString());
        }

        /// <summary>
        /// 刪除全部訊息
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public BaseResult DeleteAllNotifyMessage(long MID)
        {
            return _memberNotifyMessageRepository.DeleteNotifyMessage(MID);
        }

        /// <summary>
        /// 更新訊息中心已讀
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="ArrayMsgID">訊息ID陣列</param>
        /// <returns></returns>
        public BaseResult ReadNotifyMessage(long MID, List<long> ArrayMsgID)
        {
            if (ArrayMsgID == null || ArrayMsgID.Count == 0)
            {
                var result1 = new BaseResult();
                result1.SetError();
                return result1;
            }

            string sArrayMsgID = string.Join(",", ArrayMsgID);

            return _memberNotifyMessageRepository.UpdateNotifyMessageRead(MID, sArrayMsgID);
        }

        /// <summary>
        /// 更新訊息中心已讀
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="MsgID">訊息ID</param>
        /// <returns></returns>
        public BaseResult ReadNotifyMessage(long MID, long MsgID)
        {
            return _memberNotifyMessageRepository.UpdateNotifyMessageRead(MID, MsgID.ToString());
        }

        /// <summary>
        /// 更新訊息中心已讀
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public BaseResult ReadAllNotifyMessage(long MID)
        {
            return _memberNotifyMessageRepository.UpdateNotifyMessageRead(MID);
        }

        /// <summary>
        /// 取得未讀數量
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public MemberNotifyMessageUnread GetNotifyMessageUnReadCount(long MID)
        {
            return _memberNotifyMessageRepository.GetNotifyMessageUnReadCount(MID);
        }

        /// <summary>
        /// 取得訊息清單內容
        /// </summary>
        /// <param name="NotifyMessageID">訊息編號</param>
        /// <param name="MID">檢查 MID</param>
        /// <returns></returns>
        public DataResult<MemberNotifyMessageDetail> GetNotifyMessage(long NotifyMessageID, long? MID = null, byte? Status = 1)
        {
            var result = new DataResult<MemberNotifyMessageDetail>();
            result.SetError();

            var rtnData = _memberNotifyMessageRepository.GetNotifyMessage(NotifyMessageID, MID, Status);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
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
        public BaseResult AddNotifyMessage(long mid, string subject, string body, int categoryId = 0, int push = 0)
        {
            return _memberNotifyMessageRepository.AddNotifyMessage(mid, subject, body, categoryId, push);
        }
    }
}
