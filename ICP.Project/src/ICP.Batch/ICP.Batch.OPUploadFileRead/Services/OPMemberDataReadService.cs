using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileRead.Services
{
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.MemberModels;
    using Library.Repositories.SystemRepositories;
    using Models;
    using Repositories;

    public class OPMemberDataReadService
    {
        ConfigRepository _configRepository;
        ConfigKeyValueRepository _configKeyValueRepository;
        OPMemberDataReadRepository _oPMemberDataReadRepository;

        public OPMemberDataReadService(
            ConfigRepository configRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            OPMemberDataReadRepository oPMemberDataReadRepository
            )
        {
            _configRepository = configRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _oPMemberDataReadRepository = oPMemberDataReadRepository;
        }

        /// <summary>
        /// 取得尚未讀取的 會員狀態變更 壓縮檔
        /// </summary>
        /// <returns></returns>
        public List<string> List_OPMemberStatusChangeZip()
        {
            string dir = _configRepository.op_client_ReadPath;

            var list = System.IO.Directory.GetFiles(dir).ToList();

            string clientId = _configKeyValueRepository.op_client_id;

            string prefix = $"MSC_{clientId}_";

            list = list
                .Select(t => new {
                    filePath = t,
                    fileName = System.IO.Path.GetFileName(t)
                })
                .Where(t => t.fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Where(t => t.fileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                .Select(t => t.filePath)
                .ToList();

            return list;
        }


        /// <summary>
        /// 讀取會員狀態變更檔
        /// </summary>
        /// <param name="zipPath"></param>
        /// <returns></returns>
        public DataResult<OPMemberStatusChangeFile> Read_MemberStatusChangeFile(string zipPath)
        {
            var result = new DataResult<OPMemberStatusChangeFile>();
            result.SetError();

            //檔案目錄
            string zippw = _configKeyValueRepository.op_client_zippw;

            //讀取
            var rtnData = _oPMemberDataReadRepository.Read_MemberStatusChangeFile(zipPath, zippw);
            if (rtnData == null)
            {
                string zipName = System.IO.Path.GetFileName(zipPath);
                result.RtnMsg = $"{zipName} 讀取失敗"; ;
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 會員狀態變更檔案備份
        /// </summary>
        /// <param name="zipPath"></param>
        public void Back_MemberStatusChangeFile(string zipPath)
        {
            //備份目錄
            string backDir = _configRepository.op_client_ReadBackDir;

            _oPMemberDataReadRepository.Back_MemberStatusChangeFile(zipPath, backDir);
        }

        /// <summary>
        /// 取得要進行檔案刪除的OP會員
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<MemberDeleteModel> List_MemberDelete_Item(OPMemberStatusChangeFile file)
        {
            var list = new List<MemberDeleteModel>();

            return file
                .items.Where(t => t.Status == "D")
                .Select(t => new MemberDeleteModel
                {
                    OPMID = t.OPMID,
                    Source = 2
                })
                .ToList();
        }
    }
}
