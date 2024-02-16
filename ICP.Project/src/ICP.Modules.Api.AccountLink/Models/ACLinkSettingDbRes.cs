namespace ICP.Modules.Api.AccountLink.Models
{
    /// <summary>
    /// AccountLink相關設定回應
    /// </summary>
    public class ACLinkSettingDbRes
    {
        public string ACLinkKey { get; set; }
        public string ACLinkValue { get; set; }
        public string BankCode { get; set; }
    }
}
