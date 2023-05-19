using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class FreezeCoinsModel : BaseListModel
    {
        public string CName { get; set; }
        public long FreezeID { get; set; }
        public long MID { get; set; }
        public long RtnMID { get; set; }
        public decimal FreezeCash { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public long RtnICPMID { get; set; }

    }
}
