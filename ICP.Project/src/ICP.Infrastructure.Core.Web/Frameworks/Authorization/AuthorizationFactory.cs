using Autofac.Features.Metadata;
using ICP.Infrastructure.Abstractions.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Frameworks.Authorization
{
    public class AuthorizationFactory : IAuthorizationFactory
    {
        private readonly IEnumerable<Meta<Func<IUserManager>>> _userManagers = null;

        public AuthorizationFactory(IEnumerable<Meta<Func<IUserManager>>> userManagers)
        {
            _userManagers = userManagers;
        }

        public IUserManager Create(AuthorizationType authorizationType)
        {
            return _userManagers.FirstOrDefault(x => authorizationType.Equals(x.Metadata[nameof(AuthorizationType)]))?.Value();
        }
    }
}
