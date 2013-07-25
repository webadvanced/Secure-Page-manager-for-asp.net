namespace SecurePages.Mvc.Tests.App_Start {
    using System;
    using System.Text.RegularExpressions;

    using SecurePages.Infrastructure;

    public class SecurePagesConfig {
        #region Public Methods and Operators

        public static void RegisterUrls(SecureUrlCollection secureUrls) {
            //Ignore defaults
            secureUrls.IgnoreUrl(@"(.*)\.css");
            secureUrls.IgnoreUrl(@"(.*)\.js");
            secureUrls.IgnoreUrl(@"(.*)\.png");
            secureUrls.IgnoreUrl(@"(.*)\.jpg");
            secureUrls.IgnoreUrl(@"(.*)\.gif");

            // Add urls here
            secureUrls.AddRegex(
                @"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            //Secure Cart
            secureUrls.AddUrl("/cart");

            //Custom rules
            SecurePagesConfiguration.RegisterCustomMatchRule(
                c =>
                string.Equals(
                    c.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase));

#if DEBUG
            //For testing only
            SecurePagesConfiguration.IgnoreLocalRequests = false;
            SecurePagesConfiguration.HttpRootUrl = "http://localhost:63670/";
            SecurePagesConfiguration.HttpsRootUrl = "https://localhost:44301/";
#endif
        }

        #endregion
    }
}