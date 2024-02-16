using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberDataModel
    {
        public MemberBasicModel basic { get; set; }

        public MemberDetailModel detail { get; set; }
    }
}
