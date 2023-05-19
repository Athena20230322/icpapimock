namespace ICP.Modules.Api.AccountLink.Interfaces
{
    /// <summary>
    /// AccountLink 基本功能
    /// </summary>
    public interface IBaseACLinkCommand
    {
        /// <summary>
        /// 綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkBind(string json);

        /// <summary>
        /// 綁定結果通知
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkBindResult(string json);

        /// <summary>
        /// 解除綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkCancel(string json);

        /// <summary>
        /// 交易付款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkPay(string json);

        /// <summary>
        /// 儲值
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkDeposit(string json);

        /// <summary>
        /// 交易退款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkRefund(string json);

        /// <summary>
        /// 交易提領
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkWithdrawal(string json);

        /// <summary>
        /// 綁定查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkBindQuery(string json);

        /// <summary>
        /// 交易查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string ACLinkPayQuery(string json);
    }
}
