using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class AddClientAesCertDTO
    {
        public long RSAKeyClientCertId { get; set; }

        public string AES_Key { get; set; }

        public string AES_IV { get; set; }
    }
}
