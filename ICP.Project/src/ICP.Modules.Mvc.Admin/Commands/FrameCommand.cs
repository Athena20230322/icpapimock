using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Models.ViewModels;
    using Services;

    public class FrameCommand
    {
        FrameService _frameService;
        PrivilegeService _privilegeService;

        public FrameCommand(FrameService frameService, PrivilegeService privilegeService)
        {
            _frameService = frameService;
            _privilegeService = privilegeService;
        }

        public List<FrameMenuItem> ListMenuItem(int UserID)
        {
            var userFunctions = _privilegeService.ListUserFunction(UserID);

            return _frameService.ListMenuItem(userFunctions);
        }
    }
}
