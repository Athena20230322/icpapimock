using System;

namespace ICP.Library.Models.AppRssLibrary
{
    public class AppRssContent
    {   
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// OPEN WALLET MID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 發送的優先順序，0為立即發送，其餘按數字由小至大順序發送
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 推播訊息對話方塊之標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 訊息主旨，最長80個全形字
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 通知訊息編號 Share_NotifyMessage_Detail.NotifyMessageID
        /// </summary>
        public int NotifyMessageID { get; set; }

        /// <summary>
        /// 點擊推播開啟指定URL頁面
        /// </summary>
        public string Hyper_link { get; set; }

        /// <summary>
        /// 點擊推播開APP指定頁
        /// </summary>
        public string Functionid { get; set; }

        /// <summary>
        /// APP協議資料
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 環境識別碼
        /// </summary>
        public string Identifier { get; set;}

        /// <summary>
        /// 推播訊息失效時間
        /// </summary>
        public DateTime? ExpireTime { get; set; }

    }
}