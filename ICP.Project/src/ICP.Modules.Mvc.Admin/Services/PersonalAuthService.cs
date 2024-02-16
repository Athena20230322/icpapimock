using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models.MemberModels;
    using Models.ViewModels;
    using ICP.Infrastructure.Core.Web.Helpers;
    using System.Web;
    using System.Linq.Expressions;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Abstractions.Logging;
    using Newtonsoft.Json;

    public class PersonalAuthService
    {
        PersonalAuthRepository _personalAuthRepository;
        SaveFileHelper _saveFileHelper;
        ILogger<PersonalAuthService> _logger;

        public PersonalAuthService(
            PersonalAuthRepository personalAuthRepository,
            ILogger<PersonalAuthService> logger
            )
        {
            _personalAuthRepository = personalAuthRepository;
            _saveFileHelper = new SaveFileHelper();
            _logger = logger;
        }

        /// <summary>
        /// P11驗證記錄查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<P11AuthHistoryQuery> QueryP11AuthLog(P11AuthHistory model)
        {
            return _personalAuthRepository.QueryP11AuthLog(model);
        }

        /// <summary>
        /// P11驗證記錄查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<P33AuthHistoryQuery> QueryP33AuthLog(P33AuthHistory model)
        {
            return _personalAuthRepository.QueryP33AuthLog(model);
        }

        /// <summary>
        /// P33查詢 聯徵回傳的Json資料列表序列化
        /// </summary>
        /// <param name="DataListString">要序列化的json字串</param>
        /// <returns></returns>
        public List<P33AuthData> DataListJsonStringDeserialize(string DataListString)
        {
            List<P33AuthData> results = new List<P33AuthData>();
            if (string.IsNullOrWhiteSpace(DataListString))
                return results;
            try
            {
                results = JsonConvert.DeserializeObject<List<P33AuthData>>(DataListString);
            }
            catch { }
            if (results == null)
                results = new List<P33AuthData>();
            return results;
        }
    }
}
