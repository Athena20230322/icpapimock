using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using ICP.Library.Models.MemberModels;
    using ICP.Infrastructure.Core.Frameworks.DbUtil;

    public class MemberInfoRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberInfoRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 檢查帳號是否唯一不重覆
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="MID"></param>
        /// <returns></returns>
        public BaseResult CheckUserCodeUnique(string UserCode, long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CheckUserCodeUnique_S";

            var args = new
            {
                UserCode,
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 檢查手機號碼是否可使用
        /// </summary>
        /// <param name="CellPhone"></param>
        /// <returns></returns>
        public BaseResult CheckCellPhone(string CellPhone, long MID = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CheckCellPhone_S";

            var args = new
            {
                CellPhone,
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <param name="MID">MID</param>
        /// <returns></returns>
        public MemberDataModel GetMemberData(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMember_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            var types = new Type[]
            {
                typeof(MemberBasicModel),
                typeof(MemberDetailModel)
            };

            var results = db.QueryMultiple(types, sql, args);

            var result = new MemberDataModel();
            result.basic = results[0].Cast<MemberBasicModel>().FirstOrDefault();
            result.detail = results[1].Cast<MemberDetailModel>().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 取得 APP Token資料
        /// </summary>
        /// <param name="MID">MID</param>
        /// <returns></returns>
        public MemberAppToken GetMemberAppTokenByMID(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppTokenByMID_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        public MemberLawBasicModel GetMemberLawBasic(int LevelID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Law_GetLawBasic_S";

            var args = new
            {
                LevelID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberLawBasicModel>(sql, args);
        }

        public MemberAppToken GetMemberAppToken(string OPMID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppToken_S";

            var args = new
            {
                OPMID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        /// <summary>
        /// 更新會員偏號設定
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="OptionType">設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)</param>
        /// <param name="OptionName">設定名稱</param>
        /// <param name="OptionValue">設定值</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateMemberPreference(long MID, byte OptionType, string OptionName, string OptionValue, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateMemberPreferences_IU";

            var args = new
            {
                MID,
                OptionType,
                OptionName,
                OptionValue,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得會員偏號設定
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="OptionType">設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)</param>
        /// <param name="OptionName">設定名稱</param>
        /// <returns></returns>
        public MemberPreferenceModel GetMemberPreference(long MID, byte OptionType, string OptionName)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMemberPreferences_S";

            var args = new
            {
                MID,
                OptionType,
                OptionName
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberPreferenceModel>(sql, args);
        }

        /// <summary>
        /// 取得會員偏號設定清單
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="OptionType">設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)</param>
        /// <param name="PageSize">分頁大小</param>
        /// <param name="PageIndex">分頁索引</param>
        /// <returns></returns>
        public List<MemberPreferenceModel> ListMemberPreferences(long MID, byte OptionType, int PageSize = 0, int PageIndex = 1)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberPreferences_S";

            var args = new
            {
                MID,
                OptionType,
                PageSize,
                PageIndex
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberPreferenceModel>(sql, args);
        }


        /// <summary>
        /// 取得同意事項結果
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="WebSiteType">顯示平台(&運算) 1:官網 2:廠商後台 4:APP 8:非必要顯示</param>
        /// <returns></returns>
        public List<MemberAgreeResult> ListMemberAgreeItem(long MID, int WebSiteType)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberAgreeItem_S";

            var args = new
            {
                MID,
                WebSiteType
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberAgreeResult>(sql, args);
        }

        /// <summary>
        /// 更新會員同意項目
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="AgreeType">同意項目編號</param>
        /// <param name="AgreeStatus">同意狀態 0:尚未同意 1:同意 2:不同意 4:略過</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateMemberAgreeResult(long MID, int AgreeType, byte AgreeStatus, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateMemberAgreeResult_IU";

            var args = new
            {
                MID,
                AgreeType,
                AgreeStatus,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public GetMemberLevelResult GetMemberLevelID(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMemberLevelIDByMID_SU";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<GetMemberLevelResult>(sql, args);
        }

        /// <summary>
        /// 更新OP帳號綁定記錄
        /// </summary>
        /// <param name="RecordID">記錄編號</param>
        /// <param name="Status">通知結果 1: 成功 2: 失敗</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult UpdateBindOPAccountNotifyRecord(long RecordID, byte Status)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OP_UpdateBindOPAccountNotifyRecord_U";

            var args = new
            {
                RecordID,
                Status
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddMemberBankAccount(MemberBankAccount model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_AddBankAccount_SI";

            var args = new
            {
                model.MID,
                model.Category,
                model.BankCode,
                model.BankBranchCode,
                model.BankAccount,
                model.INDTAccount,
                model.isDefault,
                model.AuthCategory,
                model.AuthType,
                model.PaperAuthStatus,
                model.FilePath1,
                model.FilePath2,
                model.CreateUser,
                model.Source
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateMemberBankAccountStatus(UpdateBankAccountStatusModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateBankAccountStatus_IU";

            var args = new
            {
                model.MID,
                model.Category,
                model.BankCode,
                model.BankAccount,
                model.AccountStatus,
                model.INDTAccount
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddBankTransferOnBankAccount(MemberBankAccount model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_AddBankTransferOnBankAccount_I";

            var args = new
            {
                model.MID,
                model.BankCode,
                model.BankAccount
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public List<MemberBankInfo> ListMemberBankInfo(long MID, byte? Category = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberBankInfo_S";

            var args = new
            {
                MID,
                Category
            };

            sql += db.GenerateParameter(args);

            return db.Query<MemberBankInfo>(sql, args);
        }

        public UserCoinsBalance GetUserCoinsBalance(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_GetUserCoinsBalance_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UserCoinsBalance>(sql, args);
        }

        /// <summary>
        /// 取得會員編號
        /// </summary>
        /// <param name="IDNO">身份證</param>
        /// <param name="ICPMID">電支帳號</param>
        /// <returns></returns>
        public DataResult<long> GetMID(string IDNO = null, string ICPMID = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMID_S";

            var args = new
            {
                IDNO,
                ICPMID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 檢查使用者IP是否被列為鎖定的IP黑名單
        /// </summary>
        /// <param name="RealIP"></param>
        /// <returns></returns>
        public BaseResult CheckIPBlackList(long IP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_CheckIP_BlackList_S";

            var args = new
            {
                IP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 檢查使用者IP是否被列為鎖定的IP黑名單
        /// </summary>
        /// <param name="CellPhone"></param>
        /// <returns></returns>
        public BaseResult CheckOTPBlackList(string CellPhone)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_CheckOTP_BlackList_S";

            var args = new
            {
                CellPhone
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得會員編號
        /// </summary>
        /// <param name="IDNO">身份證</param>
        /// <param name="ICPMID">電支帳號</param>
        /// <returns></returns>
        public DataResult<long> GetMID(string IDNO = null, string ICPMID = null, string Account = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMID_S";

            var args = new
            {
                IDNO,
                ICPMID,
                Account
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 取得會員狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public byte GetMemberStatus(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetMemberStatus_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<byte>(sql, args);
        }

        /// <summary>
        /// 檢查裝置ID是否是黑名單
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public BaseResult CheckMemberDeviceStatus(string deviceId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_Member_DeviceIDBlock_GetMemberDeviceStatus_S";

            var args = new
            {
                deviceId
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得縣市, 區
        /// </summary>
        /// <param name="AreaID">區域代碼</param>
        /// <returns></returns>
        public List<ZipCodeModel> ListZipCodeArea(string AreaID = "")
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_Address_ListZipCodeArea_S";
            var args = new { AreaID };
            sql += db.GenerateParameter(args);
            return db.Query<ZipCodeModel>(sql, args);
        }

        /// <summary>
        /// 取得職業
        /// </summary>
        /// <returns></returns>
        public List<OccupationModel> ListOccupation()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_MemberInfo_ListOccupation_S";
            return db.Query<OccupationModel>(sql);
        }

        /// <summary>
        /// 取得國家
        /// </summary>
        /// <returns></returns>
        public List<NationalityModel> ListNationality()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_MemberInfo_ListNationality_S";
            return db.Query<NationalityModel>(sql);
        }
    }
}
