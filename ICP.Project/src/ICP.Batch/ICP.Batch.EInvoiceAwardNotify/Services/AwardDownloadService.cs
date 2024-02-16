using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.EInvoiceAwardNotify.Services
{
    using ICP.Infrastructure.Core.Helpers;
    using Infrastructure.Abstractions.Logging;
    using Repositories;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class AwardDownloadService
    {
        ILogger _logger;
        ConfigRepository _configRepository;
        NetworkHelper _networkHelper;
        string _downloadDomain;

        public AwardDownloadService(
            ILogger<AwardDownloadService> logger,
            ConfigRepository configRepository
            )
        {
            _logger = logger;
            _configRepository = configRepository;
            _networkHelper = new NetworkHelper();
            _downloadDomain = "https://www.einvoice.nat.gov.tw";

            //統編
            UniformCode = _configRepository.UniformCode;

            //帳號
            UserName = _configRepository.UserName;

            //密碼
            Password = _configRepository.Password;
        }

        //統編
        string UniformCode { get; set; }
        //帳號
        string UserName { get; set; }
        //密碼
        string Password { get; set; }


        //檢查輸入值
        private void CheckInput(string year, string month)
        {
            string strMsg = string.Empty;

            int intTmp;

            if (string.IsNullOrEmpty(year) || !int.TryParse(month, out intTmp) || intTmp < 0)
                strMsg += "「民國年」輸入錯誤" + Environment.NewLine;

            if (string.IsNullOrEmpty(month))
                strMsg += "請輸入「月份」" + Environment.NewLine;
            else if (month.Length != 4)
                strMsg += "「月份」格式錯誤，ex:七八月為0708" + Environment.NewLine;


            if (!string.IsNullOrEmpty(strMsg))
                throw new Exception(strMsg);
        }

        /// <summary>
        /// 開始下載
        /// </summary>
        /// <param name="year">下載年份(ex:104)</param>
        /// <param name="month">下載月份(ex:0506)</param>
        /// <param name="strDownloadDir">下載路徑(會自動建目錄)</param>
        public void Start(string year, string month, string strDownloadDir)
        {
            try
            {
                _logger.Info("檢查查詢參數...");
                //檢查
                CheckInput(year, month);

                var cookieContainer = new CookieContainer();

                //登入
                _logger.Info("登入中...");
                if (!DoLogin(cookieContainer)) throw new Exception("登入失敗");

                //下載清冊序號
                _logger.Info("下載清冊序號...");
                List<string> lstDownloadSerial = null;
                int count = 0;

                while (lstDownloadSerial == null || !lstDownloadSerial.Any())
                {
                    try
                    {
                        lstDownloadSerial = GetList(cookieContainer, year, month);

                        count += 1;

                        _logger.Info(string.Format("下載清冊序號第{0}次嘗試下載", count));
                    }
                    catch (Exception ex)
                    {
                        _logger.Info(string.Format("下載清冊序號第{0}次下載失敗：{1}", count, ex.Message), 0);
                    }
                }

                if (lstDownloadSerial == null)
                    throw new Exception("下載清冊序號失敗");

                if (lstDownloadSerial.Count > 0)
                {
                    _logger.Info(string.Format("{0}月有{1}個清冊，下載中", month, lstDownloadSerial.Count));

                    if (!System.IO.Directory.Exists(strDownloadDir))
                        System.IO.Directory.CreateDirectory(strDownloadDir);

                    lstDownloadSerial.ForEach(t =>
                    {
                        //嘗試下載3次
                        for (int i = 1; i <= 3; i++)
                            try
                            {
                                GetFile(cookieContainer, t, strDownloadDir);

                                break;
                            }
                            catch (Exception ex)
                            {
                                _logger.Info(string.Format("清冊序號{0}第{1}次下載失敗：{2}", t, i, ex.Message, 0));
                            }
                    });

                    //下載完成
                }
                else
                    _logger.Info(string.Format("{0}月有0個清冊", month));

                //登出
                _logger.Info("登出中...");
                DoLogout(cookieContainer);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message, 0);
            }
        }

        private string GetUrl(string urlPath)
        {
            return _downloadDomain + urlPath;
        }

        //登入
        private bool DoLogin(CookieContainer cookieContainer)
        {
            var nv = new System.Collections.Specialized.NameValueCollection();

            nv.Add("userType", "B");
            nv.Add("loginType", "U");
            nv.Add("loginWay", "W");
            nv.Add("serviceType", "I");
            nv.Add("serial", "");
            nv.Add("pincode", Password);
            nv.Add("signatur", "");
            nv.Add("ban", UniformCode);
            nv.Add("userID", UserName);
            nv.Add("password", Password);
            nv.Add("pid", UserName);
            nv.Add("orgType", "");
            nv.Add("bindata", "");
            nv.Add("typeCheck", "");
            nv.Add("radioB", "0");
            nv.Add("textfield2", UniformCode);
            nv.Add("textfield3", UserName);
            nv.Add("textfield4", Password);
            nv.Add("textfield6", "");
            nv.Add("textfield6", "");

            try
            {
                string strHtml = _networkHelper.DoRequestWithUrlEncode(
                    url: GetUrl("/Login"),
                    data: nv,
                    timeoutSeconds: 60,
                    cookieContainer: cookieContainer,
                    headers: null);
                    

                if (strHtml.IndexOf("統一編號或帳號或密碼錯誤") == -1 &&
                    strHtml.IndexOf("登入失敗") == -1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //登出
        private bool DoLogout(CookieContainer cookieContainer)
        {
            System.Collections.Specialized.NameValueCollection nv = null;

            try
            {
                string strHtml = _networkHelper.DoRequestWithUrlEncode(
                    url: GetUrl("/APMEMBERVAN/GeneralCarrier/SSOut"),
                    data: nv,
                    timeoutSeconds: 60,
                    cookieContainer: cookieContainer,
                    headers: null);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //取得下載序號
        private List<string> GetList(CookieContainer cookieContainer, string year, string months)
        {
            //該月份的下載清冊序號
            List<string> serials;

            //查詢參數
            var nv = new System.Collections.Specialized.NameValueCollection();

            //下載年份
            nv.Add("downloadListQryVO.year", year);

            //下載月份
            nv.Add("downloadListQryVO.month", months);

            //查詢下載清冊
            string strHtml = _networkHelper.DoRequestWithUrlEncode(
                url: GetUrl("/APMEMBERVAN/FileDownload/DownloadList!doDownloadAwardList"),
                data: nv,
                timeoutSeconds: 60,
                cookieContainer: cookieContainer,
                headers: null);

            #region 解析HTML

            #region 假設的HTML

            /** 
             *  將 html 中每一個 downloadFile("47560") 中的數字提取出來
             *  
<table id="downloadListTable" class="lpTb tablesorter" width="98%" border="1" cellspacing="0" cellpadding="0" align="center" frame="hsides" rules="rows">
    <thead>
        <tr><th>序號</th>
            <th>清冊年月</th>
            <th>清冊名稱</th>
            <th>檔案名稱</th>
            <th>產生日期時間</th>
            <th>檔案下載</th>
        </tr>
    </thead>
    <tbody><tr>
                <td>1</td>
                <td>104&#24180;5-6&#26376;</td>
                <td>&#26371;&#21729;&#20013;&#29518;&#28165;&#20874;</td>
                <td>Y_53746788_10406_20150727202140.bin</td>
                <td>2015-07-28</td>
                <td><input type='button' class='btn' value='下載' onclick='downloadFile("47560")'></td>
            </tr>
    </tbody>
</table>
            */

            #endregion

            int index = 0;

            serials = new List<string>();

            strHtml = System.Web.HttpUtility.HtmlDecode(strHtml);

            string strSearchKey = "downloadFile(\"";

            index = strHtml.IndexOf(strSearchKey);

            while (index != -1)
            {
                strHtml = strHtml.Substring(index + strSearchKey.Length);

                index = strHtml.IndexOf('"');

                string vaule = strHtml.Substring(0, index);

                serials.Add(vaule);

                index = strHtml.IndexOf(strSearchKey);
            }

            #endregion

            return serials;
        }

        //取得中獎清冊
        private async void GetFile(CookieContainer cookieContainer, string serial, string downloadDir)
        {
            //查詢參數
            var nv = new Dictionary<string, string>();

            nv.Add("ban", "");
            nv.Add("listTerm", "");
            nv.Add("listType", "");
            nv.Add("signature", "");
            nv.Add("pincodeVal", "");
            nv.Add("seqOfDownloadList", serial);
            var content = new FormUrlEncodedContent(nv);

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(_downloadDomain) })
            {
                var result = client.PostAsync("/APMEMBERVAN/FileDownload/DownloadList!downloadFile", content).Result;
                result.EnsureSuccessStatusCode();

                string strFileName = GetDownloadFileName(result.Content.Headers);
                string strFilePath = downloadDir + "\\" + strFileName;

                //save
                using (Stream 
                    contentStream = result.Content.ReadAsStreamAsync().Result, 
                    stream = new FileStream(strFilePath, FileMode.Create))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }
        }

        /// <summary>
        /// 從 Header 取得檔名
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private string GetDownloadFileName(HttpContentHeaders headers)
        {
            var strContentDisposition = headers.Where(x => x.Key == "Content-Disposition").FirstOrDefault().Value?.FirstOrDefault();
            string strSearchKey = "filename=\"";
            int index;
            index = strContentDisposition.IndexOf(strSearchKey);
            strContentDisposition = strContentDisposition.Substring(index + strSearchKey.Length);
            index = strContentDisposition.IndexOf('"');
            string strFileName = strContentDisposition.Substring(0, index);
            return strFileName;
        }
    }
}