namespace SecurePages.TestHelpers {
    using System;
    using System.Web;

    using SecurePages.Services;

    public class RequestedUrl {
        #region Constants and Fields

        private readonly string _url;

        #endregion

        #region Constructors and Destructors

        public RequestedUrl(string url) {
            _url = url;
        }

        #endregion

        #region Public Methods and Operators

        public static RequestedUrl When(string url) {
            return new RequestedUrl(url);
        }

        public bool ShouldBeHttp() {
            bool isSecureRequest = _url.Contains("https://");
            bool isSecureUrl = SecurePagesService.IsSecureUrl(new Uri(_url));
            string resultUrl = string.Empty;

            SecurePagesService.HandelRequest(
                isSecureRequest,
                isSecureUrl,
                HttpContextBaseFactory(),
                (c, s) => resultUrl = s);

            return resultUrl.Contains("http://") || (resultUrl == string.Empty && !isSecureRequest);
        }

        public bool ShouldBeHttps() {
            bool isSecureRequest = _url.Contains("https://");
            bool isSecureUrl = SecurePagesService.IsSecureUrl(new Uri(_url));
            string resultUrl = string.Empty;

            SecurePagesService.HandelRequest(
                isSecureRequest,
                isSecureUrl,
                HttpContextBaseFactory(),
                (c, s) => resultUrl = s);

            return resultUrl.Contains("https://") || (resultUrl == string.Empty && isSecureRequest);
        }

        public bool ShouldIgnore() {
            return SecurePagesService.IsIgnoreUrl(new Uri(_url));
        }

        #endregion

        #region Methods

        private HttpContextBase HttpContextBaseFactory() {
            return new HttpContextWrapper(new HttpContext(new HttpRequest(null, _url, null), new HttpResponse(null)));
        }

        #endregion
    }
}