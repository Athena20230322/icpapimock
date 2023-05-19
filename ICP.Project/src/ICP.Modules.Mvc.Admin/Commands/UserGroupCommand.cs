using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;
    using Services;

    public class UserGroupCommand
    {
        UserGroupService _UserGroupService;

        public UserGroupCommand(UserGroupService UserGroupService)
        {
            _UserGroupService = UserGroupService;
        }

        public DataResult<int> AddUserGroup(UserGroup model)
        {
            return _UserGroupService.AddUserGroup(model);
        }

        public List<UserGroupQueryResult> ListUserGroup(UserGroupQuery model)
        {
            return _UserGroupService.ListUserGroup(model);
        }

        public UserGroup GetUserGroup(int UserGroupID)
        {
            return _UserGroupService.GetUserGroup(UserGroupID);
        }

        public BaseResult UpdateUserGroup(int UserGroupID, UserGroup model)
        {
            return _UserGroupService.UpdateUserGroup(UserGroupID, model);
        }

        public BaseResult DeleteUserGroup(int UserGroupID)
        {
            return _UserGroupService.DeleteUserGroup(UserGroupID);
        }
    }
}
