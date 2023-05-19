using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord
{
    public class CustomerServiceRecordNoteVM
    {
        /// <summary>
        /// 時間
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string Note { get; set; }
    }
}
