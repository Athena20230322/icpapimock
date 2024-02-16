using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileRead.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Library.Services.MemberServices;
    using Services;

    public class MemberStatusChangeCommand
    {
        ILogger _logger;
        EMailNotifyService _eMailNotifyService;
        OPMemberDataReadService _oPMemberDataReadService;
        LibMemberDelService _libMemberDelService;

        public MemberStatusChangeCommand(
            ILogger<MemberStatusChangeCommand> logger,
            EMailNotifyService eMailNotifyService,
            OPMemberDataReadService oPMemberDataReadService,
            LibMemberDelService libMemberDelService
            )
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _oPMemberDataReadService = oPMemberDataReadService;
            _libMemberDelService = libMemberDelService;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            //會員狀態變更壓縮檔讀取
            execWithTryCatch(OPMemberStatusChangeZip_Read);
        }

        /// <summary>
        /// 使用 try_catch 執行
        /// </summary>
        /// <param name="action"></param>
        private void execWithTryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// OP 會員狀態變更壓縮檔讀取
        /// </summary>
        /// <returns></returns>
        private void OPMemberStatusChangeZip_Read()
        {
            string commandMethod = "OPMemberStatusChangeZip_Read";
            var errorResults = new List<BaseResult>();
            var result = new BaseResult();
            result.SetError();

            _logger.Info($"{commandMethod} Begin");

            #region 取得待讀清單
            var list = _oPMemberDataReadService.List_OPMemberStatusChangeZip();
            _logger.Info("尚有 {0} 個壓縮檔 待讀取", list.Count);
            #endregion

            foreach (string zipPath in list)
            {
                var readFileResult = OPMemberStatusChangeZip_ReadFile(zipPath);
                if (!readFileResult.IsSuccess)
                {
                    errorResults.Add(readFileResult);
                }
            }

            _logger.Info("讀取結束");
            _logger.Info($"{commandMethod}_Read End");

            if (errorResults.Count > 0)
            {
                #region EMAIL 錯誤結果
                string errMsg = string.Empty;

                errorResults.ForEach(t =>
                {
                    errMsg += $"RtnCode: {t.RtnCode}, RtnMsg: {t.RtnMsg}" + Environment.NewLine;
                });

                result.RtnMsg = errMsg;

                _eMailNotifyService.SendResultEmail(commandMethod, result);
                #endregion
            }
        }

        private BaseResult OPMemberStatusChangeZip_ReadFile(string zipPath)
        {
            var result = new BaseResult();
            result.SetError();

            string zipName = System.IO.Path.GetFileName(zipPath);

            #region 讀取
            //讀取
            var readResult = _oPMemberDataReadService.Read_MemberStatusChangeFile(zipPath);
            if (!readResult.IsSuccess)
            {
                _logger.Error(readResult.RtnMsg);
                result.SetError(readResult);
                return result;                
            }
            #endregion

            #region D:會員個資銷毀
            //取得準備刪除資料
            var list_del = _oPMemberDataReadService.List_MemberDelete_Item(readResult.RtnData);

            //執行批次刪除
            var batchDelResult = _libMemberDelService.BatchDeleteMember(list_del);
            if (!batchDelResult.IsSuccess)
            {
                batchDelResult.RtnMsg = $"{zipName}: {batchDelResult.RtnMsg}";

                _logger.Error(batchDelResult.RtnMsg);
                result.SetError(batchDelResult);
                return result;
            }
            #endregion

            // 備份
            _oPMemberDataReadService.Back_MemberStatusChangeFile(zipPath);

            result.SetSuccess();
            return result;
        }
    }
}