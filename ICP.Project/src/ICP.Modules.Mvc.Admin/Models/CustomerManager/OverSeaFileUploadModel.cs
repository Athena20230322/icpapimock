using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class OverSeaFileUploadModel
    {
        /// <summary>
        /// 居留證字號
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 居留證正面
        /// </summary>        
        public string FilePath1 { get; set; }

        /// <summary>
        /// 居留證反面
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 銀行存摺封面
        /// </summary>
        public string FilePath3 { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Modifier { get; set; }


    }
}
