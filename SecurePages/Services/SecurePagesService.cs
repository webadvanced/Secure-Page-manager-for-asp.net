namespace SecurePages.Services {
    using System;
    using System.Security.Policy;
    using System.Text.RegularExpressions;
    using System.Web;

    using SecurePages.Entities;
    using SecurePages.Infrastructure;

    public class SecurePagesService {
        #region Public Methods and Operators

        public static void HandelRequest(bool isSecureRequest, bool isSecureUrl, HttpContextBase context, Action<HttpContextBase, string> responseHandler = null){
            responseHandler = responseHandler ?? RedirectPermanent;
            // Exit asap if no action is needed
            if ((!isSecureRequest && !isSecureUrl) || (isSecureRequest && isSecureUrl)
                || (context.Request.IsLocal && SecurePagesConfiguration.IgnoreLocalRequests))
            {
                return;
            }

            Uri uri = context.Request.Url;

            // If the url is supposed to be secure and the request is not
            if (isSecureUrl)
            {
                responseHandler(context, SecureUrl(uri));
                return;
            }

            // If the url is not supposed to be secure and the request is
            responseHandler(context, NonSecureUrl(uri));
        }

        public static bool IsSecureRequest(HttpContextBase context) {
            // check custom match rules first
            foreach(var rule in SecurePagesConfiguration.CustomMatchRules) {
                bool result = rule(context);
                if (result) {
                    return true;
                }
            }

            // Check if the request is secure
            return context.Request.IsSecureConnection;
        }

        public static string SecureUrl(Uri  uri)  {
            return string.IsNullOrEmpty(SecurePagesConfiguration.HttpsRootUrl) 
                            ? uri.AbsoluteUri.Replace("http", "https")
                            : string.Format("{0}{1}", SecurePagesConfiguration.HttpsRootUrl, uri.PathAndQuery.TrimStart('/'));
        }

        public static string NonSecureUrl(Uri uri) {
            return string.IsNullOrEmpty(SecurePagesConfiguration.HttpRootUrl)
                            ? uri.AbsoluteUri.Replace("https", "http")
                            : string.Format("{0}{1}", SecurePagesConfiguration.HttpRootUrl, uri.PathAndQuery.TrimStart('/'));
        }

        public static bool IsSecureUrl(string url, Func<string, SecureUrl, bool> matchRegexFunc = null) {
            matchRegexFunc = matchRegexFunc ?? MatchRegex;

            if (string.IsNullOrEmpty(url)) {
                return false;
            }

            bool match = false;

            foreach (SecureUrl secureUrl in SecurePagesConfiguration.Urls) {
                if (secureUrl.MatchType == SecureUrlMatchType.Regex) {
                    match = matchRegexFunc(url, secureUrl);
                }
                else {
                    match = (secureUrl.MatchType == SecureUrlMatchType.CaseInsensitive)
                                ? url.Equals(secureUrl.Url, StringComparison.InvariantCultureIgnoreCase)
                                : url.Equals(secureUrl.Url);
                }
                if (match) {
                    break;
                }
            }

            return match;
        }

        public static bool MatchRegex(string url, SecureUrl secureUrl) {
            return Regex.IsMatch(url, secureUrl.Url, secureUrl.RegexOptions);
        }

        public static void RedirectPermanent(HttpContextBase context, string url) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(url)) {
                throw new ArgumentException("url");
            }
            context.Response.Redirect(url, false);
            context.Response.StatusCode = 301;
            context.Response.Status = "301 Moved Permanently";
            context.Response.End();
        }

        #endregion
    }
}