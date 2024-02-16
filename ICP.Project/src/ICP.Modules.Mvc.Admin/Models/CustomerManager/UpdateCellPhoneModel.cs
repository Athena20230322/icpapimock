using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class UpdateCellPhoneModel
    {
        public string CellPhone { get; set; }

        public long MID { get; set; }       

        public string Remark { get; set; }

        public string ModifyUser { get; set; }
        
        public long RealIP { get; set; }

        public long ProxyIP { get; set; }
    }
}
