using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
   public class UserDataType
    {
        public const string AuthorizationApiKeyContext = nameof(AuthorizationApiKeyContext);

        public const string EncryptData = "EncryptData";

        public const string DecryptData = "DecryptData";

        public const string MID = "MID";

        public const string IsLogin = "IsLogin";

        public const string OPMID = "OPMID";

        public const string AppTokenID = "AppTokenID";
    }
}
