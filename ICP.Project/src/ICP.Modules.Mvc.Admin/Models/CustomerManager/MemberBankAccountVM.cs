using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class MemberBankAccountVM
    {
        public string BankName { get; set; }

        public string BankAccount { get; set; }

        public int IsDefault { get; set; }

        public DateTime BankAccountCreateDate { get; set; }

        public long MID { get; set; }

        public string BankCode { get; set; }

        public string BankBranchCode { get; set; }

        public string BankBranchName { get; set; }

        public string AccountName { get; set; }

        public int AccountStatus { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public string AuthMsg { get; set; }        
    }
}
