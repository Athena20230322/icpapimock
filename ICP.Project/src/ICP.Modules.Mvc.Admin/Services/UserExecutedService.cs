using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Modules.Mvc.Admin.Repositories;
    using Models;
    using System.Collections.Specialized;
    using Newtonsoft.Json;

    public class UserExecutedService
    {
        
        private readonly UserExecutedRepository _repository = null;

        public UserExecutedService(UserExecutedRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 新增使用者操作記錄
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public void AddActionLog(UserExecuted AddModel)
        {
            _repository.AddActionLog(AddModel);
        }


        /// <summary>
        /// 將NameValueCollection轉換為Json
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ignoreKey"></param>
        /// <returns></returns>
        public string ToDictionary(NameValueCollection list, params string[] ignoreKey)
        {
            var dty = list.AllKeys.Where(k => !string.IsNullOrWhiteSpace(k) &&
                                              !ignoreKey.Any(x => k == x) &&
                                              !ignoreKey.Any(x => k.ToLower().IndexOf(x) > 0) &&
                                              list[k] != null)
                                  .ToDictionary(k => k, k => list[k]);

            return JsonConvert.SerializeObject(dty);
        }
    }
}
