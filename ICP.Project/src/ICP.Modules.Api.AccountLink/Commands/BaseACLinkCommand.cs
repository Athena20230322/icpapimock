using ICP.Modules.Api.AccountLink.Interfaces;
using System;

namespace ICP.Modules.Api.AccountLink.Commands
{
    /// <summary>
    /// AccountLink 功能
    /// </summary>
    public abstract class BaseACLinkCommand : IBaseACLinkCommand
    {
        public abstract string ACLinkBind(string json);
        public abstract string ACLinkBindResult(string json);
        public abstract string ACLinkCancel(string json);
        public abstract string ACLinkPay(string json);
        public abstract string ACLinkDeposit(string json);
        public abstract string ACLinkRefund(string json);
        public abstract string ACLinkWithdrawal(string json);
        public abstract string ACLinkBindQuery(string json);
        public abstract string ACLinkPayQuery(string json);

        public virtual string Test(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 綁定申請
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string ACLinkApply(string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 中國信託端取消綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string ChinaTrustCancelBind(string json)
        {
            throw new NotImplementedException();
        }
    }
}
