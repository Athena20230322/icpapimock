using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{

    public class P33AuthVM
    {
        /// <summary>
        /// ID的類別
        /// 0 : 身分證字號
        /// 1 : 統編
        /// </summary>
        [Required]
        public byte IDTypes { get; set; } = 0;
        /// <summary>
        /// 身份證字號
        /// </summary>
        [Required]
        [Display(Name = "身份證字號")]
        public string IDNO { get; set; }
        /// <summary>
        /// 統一編號
        /// </summary>
        [Required]
        [Display(Name = "統一編號")]
        public string UnifiedBusinessNo { get; set; }
    }
}
