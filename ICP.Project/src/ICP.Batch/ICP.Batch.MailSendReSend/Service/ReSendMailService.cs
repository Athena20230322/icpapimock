using ICP.Batch.MailSendReSend.Repositories;
using ICP.Library.Models.MailLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.MailSendReSend.Service
{
    public class ReSendMailService
    {
        ReSendMailRepositories _reSendMailRepositories;

        public ReSendMailService(
            ReSendMailRepositories reSendMailRepositories
            )
        {

            _reSendMailRepositories = reSendMailRepositories;
        }

        /// <summary>
        /// Mail重送
        /// </summary>
        /// <returns></returns>
        public List<MailSendDTO> ReSendMail()
        {
            var list = GetReSendMailList();

            return ReSendMailSendDTOs(list);
        }

        /// <summary>
        /// 獲取Maill發送失敗清單
        /// </summary>
        /// <returns></returns>
        private List<MailSendContent> GetReSendMailList()
        {
            return _reSendMailRepositories.GetReSendMailList();

        }

        /// <summary>
        /// 重組發送MailDTO
        /// </summary>
        /// <param name="mailSendContents"></param>
        /// <returns></returns>
        private List<MailSendDTO> ReSendMailSendDTOs(List<MailSendContent> mailSendContents)
        {
            return _reSendMailRepositories.ReSendMailSendDTOs(mailSendContents);
        }
    }
}
