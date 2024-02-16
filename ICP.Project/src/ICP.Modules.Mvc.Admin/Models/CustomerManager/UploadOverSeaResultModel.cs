using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class UploadOverSeaResultModel 
    { 
        /// <summary>
        /// 居留證字號
        /// </summary>       
        public string UniformID { get; set; }
                        
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMsg { get; set; }
                
        
    }
}
