﻿namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System.Collections.Specialized;
    using System.Web;

    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;

        public FakeHttpRequest(NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
        }

        public override NameValueCollection Form
        {
            get
            {
                return _formParams;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _queryStringParams;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

    }



}
