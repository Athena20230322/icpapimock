using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models;
    using Models.ViewModels;

    public class UserGroupService
    {
        UserGroupRepository _UserGroupRepository;

        public UserGroupService(UserGroupRepository UserGroupRepository)
        {
            _UserGroupRepository = UserGroupRepository;
        }

        public DataResult<int> AddUserGroup(UserGroup model)
        {
            return _UserGroupRepository.AddUserGroup(model);
        }

        public List<UserGroupQueryResult> ListUserGroup(UserGroupQuery model = null)
        {
            return _UserGroupRepository.ListUserGroup(model);
        }

        public UserGroup GetUserGroup(int UserGroupID)
        {
            return _UserGroupRepository.GetUserGroup(UserGroupID);
        }

        public BaseResult UpdateUserGroup(int UserGroupID, UserGroup model)
        {
            return _UserGroupRepository.UpdateUserGroup(UserGroupID, model);
        }

        public BaseResult DeleteUserGroup(int UserGroupID)
        {
            return _UserGroupRepository.DeleteUserGroup(UserGroupID);
        }
    }
}
