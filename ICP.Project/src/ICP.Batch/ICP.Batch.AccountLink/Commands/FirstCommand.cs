using ICP.Batch.AccountLink.Enums;
using ICP.Batch.AccountLink.Models.First;
using ICP.Batch.AccountLink.Services;
using ICP.Infrastructure.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ICP.Batch.AccountLink.Commands
{
    public class FirstCommand : BaseCommand
    {
        private FirstService _firstService = null;

        public FirstCommand(
             ILogger<FirstCommand> logger,
             FirstService firstService,
             EMailNotifyService eMailNotifyService
            )
        {
            _logger = logger;
            _firstService = firstService;
            _eMailNotifyService = eMailNotifyService;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public override void exec()
        {
            _logger.Info($"執行開始");
            execWithTryCatch(SaveTradeData);
            _logger.Info($"執行結束");
            _logger.Info($"========================================");
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
                _logger.Warning(ex.ToString());
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// 儲存明細資料
        /// </summary>
        private void SaveTradeData()
        {
            int iNowFileCount = 0;      //目前執行的檔案檔案數
            int iNowReadLineCount = 0;  //目前執行的資料筆數
            int iFailCount = 0;         //執行失敗的資料筆數
            TradeModel bankSource = new TradeModel();//銀行資料物件
            TradeDbReq dbReq;//儲存請求
            List<FileInfo> listFiles = new List<FileInfo>(); //符合格式的檔案

            _logger.Info($"下載檔案");

            #region 先將檔案搬移至本地
            var moveAllFilesResult = _firstService.MoveAllFiles(_firstService.filePattern, _firstService.fileExtension, _firstService.DownloadPath, MoveFileType.Process);
            if (!moveAllFilesResult.IsSuccess)
            {
                _logger.Warning(moveAllFilesResult.RtnMsg, moveAllFilesResult);
                _eMailNotifyService.SendErrorEmail(moveAllFilesResult.RtnData);
                return;
            }
            #endregion

            #region 取出符合格式的檔案
            var listFilesResult = _firstService.GetFiles(_firstService.filePattern, _firstService.ProcessPath);
            if (!listFilesResult.IsSuccess)
            {
                _logger.Warning(listFilesResult.RtnMsg, listFilesResult);
                _eMailNotifyService.SendErrorEmail(listFilesResult.RtnMsg);
                return;
            }
            listFiles = listFilesResult.RtnData;
            #endregion

            #region 讀取檔案
            foreach (var file in listFiles)
            {
                string fileName = file.Name.Replace("." + _firstService.fileExtension, "");
                if (Regex.IsMatch(fileName, _firstService.filePattern, RegexOptions.None))
                {
                    iNowFileCount++;
                    _logger.Info($"讀取檔案({iNowFileCount}/{listFiles.Count})：{file.Name}");

                    StreamReader srFile = new StreamReader(_firstService.ProcessPath + file.Name, Encoding.GetEncoding(950));
                    StringBuilder sbFailMsg = new StringBuilder();

                    try
                    {
                        List<string> listReadLines = new List<string>();
                        while (!srFile.EndOfStream)
                        {
                            listReadLines.Add(srFile.ReadLine());
                        }

                        foreach (var lineData in listReadLines)
                        {
                            if (lineData.Trim() == "")
                            {
                                continue;
                            }

                            iNowReadLineCount++;

                            //讀取每筆資料,拆解為欄位依序記入物件
                            bankSource = _firstService.BuildBankSourceObj(bankSource, lineData, _firstService.arrColLength);

                            //將原始物件處理後放入要儲存的物件
                            var dbReqResult = _firstService.BuildRecordRequest(bankSource);
                            if (!dbReqResult.IsSuccess)
                            {
                                iFailCount++;
                                _logger.Warning($"--記錄失敗({listReadLines.Count}-{iNowReadLineCount})：{dbReqResult.RtnMsg}({dbReqResult.RtnCode})");
                                sbFailMsg.AppendFormat("‧{0}-{1}：{2}({3})<br/>", listReadLines.Count, iNowReadLineCount, dbReqResult.RtnMsg, dbReqResult.RtnCode);
                                continue;
                            }

                            dbReq = dbReqResult.RtnData;
                            dbReq.FileName = fileName;

                            //存入DB
                            var createRecordResult = _firstService.CreateRecord(dbReq);
                            if (!createRecordResult.IsSuccess)
                            {
                                iFailCount++;
                                _logger.Warning($"--記錄失敗({listReadLines.Count}-{iNowReadLineCount})：{createRecordResult.RtnMsg}({createRecordResult.RtnCode})");
                                sbFailMsg.AppendFormat("‧{0}-{1}：{2}({3})<br/>", listReadLines.Count, iNowReadLineCount, createRecordResult.RtnMsg, createRecordResult.RtnCode);
                                continue;
                            }

                            _logger.Info($"--記錄成功({listReadLines.Count}-{iNowReadLineCount})");

                            //記錄完最後一筆且沒有失敗筆數 搬移檔案
                            if (iFailCount == 0 && iNowReadLineCount == listReadLines.Count)
                            {
                                _firstService.MoveFile(file.Name, _firstService.ProcessPath, MoveFileType.Success);
                            }
                        }

                        iNowReadLineCount = 0;

                        #region 記錄結束收尾處理

                        srFile.Close();

                        if (listReadLines.Count == 0)
                        {
                            _logger.Info($"檔案無資料");
                            //搬移檔案
                            _firstService.MoveFile(file.Name, _firstService.ProcessPath, MoveFileType.Success);
                        }
                        else if (iFailCount > 0)
                        {
                            iFailCount = 0;

                            _eMailNotifyService.SendErrorEmail(sbFailMsg.ToString(), true);

                            //搬移檔案
                            _firstService.MoveFile(file.Name, _firstService.ProcessPath, MoveFileType.Fail);
                        }

                        _logger.Info($"全部作業完成");

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        srFile.Close();

                        _logger.Warning($"執行失敗(非預期狀況)({iNowFileCount}/{ listFiles.Count})：{ex.ToString()}");
                        _eMailNotifyService.SendErrorEmail(ex);

                        //搬移檔案
                        _firstService.MoveFile(file.Name, _firstService.ProcessPath, MoveFileType.Fail);
                    }

                    srFile.Dispose();
                }
            }

            if (iNowFileCount == 0)
            {
                _logger.Info($"查無檔案");
            }
            #endregion
        }
    }
}
