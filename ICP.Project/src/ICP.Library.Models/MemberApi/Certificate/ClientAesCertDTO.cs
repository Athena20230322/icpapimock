using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberApi.Certificate
{
    public class ClientAesCertDTO
    {
        public long ClientCertId { get; set; }

        public long RSAKeyClientCertId { get; set; }

        public string AES_Key { get; set; }

        public string AES_IV { get; set; }

        public long MID { get; set; }

        public DateTime ExpireDT { get; set; }

        public bool IsExpired { get; set; }
    }
}
