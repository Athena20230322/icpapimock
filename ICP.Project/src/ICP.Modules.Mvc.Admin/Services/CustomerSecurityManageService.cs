using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class CustomerSecurityManageService
    {
        CustomerSecurityManageRepository _customerSecurityManageRepository;

        public CustomerSecurityManageService
            (
            CustomerSecurityManageRepository customerSecurityManageRepository
            )
        {
            _customerSecurityManageRepository = customerSecurityManageRepository;
        }

        #region IP黑名單相關

        #region IP黑名單資料列表查詢
        public List<IPBlackListModel> ListIPBlackList(IPBlackQryModel query)
        {
            return _customerSecurityManageRepository.ListIPBlackList(query);
        }
        #endregion

        #region 新增IP黑名單
        public BaseResult AddIPBlackList(IPBlackAddModel model)
        {
            return _customerSecurityManageRepository.AddIPBlackList(model);
        }
        #endregion

        #region 鎖定/解鎖IP黑名單
        public BaseResult UpdateIPBlackList(IPBlackUpdateModel model)
        {
            return _customerSecurityManageRepository.UpdateIPBlackList(model);
        }
        #endregion

        #region IP黑名單歷程
        public List<IPBlackListLogModel> ListIPBlackListLog(string IP)
        {
            return _customerSecurityManageRepository.ListIPBlackListLog(IP);
        }
        #endregion

        #endregion        

        #region 身份證黑名單相關

        #region 新增/解鎖/封鎖身份證黑名單
        public BaseResult AddOrUpdateIDNOBlackList(IDNOBlackAddOrUpdateModel model)
        {
            return _customerSecurityManageRepository.AddOrUpdateIDNOBlackList(model);
        }
        #endregion

        #region 身份證黑名單列表
        public List<IDNOBlackListLogModel> ListIDNOBlackList(IDNOBlackQryModel query)
        {
            
            return _customerSecurityManageRepository.ListIDNOBlackList(query);
        }
        #endregion

        #region 身份證黑名單Log列表
        public List<IDNOBlackListLogModel> ListIDNOBlackListLog(string IDNO)
        {
            return _customerSecurityManageRepository.ListIDNOBlackListLog(IDNO);
        }
        #endregion

        #endregion

        #region OTP黑名單相關

        #region 列表-[OTP黑名單]
        public List<OTPBlackListModel> ListBlackOTP(OTPBlackQryModel model)
        {
            return _customerSecurityManageRepository.ListBlackOTP(model);
        }
        #endregion

        #region 新增/鎖定-[OTP黑名單]
        public BaseResult AddBlackOTP(OTPBlackLockModel model)
        {
            return _customerSecurityManageRepository.AddBlackOTP(model);
        }
        #endregion

        #region 解鎖-[OTP黑名單]
        public BaseResult UnLockBlackOTP(OTPBlackUnLockModel model)
        {
            return _customerSecurityManageRepository.UnLockBlackOTP(model);            
        }
        #endregion

        #region 列表-[OTP黑名單歷程紀錄]
        public List<OTPBlackListLogModel> ListOTPLog(string CellPhone)
        {
            return _customerSecurityManageRepository.ListOTPLog(CellPhone);
        }
        #endregion

        #endregion

        #region 提領限制黑名單相關

        #region 新增/解鎖/封鎖提領限制黑名單
        public BaseResult AddOrUpdateTakeCashLimitList(TakeCashLimitAddOrUpdateModel model)
        {
            return _customerSecurityManageRepository.AddOrUpdateTakeCashLimitList(model);
        }
        #endregion

        #region 提領限制黑名單列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitList(TakeCashLimitQryModel query)
        {

            return _customerSecurityManageRepository.ListTakeCashLimitList(query);
        }
        #endregion

        #region 提領限制黑名單Log列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitListLog(string MID)
        {
            return _customerSecurityManageRepository.ListTakeCashLimitListLog(MID);
        }
        #endregion

        #endregion

        #region 註冊同IP預警名單相關

        #region 註冊同IP預警名單列表
        public List<RegistIPListLogModel> ListRegistIPList(RegistIPBlackQryModel query)
        {
            return _customerSecurityManageRepository.ListRegistIPList(query);
        }
        #endregion

        #region 註冊同IP預警名單明細
        public List<RegistIPListLogModel> ListRegistIPListLog(string IP)
        {
            return _customerSecurityManageRepository.ListRegistIPListLog(IP);
        }
        #endregion

        #region 註冊同IP加入黑名單

        #endregion

        #endregion

    }
}
