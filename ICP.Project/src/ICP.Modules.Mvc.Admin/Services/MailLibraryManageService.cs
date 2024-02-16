using ICP.Modules.Mvc.Admin.Models.MailLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Repositories;

    public class MailLibraryManageService
    {
        MailLibraryManageRepository _mailLibraryManageRepository;

        public MailLibraryManageService(MailLibraryManageRepository mailLibraryManageRepository)
        {
            _mailLibraryManageRepository = mailLibraryManageRepository;
        }

        //LIST
        public List<ContentQueryResult> ListContent(ContentQueryModel query)
        {
            return _mailLibraryManageRepository.ListContent(query);
        }
    }
}
