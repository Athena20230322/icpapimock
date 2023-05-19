using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.Authorization
{
    public interface IAuthorizationFactory
    {
        IUserManager Create(AuthorizationType authorizationType);
    }
}
