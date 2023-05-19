namespace ICP.Batch.AppRssPush.Models
{
    /// <summary>
    /// 推播回傳內容
    /// </summary>
    public class AppRssResponseContent
    {
        /// <summary>
        /// 追蹤編號
        /// </summary>
        public string traceid { get; set; }
        /// <summary>
        ///存檔狀態 
        /// </summary>
        public int SaveStatus { get; set; }
        /// <summary>
        ///原始狀態 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 回應說明
        /// </summary>
        public string message { get; set; }
    }
}