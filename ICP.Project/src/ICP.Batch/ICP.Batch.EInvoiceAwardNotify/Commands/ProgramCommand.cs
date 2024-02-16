using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ICP.Batch.EInvoiceAwardNotify.Commands
{
    using ICP.Batch.EInvoiceAwardNotify.Repositories;
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Abstractions.Logging;
    using Services;

    class ProgramCommand
    {
        ILogger<ProgramCommand> _logger;
        ProgramService _programService;
        AwardDownloadService _awardDownloadService;
        AwardFileService _awardFileService;
        ConfigRepository _configRepository;
        GlobalAppSetting _globalAppSetting;
        private invoiceAwardService _invoiceAwardService;

        public ProgramCommand(
            ILogger<ProgramCommand> logger,
            ProgramService programService,
            AwardDownloadService awardDownloadService,
            AwardFileService awardFileService,
            ConfigRepository configRepository,
            GlobalAppSetting globalAppSetting,
            invoiceAwardService invoiceAwardService)
        {
            _logger = logger;
            _programService = programService;
            _awardDownloadService = awardDownloadService;
            _awardFileService = awardFileService;
            _configRepository = configRepository;
            _globalAppSetting = globalAppSetting;
            _invoiceAwardService = invoiceAwardService;
        }


        #region 參數

        //年
        string year;

        //月
        string month;

        //起始日
        string startDate;

        //結束日
        string endDate;

        //執行目錄
        string strAppDir;

        //下載目錄
        string strDownloadDir;

        //解密輸出目錄
        string strDecrypOutputDir;

        //解壓目錄
        string strDecompressDir;

        #endregion

        public DateTime ToDay()
        {
            DateTime today;
            today = DateTime.Today;
#if DEBUG
            today = DateTime.Parse("2019/03/28");
#endif
            return today;
        }


        public void exec()
        {
            //初始
            init();

            if (_configRepository.DownloadAwardNumber)
            {
                //下載至目錄
                _awardDownloadService.Start(year, month, strDownloadDir);

                if (Directory.GetFiles(strDownloadDir).Length == 0)
                {
                    _logger.Info("沒有檔案，結束主程式");
                    return;
                }
            }

            if (_configRepository.DecryptFiles)
            {
                //解密至目錄
                _awardFileService.DecryptFiles(strAppDir, strDownloadDir, strDecrypOutputDir);
            }

            if (_configRepository.DecompressFiles)
            {
                //解壓至目錄
                _awardFileService.DecompressFiles(strDecrypOutputDir, strDecompressDir);
            }

            if (_configRepository.SaveToDB)
            {
                //讀取內容
                var list = _awardFileService.ReadFiles(strDecompressDir);

                //儲存至資料庫
                _awardFileService.SaveToDB(list);
                if (_configRepository.SendNotify)
                {
                    //發送通知
                list.ForEach(x => x.details.ForEach(a => { _invoiceAwardService.SendAwardNotify(a.CarrierId_Clear); }));
                }
            }

            if (_configRepository.InvoiceAwardCheck)
            {
                _invoiceAwardService.InvoiceIssueAwardCheck(startDate, endDate, year +_programService.GetInitMonth(ToDay()) );
                
            }

        }

        private void init()
        {
            #region 參數


            //初始年月
            string[] arrYearMonth = _programService.GetInitYearMonth(ToDay());

            //年
            year = arrYearMonth[0];

            //月
            month = arrYearMonth[1];

            //雙月
            string biMonthly = _programService.GetInitMonth(ToDay());

            //西元年
            int intyear = Convert.ToInt32(year) + 1911;

            //起始日
            startDate = new DateTime(intyear, Convert.ToInt32(biMonthly) - 1, 1).ToString("yyyy/MM/dd 00:00:00");

            //結束日
            endDate = new DateTime(intyear, Convert.ToInt32(biMonthly),
                DateTime.DaysInMonth(intyear, Convert.ToInt32(biMonthly))).ToString("yyyy/MM/dd 23:59:59");

            //執行目錄
            strAppDir = _globalAppSetting.RootPath;

            //下載目錄
            strDownloadDir = string.Format("{0}\\Download\\{1}\\{2}", strAppDir, year, month);

            //解密輸出目錄
            strDecrypOutputDir = string.Format("{0}\\DecrypOutput\\{1}\\{2}", strAppDir, year, month);

            //解壓目錄
            strDecompressDir = string.Format("{0}\\Decompress\\{1}\\{2}", strAppDir, year, month);

            #endregion

            #region 產生目錄

            if (!Directory.Exists(strDownloadDir))
                Directory.CreateDirectory(strDownloadDir);

            if (!Directory.Exists(strDecrypOutputDir))
                Directory.CreateDirectory(strDecrypOutputDir);

            if (!Directory.Exists(strDecompressDir))
                Directory.CreateDirectory(strDecompressDir);

            #endregion
        }
    }
}