using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    using Infrastructure.Core.ValidationAttributes;
    using Infrastructure.Core.Models;

    /// <summary>
    /// 特店
    /// </summary>
    public class CustomerDataModel: ValidatableObject
    {
        /// <summary>
        /// 主檔
        /// </summary>
        [ValidateObject]
        public CustomerBasic basic { get; set; }

        /// <summary>
        /// 明細
        /// </summary>
        [ValidateObject]
        public CustomerDetailModel detail { get; set; }

        /// <summary>
        /// 合約
        /// </summary>
        [ValidateObject]
        public CustomerContractModel contract { get; set; }

        /// <summary>
        /// 撥款天數
        /// </summary>
        [ValidateObject]
        public List<CustomerAllocateDayModel> allocateDays { get; set; }
    }
}
