using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.MailSendReSend.Repositories
{
    using ICP.Infrastructure.Core.Frameworks.DbUtil;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Models.MailLibrary;
    using Infrastructure.Abstractions.DbUtil;

    public class ReSendMailRepositories
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public ReSendMailRepositories(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 獲取Maill發送失敗清單
        /// </summary>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual List<MailSendContent> GetReSendMailList()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Mail);

            string sql = "EXEC ausp_Mail_ReSendMailList_S";

            return db.Query<MailSendContent>(sql);
        }

        /// <summary>
        /// 重組發送MailDTO
        /// </summary>
        /// <param name="mailSendContents"></param>
        /// <returns></returns>
        public List<MailSendDTO> ReSendMailSendDTOs(List<MailSendContent> mailSendContents)
        {
            List<MailSendDTO> mailSendDTOList = new List<MailSendDTO>();
            MailSendDTO mailSendDTO = new MailSendDTO();

            mailSendContents.ForEach(t =>
            {
                mailSendDTO.MailID = t.MailID;
                mailSendDTO.Body = t.Body;
                mailSendDTO.ErrorMail = t.ErrorMail;
                mailSendDTO.MailFrom = t.MailFrom;
                mailSendDTO.MailTo = t.MailTo.Split(';').ToList<string>();
                mailSendDTO.MID = t.MID;
                mailSendDTO.Sbcc = mailSendDTO.Sbcc!=null?t.Sbcc.Split(';').ToList<string>():null;
                mailSendDTO.Scc = mailSendDTO.Scc!=null?t.Scc.Split(';').ToList<string>():null;
                mailSendDTO.SMTPIP = t.SMTPIP;
                mailSendDTO.Source = t.Source;
                mailSendDTO.Subject = t.Subject;
                mailSendDTOList.Add(mailSendDTO);
            });
            return mailSendDTOList;

        }
    }
}
