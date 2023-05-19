using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class CustomerSecurityManageCommand
    {
        CustomerSecurityManageService _customerSecurityManageService;        
        ExportDataService _exportDataService;
        LibMemberInfoService _LibMemberInfoService;

        public CustomerSecurityManageCommand(
             CustomerSecurityManageService customerSecurityManageService,             
             ExportDataService exportDataService,
             LibMemberInfoService LibMemberInfoService
            )
        {
            _customerSecurityManageService = customerSecurityManageService;            
            _exportDataService = exportDataService;
            _LibMemberInfoService = LibMemberInfoService;
        }

        #region IP黑名單相關

        #region IP黑名單資料列表查詢
        public List<IPBlackListModel> ListIPBlackList(IPBlackQryModel query)
        {

            return _customerSecurityManageService.ListIPBlackList(query);
        }
        #endregion

        #region 新增IP黑名單
        public BaseResult AddIPBlackList(IPBlackAddModel model)
        {
            return _customerSecurityManageService.AddIPBlackList(model);
        }
        #endregion
                
        #region 鎖定/解鎖IP黑名單
        public BaseResult UpdateIPBlackList(IPBlackUpdateModel model)
        {
            return _customerSecurityManageService.UpdateIPBlackList(model);
        }
        #endregion

        #region IP黑名單歷程
        public List<IPBlackListLogModel> ListIPBlackListLog(string IP)
        {
            return _customerSecurityManageService.ListIPBlackListLog(IP);
        }
        #endregion

        #region 匯出IP黑名單
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetIPBlackExport(IPBlackQryModel model)
        {
            var list = _customerSecurityManageService.ListIPBlackList(model);
            if (!list.Any()) return null;

            string functionName = "註冊IP黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "註冊IP位置", "最近鎖定時間", "建立人員", "最新解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<IPBlackListModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IP,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.CreateUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,                    
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #region 匯出IP黑名單歷程
        public MemoryStream GetIPBlackLogExport(string IP)
        {
            var list = _customerSecurityManageService.ListIPBlackListLog(IP);
            if (!list.Any()) return null;

            string functionName = "註冊IP黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "IP位置", "建立時間", "建立人員", "解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<IPBlackListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.ModifyDate != null) ? t.ModifyDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IP,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.CreateUser,
                    ModifyD,
                    t.Modifier,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #endregion

        #region 身份證黑名單相關

        #region 新增/解鎖/封鎖身份證黑名單
        public BaseResult AddOrUpdateIDNOBlackList(IDNOBlackAddOrUpdateModel model)
        {
            return _customerSecurityManageService.AddOrUpdateIDNOBlackList(model);
        }
        #endregion

        #region 身份證黑名單列表
        public List<IDNOBlackListLogModel> ListIDNOBlackList(IDNOBlackQryModel query)
        {
            return _customerSecurityManageService.ListIDNOBlackList(query);
        }
        #endregion

        #region 身份證黑名單Log列表
        public List<IDNOBlackListLogModel> ListIDNOBlackListLog(string IDNO)
        {
            return _customerSecurityManageService.ListIDNOBlackListLog(IDNO);
        }
        #endregion

        #region 匯出身份證黑名單
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetIDNOBlackExport(IDNOBlackQryModel model)
        {
            var list = _customerSecurityManageService.ListIDNOBlackList(model);
            if (!list.Any()) return null;

            string functionName = "身份證黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "身分證字號/居留證", "最近鎖定時間", "建立人員", "最新解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<IDNOBlackListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IDNO,
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #region 匯出身份證黑名單歷程
        public MemoryStream GetIDNOBlackLogExport(string IDNO)
        {
            var list = _customerSecurityManageService.ListIDNOBlackListLog(IDNO);
            if (!list.Any()) return null;

            string functionName = "身份證黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "身分證字號/居留證", "建立時間", "建立人員", "解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<IDNOBlackListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IDNO,
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #endregion

        #region OTP黑名單相關

        #region 列表-[OTP黑名單]
        public List<OTPBlackListModel> ListBlackOTP(OTPBlackQryVM model)
        {
            DateTime startDate = Convert.ToDateTime(model.StartDate);
            DateTime endDate = Convert.ToDateTime(model.EndDate);

            var query = new OTPBlackQryModel
            {
                StartDate = startDate,
                EndDate = endDate,
                CellPhone = model.CellPhone,
                IDNO = model.IDNO,
                Email = model.Email,
                PageNo = model.PageNo,
                PageSize = model.PageSize
            };

            return _customerSecurityManageService.ListBlackOTP(query);
        }
        #endregion

        #region 新增/鎖定-[OTP黑名單]
        public BaseResult AddBlackOTP(OTPBlackLockModel model)
        {
            return _customerSecurityManageService.AddBlackOTP(model);
        }
        #endregion

        #region 解鎖-[OTP黑名單]
        public BaseResult UnLockBlackOTP(OTPBlackUnLockModel model)
        {
            return _customerSecurityManageService.UnLockBlackOTP(model);
        }
        #endregion

        #region 列表-[OTP黑名單歷程紀錄]
        public List<OTPBlackListLogModel> ListOTPLog(string CellPhone)
        {
            return _customerSecurityManageService.ListOTPLog(CellPhone);
        }
        #endregion

        #region 匯出OTP黑名單
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetOTPBlackExport(OTPBlackQryVM model)
        {
            
            var list = ListBlackOTP(model);
            if (!list.Any()) return null;

            string functionName = "OTP黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "手機號碼", "最近鎖定時間", "建立人員", "最新解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion

            #region 查詢日期區間
            string dateRange = "查詢日期:" + model.StartDate.Replace('-', '/') + "~" + model.EndDate.Replace('-', '/');
            #endregion


            Func<OTPBlackListModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.CellPhone,                    
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName, dateRange);
        }
        #endregion

        #region 匯出OTP黑名單歷程
        public MemoryStream GetOTPBlackLogExport(string CellPhone)
        {
            var list = _customerSecurityManageService.ListOTPLog(CellPhone);
            if (!list.Any()) return null;

            string functionName = "OTP黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "手機號碼", "身分證字號", "電子郵件", "建立時間", "建立人員", "解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<OTPBlackListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.CellPhone,
                    t.IDNO,
                    t.Email,
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #endregion

        #region 提領限制黑名單相關

        #region 新增/解鎖/封鎖提領限制黑名單
        public BaseResult AddOrUpdateTakeCashLimitList(TakeCashLimitAddOrUpdateModel model)
        {
            return _customerSecurityManageService.AddOrUpdateTakeCashLimitList(model);
        }
        #endregion

        #region 提領限制黑名單列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitList(TakeCashLimitQryVM model)
        {
            DateTime startDate = Convert.ToDateTime(model.StartDate);
            DateTime endDate = Convert.ToDateTime(model.EndDate);

            var query = new TakeCashLimitQryModel
            {
                StartDate = startDate,
                EndDate = endDate,
                CellPhone = model.CellPhone,
                IDNO = model.IDNO,
                ICPMID = model.ICPMID,
                Email = model.Email,
                PageNo = model.PageNo,
                PageSize = model.PageSize
            };

            var list = _customerSecurityManageService.ListTakeCashLimitList(query);
            list.ForEach(t => t.CName = _LibMemberInfoService.ConcealPartialCName(t.CName));
            return list;
        }
        #endregion

        #region 提領限制黑名單Log列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitListLog(string MID)
        {
            var list = _customerSecurityManageService.ListTakeCashLimitListLog(MID);
            list.ForEach(t => t.CName = _LibMemberInfoService.ConcealPartialCName(t.CName));
            return list;
        }
        #endregion

        #region 匯出提領限制黑名單
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetTakeCashLimitListExport(TakeCashLimitQryVM model)
        {

            var list = ListTakeCashLimitList(model);
            if (!list.Any()) return null;

            string functionName = "提領限制黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "電支帳號", "最近鎖定時間", "建立人員", "最新解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion

            #region 查詢日期區間
            string dateRange = "查詢日期:" + model.StartDate.Replace('-','/') + "~" + model.EndDate.Replace('-', '/');
            #endregion

            Func<TakeCashLimitListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.ICPMID,
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName, dateRange);
        }
        #endregion

        #region 匯出提領限制黑名單歷程
        public MemoryStream GetTakeCashLimitListLogExport(string MID)
        {
            var list = _customerSecurityManageService.ListTakeCashLimitListLog(MID);
            if (!list.Any()) return null;

            string functionName = "提領限制黑名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "電支帳號", "姓名", "手機號碼", "身分證字號", "電子郵件", "建立時間", "建立人員", "解除時間", "解除人員", "鎖定原因", "解鎖原因", "黑名單狀態"
            };
            #endregion


            Func<TakeCashLimitListLogModel, string[]> arryDataGenerator = t =>
            {
                string Status = (t.Status == 0) ? "已解除" : "封鎖";
                string ModifyD = (t.UnLockDate != null) ? t.UnLockDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.ICPMID,
                    t.CName,
                    t.CellPhone,
                    t.IDNO,
                    t.Email,
                    t.LockDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.LockUser,
                    ModifyD,
                    t.UnLockUser,
                    t.LockMemo,
                    t.UnLockMemo,
                    Status
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #endregion

        #region 註冊同IP預警名單相關

        #region 註冊同IP預警名單列表
        public List<RegistIPListLogModel> ListRegistIPList(RegistIPBlackQryModel query)
        {
            return _customerSecurityManageService.ListRegistIPList(query);
        }
        #endregion

        #region 註冊同IP預警名單明細
        public List<RegistIPListLogModel> ListRegistIPListLog(string IP)
        {
            return _customerSecurityManageService.ListRegistIPListLog(IP);
        }
        #endregion

        #region 匯出註冊同IP預警名單
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetRegistBlackListExport(RegistIPBlackQryModel model)
        {

            var list = ListRegistIPList(model);
            if (!list.Any()) return null;

            string functionName = "註冊同IP預警名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "IP位置", "累計次數"
            };
            #endregion

            Func<RegistIPListLogModel, string[]> arryDataGenerator = t =>
            {                
                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IP,
                    t.Tcount.ToString()
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #region 匯出註冊同IP預警名單歷程
        public MemoryStream GetRegistIPBlackListLogExport(string MID)
        {
            var list = _customerSecurityManageService.ListRegistIPListLog(MID);
            if (!list.Any()) return null;

            string functionName = "註冊同IP預警名單";

            #region 標題
            string[] header = new string[]
            {
                "序號", "IP位置", "建立時間", "電支帳號"
            };
            #endregion

            Func<RegistIPListLogModel, string[]> arryDataGenerator = t =>
            {

                var values = new string[]
                {
                    t.RowNo.ToString(),
                    t.IP,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.ICPMID
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName);
        }
        #endregion

        #endregion
    }
}
