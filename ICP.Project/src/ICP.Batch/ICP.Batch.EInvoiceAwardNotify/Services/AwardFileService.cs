using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using Ionic.Zip;

namespace ICP.Batch.EInvoiceAwardNotify.Services
{
    using Infrastructure.Abstractions.Logging;
    using Models;
    using Repositories;

    public class AwardFileService
    {
        ILogger _logger;
        ConfigRepository _configRepository;
        private InvoiceAwardRepository _programRepository;

        string UniformCode { get; set; }

        public AwardFileService(
            ILogger<AwardFileService> logger,
            ConfigRepository configRepository, 
            InvoiceAwardRepository programRepository)
        {
            _logger = logger;
            _configRepository = configRepository;
            _programRepository = programRepository;
            UniformCode = _configRepository.UniformCode;
        }

        /// <summary>
        /// 取得解壓密碼
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetPassword(string filePath)
        {
            string year = null;
            string months = null;
            int month;

            try
            {
                #region 使用檔名判斷密碼
                //ex: A_03583704_10802_20190327231947.bin

                // A_03583704_10802_20190327231947
                var name = Path.GetFileNameWithoutExtension(filePath);

                // 10802
                var yyyTerm = name.Split('_')[2];

                // 108
                year = (Convert.ToInt32(yyyTerm.Substring(0, 3)) + 1911).ToString();

                // 2
                month = Convert.ToInt32(yyyTerm.Substring(3, 2));

                if (month < 1 || month > 12) throw new Exception("file name format error");
                #endregion
            }
            catch
            {
                year = DateTime.Today.Year.ToString();
                month = DateTime.Today.Month;
            }

            switch (month)
            {
                case 1:
                case 2:
                    months = "0102";
                    break;
                case 3:
                case 4:
                    months = "0304";
                    break;
                case 5:
                case 6:
                    months = "0506";
                    break;
                case 7:
                case 8:
                    months = "0708";
                    break;
                case 9:
                case 10:
                    months = "0910";
                    break;
                default:
                    months = "1112";
                    break;
            }

            //解密KEY (統編前4碼 + 對獎西元年+月份4碼ex:20150708 + 統編後4碼)
            return UniformCode.Substring(0, 4) + year + months + UniformCode.Substring(4);
        }

        #region 解密中獎清冊檔
        /// <summary>
        /// 解密中獎清冊檔
        /// </summary>
        /// <param name="strAppPath">程式目錄</param>
        /// <param name="strDir">來源目錄</param>
        /// <param name="strOutDir">輸出目錄</param>
        public void DecryptFiles(string strAppPath, string strDir, string strOutDir)
        {
            //檢查並產生批次檔

            _logger.Info("檢查批次檔...");
            string strBATPath = GenerateBAT(strAppPath);

            //中獎清冊檔案路徑
            var listFiles = Directory.GetFiles(strDir).Select(t => new
            {
                filePath = t,
                password = GetPassword(t)

            }).ToList();

            if (listFiles.Count == 0) return;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    //解密程式(批次檔內的路徑，記得要使用絕對路徑)
                    FileName = strBATPath,
                    UseShellExecute = false,
                    ErrorDialog = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                },
            };

            _logger.Info("啟動外部程式...");
            process.Start();

            //解密程式輸出的文字
            string line;

            //解密輸入路徑
            string strInputPath = null;

            //解密輸出路徑
            string strOutputPath = null;

            _logger.Info("檔案解密中...");

            while (listFiles.Count > 0)
            {
                var file = listFiles[0];

                line = TryReadLastLine(process);

                switch (line)
                {
                    case "Enter 1:encrypt or 2:decrypt:"://1.加密 或 2.解密

                        #region 檢查上個迴圈檔案解密是否成功

                        if (!string.IsNullOrEmpty(strOutputPath))
                        {
                            if (File.Exists(strOutputPath))
                            {
                                //解密成功
                            }
                            else
                            {
                                _logger.Info(string.Format("檔案{0}解密失敗", Path.GetFileName(strInputPath)), 0);
                            }

                            strOutputPath = null;

                            listFiles.RemoveAt(0);

                            if (listFiles.Count == 0)
                            {
                                //輸入 q 關閉解密程式
                                process.StandardInput.WriteLine("q");
                                break;
                            }
                        }

                        #endregion

                        process.StandardInput.WriteLine("2");
                        break;

                    case "Enter passphrase:"://輸入解碼KEY
                        process.StandardInput.WriteLine(file.password);
                        break;

                    case "Enter Source File Path:"://輸入來源路徑
                        strInputPath = file.filePath;

                        process.StandardInput.WriteLine(strInputPath);
                        break;

                    case "Enter Target File Path:"://輸入輸出路徑
                        strOutputPath = string.Format(
                            "{0}\\{1}.zip",
                            strOutDir,
                            Path.GetFileNameWithoutExtension(file.filePath));

                        process.StandardInput.WriteLine(strOutputPath);
                        break;
                }

                process.StandardInput.Flush();
            }

            _logger.Info("檔案解密結束，關閉外部程式...");

            //等待關閉並釋放資源
            process.WaitForExit();
            process.Close();
            process.Dispose();
            process = null;
        }
        #endregion

        #region 嘗試讀取最後一行
        /// <summary>
        /// 嘗試讀取最後一行
        /// </summary>
        /// <param name="process">外部程式</param>
        /// <returns>回傳最後一行</returns>
        private string TryReadLastLine(Process process)
        {
            string line = string.Empty;

            do
            {
                line += (char)process.StandardOutput.Read();
            }
            while (process.StandardOutput.Peek() != -1);

            return line.Split('\n').Last().Trim();
        }
        #endregion

        #region 檢查並產生絕對路徑的批次檔
        /// <summary>
        /// 產生絕對路徑的批次檔
        /// </summary>
        /// <param name="strAppDir">程式目錄</param>
        /// <returns>回傳批次檔路徑</returns>
        private string GenerateBAT(string strAppDir)
        {
            /*
             * 因為程式執行批次檔時
             * 批次檔內容需要絕對路徑才會正常執行
             * 所以為確保執行正常
             * 在執行階段才產生批次檔內容
             * 並將絕對路徑存到設定檔
             * 一但程式目錄被移動並檢查路徑不相同時
             * 或批次檔不見了時
             * 將重新產生批次檔
             */

            //工具絕對路徑
            string strToolPath = string.Format("{0}\\Tools", strAppDir);

            //工具絕對路徑
            string strDecryptToolPath = string.Format("{0}\\Decrypt", strToolPath);

            //批次檔
            string strBATPath = string.Format("{0}\\decrypFile-AbsolutePath.bat", strDecryptToolPath);

            if (!File.Exists(strBATPath))
            {
                string strBATCmd = string.Format(
                    "java -classpath {1}/bin;lib/commons-beanutils-1.4.jar;{1}/lib/commons-beanutils-core-1.7.0.jar;{1}/lib/commons-codec-1.3.jar;{1}/lib/commons-collections-3.2.jar;{1}/lib/commons-configuration-1.4.jar;{1}/lib/commons-jxpath-1.2.jar;{1}/lib/commons-lang-2.3.jar;{1}/lib/commons-logging-1.0.jar;{1}/lib/geinv-kms-dist-1.0.1.jar;{1}/lib/geinv-kms-core-1.0.4.jar;{1}/lib/tv-commons-1.0.4.jar;{1}/lib/tv-config-1.0.2.jar;{1}/lib/tv-logging-core-1.0.3.jar  com.tradevan.geinv.kms.dist.DistKMSService",
                    strToolPath.Replace('\\', '/'),
                    strDecryptToolPath.Replace('\\', '/')
                );

                File.WriteAllText(strBATPath, strBATCmd);
            }

            return strBATPath;
        }
        #endregion

        #region 解壓縮目錄下的檔案
        /// <summary>
        /// 解壓縮目錄下的檔案
        /// </summary>
        /// <param name="strDir">來源目錄</param>
        /// <param name="strOutputDir">輸出目錄</param>
        public void DecompressFiles(string strDir, string strOutputDir)
        {
            _logger.Info("檔案解壓中...");

            foreach (string strFilePath in Directory.GetFiles(strDir, "*.zip", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    using (var zip = ZipFile.Read(strFilePath))
                    {
                        foreach (ZipEntry e in zip)
                        {
                            e.Extract(strOutputDir, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
                catch
                {
                    //解壓失敗
                    _logger.Info(string.Format("檔案{0}解壓失敗", Path.GetFileName(strFilePath)), 0);
                }
            }

            _logger.Info("檔案解壓結束...");
        }
        #endregion

        #region 檔案目錄的內容
        /// <summary>
        /// 檔案目錄的內容
        /// </summary>
        /// <param name="strOutputDir">檔案目錄</param>
        /// <returns>回傳強型別結果</returns>
        public List<Invoice_Data> ReadFiles(string strOutputDir)
        {
            var list = new List<Invoice_Data>();

            var arrFiles = Directory.GetFiles(strOutputDir);

            var dteNow = DateTime.Now;

            _logger.Info("讀取檔案中...");
            foreach (string strFilePath in arrFiles)
            {
                //所有行
                List<string> arrLines = File.ReadAllLines(strFilePath, Encoding.Default).ToList();

                //沒資料換下一個檔案
                if (arrLines.Count == 0) continue;

                //最後一行為footer
                string strLastLine = arrLines.Last();

                //移除最後一行，剩下都為body
                arrLines.RemoveAt(arrLines.Count - 1);

                //沒資料換下一個檔案
                if (arrLines.Count == 0) continue;

                string strLine;

                byte[] bytLine;

                var data = new Invoice_Data
                {
                    master = new Invoice_Award(),
                    details = new List<Invoice_AwardDetail>()
                };
                list.Add(data);

                #region 讀取主檔-格式請參閱 消費通路中領獎清冊-V2.0.2.pdf

                strLine = arrLines[0];

                bytLine = Encoding.Default.GetBytes(strLine);

                Invoice_Award master = data.master;

                master.TMer_Identifier = TakeBytes2Str(bytLine, 0, 10);

                master.Type = Path.GetFileNameWithoutExtension(strFilePath).Substring(0, 1);

                master.FileName = Path.GetFileName(strFilePath);

                master.Main = strLastLine.Substring(0, 1);

                master.YearMonth = strLastLine.Substring(1, 5);

                master.Total_Count = int.Parse(strLastLine.Substring(6, 7));

                master.TotalPrize_Amount = strLastLine.Substring(13, 15).TrimStart('0');

                master.AwardBegin_Time = Convert.ToDateTime(
                    string.Format("{0}/{1}/{2}",
                    1911 + int.Parse(strLastLine.Substring(28, 3)),
                    strLastLine.Substring(31, 2),
                    strLastLine.Substring(33, 2)
                    ));

                master.AwardEnd_Time = Convert.ToDateTime(
                    string.Format("{0}/{1}/{2} 23:59:59.997",
                    1911 + int.Parse(strLastLine.Substring(35, 3)),
                    strLastLine.Substring(38, 2),
                    strLastLine.Substring(40, 2)
                    ));

                master.Download_Time = dteNow;

                #endregion

                #region 讀取內容-格式請參閱 消費通路中領獎清冊-V2.0.2.pdf

                for (int i = 0; i < arrLines.Count; i++)
                {
                    strLine = arrLines[i];

                    //沒資料換下一行
                    if (string.IsNullOrEmpty(strLine))
                        continue;

                    //行數位置要用byte來判斷，中文佔 2 Byte
                    bytLine = Encoding.Default.GetBytes(strLine);

                    //資料長度不夠下一行
                    if (bytLine.Length < 474)
                    {
                        _logger.Info(string.Format("第{0}行資料長度不夠 {0}", i + 1, strFilePath), 1);
                        continue;
                    }

                    var detail = new Invoice_AwardDetail();

                    detail.Number_ID = TakeBytes2Str(bytLine, 15, 2);
                    detail.Number = TakeBytes2Str(bytLine, 17, 8);
                    detail.Mer_Name = TakeBytes2Str(bytLine, 25, 60);
                    detail.Mer_Identifier = TakeBytes2Str(bytLine, 85, 10);
                    detail.Mer_Address = TakeBytes2Str(bytLine, 95, 100);
                    detail.Invoice_Time = Convert.ToDateTime(TakeBytes2Str(bytLine, 195, 10) + " " + TakeBytes2Str(bytLine, 205, 8));
                    detail.Sales_Amount = TakeBytes2Str(bytLine, 213, 12).TrimStart('0');
                    detail.Carrier_Type = TakeBytes2Str(bytLine, 225, 6);
                    detail.Carrier_Name = TakeBytes2Str(bytLine, 231, 60);
                    detail.CarrierId_Clear = TakeBytes2Str(bytLine, 291, 64);
                    detail.CarrierId_Hide = TakeBytes2Str(bytLine, 355, 64);
                    detail.Random_Number = TakeBytes2Str(bytLine, 419, 4);
                    detail.Prize_Type = TakeBytes2Str(bytLine, 423, 1);
                    detail.Prize_Amount = TakeBytes2Str(bytLine, 424, 10).TrimStart('0');
                    detail.Customer_Identifier = TakeBytes2Str(bytLine, 434, 10);
                    detail.Platform_Deposit_Mark = TakeBytes2Str(bytLine, 444, 1);

                    detail.Exception_Code = TakeBytes2Str(bytLine, 446, 2).ToUpper();
                    detail.Print_Type = TakeBytes2Str(bytLine, 448, 2).ToUpper();
                    detail.Unique_Identifier = TakeBytes2Str(bytLine, 450, 24);

                    detail.Create_Time = dteNow;

                    data.details.Add(detail);
                }

                #endregion
            }
            _logger.Info("讀取檔案結束...");

            return list;
        }
        #endregion

        #region 提取陣列中部份
        /// <summary>
        /// 提取陣列中部份bytes轉回字串
        /// </summary>
        /// <param name="bytes">陣列bytes</param>
        /// <param name="startIndex">開始位置</param>
        /// <param name="count">數量</param>
        /// <returns>回傳部份bytes轉字串</returns>
        private string TakeBytes2Str(byte[] bytes, int startIndex, int count)
        {
            bytes = bytes.Skip(startIndex).Take(count).ToArray();

            return Encoding.Default.GetString(bytes).Trim();
        }
        #endregion

        #region 儲存至資料庫
        /// <summary>
        /// 儲存至資料庫
        /// </summary>
        /// <param name="list">中獎清冊資料</param>
        public void SaveToDB(List<Invoice_Data> list)
        {

            try
            {
                _logger.Info("儲存至資料庫中...");

                foreach (var data in list)
                {
                    var master = data.master;

                    var result = _programRepository.AddInvoiceAward(master);

                    if (!result.IsSuccess)
                    {
                        if (result.RtnCode == 160002)
                            _logger.Info($"{master.FileName} 重覆新增");
                        else
                            _logger.Error($"{master.FileName} 新增失敗");

                        continue;
                    }

                    foreach (var invoice in data.details)
                    {
                        invoice.Sys_ID = master.Sys_ID;

                        var result2 = _programRepository.AddInvoiceAwardDetail(invoice);

                        if (!result.IsSuccess)
                            _logger.Info(string.Format("{0}的{1}新增失敗", data.master.FileName, invoice.Unique_Identifier), 0);
                    }
                }

                _logger.Info("儲存至資料庫中結束");
            }
            catch (Exception ex)
            {
                _logger.Info(string.Format("儲存至資料庫失敗：{0}", ex.Message), 0);
            }
        }
        #endregion
    }
}
