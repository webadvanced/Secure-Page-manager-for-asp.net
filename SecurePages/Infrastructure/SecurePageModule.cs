namespace SecurePages.Infrastructure {
    using System;
    using System.Web;

    using SecurePages.Services;

    public class SecurePageModule : IHttpModule {
        #region Public Methods and Operators

        public void Dispose() {
            // do nothing, well.
        }

        public void Init(HttpApplication context) {
            context.BeginRequest += ContextBeginRequest;
        }

        #endregion

        #region Methods

        public static void ContextBeginRequest(object sender, EventArgs e) {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            string url = context.Request.RawUrl;
            bool isSecureUrl = SecurePagesService.IsSecureUrl(url);
            bool isSecureRequest = SecurePagesService.IsSecureRequest(context);
            string httpValue = SecurePagesConfiguration.HttpValue;
            string httpsValue = SecurePagesConfiguration.HttpsValue;
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context, httpValue, httpsValue);
        }
        
        #endregion
    }
}