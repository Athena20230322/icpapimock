using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 職業
    /// </summary>
    public class OccupationModel
    {
        /// <summary>
        /// 職業編號
        /// </summary>
        public int OccupationID { get; set; }

        /// <summary>
        /// 職業名稱
        /// </summary>
        public string Occupation { get; set; }
    }
}
