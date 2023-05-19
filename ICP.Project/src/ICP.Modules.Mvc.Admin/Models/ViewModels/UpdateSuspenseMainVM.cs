using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class UpdateSuspenseMainVM
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [Required]
        public long SuspenseID { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Note { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        [Required]
        [Range(0, 3)]
        public byte AuthStatus { get; set; }
    }
}
