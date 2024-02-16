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

    public class UserGroupRelationCommand
    {
        UserGroupRelationService _userGroupRelationService;
        UserService _userService;

        public UserGroupRelationCommand(UserGroupRelationService userGroupRelationService, UserService userService)
        {
            _userGroupRelationService = userGroupRelationService;
            _userService = userService;
        }

        public BaseResult AddUserGroupRelation(int UserGroupID, int UserID)
        {
            return _userGroupRelationService.AddUserGroupRelation(UserGroupID, UserID);
        }

        public BaseResult DeleteUserGroupRelation(int UserGroupID, int UserID)
        {
            return _userGroupRelationService.DeleteUserGroupRelation(UserGroupID, UserID);
        }

        public BaseResult BatchAddUserGroupRelation(int UserGroupID, List<int> UserIDs)
        {
            BaseResult result = null;

            if (UserIDs == null || UserIDs.Count == 0)
            {
                result = new BaseResult
                {
                    RtnMsg = "請選擇使用者"
                };
                return result;
            }

            foreach (int UserID in UserIDs)
            {
                result = _userGroupRelationService.AddUserGroupRelation(UserGroupID, UserID);

                if (!result.IsSuccess)
                {
                    break;
                }
            }

            return result;
        }

        public List<UserQueryResult> QueryNotJoinUsersByDeptID(int UserGroupID, int DeptID)
        {
            var query = new UserQuery
            {
                PageSize = int.MaxValue,
                DeptID = DeptID
            };
            var users = _userService.ListUser(query);
            var groupUsers = _userService.ListUserByGroup(UserGroupID);
            var list = users.Where(t => !groupUsers.Any(t2 => t2.UserID == t.UserID)).ToList();

            return list;
        }
    }
}
