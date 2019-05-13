namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;

    public class FakeHttpContext : HttpContextBase
    {
        private readonly FakePrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpContext(FakePrincipal Principal, NameValueCollection FormParams, NameValueCollection QueryStringParams, HttpCookieCollection Cookies, SessionStateItemCollection SessionItems )
        {
            _principal = Principal;
            _formParams = FormParams;
            _queryStringParams = QueryStringParams;
            _cookies = Cookies;
            _sessionItems = SessionItems;
        }

        public override HttpRequestBase Request
        {
            get
            {
                return new FakeHttpRequest(_formParams, _queryStringParams, _cookies);
            }
        }

        public override IPrincipal User
        {
            get
            {
                return _principal;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(_sessionItems);
            }
        }

    }


}
