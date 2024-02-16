using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Interfaces
{
    public interface IMenuItem<T> where T: IMenuItem<T>
    {
        /// <summary>
        /// 功能編號
        /// </summary>
        int FunctionID { get; set; }

        /// <summary>
        /// 功能群組編號
        /// </summary>
        int FunctionGroupID { get; set; }

        /// <summary>
        /// 功能層級
        /// </summary>
        byte FunctionLevel { get; set; }

        /// <summary>
        /// 子項目
        /// </summary>
        List<T> ChildrenFunction { get; set; }
    }
}
