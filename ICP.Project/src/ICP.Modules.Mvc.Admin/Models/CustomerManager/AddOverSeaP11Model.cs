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
    public class AddOverSeaP11Model
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }              

        /// <summary>
        /// 居留證字號
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        public DateTime UniformIssueDate { get; set; }

        /// <summary>
        /// 居留證到期日
        /// </summary>
        public DateTime UniformExpireDate { get; set; }

        /// <summary>
        /// 背面流水號
        /// </summary>
        public string UniformNumber { get; set; }

        /// <summary>
        /// 居留證正面
        /// </summary>
        public string FilePath1 { get; set; }

        /// <summary>
        /// 居留正反面
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        /// 來源  0:使用者輸入 1:批次匯入
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }
        
        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }
        
        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }
                        
    }
}
