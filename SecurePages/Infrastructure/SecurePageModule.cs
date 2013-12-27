namespace SecurePages.Infrastructure {
    using System;
    using System.Web;

    using SecurePages.Entities;
    using SecurePages.Services;

    public class SecurePageModule : IHttpModule {
        #region Constructors and Destructors

        static SecurePageModule() {
            IsIgnoreUrlFunc = SecurePagesService.IsIgnoreUrl;
            IsSecureUrlFunc = SecurePagesService.IsSecureUrl;
        }

        #endregion

        #region Public Properties

        public static Func<Uri, Func<Uri, IUrlBase, bool>, bool> IsIgnoreUrlFunc { get; set; }

        public static Func<Uri, Func<Uri, IUrlBase, bool>, bool> IsSecureUrlFunc { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void ContextBeginRequest(object sender, EventArgs e) {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            Uri uri = context.Request.Url;
            bool shouldIgnore = IsIgnoreUrlFunc.Invoke(uri, null);
            if (shouldIgnore) {
                return;
            }
            bool isSecureUrl = IsSecureUrlFunc.Invoke(uri, null);
            bool isSecureRequest = SecurePagesService.IsSecureRequest(context);
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context);
        }

        public void Dispose() {
            // do nothing, well.
        }

        public void Init(HttpApplication context) {
            context.BeginRequest += ContextBeginRequest;
        }

        #endregion
    }
}