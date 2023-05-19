using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class CustomServiceRecordRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db = null;

        public CustomServiceRecordRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }
        /// <summary>
        /// 查詢客服紀錄清單
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        public List<QueryCustomServiceRecordResult> ListCustomServiceRecords(QueryCustomServiceRecord query)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_CustomerService_ListRecord_S";
            sql += db.GenerateParameter(query);
            return db.Query<QueryCustomServiceRecordResult>(sql, query);
        }
        /// <summary>
        /// 查詢客服紀錄
        /// </summary>
        /// <param name="CustomerServiceID">PK 紀錄編號流水號</param>
        /// <returns></returns>
        public QueryCustomServiceRecordResult GetCustomServiceRecord(long CustomerServiceID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_CustomerService_GetRecord_S";
            var args = new
            {
                CustomerServiceID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<QueryCustomServiceRecordResult>(sql, args);
        }
        /// <summary>
        /// 取得客服紀錄查詢設定檔清單
        /// </summary>
        /// <param name="Type">管道 1(問題類別)/2(進項管道) </param>
        /// <returns></returns>
        public List<SettingOptions> ListSettingOptions(byte Type)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_CustomerService_ListRecordSettingOptions_S";
            var args = new
            {
                Type
            };
            sql += db.GenerateParameter(args);
            return db.Query<SettingOptions>(sql, args);
        }
        /// <summary>
        /// 查詢某筆案件的所有紀錄內容
        /// </summary>
        /// <param name="CustomerServiceID">PK 案件紀錄編號</param>
        /// <returns></returns>
        public List<CustomerServiceRecordNote> ListCustomServiceRecordLogs(long CustomerServiceID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Admin_Share_ListCustomerServiceRecordLogBytCustomerServiceID_S";
            var args = new
            {
                CustomerServiceID
            };
            sql += db.GenerateParameter(args);
            return db.Query<CustomerServiceRecordNote>(sql, args);
        }
        /// <summary>
        /// 新增客服紀錄內容
        /// </summary>
        /// <param name="model">新增物件</param>
        /// <returns></returns>
        public BaseResult AddCustomServiceRecord(AddCustomServiceRecord model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_CustomerService_AddRecord_I";
            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        /// <summary>
        /// 新增客服紀錄查詢設定檔內容
        /// </summary>
        /// <param name="ID">設定編號</param>
        /// <param name="Type">管道 1(問題類別)/2(進項管道) </param>
        /// <param name="Description">設定顯式名稱</param>
        /// <returns></returns>
        public BaseResult AddCustomServiceRecordSetting(byte ID, byte Type, string Description)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_CustomerService_AddRecordSetting_I";
            var args = new
            {
                ID,
                Type,
                Description
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        /// <summary>
        /// 更新客服紀錄內容
        /// </summary>
        /// <param name="CustomerServiceID">PK 紀錄編號流水號</param>
        /// <param name="Status">案件進度 : 0建立案件 1 客服處理 2客服更改處理結果</param>
        /// <param name="Note">紀錄內容</param>
        /// <param name="Modifier">最後修改人員</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateCustomServiceRecord(long CustomerServiceID, byte Status, string Note, string Modifier, long RealIP, long ProxyIP)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_Admin_CustomerService_UpdateRecord_SIU";
            var args = new
            {
                CustomerServiceID,
                Status,
                Note,
                Modifier,
                RealIP,
                ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        /// <summary>
        /// 更新客服紀錄查詢設定檔內容
        /// </summary>
        /// <param name="ID">設定編號(PK 用來尋找用不能變更)</param>
        /// <param name="Type">管道 1(問題類別)/2(進項管道) </param>
        /// <param name="Description">設定顯式名稱</param>
        /// <returns></returns>
        public BaseResult UpdateCustomServiceRecordSetting(byte ID, byte Type, string Description)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Share_CustomerService_UpdateRecordSetting_U";
            var args = new
            {
                ID,
                Type,
                Description
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        
    }
}
