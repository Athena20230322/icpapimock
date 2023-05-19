using System.Collections.Generic;
using System.Data.SqlClient;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MerchantModels;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class MemberDeviceIdManageRepository
    {
         IDbConnectionPool _dbConnectionPool;

         public MemberDeviceIdManageRepository(IDbConnectionPool dbConnectionPool)
         {
             _dbConnectionPool = dbConnectionPool;
         }

         /// <summary>
         /// 查詢 裝置ID封鎖列表
         /// </summary>
         /// <param name="model"></param>
         /// <returns></returns>
         public List<MemberDeviceIdModel> MemberDeviceIdManageList(MemberDeviceIdQuery model)
         {
             var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
             string sql = "EXEC ausp_Admin_Member_DeviceIDBlock_ListMemberDevice_S";
             var args =new 
             {
                 PageNo=model.PageNo,
                 PageSize=model.PageSize,
                 CreateDateBegin=model.CreateDateBegin,
                 CreateDateEnd=model.CreateDateEnd,
                 Status=model.Status,
                 DeviceID=model.DeviceID
             };
             sql += db.GenerateParameter(args);

             return db.Query<MemberDeviceIdModel>(sql, args);
         }

         /// <summary>
         /// 增加 裝置ID封鎖
         /// </summary>
         /// <param name="deviceId"></param>
         /// <param name="status"></param>
         /// <param name="user"></param>
         /// <param name="memo"></param>
         /// <returns></returns>
         public BaseResult AddMemberDeviceId(string deviceId,int status,string user,string memo)
         {
             var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
             string sql = "EXEC ausp_Admin_Member_DeviceIDBlock_AddMemberDevice_IU";
             var args = new
             {
                 DeviceID=deviceId,
                 Status=status,
                 User=user,
                 Memo=memo

             };
             sql += db.GenerateParameter(args);
             return db.QuerySingleOrDefault<BaseResult>(sql, args);
         }

         /// <summary>
         /// 查詢 裝置ID封鎖歷程
         /// </summary>
         /// <param name="deviceId"></param>
         /// <returns></returns>
         public List<MemberDeviceIdVM> MemberDeviceIdVmList(string deviceId)
         {
             var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
             string sql = "EXEC ausp_Admin_AddMember_ListDeviceIDBlockLog_S";
             var args = new
             {
                 DeviceID=deviceId
             };

             sql += db.GenerateParameter(args);
             return db.Query<MemberDeviceIdVM>(sql, args);
         }

         /// <summary>
         /// 新增 裝置ID修改歷程
         /// </summary>
         /// <param name="deviceId"></param>
         /// <param name="user"></param>
         /// <param name="realIp"></param>
         /// <param name="proxyIp"></param>
         public void AddMemberDeviceIdLog(string deviceId, string user, long realIp, long proxyIp)
         {
             var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
             string sql = "EXEC ausp_Admin_AddMember_DeviceIDBlockLog_I";
             var args = new
             {
                 DeviceID=deviceId,
                 CreateUser = user,
                 RealIP=realIp,
                 ProxyIP=proxyIp
             };

             sql += db.GenerateParameter(args);
             db.QuerySingleOrDefault<BaseResult>(sql,args);
         }
    }
}