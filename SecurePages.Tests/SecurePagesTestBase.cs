namespace SecurePages.Tests {
    using System;

    public class SecurePagesTestBase {
        private const string BaseUrl = "http://mock.us/";
        private const string BaseUrlSecure = "https://mock.us/";
        
        protected Uri AbsoluteUri(string relativeUrl) {
            return new Uri(BaseUrl + CleanRelativeUrl(relativeUrl));
        }
        protected Uri AbsoluteUriSecure(string relativeUrl) {
            return new Uri(BaseUrlSecure + CleanRelativeUrl(relativeUrl));
        }

        private string CleanRelativeUrl(string relativeUrl) {
            return relativeUrl.StartsWith("/") ? relativeUrl.TrimStart('/') : relativeUrl;
        }
    }
}