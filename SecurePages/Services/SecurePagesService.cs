namespace SecurePages.Services {
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;

    using SecurePages.Entities;
    using SecurePages.Infrastructure;

    public class SecurePagesService {
        #region Public Methods and Operators

        public static void HandelRequest(
            bool isSecureRequest,
            bool isSecureUrl,
            HttpContextBase context,
            Action<HttpContextBase, string> responseHandler = null) {
            responseHandler = responseHandler ?? RedirectPermanent;
            // Exit asap if no action is needed
            if ((!isSecureRequest && !isSecureUrl) || (isSecureRequest && isSecureUrl)
                || (context.Request.IsLocal && SecurePagesConfiguration.IgnoreLocalRequests)) {
                return;
            }

            Uri uri = context.Request.Url;

            // If the url is supposed to be secure and the request is not
            if (isSecureUrl) {
                responseHandler(context, SecureUrl(uri));
                return;
            }

            // If the url is not supposed to be secure and the request is
            responseHandler(context, NonSecureUrl(uri));
        }

        public static bool IsIgnoreUrl(string url, Func<string, IUrlBase, bool> matchRegexFunc = null) {
            return MatchUrls(url, SecurePagesConfiguration.Urls.IgnoreUrls, matchRegexFunc);
        }

        public static bool IsSecureRequest(HttpContextBase context) {
            // check custom match rules first
            foreach (var rule in SecurePagesConfiguration.CustomMatchRules) {
                bool result = rule(context);
                if (result) {
                    return true;
                }
            }

            // Check if the request is secure
            return context.Request.IsSecureConnection;
        }

        public static bool IsSecureUrl(string url, Func<string, IUrlBase, bool> matchRegexFunc = null) {
            return MatchUrls(url, SecurePagesConfiguration.Urls.SecureUrls, matchRegexFunc);
        }

        public static bool MatchRegex(string url, IUrlBase secureUrl) {
            return Regex.IsMatch(url, secureUrl.Url, secureUrl.RegexOptions);
        }

        public static string NonSecureUrl(Uri uri) {
            return string.IsNullOrEmpty(SecurePagesConfiguration.HttpRootUrl)
                       ? CompleteUrl("http", uri)
                       : string.Format("{0}{1}", SecurePagesConfiguration.HttpRootUrl, uri.PathAndQuery.TrimStart('/'));
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

        public static string SecureUrl(Uri uri) {
            return string.IsNullOrEmpty(SecurePagesConfiguration.HttpsRootUrl)
                       ? CompleteUrl("https", uri)
                       : string.Format("{0}{1}", SecurePagesConfiguration.HttpsRootUrl, uri.PathAndQuery.TrimStart('/'));
        }

        #endregion

        #region Methods

        protected static string CompleteUrl(string scheme, Uri uri) {
            return string.Format("{0}://{1}{2}", scheme, uri.Host, uri.PathAndQuery);
        }

        protected static bool MatchUrls<T>(
            string url, IList<T> urls, Func<string, IUrlBase, bool> matchRegexFunc = null) where T : IUrlBase {
            matchRegexFunc = matchRegexFunc ?? MatchRegex;

            if (string.IsNullOrEmpty(url)) {
                return false;
            }

            bool match = false;

            foreach (T secureUrl in urls) {
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

        #endregion
    }
}