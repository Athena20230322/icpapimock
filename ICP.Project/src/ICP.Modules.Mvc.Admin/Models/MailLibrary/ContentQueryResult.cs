using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MailLibrary
{
    using Infrastructure.Core.Models;

    public class ContentQueryResult: BaseListModel
    {
        public long MailID { get; set; }

        public string Creator { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string Modifier { get; set; }

        public long LayoutID { get; set; }


        public long NotifyID { get; set; }

        public string NotifyCreator { get; set; }

        public DateTime? NotifyModifyDate { get; set; }

        public string NotifyModifier { get; set; }

        public long NotifyLayoutID { get; set; }


        public DateTime CreateDate { get; set; }

        public string MailKey { get; set; }

        public string Description { get; set; }


        public string LayoutKey { get; set; }

        public string NotifyLayoutKey { get; set; }
    }
}
