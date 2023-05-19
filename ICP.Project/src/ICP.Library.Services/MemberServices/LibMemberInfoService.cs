using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    using Library.Models.MemberModels;
    using Infrastructure.Core.Models;
    using Library.Repositories.MemberRepositories;
    using Infrastructure.Core.Extensions;
    using System.Text.RegularExpressions;
    using ICP.Infrastructure.Core.Models.Consts;

    public class LibMemberInfoService : LibMemberInfoCommonService
    {
        MemberConfigCyptRepository _configCyptRepository;
        MemberInfoRepository _memberInfoRepository;

        public LibMemberInfoService(
            MemberConfigCyptRepository configCyptRepository,
            MemberInfoRepository memberInfoRepository
            )
        {
            _configCyptRepository = configCyptRepository;
            _memberInfoRepository = memberInfoRepository;
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <param name="MID">MID</param>
        /// <returns></returns>
        public MemberDataModel GetMemberData(long MID)
        {
            var result = _memberInfoRepository.GetMemberData(MID);

            result.basic.Account = _configCyptRepository.Decrypt_UserCode(result.basic.Account);

            return result;
        }

        /// <summary>
        /// 取得 APP Token資料
        /// </summary>
        /// <param name="MID">MID</param>
        /// <returns></returns>
        public DataResult<MemberAppToken> GetAppTokenByMID(long MID)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            if (MID == 0)
            {
                return result;
            }

            var rtnData = _memberInfoRepository.GetMemberAppTokenByMID(MID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得手機號碼 (未註冊用 OPCellPhone, 已註冊用 Detail.CellPhone)
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="OPCellPhone"></param>
        /// <returns></returns>
        public DataResult<string> GetCellPhone(long MID, string OPCellPhone, string requestCellPhone = null)
        {
            var result = new DataResult<string>();
            result.SetError();

            string CellPhone = null;

            if (MID > 0)
            {
                //已註冊則使用 會員資料的 CellPhone
                var member = _memberInfoRepository.GetMemberData(MID);

                var detail = member.detail;

                CellPhone = detail.CellPhone;
                
            }
            //尚未註冊完成使用 OPW 的 CellPhone
            else
            {
                CellPhone = OPCellPhone;
            }

            if (string.IsNullOrWhiteSpace(CellPhone))
            {
                result.SetError(new BaseResult { RtnMsg = "會員無手機號碼資料" });
                return result;
            }

            if (!string.IsNullOrEmpty(requestCellPhone) && requestCellPhone != CellPhone)
            {
                result.SetError(new BaseResult { RtnMsg = "會員無手機號碼資料 不一至，無法驗證" });
                return result;
            }

            result.SetSuccess(CellPhone);
            return result;
        }

        public BaseResult CheckFormat_UserCode(string value)
        {
            var result = new BaseResult();
            result.SetError();

            if (new Regex(RegexConst.IDNO).IsMatch(value))
            {
                result.SetError(new BaseResult { RtnMsg = "登入帳號不得與手機號碼一致或者身分證字號規則相同。" });
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 檢查帳號是否唯一不重覆
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="MID"></param>
        /// <returns></returns>
        public BaseResult CheckUserCodeUnique(string UserCode, long MID = 0)
        {
            string enUserCode = _configCyptRepository.Encrypt_UserCode(UserCode);

            return _memberInfoRepository.CheckUserCodeUnique(enUserCode, MID);
        }

        /// <summary>
        /// 檢查手機號碼是否可使用
        /// </summary>
        /// <param name="CellPhone"></param>
        /// <returns></returns>
        public BaseResult CheckCellPhone(string CellPhone, long MID = 0)
        {
            return _memberInfoRepository.CheckCellPhone(CellPhone, MID);
        }

        public MemberLawBasicModel GetMemberLawBasic(int LevelID)
        {
            return _memberInfoRepository.GetMemberLawBasic(LevelID);
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
            return _memberInfoRepository.UpdateMemberPreference(MID, OptionType, OptionName, OptionValue, RealIP, ProxyIP);
        }

        /// <summary>
        /// 批次更新會員偏號設定
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="OptionType">設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)</param>
        /// <param name="options">設定集合</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateMemberPreferences(long MID, byte OptionType, List<MemberPreferenceModel> options, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            bool hasError = false;

            options.ForEach(option => 
            {
                var updateResult = _memberInfoRepository.UpdateMemberPreference(MID, OptionType, option.OptionName, option.OptionValue, RealIP, ProxyIP);
                if (!updateResult.IsSuccess)
                {
                    hasError = true;
                }
            });

            if (hasError)
            {
                return result;
            }

            result.SetSuccess();
            return result;
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
            return _memberInfoRepository.GetMemberPreference(MID, OptionType, OptionName);
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
            return _memberInfoRepository.ListMemberPreferences(MID, OptionType, PageSize, PageIndex);
        }


        /// <summary>
        /// 取得同意事項結果
        /// </summary>
        /// <param name="MID">MID</param>
        /// <param name="WebSiteType">顯示平台(&運算) 1:官網 2:廠商後台 4:APP 8:非必要顯示</param>
        /// <returns></returns>
        public List<MemberAgreeResult> ListMemberAgreeItem(long MID, int WebSiteType)
        {
            return _memberInfoRepository.ListMemberAgreeItem(MID, WebSiteType);
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
            return _memberInfoRepository.UpdateMemberAgreeResult(MID, AgreeType, AgreeStatus, RealIP, ProxyIP);
        }

        /// <summary>
        /// 更新並取得會員等級
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetMemberLevelResult GetMemberLevelID(long MID)
        {
            return _memberInfoRepository.GetMemberLevelID(MID);
        }

        /// <summary>
        /// 更新OP帳號綁定記錄
        /// </summary>
        /// <param name="RecordID">記錄編號</param>
        /// <param name="Status">通知結果 1: 成功 2: 失敗</param>
        /// <returns></returns>
        public BaseResult UpdateBindOPAccountNotifyRecord(long RecordID, byte Status)
        {
            return _memberInfoRepository.UpdateBindOPAccountNotifyRecord(RecordID, Status);
        }

        /// <summary>
        /// 取得會員的銀行帳號資料
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<MemberBankInfo> ListMemberBankInfo(long MID, byte? Category = null)
        {
            return _memberInfoRepository.ListMemberBankInfo(MID, Category);
        }

        /// <summary>
        /// 取得會員編號
        /// </summary>
        /// <param name="IDNO">身份證</param>
        /// <param name="ICPMID">電支帳號</param>
        /// <param name="Account">登入帳號</param>
        /// <returns></returns>
        public DataResult<long> GetMID(string IDNO = null, string ICPMID = null, string Account = null)
        {
            string encryptAccount = null;
            if (!string.IsNullOrEmpty(Account))
            {
                encryptAccount = _configCyptRepository.Encrypt_UserCode(Account);
            }

            return _memberInfoRepository.GetMID(IDNO, ICPMID, encryptAccount);
        }

        /// <summary>
        /// 檢查使用者IP是否被列為鎖定的IP黑名單
        /// </summary>
        /// <param name="RealIP"></param>
        /// <returns></returns>
        public BaseResult CheckIPBlackList(long RealIP)
        {
            return _memberInfoRepository.CheckIPBlackList(RealIP);
        }


        /// <summary>
        /// 檢查使用者手機號碼是否被列為鎖定的OTP黑名單
        /// </summary>
        /// <param name="CellPhone"></param>
        /// <returns></returns>
        public BaseResult CheckOTPBlackList(string CellPhone)
        {
            return _memberInfoRepository.CheckOTPBlackList(CellPhone);
        }

        /// <summary>
        /// 取得縣市, 區
        /// </summary>
        /// <param name="AreaID">區域代碼</param>
        /// <returns></returns>
        public List<ZipCodeModel> ListZipCodeArea(string AreaID = "") => _memberInfoRepository.ListZipCodeArea(AreaID);

        /// <summary>
        /// 取得職業
        /// </summary>
        /// <returns></returns>
        public List<OccupationModel> ListOccupation() => _memberInfoRepository.ListOccupation();

        /// <summary>
        /// 取得國家
        /// </summary>
        /// <returns></returns>
        public List<NationalityModel> ListNationality() => _memberInfoRepository.ListNationality();
		/// <summary>
		/// 取得會員狀態
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public byte GetMemberStatus(long MID)
        {
            return _memberInfoRepository.GetMemberStatus(MID);
        }
        /// <summary>
        /// 檢查裝置ID是否是黑名單
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public BaseResult CheckMemberDeviceStatus(string deviceId)
        {
            return _memberInfoRepository.CheckMemberDeviceStatus(deviceId);
        }

    }
}
