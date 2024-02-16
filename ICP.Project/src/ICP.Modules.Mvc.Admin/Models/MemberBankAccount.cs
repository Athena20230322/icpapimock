using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class MemberBankAccount : BaseListModel
    {
        public long MID { get; set; }

        public string ICPMID { get; set; }

        public string CName { get; set; }

        public string BankName { get; set; }

        public long AccountID { get; set; }

        public string BankCode { get; set; }

        public string BankAccount { get; set; }

        public string BankBranchCode { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public string AuthMsg { get; set; }

        public byte AccountStatus { get; set; }

        public byte PaperAuthStatus { get; set; }

        public string Modifier { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
