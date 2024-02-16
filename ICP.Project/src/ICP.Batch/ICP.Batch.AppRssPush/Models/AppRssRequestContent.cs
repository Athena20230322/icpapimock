namespace ICP.Batch.AppRssPush.Models
{
    /// <summary>
    /// 推播Request
    /// </summary>
    public class AppRssRequestContent
    {
        /// <summary>
        /// 追蹤編號
        /// </summary>
        public string traceid { get; set; }
        /// <summary>
        /// 發動來源
        /// </summary>
        public string source_name { get; set; }
        /// <summary>
        /// 推播對象ID
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 推播內文
        /// </summary>
        public string alert { get; set; }
        /// <summary>
        /// 推播超連結
        /// </summary>
        public string hyper_link { get; set; }
        /// <summary>
        /// APP功能ID
        /// </summary>
        public string functionid { get; set; }
        /// <summary>
        /// 其他參數
        /// </summary>
        public string param { get; set; }
        /// <summary>
        /// mask欄位
        /// </summary>
        public string mask { get; set; }
    }
}