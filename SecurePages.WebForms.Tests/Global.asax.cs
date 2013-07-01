namespace SecurePages.WebForms.Tests {
    using System;
    using System.Web;
    using System.Web.Optimization;

    using SecurePages.Infrastructure;
    using SecurePages.WebForms.Tests.App_Start;

    public class Global : HttpApplication {
        #region Methods

        private void Application_End(object sender, EventArgs e) {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e) {
            // Code that runs when an unhandled error occurs
        }

        private void Application_Start(object sender, EventArgs e) {
            // Code that runs on application startup
            SecurePagesConfig.RegisterUrls(SecurePagesConfiguration.Urls);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
        }

        #endregion
    }
}