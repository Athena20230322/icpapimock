using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class UpdateUniformIDStatusModel
    {
        /// <summary>
        /// 居留證字號
        /// </summary>
        public string UniformID { get; set; }
        
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>        
        public string CreateUser { get; set; }
          
        public long RealIP { get; set; }

        public long ProxyIP { get; set; }
    }
}
