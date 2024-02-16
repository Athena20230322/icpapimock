using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class CalendarQuery : PageModel
    {
        /// <summary>
        /// 假日或補班日的年度
        /// </summary>
        public int DayYear { get; set; }
    }
}
