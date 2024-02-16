using System.Collections.Generic;
using ICP.Host.APIService.Models;
using ICP.Host.APIService.Repositories;
using ICP.Host.Middleware.SMS.Models;
using ICP.Infrastructure.Core.Models;

namespace ICP.Host.APIService.Services
{
    public class MistakeService
    {
        MistakeRepository _mistakerepository;
        public MistakeService(MistakeRepository mistakeRepository)
        {
            _mistakerepository = mistakeRepository;
        }

        /// <summary>
        /// 取得待發送簡訊
        /// </summary>
        /// <param name="States"></param>
        /// <param name="ChangeStates"></param>
        /// <returns></returns>
        public List<MistakeTemp> ListMistakeTemp(byte States, byte ChangeStates)
        {
            return _mistakerepository.ListMistakeTemp(States, ChangeStates);
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
            return _mistakerepository.UpdateReceiveSMS(AutoID, RtnCode, RtnCode, MessageId);
        }

        /// <summary>
        /// 接收三竹簡訊發送結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddMistakeRtnInfo(MistakeRtnModel model)
        {
            return _mistakerepository.AddMistakeRtnInfo(model);
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
            return _mistakerepository.MistakeUrlBody(Phone, GUID, MsgData);
        }

        /// <summary>
        /// 把三竹回傳資料更新至簡訊狀態
        /// </summary>
        /// <param name="AutoID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public BaseResult StrToModel(long AutoID, string data)
        {
            return _mistakerepository.StrToModel(AutoID, data);
        }
    }
}