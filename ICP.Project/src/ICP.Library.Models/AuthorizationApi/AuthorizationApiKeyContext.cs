using ICP.Library.Models.MemberApi.Certificate;

namespace ICP.Library.Models.AuthorizationApi
{
    public class AuthorizationApiKeyContext
    {
        public ClientAesCertDTO ClientAesCert { get; set; }

        public ClientRsaCertDTO ClientRsaCert { get; set; }
    }
}
