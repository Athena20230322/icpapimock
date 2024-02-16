using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class UserGroupRelation
    {
        [Required]
        public int UserGroupID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
