namespace SecurePages.Mvc.Tests.App_Start {
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

            //For testing only
            SecurePagesConfiguration.IgnoreLocalRequests = false;

            //For testing with a different domain 
            // SecurePagesConfiguration.HttpRootUrl = "http://localhost:63670/";
            // SecurePagesConfiguration.HttpsRootUrl = "https://localhost:44300/";
        }

        #endregion
    }
}