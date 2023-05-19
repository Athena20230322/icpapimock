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

    public class UserGroupRelationService
    {
        UserGroupRelationRepository _userGroupRelationRepository;

        public UserGroupRelationService(UserGroupRelationRepository userGroupRelationRepository)
        {
            _userGroupRelationRepository = userGroupRelationRepository;
        }

        public BaseResult AddUserGroupRelation(int UserGroupID, int UserID)
        {
            return _userGroupRelationRepository.AddUserGroupRelation(UserGroupID, UserID);
        }

        public BaseResult DeleteUserGroupRelation(int UserGroupID, int UserID)
        {
            return _userGroupRelationRepository.DeleteUserGroupRelation(UserGroupID, UserID);
        }
    }
}
