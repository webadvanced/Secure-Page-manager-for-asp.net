using System.Web;
using System.Web.Mvc;
using SecurePages.Infrastructure;
using System.Text.RegularExpressions;
using System;

[assembly: WebActivator.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.SecurePagesConfig), "Start")]

namespace $rootnamespace$.App_Start {
    public static class SecurePagesConfig {
        public static void Start() {
            RegisterSecurePagesConfigurations();            
        }

public static void RegisterSecurePagesConfigurations() {
            // Securing a collection of pages with Regex
            //SecurePagesConfiguration.Urls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            //Securing a page by specifying its url
            //SecurePagesConfiguration.Urls.Add("/cart");

            //Registering a custom rule (Example for AppHarbor)
            //SecurePagesConfiguration.RegisterCustomMatchRule(c => string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase));

            //For testing only
            //SecurePagesConfiguration.IgnoreLocalRequests = false;
        }
    }
}