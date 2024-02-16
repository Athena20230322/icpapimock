using ICP.Batch.AccountLink.Enums;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace ICP.Batch.AccountLink.Services
{
    public class BaseService : ConfigService
    {
        protected ILogger _logger = null;

        #region 檔案處理

        /// <summary>
        /// 取出符合格式的檔案
        /// </summary>
        /// <param name="filePattern">檔案格式</param>
        /// <param name="processPath">來源路徑</param>
        /// <param name="fileExtension">副檔名</param>
        /// <returns></returns>
        public DataResult<List<FileInfo>> GetFiles(string filePattern, string processPath, string fileExtension = "txt")
        {
            var result = new DataResult<List<FileInfo>>();
            result.SetError();

            try
            {
                //檢查目標路徑是否存在
                if (!Directory.Exists(processPath))
                {
                    Directory.CreateDirectory(processPath);
                }

                FileInfo[] arrFiles = new DirectoryInfo(processPath).GetFiles("*." + fileExtension, SearchOption.TopDirectoryOnly);    //取得資料交換路徑裡所有檔案
                List<FileInfo> listFiles = arrFiles.Where<FileInfo>(x => Regex.IsMatch(x.Name.ToUpper().Replace("." + fileExtension.ToUpper(), ""), filePattern, RegexOptions.None)).ToList(); //先過濾掉不合的txt

                result.SetSuccess(listFiles);
            }
            catch (Exception ex)
            {
                result.SetCode(7499);
                result.RtnMsg = ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// 搬移目前目錄中指定檔案
        /// </summary>
        /// <param name="fileName">檔案名稱(含副檔名)</param>
        /// <param name="fromPath">來源路徑</param>
        /// <param name="moveFileType">搬移類型</param>
        /// <param name="isDeleteSourceFile">是否刪除來源檔案</param>
        public void MoveFile(string fileName, string fromPath, MoveFileType moveFileType, bool isDeleteSourceFile = true)
        {
            string sMovePath = string.Empty;

            #region 搬移路徑

            switch (moveFileType)
            {
                default:
                    sMovePath = ProcessPath;
                    break;
                case MoveFileType.Zip:
                    sMovePath = ZipPath;
                    break;
                case MoveFileType.Success:
                    sMovePath = SuccessPath;
                    break;
                case MoveFileType.Fail:
                    sMovePath = FailPath;
                    break;
            }

            #endregion

            #region 檢查目標路徑是否存在

            if (!Directory.Exists(sMovePath))
            {
                Directory.CreateDirectory(sMovePath);
            }

            #endregion

            #region 搬移

            if (File.Exists(fromPath + fileName))
            {
                string sNewFileName = fileName;

                if (File.Exists(sMovePath + fileName))
                {
                    //目標檔名重覆加時間序號
                    string[] sName = fileName.Split('.');
                    sNewFileName = string.Format(@"{0}_{1}.{2}", sName[0], DateTime.Now.ToString("MMddHHmmss"), sName[1]);

                    //移動檔案,目標檔案存在則先刪除
                    ///File.Delete(sMovePath + fileName);
                }

                if (isDeleteSourceFile)
                {
                    File.Move(fromPath + fileName, sMovePath + sNewFileName);
                }
                else
                {
                    File.Copy(fromPath + fileName, sMovePath + sNewFileName);
                }
            }

            #endregion 
        }

        /// <summary>
        /// 搬移目前目錄中指定格式所有檔案
        /// </summary>
        /// <param name="filePattern">檔案格式</param>
        /// <param name="extension">副檔名</param>
        /// <param name="fromPath">來源路徑</param>
        /// <param name="moveFileType">搬移類型</param>
        public DataResult<string> MoveAllFiles(string filePattern, string extension, string fromPath, MoveFileType moveFileType)
        {
            var result = new DataResult<string>();
            result.SetError();

            try
            {
                //檢查目標路徑是否存在
                if (!Directory.Exists(fromPath))
                {
                    Directory.CreateDirectory(fromPath);
                }

                FileInfo[] arrFiles = new DirectoryInfo(fromPath).GetFiles(string.Format(@"*.{0}", extension), SearchOption.TopDirectoryOnly); //取得資料交換路徑裡所有符合副檔名的檔案

                foreach (var oFile in arrFiles)
                {
                    //搬移符合格式的檔案
                    if (Regex.IsMatch(oFile.Name.Replace("." + extension, ""), filePattern, RegexOptions.None))
                    {
                        MoveFile(oFile.Name, fromPath, moveFileType);
                    }
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetCode(7499);
                result.RtnData = ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        /// <param name="filePattern">要壓縮的檔案格式</param>
        /// <param name="fromPath">來源路徑</param>
        /// <param name="saveFullName">存放路徑檔名</param>
        /// <param name="password">密碼</param>
        public void ZipFiles(string filePattern, string fromPath, string saveFullName, string password = null)
        {
            string sNewFileName = saveFullName;

            #region 檢查目標路徑是否存在

            if (!Directory.Exists(fromPath))
            {
                Directory.CreateDirectory(fromPath);
            }

            #endregion

            if (File.Exists(saveFullName))
            {
                //目標檔名重覆加時間序號
                string[] sName = saveFullName.Split('.');
                sNewFileName = string.Format(@"{0}_{1}.{2}", sName[0], DateTime.Now.ToString("MMddHHmmss"), sName[1]);

                //移動檔案,目標檔案存在則先刪除
                ///File.Delete(sMovePath + fileName);
            }

            using (ZipFile oZipFile = new ZipFile(sNewFileName))
            {
                if (!string.IsNullOrEmpty(password))
                {
                    oZipFile.Password = password;
                }

                FileInfo[] arrFiles = new DirectoryInfo(fromPath).GetFiles("*.txt", SearchOption.TopDirectoryOnly); //取得資料交換路徑裡所有符合副檔名的檔案

                foreach (var oFile in arrFiles)
                {
                    if (Regex.IsMatch(oFile.Name, filePattern, RegexOptions.None))
                    {
                        oZipFile.AddFile(oFile.FullName, string.Empty);
                    }
                }

                oZipFile.Save();
            }
        }

        /// <summary>
        /// 解壓縮檔案(指定目錄)
        /// </summary>
        /// <param name="filePattern">要解壓縮的檔案格式</param>
        /// <param name="fromPath">來源路徑</param>
        /// <param name="extractPath">解壓縮檔案存放路徑</param>
        /// <param name="password">密碼</param>
        public void UnZipFiles(string filePattern, string fromPath, string extractPath, string password = null)
        {
            #region 檢查目標路徑是否存在

            if (!Directory.Exists(fromPath))
            {
                Directory.CreateDirectory(fromPath);
            }

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            #endregion

            FileInfo[] arrFiles = new DirectoryInfo(fromPath).GetFiles("*.zip", SearchOption.TopDirectoryOnly); //取得資料交換路徑裡所有符合副檔名的檔案

            try
            {
                foreach (var oFile in arrFiles)
                {
                    using (ZipFile oZipFile = ZipFile.Read(oFile.FullName))
                    {
                        if (Regex.IsMatch(oFile.Name.Replace(".zip", ""), filePattern, RegexOptions.None))
                        {
                            if (!string.IsNullOrEmpty(password))
                            {
                                oZipFile.Password = password;
                            }

                            foreach (ZipEntry e in oZipFile)
                            {
                                e.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);
                            }

                            foreach (ZipEntry e in oZipFile)
                            {
                                //zip檔和裡面的txt檔要同名
                                if (oFile.Name.Split('.')[0] != e.FileName.Split('.')[0])
                                {
                                    Exception ex = new Exception(string.Format(@"檔名不符：{0} -> {1}", oFile.Name, e.FileName));
                                    throw ex;
                                }
                                else
                                {
                                    e.Extract(extractPath, ExtractExistingFileAction.Throw);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 解壓縮檔案(指定檔案)
        /// </summary>
        /// <param name="directoryName">來源目錄完整路徑</param>
        /// <param name="fileName">來源檔案名稱</param>
        /// <param name="extractPath">解壓縮檔案存放路徑</param>
        /// <param name="password">密碼</param>
        public void UnZipFile(string directoryName, string fileName, string extractPath, string password = null)
        {
            #region 檢查目標路徑是否存在

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            #endregion

            using (ZipFile oZipFile = ZipFile.Read(string.Format(@"{0}\{1}", directoryName, fileName)))
            {
                if (!string.IsNullOrEmpty(password))
                {
                    oZipFile.Password = password;
                }

                foreach (ZipEntry e in oZipFile)
                {
                    //zip檔和裡面的txt檔要同名
                    if (fileName.Split('.')[0] != e.FileName.Split('.')[0])
                    {
                        Exception ex = new Exception(string.Format(@"檔名不符：{0} -> {1}", fileName, e.FileName));
                        throw ex;
                    }
                    else
                    {
                        e.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 將讀取的資料建立為物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">接值型別</param>
        /// <param name="readLine">要分解的記錄字串</param>
        /// <param name="colLength">各欄位長度</param>
        /// <returns></returns>
        public T BuildBankSourceObj<T>(T item, string readLine, int[] colLength)
        {
            //將物件property依照設定排序
            var oOrderItem = item.GetType().GetProperties().OrderBy(p => ((DataMemberAttribute[])p.GetCustomAttributes(typeof(DataMemberAttribute), true)).SingleOrDefault().Order).ToList();
            //接值物件
            var oTargetItem = item;

            #region 資料依序放入物件(字數)

            ///for (int i = 0; i < oOrderItem.Count; i++)
            ///{
            ///    //拆解欄位
            ///    string sColVal = readLine.Substring(colLength.Take(i).Sum(), colLength[i]);
            ///    //給值
            ///    oTargetItem.GetType().GetProperty(oOrderItem[i].Name).SetValue(item, sColVal, null);
            ///}

            #endregion

            #region 資料依序放入物件(字元)

            Char[] cReadLine = readLine.ToCharArray();
            int iNowCharCount = 0;

            for (int i = 0; i < oOrderItem.Count; i++)
            {
                int iTempColLength = 0;//字元數
                for (int j = 0; j < cReadLine.Count(); j++)
                {
                    iTempColLength++;
                    //每個字轉換字元數
                    if (Encoding.GetEncoding(950).GetByteCount(cReadLine[j].ToString()) == 2)
                    {
                        iTempColLength++;
                    }

                    if (iTempColLength == colLength[i])
                    {
                        cReadLine = cReadLine.Skip(j + 1).ToArray();

                        string sColVal = readLine.Substring(iNowCharCount, j + 1).Trim();
                        //給值
                        oTargetItem.GetType().GetProperty(oOrderItem[i].Name).SetValue(item, sColVal, null);

                        iNowCharCount += j + 1;
                    }
                }
            }

            #endregion

            return oTargetItem;
        }

        /// <summary>
        /// 複製資料(複製同名參數的值)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="sourceItem">來源物件</param>
        /// <param name="targetItem">目標物件</param>
        /// <returns></returns>
        public T2 CopyData<T1, T2>(T1 sourceItem, T2 targetItem)
        {
            var oSourceProps = sourceItem.GetType().GetProperties();//來源物件的參數
            var oTargetProps = targetItem.GetType().GetProperties();//目標物件的參數

            //比對相同的參數則複製(目標物件如有特殊型別則排除)
            foreach (var oSourceProp in oSourceProps)
            {
                foreach (var oTargetProp in oTargetProps)
                {
                    if ((oSourceProp.Name == oTargetProp.Name) && !(oTargetProp.PropertyType == typeof(DateTime) || oTargetProp.PropertyType == typeof(DateTime?)))
                    {
                        if (oTargetProp.PropertyType == typeof(bool))
                        {
                            string sBool = oSourceProp.GetValue(sourceItem, null).ToString().Trim().ToLower();
                            oTargetProp.SetValue(targetItem, !(sBool == "n" || sBool == "0" || sBool == ""), null);
                        }
                        else if (oTargetProp.PropertyType == typeof(int))
                        {
                            oTargetProp.SetValue(targetItem, Convert.ToInt32(oSourceProp.GetValue(sourceItem, null).ToString().Trim()), null);
                        }
                        else if (oTargetProp.PropertyType == typeof(long))
                        {
                            oTargetProp.SetValue(targetItem, Convert.ToInt64(oSourceProp.GetValue(sourceItem, null).ToString().Trim()), null);
                        }
                        else if (oTargetProp.PropertyType == typeof(decimal))
                        {
                            oTargetProp.SetValue(targetItem, Convert.ToDecimal(oSourceProp.GetValue(sourceItem, null).ToString().Trim()), null);
                        }
                        else
                        {
                            oTargetProp.SetValue(targetItem, oSourceProp.GetValue(sourceItem, null).ToString().Trim(), null);
                        }

                        break;
                    }
                }
            }

            return targetItem;
        }
    }
}
