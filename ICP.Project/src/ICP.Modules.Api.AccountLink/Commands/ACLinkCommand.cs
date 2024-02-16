using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Factories;

namespace ICP.Modules.Api.AccountLink.Commands
{
    public class ACLinkCommand
    {
        private readonly ACLinkFactory _acLinkFactory = null;
        private BaseACLinkCommand _baseACLinkCommand = null;

        public ACLinkCommand(
            ACLinkFactory acLinkFactory
            )
        {
            _acLinkFactory = acLinkFactory;
        }

        /// <summary>
        /// 指定來源銀行
        /// </summary>
        /// <param name="bankType"></param>
        public void CreateBank(BankType bankType)
        {
            _baseACLinkCommand = _acLinkFactory.Create(bankType);
        }

        public string Test(string json)
        {
            return _baseACLinkCommand?.Test(json) ?? string.Empty;
        }

        /// <summary>
        /// 綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkBind(string json)
        {
            return _baseACLinkCommand?.ACLinkBind(json) ?? string.Empty;
        }

        /// <summary>
        /// 綁定申請
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkApply(string json)
        {
            return _baseACLinkCommand?.ACLinkApply(json) ?? string.Empty;
        }

        /// <summary>
        /// 解除綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkCancel(string json)
        {
            return _baseACLinkCommand?.ACLinkCancel(json) ?? string.Empty;
        }

        /// <summary>
        /// 綁定結果
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkBindResult(string json)
        {
            return _baseACLinkCommand?.ACLinkBindResult(json) ?? string.Empty;
        }

        /// <summary>
        /// 綁定查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkBindQuery(string json)
        {
            return _baseACLinkCommand?.ACLinkBindQuery(json) ?? string.Empty;
        }

        /// <summary>
        /// 交易(付款)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkPay(string json)
        {
            return _baseACLinkCommand?.ACLinkPay(json) ?? string.Empty;
        }

        /// <summary>
        /// 交易(儲值)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkDeposit(string json)
        {
            return _baseACLinkCommand?.ACLinkDeposit(json) ?? string.Empty;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkRefund(string json)
        {
            return _baseACLinkCommand?.ACLinkRefund(json) ?? string.Empty;
        }

        /// <summary>
        /// 交易查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkPayQuery(string json)
        {
            return _baseACLinkCommand?.ACLinkPayQuery(json) ?? string.Empty;
        }

        /// <summary>
        /// 交易提領
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ACLinkWithdrawal(string json)
        {
            return _baseACLinkCommand?.ACLinkWithdrawal(json) ?? string.Empty;
        }

        /// <summary>
        /// 中國信託端取消綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ChinaTrustCancelBind(string json)
        {
            return _baseACLinkCommand?.ChinaTrustCancelBind(json) ?? string.Empty;
        }
    }
}
