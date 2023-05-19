using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MailLibrary
{
    using Infrastructure.Core.Models;

    public class ContentQueryModel: PageModel
    {
        public string MailKey { get; set; }

        public string Description { get; set; }
    }
}
