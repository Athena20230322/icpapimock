using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Helpers;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Services.MemberServices;
    using ICP.Modules.Mvc.Admin.Models.MerchantModels;
    using Models.ViewModels;
    using Services;
    using System.Linq.Expressions;

    public class MerchantDataCommand
    {
        MerchantDataService _merchantDataService;
        LibMemberInfoService _libMemberInfoService;

        public MerchantDataCommand(
            MerchantDataService merchantDataService,
            LibMemberInfoService libMemberInfoService
            )
        {
            _merchantDataService = merchantDataService;
            _libMemberInfoService = libMemberInfoService;
        }

        /// <summary>
        /// 查詢未過件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<CustomerDataQueryResult> ListUnFinishedData(CustomerDataQueryModel query)
        {
            query.CustomerStatus = 0;

            return _merchantDataService.ListCustomerData(query);
        }

        /// <summary>
        /// 查詢已過件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<CustomerDataQueryResult> ListFinishedData(CustomerDataQueryModel query)
        {
            query.CustomerStatus = 1;

            return _merchantDataService.ListCustomerData(query);
        }

        /// <summary>
        /// 新增 特店資料
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Creator"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public DataResult<long> AddCustomerData(CustomerDataModel model, string Creator, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<long>();
            result.SetError();

            // 檢查格式
            var ignoreFields = _merchantDataService.GetIgnoreFields(model, FieldType: 1);
            if (model.IsValid(ignoreFields.ToArray()))
            {
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            // 檢查帳號
            var checkAccountResult = _libMemberInfoService.CheckUserCodeUnique(model.basic.Account, model.basic.MID);
            if (!checkAccountResult.IsSuccess)
            {
                result.SetError(checkAccountResult);
                return result;
            }

            // 整裡資料
            _merchantDataService.SetCustomerData(ref model);

            // 新增特店
            var addResult = _merchantDataService.AddCustomerData(model, Creator, RealIP, ProxyIP);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            result.SetSuccess(addResult.RtnData);
            return result;
        }
    }
}