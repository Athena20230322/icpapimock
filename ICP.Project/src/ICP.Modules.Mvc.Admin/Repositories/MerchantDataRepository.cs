using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using ICP.Modules.Mvc.Admin.Models.MerchantModels;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;

    /// <summary>
    /// 廠商資料倉儲
    /// </summary>
    public class MerchantDataRepository
    {
        #region 建構、連線
        private readonly IDbConnectionPool _dbConnectionPool = null;
        
        private IDbConnection db;
        
        public MerchantDataRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }
        #endregion

        /// <summary>
        /// 取得MCCCode
        /// </summary>
        /// <returns></returns>
        public List<MerchantCategory> ListMerchantCategoryCode()
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_ListMerchantCategoryCode_S";
            return db.Query<MerchantCategory>(sql);
        }

        /// <summary>
        /// 取得審核狀態
        /// </summary>
        /// <param name="CustomerStatus">0: 未過件, 1: 已過件</param>
        /// <returns></returns>
        public List<AuditStatusModel> ListAuditStatus(byte CustomerStatus)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_ListAuditStatus_S";
            var args = new { CustomerStatus };
            sql += db.GenerateParameter(args);
            return db.Query<AuditStatusModel>(sql, args);
        }


        /// <summary>
        /// 取得下一步審核狀態
        /// </summary>
        /// <param name="AuditStatusID">當前審核狀態</param>
        /// <param name="Permission">權限</param>
        /// <param name="CustomerStatus">0: 未過件, 1: 已過件</param>
        /// <returns></returns>
        public List<AuditStatusModel> ListNextAuditStatus(byte AuditStatusID, int Permission, byte CustomerStatus)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_ListNextAuditStatus_S";
            var args = new { AuditStatusID, Permission, CustomerStatus };
            sql += db.GenerateParameter(args);
            return db.Query<AuditStatusModel>(sql, args);
        }

        /// <summary>
        /// 更新 歸檔編號
        /// </summary>
        /// <returns></returns>
        public BaseResult UpdateCustomerArchivingNo(long CustomerID, string ArchivingNo, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_UpdateCustomerArchivingNo_IU";
            var args = new { CustomerID, ArchivingNo, Modifier, RealIP, ProxyIP };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 新增 特店記錄
        /// </summary>
        /// <returns></returns>
        public BaseResult AddCustomerMemo(long CustomerID, string MemoNote, string Modifier, long RealIP = 0, long ProxyIP = 0)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Admin_Merchant_AddCustomerMemo_I";
            var args = new { CustomerID, MemoNote, Modifier, RealIP, ProxyIP };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得 特店資料
        /// </summary>
        /// <returns></returns>
        public CustomerDataModel GetCustomerData(long CustomerID, byte? CustomerStatus = null)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_GetCustomerData_S";
            var args = new { CustomerID, CustomerStatus };
            sql += db.GenerateParameter(args);

            var types = new Type[]
{
                typeof(List<CustomerBasic>),
                typeof(List<CustomerDetailModel>),
                typeof(List<CustomerContractModel>),
                typeof(List<CustomerAllocateDayModel>)
            };

            var results = db.QueryMultiple(types, sql, args);
            var result = new CustomerDataModel();
            result.basic = results[0].Cast<CustomerBasic>().FirstOrDefault();
            result.detail = results[0].Cast<CustomerDetailModel>().FirstOrDefault();
            result.contract = results[0].Cast<CustomerContractModel>().FirstOrDefault();
            result.allocateDays = results[0].Cast<CustomerAllocateDayModel>().ToList();
            return result;
        }

        /// <summary>
        /// 查詢 特店資料
        /// </summary>
        /// <param name="query">查詢條件</param>
        /// <returns></returns>
        public List<CustomerDataQueryResult> ListCustomerData(CustomerDataQueryModel query)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_ListCustomerData_S";
            sql += db.GenerateParameter(query);
            return db.Query<CustomerDataQueryResult>(sql, query);
        }

        /// <summary>
        /// 新增 特店資料
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Creator"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public DataResult<long> AddCustomerData(CustomerDataModel model, string Creator, long RealIP, long ProxyIP)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_Merchant_AddCustomerData_I";
            var args = new
            {

            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
    }
}