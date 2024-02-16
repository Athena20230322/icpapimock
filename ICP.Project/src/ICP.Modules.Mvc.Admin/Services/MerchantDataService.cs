using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using ICP.Infrastructure.Abstractions.Logging;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Helpers;
    using ICP.Infrastructure.Core.Models;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using Models.MerchantModels;
    using Repositories;
    using System.Linq.Expressions;

    public class MerchantDataService
    {
        #region 建構、倉儲
        ILogger _logger;
        MerchantDataRepository _merchantDataRepository;

        public MerchantDataService(
            ILogger<MerchantDataService> logger,
            MerchantDataRepository merchantDataRepository
            
            )
        {
            _logger = logger;
            _merchantDataRepository = merchantDataRepository;
        }
        #endregion

        /// <summary>
        /// 取得MCCCode
        /// </summary>
        /// <returns></returns>
        public List<MerchantCategory> ListMerchantCategoryCode() => _merchantDataRepository.ListMerchantCategoryCode();

        /// <summary>
        /// 取得審核狀態
        /// </summary>
        /// <param name="CustomerStatus">0: 未過件, 1: 已過件</param>
        /// <returns></returns>
        public List<AuditStatusModel> ListAuditStatus(byte CustomerStatus) => _merchantDataRepository.ListAuditStatus(CustomerStatus);

        /// <summary>
        /// 取得下一步審核狀態
        /// </summary>
        /// <param name="AuditStatusID">當前審核狀態</param>
        /// <param name="Permission">權限</param>
        /// <param name="CustomerStatus">0: 未過件, 1: 已過件</param>
        /// <returns></returns>
        public List<AuditStatusModel> ListNextAuditStatus(byte AuditStatusID, int Permission, byte CustomerStatus) => _merchantDataRepository.ListNextAuditStatus(AuditStatusID, Permission, CustomerStatus);

        /// <summary>
        /// 更新 歸檔編號
        /// </summary>
        /// <returns></returns>
        public BaseResult UpdateCustomerArchivingNo(long CustomerID, string ArchivingNo, string Modifier, long RealIP = 0, long ProxyIP = 0)
            => _merchantDataRepository.UpdateCustomerArchivingNo(CustomerID, ArchivingNo, Modifier, RealIP, ProxyIP);

        /// <summary>
        /// 新增 特店記錄
        /// </summary>
        /// <returns></returns>
        public BaseResult AddCustomerMemo(long CustomerID, string MemoNote, string Modifier, long RealIP = 0, long ProxyIP = 0)
            => _merchantDataRepository.AddCustomerMemo(CustomerID, MemoNote, Modifier, RealIP, ProxyIP);

        /// <summary>
        /// 取得 特店資料
        /// </summary>
        /// <returns></returns>
        public CustomerDataModel GetCustomerData(long CustomerID, byte? CustomerStatus = null)
            => _merchantDataRepository.GetCustomerData(CustomerID, CustomerStatus);

        /// <summary>
        /// 查詢 特店資料
        /// </summary>
        /// <param name="query">查詢條件</param>
        /// <returns></returns>
        public List<CustomerDataQueryResult> ListCustomerData(CustomerDataQueryModel query)
            => _merchantDataRepository.ListCustomerData(query);

        /// <summary>
        /// 取得忽略屬性欄位
        /// </summary>
        /// <param name="model"></param>
        /// <param name="FieldType">欄位類型 0: PropName, 1: MemberName</param>
        /// <returns></returns>
        public List<string> GetIgnoreFields(CustomerDataModel model, byte FieldType = 0)
        {
            const long NationalityID_TW = 1206;

            var propHelper = new PropertyHelper<CustomerDataModel>();

            var exprs = new List<Expression<Func<CustomerDataModel, dynamic>>>();

            Func<Expression<Func<CustomerDataModel, dynamic>>[], List<string>> func;

            if (FieldType == 0)
            {
                func = propHelper.GetPropNames;
            }
            else
            {
                func = propHelper.GetMemberNames;
            }

            //提領規則 0:不啟用
            if (model.detail.TransferSchedule == false)
            {
                exprs.Add(t => t.detail.ScheduleType);
                exprs.Add(t => t.detail.AMTransferType);
                exprs.Add(t => t.detail.TransferAmount);
                exprs.Add(t => t.detail.ScheduleValue);
            }
            else if (model.detail.ScheduleType == 1) {
                exprs.Add(t => t.detail.ScheduleValue);
            }

            //特店身份 1:公司戶
            if (model.basic.CustomerType == 1)
            {
                //負責人類型 0:自然人
                if (model.detail.PrincipalType == 0)
                {
                    exprs.Add(t => t.detail.PrincipalCompanyName);
                    exprs.Add(t => t.detail.PrincipalUnifiedBusinessNo);
                }
                //負責人類型 1:法人
                else if (model.detail.PrincipalType == 1)
                {
                    exprs.Add(t => t.detail.NationalityID);
                    exprs.Add(t => t.detail.IDNO);
                    exprs.Add(t => t.detail.UniformID);
                    exprs.Add(t => t.detail.OverSeasIDNO);
                    exprs.Add(t => t.detail.CName);
                    exprs.Add(t => t.detail.CName_EN);
                    exprs.Add(t => t.detail.PrincipalBirthday);
                    exprs.Add(t => t.detail.OccupationID);
                }
            }
            //特店身份 2:個人戶
            else if (model.basic.CustomerType == 2)
            {
                exprs.Add(t => t.detail.CompanyName);
                exprs.Add(t => t.detail.CompanyType);
                exprs.Add(t => t.detail.UnifiedBusinessNo);
                exprs.Add(t => t.detail.IncorporationDate);
                exprs.Add(t => t.detail.CapitalAmount);
                exprs.Add(t => t.detail.PrincipalCompanyName);
                exprs.Add(t => t.detail.PrincipalUnifiedBusinessNo);
            }

            //國籍 1206 台灣
            if (model.detail.NationalityID == NationalityID_TW)
            {
                exprs.Add(t => t.detail.UniformID);
                exprs.Add(t => t.detail.OverSeasIDNO);
            }
            //國籍 外國
            else if (model.detail.NationalityID > 0)
            {
                exprs.Add(t => t.detail.IDNO);
            }

            var fields = func(exprs.ToArray());

            return fields.Distinct().ToList();
        }

        /// <summary>
        /// 整理特店資料
        /// </summary>
        /// <param name="model"></param>
        public void SetCustomerData(ref CustomerDataModel model)
        {
            const long NationalityID_TW = 1206;

            //提領規則 0:不啟用
            if (model.detail.TransferSchedule == false)
            {
                model.detail.ScheduleType = 0;
                model.detail.AMTransferType = 0;
                model.detail.TransferAmount = 0;
                model.detail.ScheduleValue = 0;
            }
            else if (model.detail.ScheduleType == 1)
            {
                model.detail.ScheduleValue = 0;
            }

            //特店身份 1:公司戶
            if (model.basic.CustomerType == 1)
            {
                //負責人類型 0:自然人
                if (model.detail.PrincipalType == 0)
                {
                    model.detail.PrincipalCompanyName = string.Empty;
                    model.detail.PrincipalUnifiedBusinessNo = string.Empty;
                }
                //負責人類型 1:法人
                else if (model.detail.PrincipalType == 1)
                {
                    model.detail.NationalityID = 0;
                    model.detail.IDNO = string.Empty;
                    model.detail.UniformID = string.Empty;
                    model.detail.OverSeasIDNO = string.Empty;
                    model.detail.CName = string.Empty;
                    model.detail.CName_EN = string.Empty;
                    model.detail.PrincipalBirthday = null;
                    model.detail.OccupationID = 0;
                }
            }
            //特店身份 2:個人戶
            else if (model.basic.CustomerType == 2)
            {
                model.detail.CompanyName = string.Empty;
                model.detail.CompanyType = 0;
                model.detail.UnifiedBusinessNo = string.Empty;
                model.detail.IncorporationDate = null;
                model.detail.CapitalAmount = 0;
                model.detail.PrincipalCompanyName = string.Empty;
                model.detail.PrincipalUnifiedBusinessNo = string.Empty;
            }

            //國籍 1206 台灣
            if (model.detail.NationalityID == NationalityID_TW)
            {
                model.detail.UniformID = string.Empty;
                model.detail.OverSeasIDNO = string.Empty;
            }
            //國籍 外國
            else if (model.detail.NationalityID > 0)
            {
                model.detail.IDNO = string.Empty;
            }
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

            long CustomerID = 0;

            try
            {
                var addResult = _merchantDataRepository.AddCustomerData(model, Creator, RealIP, ProxyIP);
                if (!addResult.IsSuccess)
                {
                    result.SetError(addResult);
                    return result;
                }

                CustomerID = addResult.RtnData;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AddCustomerData");
                return result;
            }
            
            result.SetSuccess(CustomerID);
            return result;
        }
    }
}