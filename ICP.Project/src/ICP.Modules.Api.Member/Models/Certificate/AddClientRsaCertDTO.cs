using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class AddClientRsaCertDTO : ValidatableObject
    {
        public long CertId { get; set; }

        public string PublicCert { get; set; }

        public string PrivateCert { get; set; }

        public string ClientPublicCert { get; set; }
    }
}
