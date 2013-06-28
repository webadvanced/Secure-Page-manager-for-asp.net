namespace SecurePages.WebForms.Tests.App_Start {
    using SecurePages.Infrastructure;

    using System;
    using System.Text.RegularExpressions;

    public class SecurePagesConfig {
        #region Public Methods and Operators

        public static void RegisterUrls(SecureUrlCollection secureUrls) {
            // Add urls here
            secureUrls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            //Secure Cart
            secureUrls.AddUrl("/cart");

            //Custom rules
            SecurePagesConfiguration.RegisterCustomMatchRule(c => string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase));

#if DEBUG
            //For testing only
            SecurePagesConfiguration.IgnoreLocalRequests = false;
            SecurePagesConfiguration.HttpRootUrl = "http://localhost:50535/";
            SecurePagesConfiguration.HttpsRootUrl = "https://localhost:44302/";
        }
#endif

        #endregion
    }
}