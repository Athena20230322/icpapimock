using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class P11AuthResult : BaseResult
    {
        public long MID { get; set; }

        public string IDNO { get; set; }

        public DateTime IssueDate { get; set; }

        public byte ObtainType { get; set; }

        public string IssueLocationID { get; set; }

        public byte AuthType { get; set; }

        public int AuthStatus { get; set; }

        public string AuthMsg { get; set; }

        public DateTime? Birthday { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public byte Source { get; set; }

        public long RealIP { get; set; }

        public long ProxyIP { get; set; }
    }
}
