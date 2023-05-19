using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ICP.Modules.Mvc.Admin.Services
{
    using ICP.Infrastructure.Abstractions.EmailSender;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Library.Models.MailLibrary;
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Models.MailLibrary;
    using Repositories;

    public class MailManageService: Library.Services.MailLibrary.MailManageService
    {
        MailManageRepository _mailManageRepository;
        ConfigRepository _configRepository;

        public MailManageService(
            Library.Repositories.MailLibrary.MailManageRepository libMailManageRepository,
            MailManageRepository mailManageRepository,
            ConfigRepository configRepository
            ): base(libMailManageRepository)
        {
            _mailManageRepository = mailManageRepository;
            _configRepository = configRepository;
        }

        #region MailContent
        //ADD
        public DataResult<long> AddMailContent(MailContentModel model)
        {
            return _mailManageRepository.AddMailContent(model);
        }

        //UPDATE
        public BaseResult UpdateMailContent(MailContentModel model)
        {
            return _mailManageRepository.UpdateMailContent(model);
        }

        //DELETE
        public BaseResult DeleteMailContent(long MailID)
        {
            return _mailManageRepository.DeleteMailContent(MailID);
        }
        #endregion

        #region MailTag
        //LIST
        public List<MailTag> ListMailTag(long MailID)
        {
            return _mailManageRepository.ListMailTag(MailID);
        }
        //ADD
        public DataResult<long> AddMailTag(MailTag model)
        {
            return _mailManageRepository.AddMailTag(model);
        }
        //UPDATE
        public BaseResult UpdateMailTag(MailTag model)
        {
            return _mailManageRepository.UpdateMailTag(model);
        }
        //DELETE
        public BaseResult DeleteMailTag(long TagID)
        {
            return _mailManageRepository.DeleteMailTag(TagID);
        }
        #endregion
    }
}