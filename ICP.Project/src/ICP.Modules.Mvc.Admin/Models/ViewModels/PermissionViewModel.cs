using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Interfaces;

    public class PermissionViewModel: PermissionModel, IMenuItem<PermissionViewModel>
    {
        public List<PermissionViewModel> ChildrenFunction { get; set; }
    }
}
