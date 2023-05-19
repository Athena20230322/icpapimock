using ICP.Infrastructure.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class FtpHelper
    {
        private readonly string ftpUserName;
        private readonly string ftpPassword;
        private readonly string ftpRootPath;
        private ILogger Logger { get; set; }

        public FtpHelper(string user, string password, string server, ILogger logger = null)
        {
            this.ftpUserName = user;
            this.ftpPassword = password;
            this.ftpRootPath = server.TrimEnd('/') + "/";
            this.Logger = logger;
        }

        private void Debug(string msg, params string[] args)
        {
            if (Logger == null) return;

            Logger.Debug(msg, args);
        }

        private void Warn(string msg, params string[] args)
        {
            if (Logger == null) return;

            Logger.Warning(msg, args);
        }

        public string UploadFile(string source)
        {
            Debug("要上傳的文件為: {0}，FTP根目錄為： {1}", source, ftpRootPath);
            string fileName = GenerateFileName(source);
            Debug("動態生成的文件名為： {0}", fileName);
            var result = UploadFile(fileName, source);
            Debug("上傳成功！返回的文件路徑為：{0}", fileName);
            return fileName;
        }

        public byte[] DownloadFile(string source)
        {
            Debug("讀取FTP上的文件：{0}", source);
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRootPath + source);
            req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                using (Stream ftpStream = response.GetResponseStream())
                using (var memoryStream = new MemoryStream())
                {
                    ftpStream.CopyTo(memoryStream);
                    response.Close();
                    req.Abort();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception e)
            {
                Warn("讀取文件時發生錯誤！{0}", e.Message);
                req.Abort();
                return null;
            }
        }

        public bool DeleteFile(string source)
        {
            Debug("刪除FTP上的文件：{0}", source);
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRootPath + source);
            req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
                Warn("刪除文件時發生錯誤！{0}", e.Message);
                req.Abort();
                return false;
            }
            req.Abort();
            Debug("刪除成功！");
            return true;
        }

        #region private methods
        /// <summary>
        /// 上傳文件到FTP指定目錄
        /// </summary>
        /// <param name="ftpFileName">FTP指定的目錄，包含文件全名</param>
        /// <param name="source"></param>
        private bool UploadFile(string ftpFileName, string source)
        {
            CreateFtpDirectory(ftpFileName);
            FileInfo fi = new FileInfo(source);
            FileStream fs = fi.OpenRead();
            long length = fs.Length;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRootPath + ftpFileName);
            req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.ContentLength = length;
            req.Timeout = 10 * 1000;
            try
            {
                Stream stream = req.GetRequestStream();
                int BufferLength = 4096;
                byte[] b = new byte[BufferLength];
                int i;
                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                {
                    stream.Write(b, 0, i);
                }
                stream.Close();
                stream.Dispose();
            }
            catch (Exception e)
            {
                Logger.Error("上傳文件時發生錯誤！", e);
                return false;
            }
            finally
            {
                fs.Close();
                req.Abort();
            }
            req.Abort();
            return true;
        }

        /// <summary>
        /// 根據當前時間生成文件路徑和文件名。類似：
        /// 2014/06/17/guid.png
        /// </summary>
        /// <param name="source">要上傳文件的絕對路徑</param>
        /// <returns></returns>
        private string GenerateFileName(string source)
        {
            var extension = Path.GetExtension(source);
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            var guid = Guid.NewGuid().ToString();
            return now + "/" + guid + extension;
        }

        private void CreateFtpDirectory(string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        if (!CheckIfDirectoryExists(curDir))
                            MakeDirectory(curDir);
                    }
                    catch (Exception e)
                    {
                        Logger.Error("創建FTP目錄時出錯！", e);
                    }
                }
            }
        }

        private static string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        /// <summary>
        /// 檢查FTP服務器上，指定的路徑是否存在
        /// </summary>
        /// <param name="localFile"></param>
        /// <returns></returns>
        private bool CheckIfDirectoryExists(string localFile)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRootPath + localFile);
            req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
                Warn("FTP目錄 '{0}' 不存在！錯誤信息：{1}", localFile, e.Message);
                req.Abort();
                return false;
            }
            Debug("存在FTP目錄：{0}", localFile);
            req.Abort();
            return true;
        }

        /// <summary>
        /// 在FTP服務器上創建指定的目錄
        /// </summary>
        /// <param name="localFile"></param>
        /// <returns></returns>
        private bool MakeDirectory(string localFile)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRootPath + localFile);
            req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
                Warn("創建FTP目錄： '{0}'失敗！ 錯誤信息：{1}", localFile, e.Message);
                req.Abort();
                return false;
            }
            Debug("創建FTP目錄： '{0}'成功！", localFile);
            req.Abort();
            return true;
        }
        #endregion
    }
}