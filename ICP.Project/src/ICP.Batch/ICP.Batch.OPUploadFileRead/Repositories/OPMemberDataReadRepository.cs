using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Ionic.Zip;

namespace ICP.Batch.OPUploadFileRead.Repositories
{
    using Infrastructure.Abstractions.Logging;
    using Models;

    public class OPMemberDataReadRepository
    {
        ILogger<OPMemberDataReadRepository> _logger;

        public OPMemberDataReadRepository(ILogger<OPMemberDataReadRepository> logger)
        {
            _logger = logger;
        }

        #region 會員狀態變更檔案讀取
        /// <summary>
        /// 會員狀態變更檔案讀取
        /// </summary>
        /// <param name="zipPath">壓縮檔路徑</param>
        /// <param name="zipPassword">壓縮檔密碼</param>
        /// <returns></returns>
        public OPMemberStatusChangeFile Read_MemberStatusChangeFile(string zipPath, string zipPassword)
        {
            OPMemberStatusChangeFile result = null;

            //檢查壓縮檔是否存在
            if (!File.Exists(zipPath)) return null;

            //壓縮檔目錄
            string dir = Path.GetDirectoryName(zipPath);

            //壓縮檔名.副檔名 MSC_{clientId}_yyyyMMdd.zip
            string fullFileName = Path.GetFileName(zipPath);

            //壓縮檔名 MSC_{clientId}_yyyyMMdd
            string fileName = Path.GetFileNameWithoutExtension(zipPath);

            //文字檔名 MSC_{clientId}_yyyyMMdd.txt
            string txtFileName = string.Format("{0}.txt", fileName);

            //解壓路徑 {壓縮檔目錄}\extract
            string extractDir = Path.Combine(dir, "extract", fileName);

            //文字檔路徑 {解壓路徑}\文字檔名
            string filePath = Path.Combine(extractDir, txtFileName);

            //解壓目錄 建立
            if (!Directory.Exists(extractDir))
            {
                Directory.CreateDirectory(extractDir);
            }

            try
            {
                #region 解壓
                //解壓
                using (var zip = ZipFile.Read(zipPath))
                {
                    if (!string.IsNullOrEmpty(zipPassword))
                    {
                        zip.Password = zipPassword;
                    }

                    zip.ExtractAll(extractDir);
                }
                #endregion

                #region 檢查
                //檢查解壓是否存在目標檔案
                if (!File.Exists(filePath)) return null;

                //讀取所有行數
                List<string> lines = File.ReadAllLines(filePath).ToList();

                //無資料
                if (lines.Count == 0) return null;

                //分隔參數
                string[] args;

                //檔案
                result = new OPMemberStatusChangeFile();

                //讀取標頭
                string firstLine = lines[0];
                args = firstLine.Split(',');
                result.header.yyyyMMdd = args[0];
                result.header.count = Convert.ToInt32(args[1]);

                //檢查標頭, 內容行數
                if (result.header.count + 1 != lines.Count) throw new Exception("Header 筆數 有誤");
                #endregion

                #region 讀取
                //讀取內容
                lines = lines.Skip(1).ToList();
                lines.ForEach(t =>
                {
                    args = t.Split(',');
                    result.items.Add(new OPMemberStatusChangeFile.Item
                    {
                        OPMID = args[0],
                        Status = args[1]
                    });
                });
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
            finally
            {
                //刪除解壓檔
                Directory.Delete(extractDir, true);
            }

            return result;
        }

        /// <summary>
        /// 會員狀態變更檔案備份
        /// </summary>
        /// <param name="zipPath">壓縮檔路徑</param>
        /// <param name="backDir">備份目錄</param>
        public void Back_MemberStatusChangeFile(string zipPath, string backDir)
        {
            //壓縮檔名.副檔名 MSC_{clientId}_yyyyMMdd.zip
            string fullFileName = Path.GetFileName(zipPath);

            //備份路徑 {備份目錄}\MSC_{clientId}_yyyyMMdd.zip
            string backPath = Path.Combine(backDir, fullFileName);

            //備份目錄 建立
            if (!Directory.Exists(backDir))
            {
                Directory.CreateDirectory(backDir);
            }

            //備份檔案
            File.Move(zipPath, backPath);
        }
        #endregion
    }
}