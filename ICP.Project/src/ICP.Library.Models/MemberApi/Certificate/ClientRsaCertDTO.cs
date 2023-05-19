using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberApi.Certificate
{
    public class ClientRsaCertDTO
    {
        public long CertId { get; set; }

        public string PublicCert { get; set; }

        public string PrivateCert { get; set; }

        public bool IsExpired { get; set; }

        public long ClientCertId { get; set; }

        public string ClientPublicCert { get; set; }

        public long MID { get; set; }

        public string Device { get; set; }

        public DateTime ExpireDT { get; set; }
    }
}
