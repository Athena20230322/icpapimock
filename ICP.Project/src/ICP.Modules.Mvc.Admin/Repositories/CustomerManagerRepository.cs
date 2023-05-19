using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using ICP.Library.Models.MemberModels;
    using ICP.Modules.Mvc.Admin.Models.CustomerManager;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;

    public class CustomerManagerRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public CustomerManagerRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        #region 會員資料列表查詢
        public List<QueryMemberResultVM> ListMember(QueryMemberVM query)
        {
            string sql = "EXEC ausp_Admin_MemberInfo_ListMember_S";
            sql += db.GenerateParameter(query);
            return db.Query<QueryMemberResultVM>(sql, query);
        }

        #endregion

        //#region 後台的會員資料明細查詢(銀行帳戶)
        //public List<MemberBankAccountVM> ListMemberOnBankAccount(long MID)
        //{

        //    string sql = "ausp_Member_Admin_MemberInfo_ListMemberOnBankAccount_S";

        //    var args = new
        //    {
        //        MID
        //    };

        //    sql += db.GenerateParameter(args);
        //    return db.Query<MemberBankAccountVM>(sql, args);

        //}

        //#endregion
               
        public MemberVerifyStatus GetMemberVerifyStatus(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetVerifyStatus_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberVerifyStatus>(sql, args);
        }

        #region 取得會員身分證換補發驗證 (P11)
        public MemberAuthIDNOVM GetMemberAuthIDNO(long MID, int exceptOverSea = 0)
        {
            string sql = "EXEC ausp_Member_Admin_GetMemberAuthIDNO_S";

            var args = new
            {
                MID,
                exceptOverSea
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<MemberAuthIDNOVM>(sql, args);
        }
        #endregion

        #region 取出 OTP 當日發送次數
        public int GetOTPCount(string CellPhone)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);

            string sql = "EXEC ausp_Statistics_Admin_MemberAuth_GetAuthSMSCount_S";

            var args = new
            {
                CellPhone
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }
        #endregion

        #region 姓名修改記錄        
        public List<AuthCNameListLogVM> ListAuthCNameListLog(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListAuthCNameListLog_S";

            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.Query<AuthCNameListLogVM>(sql, args);
        }
        #endregion

        #region 手機號碼修改記錄        
        public List<AuthCellPhoneListLogVM> ListAuthCellPhoneListLog(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListAuthCellPhoneListLog_S";

            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.Query<AuthCellPhoneListLogVM>(sql, args);
        }
        #endregion

        #region 電支使用者升級歷程     
        public List<AuthMemberUpgradeListLogVM> ListMemberUpgradeListLog(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListMemberUpgradeListLog_S";

            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.Query<AuthMemberUpgradeListLogVM>(sql, args);
        }
        #endregion

        #region 簡訊解鎖記錄     
        public List<UnLockSMSLogModel> ListUnlockSMSListLog(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListUnlockSMSListLog_S";

            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.Query<UnLockSMSLogModel>(sql, args);
        }
        #endregion

        
        #region 簡訊解鎖     
        public BaseResult UpdateUnLockSMS(long MID, string ModifyUser)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            
            string sql = "EXEC ausp_Admin_Statistics_MemberInfo_UpdateAuthSMSCounts_IU";

            var args = new
            {
                MID,
                ModifyUser
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 手機號碼修改

        public BaseResult UpdateCellPhone(EditCellPhoneModel model)
        {     
            string sql = "EXEC ausp_Admin_MemberInfo_CheckUpdateMemberCellPhone_U";
                       
            var args = new
            {
                model.MID,
                model.CellPhone,
                model.Remark,
                model.ModifyUser,
                model.ProxyIP,
                model.RealIP
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion


        #region 凍結款項列表
        public List<FreezeCoinsModel> ListFreezeCoins(QueryFreezeCoinsModel model = null)
        {
            if (model == null)
            {
                model = new QueryFreezeCoinsModel
                {
                    PageNo = 1,
                    PageSize = 10
                };
            }

            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Admin_Coins_GetFreezeCoins_S";
                        
            sql += db.GenerateParameter(model);

            return db.Query<FreezeCoinsModel>(sql, model);

        }
        #endregion

        #region 凍結款項明細
        public List<FreezeCoinsModel> ListFreezeCoinsLog(long MID, long FreezeID)
        {            

            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Admin_Coins_GetFreezeLog_S";

            var args = new
            {
                MID,
                FreezeID
            };

            sql += db.GenerateParameter(args);

            return db.Query<FreezeCoinsModel>(sql, args);

        }
        #endregion

        #region 新增凍結款項
        public BaseResult AddFreezeCoins(AddFreezeCoinsModel model)
        {

            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Admin_Coins_AddFreezeCash_IU";

            var args = new
            {
                model.MID,
                model.Status,
                model.Remark,
                model.Creator,
                model.FreezeCash
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }
        #endregion

        #region 查詢待提領帳戶餘額(指定)
        public decimal GetUserCoinsOnBalanceByType(long MID, int Type = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_GetUserCoinsOnBalanceByType_S";

            var args = new
            {
                MID,
                Type
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<decimal>(sql, args);
        }
        #endregion

        #region 返還金額/解除凍結金額
        public BaseResult UpdateFreezeCoinsStatus(FreezeCoinsModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Admin_Coins_UpdateFreezeCash_U";

            var args = new
            {
                model.MID,
                model.FreezeCash,
                model.FreezeID,
                model.Status,
                model.Remark,
                model.Creator,
                model.RtnICPMID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 海外會員相關

        #region 海外會員資料列表
        public List<ListOverSeaUserResultModel> ListOverSeaUser(ListOverSeaUserQryVM model = null)
        {
            if (model == null)
            {
                model = new ListOverSeaUserQryVM
                {
                    PageNo = 1,
                    PageSize = 20
                };
            }

            string sql = "EXEC ausp_Admin_MemberAuth_ListAuthOverSeaMember_S";
                       
            sql += db.GenerateParameter(model);

            return db.Query<ListOverSeaUserResultModel>(sql, model);
        }
        #endregion

        #region 驗證資料是否重複
        public BaseResult VaildOverSeaMemberDataModel(AddOverSeaMemberDataModel model)
        {
            string sql = "EXEC ausp_Admin_MemberInfo_CheckOverSeaMemberData_S";

            var args = new
            {
                model.CellPhone,
                model.OPMID,
                model.Email,
                model.UniformID,
                model.BankCode,
                model.BankBranchCode,
                model.Account
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 匯入資料建立帳號
        public BaseResult AddOverSeaMemberData(AddOverSeaMemberDataModel model)
        {
            string sql = "EXEC ausp_Admin_MemberInfo_AddOverSeaMemberData_IU";

            var args = new
            {
                model.CellPhone,
                model.CName,
                model.NationalID,
                model.AreaID,
                model.Address,
                model.OPMID,
                model.Email,
                model.CreateUser,
                model.Account,
                model.Pwd,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 新增海外會員P11資料
        public BaseResult AddOverSeaMemberP11(AddOverSeaP11Model model)
        {
            string sql = "EXEC ausp_Member_MemberAuth_AddAuthUniformID_SIU";                       

            sql += db.GenerateParameter(model);

            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        #endregion

        #region 上傳證件圖檔
        public BaseResult OverSeaFileUpload(OverSeaFileUploadModel model)
        {
            string sql = "EXEC ausp_Admin_MemberInfo_UpdateOverSeaFilPath_U";

            sql += db.GenerateParameter(model);

            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

        #endregion

        #region 身份驗證確認
        public BaseResult UpdateUniformIDStatus(UpdateUniformIDStatusModel model)
        {
            string sql = "EXEC ausp_Admin_MemberAuth_UpdateOverSeaIDNOPass_IU";

            sql += db.GenerateParameter(model);

            return db.QuerySingleOrDefault<BaseResult>(sql, model);

        }
        #endregion

        #region 記錄匯入資料Log
        public BaseResult AddMemberForeignBasicLog(AddOverSeaMemberDataModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Member_MemberInfo_AddMemberForeignBasicLog_I";

            var args = new
            {
                model.OPMID,
                model.MID,
                model.CellPhone,
                model.Account,
                model.Pwd,
                model.CName,
                model.Email,
                model.NationalID,
                model.UniformID,
                model.UniformIssueDate,
                model.UniformExpireDate,
                model.UniformNumber,
                model.BankCode,
                model.BankBranchCode,
                model.BankAccount,
                model.Status,
                model.BatchNo,
                model.CreateUser
            };


            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }

        #endregion


        #endregion

        #region 取得AreaID
        public CountryTownIDModel GetAreaID(string ZipCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetCountyTownID_S";

            var args = new
            {
                ZipCode
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<CountryTownIDModel>(sql, args);

        }
        #endregion

    }
}
