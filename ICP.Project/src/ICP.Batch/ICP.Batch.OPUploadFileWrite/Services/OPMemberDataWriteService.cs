using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileWrite.Services
{
    using ICP.Infrastructure.Abstractions.Logging;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Repositories.SystemRepositories;
    using Models;
    using Repositories;

    public class OPMemberDataWriteService
    {
        ILogger<OPMemberDataWriteService> _logger;
        ConfigRepository _configRepository;
        ConfigKeyValueRepository _configKeyValueRepository;
        OPUploadFileWriteRepository _oPUploadFileWriteRepository;
        OPMemberDataWriteRepository _oPMemberDataWriteRepository;

        public OPMemberDataWriteService(
            ILogger<OPMemberDataWriteService> logger,
            ConfigRepository configRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            OPUploadFileWriteRepository oPUploadFileWriteRepository,
            OPMemberDataWriteRepository oPMemberDataWriteRepository
            )
        {
            _logger = logger;
            _configRepository = configRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _oPUploadFileWriteRepository = oPUploadFileWriteRepository;
            _oPMemberDataWriteRepository = oPMemberDataWriteRepository;
        }

        /// <summary>
        /// 取得綁定帳號重送記錄
        /// </summary>
        /// <param name="Source">產生重送來源 2:API重送排程 3: SFTP 通知排程</param>
        /// <param name="DataDate">重送資料日期</param>
        /// <returns></returns>
        public DataResult<List<BindOPAccountNotifyRecord>> ListBindOPAccountNotify_ReSendRecord(byte Source, DateTime DataDate)
        {
            var result = new DataResult<List<BindOPAccountNotifyRecord>>();
            result.SetError();

            var rtnData = _oPUploadFileWriteRepository.ListBindOPAccountNotify_ReSendRecord(Source, DataDate);
            if (rtnData == null || rtnData.Count == 0)
            {
                result.RtnMsg = "無資料";
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 產生TXT
        /// </summary>
        /// <param name="DataDate">資料日期</param>
        /// <param name="Records">資料</param>
        /// <returns></returns>
        public DataResult<string> GenerateTxt_MemberStatusChangeFile(DateTime DataDate, List<BindOPAccountNotifyRecord> Records)
        {
            var result = new DataResult<string>();
            result.SetError();

            //暫存目錄
            string tempDir = _configRepository.op_client_WriteTempDir;
            string clientId = _configKeyValueRepository.op_client_id;
            string sDataDate = DataDate.ToString("yyyyMMdd");
            string txtFileName = $"OPW_{clientId}_{sDataDate}.txt";
            string txtFilePath = System.IO.Path.Combine(tempDir, "MemberStatusChangeFile", txtFileName);

            var genTxtResult = _oPMemberDataWriteRepository.GenerateTxt_MemberStatusChangeFile(DataDate, Records, txtFilePath);
            if (!genTxtResult.IsSuccess)
            {
                result.SetError(genTxtResult);
                return result;
            }

            result.SetSuccess(txtFilePath);
            return result;
        }

        /// <summary>
        /// 產生ZIP
        /// </summary>
        /// <param name="txtFilePath">文字檔路徑</param>
        /// <returns></returns>
        public DataResult<string> WriteZip_MemberStatusChangeFile(string txtFilePath)
        {
            var result = new DataResult<string>();
            result.SetError();

            string zippw = _configKeyValueRepository.op_client_zippw;

            var zipResult = _oPMemberDataWriteRepository.WriteZip_MemberStatusChangeFile(zippw, txtFilePath);
            if (!zipResult.IsSuccess)
            {
                result.SetError(zipResult);
                return result;
            }

            result.SetSuccess(zipResult.RtnData);
            return result;
        }

        /// <summary>
        /// 備份
        /// </summary>
        /// <param name="txtFilePath">文字檔路徑</param>
        /// <param name="zipFilePath">壓縮檔路徑</param>
        /// <returns></returns>
        public void Back_MemberStatusChangeFile(string txtFilePath, string zipFilePath)
        {
            //備份目錄
            string backDir = _configRepository.op_client_WriteBackDir;

            string txtFileName = System.IO.Path.GetFileName(txtFilePath);
            string zipFileName = System.IO.Path.GetFileName(zipFilePath);

            string backTxtPath = System.IO.Path.Combine(backDir, "MemberStatusChangeFile", txtFileName);
            string backZipPath = System.IO.Path.Combine(backDir, "MemberStatusChangeFile", zipFileName);

            System.IO.File.Move(txtFilePath, backTxtPath);
            System.IO.File.Move(zipFilePath, backZipPath);
        }
    }
}
