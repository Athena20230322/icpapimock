using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class RsaCertDTO
    {
        public long CertId { get; set; }

        public string PublicCert { get; set; }

        public string PrivateCert { get; set; }

        public bool IsExpired { get; set; }
    }
}
