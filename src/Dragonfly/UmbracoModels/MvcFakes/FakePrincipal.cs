namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System.Linq;
    using System.Security.Principal;

    public class FakePrincipal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly string[] _roles;

        public FakePrincipal(IIdentity Identity, string[] Roles)
        {
            _identity = Identity;
            _roles = Roles;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string Role)
        {
            if (_roles == null)
                return false;
            return _roles.Contains(Role);
        }
    }



}
