namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System;
    using System.Security.Principal;


    public class FakeIdentity : IIdentity
    {
        private readonly string _name;

        public FakeIdentity(string UserName)
        {
            _name = UserName;

        }

        public string AuthenticationType
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return !String.IsNullOrEmpty(_name); }
        }

        public string Name
        {
            get { return _name; }
        }

    }


}
