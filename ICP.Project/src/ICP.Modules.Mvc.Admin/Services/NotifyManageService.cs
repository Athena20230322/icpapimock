using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Models.MailLibrary;
    using Repositories;

    public class NotifyManageService : Library.Services.MailLibrary.NotifyManageService
    {
        NotifyManageRepository _NotifyManageRepository;

        public NotifyManageService(
            Library.Repositories.MailLibrary.NotifyManageRepository libNotifyManageRepository,
            NotifyManageRepository NotifyManageRepository
            ) : base(libNotifyManageRepository)
        {
            _NotifyManageRepository = NotifyManageRepository;
        }

        #region NotifyContent
        //ADD
        public DataResult<long> AddNotifyContent(NotifyContentModel model)
        {
            return _NotifyManageRepository.AddNotifyContent(model);
        }

        //UPDATE
        public BaseResult UpdateNotifyContent(NotifyContentModel model)
        {
            return _NotifyManageRepository.UpdateNotifyContent(model);
        }

        //DELETE
        public BaseResult DeleteNotifyContent(long NotifyID)
        {
            return _NotifyManageRepository.DeleteNotifyContent(NotifyID);
        }
        #endregion

        #region NotifyTag
        //LIST
        public List<NotifyTag> ListNotifyTag(long NotifyID)
        {
            return _NotifyManageRepository.ListNotifyTag(NotifyID);
        }
        //ADD
        public DataResult<long> AddNotifyTag(NotifyTag model)
        {
            return _NotifyManageRepository.AddNotifyTag(model);
        }
        //UPDATE
        public BaseResult UpdateNotifyTag(NotifyTag model)
        {
            return _NotifyManageRepository.UpdateNotifyTag(model);
        }
        //DELETE
        public BaseResult DeleteNotifyTag(long TagID)
        {
            return _NotifyManageRepository.DeleteNotifyTag(TagID);
        }
        #endregion
    }
}
