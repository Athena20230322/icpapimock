using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPApiReSend.Services
{
    using Models;
    using Repositories;

    public class OPApiReSendService
    {
        OPApiReSendRepository _oPApiReSendRepository;

        public OPApiReSendService(
            OPApiReSendRepository oPApiReSendRepository
            )
        {
            _oPApiReSendRepository = oPApiReSendRepository;
        }

        /// <summary>
        /// 取得綁定帳號重送記錄
        /// </summary>
        /// <param name="Source">產生重送來源 2:API重送排程 3: SFTP 通知排程</param>
        /// <param name="DataDate">重送資料日期</param>
        /// <returns></returns>
        public List<BindOPAccountNotifyRecord> ListBindOPAccountNotify_ReSendRecord(byte Source, DateTime? DataDate = null)
        {
            return _oPApiReSendRepository.ListBindOPAccountNotify_ReSendRecord(Source, DataDate);
        }
    }
}
