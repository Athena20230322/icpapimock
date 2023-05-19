using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;
using ICP.Batch.OPUploadFileWrite.Models;

namespace ICP.Batch.OPUploadFileWrite.Repositories
{
    public class OPMemberDataWriteRepository
    {
        ILogger<OPMemberDataWriteRepository> _logger;

        public OPMemberDataWriteRepository(ILogger<OPMemberDataWriteRepository> logger)
        {
            _logger = logger;
        }

        public BaseResult GenerateTxt_MemberStatusChangeFile(DateTime DataDate, List<BindOPAccountNotifyRecord> Records, string targetPath)
        {
            var result = new BaseResult();
            result.SetError();

            var sBuilder = new StringBuilder();
            string sDataYYYYMMDD = DataDate.ToString("yyyyMMdd");

            sBuilder.AppendFormat("{0},{1}", sDataYYYYMMDD, Records.Count);
            Records.ForEach(record =>
            {
                string status = null;
                if (record.Type == 0)
                    status = "A";
                else if (record.Type == 1)
                    status = "D";
                else
                {
                    _logger.Error("BindOPAccountNotifyRecord.Type not in (0, 1)");
                    throw new ArgumentException("BindOPAccountNotifyRecord.Type not in (0, 1)", "BindOPAccountNotifyRecord.Type");
                }

                sBuilder.AppendFormat("{0},{1},{2},{3},{4},{5}", Environment.NewLine, record.OPMID, status, record.ICPMID, record.ICPCarrier, record.CarrierType);
            });

            File.WriteAllText(targetPath, sBuilder.ToString());

            result.SetSuccess();
            return result;
        }

        public DataResult<string> WriteZip_MemberStatusChangeFile(string zipPassword, string txtPath)
        {
            var result = new DataResult<string>();
            result.SetError();

            //檢查壓縮檔是否存在
            if (!File.Exists(txtPath)) return result;

            //來源檔目錄
            string dir = Path.GetDirectoryName(txtPath);

            //來源檔名.副檔名 OPW_{clientId}_yyyyMMdd.txt
            string fullFileName = Path.GetFileName(txtPath);

            //來源檔名 OPW_{clientId}_yyyyMMdd
            string fileName = Path.GetFileNameWithoutExtension(txtPath);

            //壓縮檔名 OPW_{clientId}_yyyyMMdd.zip
            string zipFileName = string.Format("{0}.zip", fileName);

            //壓縮檔目錄 {來源檔目錄}\zip
            string zipDir = Path.Combine(dir, "zip", fileName);

            //壓縮檔路徑 {來源檔目錄}\文字檔名
            string filePath = Path.Combine(zipDir, zipFileName);

            try
            {
                #region 壓縮
                //解壓
                using (var zip = new ZipFile())
                {
                    if (!string.IsNullOrEmpty(zipPassword))
                    {
                        zip.Password = zipPassword;
                    }

                    zip.AddFile(txtPath);
                    zip.Save(filePath);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return result;
            }

            result.SetSuccess(filePath);
            return result;
        }
    }
}
