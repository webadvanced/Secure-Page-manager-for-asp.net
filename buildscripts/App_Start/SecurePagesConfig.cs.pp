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
            //Ignore defaults
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.css");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.js");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.png");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.jpg");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.gif");
			
			// Securing a collection of pages with Regex
            //SecurePagesConfiguration.Urls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            //Securing a page by specifying its url
            //SecurePagesConfiguration.Urls.Add("/cart");

            //Registering a custom rule (Example for AppHarbor)
            //SecurePagesConfiguration.RegisterCustomMatchRule(c => string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase));

					
            //For testing only.  By default, secure pages will ignore all request from localhost
            #if DEBUG
            SecurePagesConfiguration.IgnoreLocalRequests = false;
			
			//Optional
			//SecurePagesConfiguration.HttpRootUrl = "http://localhost:50535/";
            //SecurePagesConfiguration.HttpsRootUrl = "https://localhost:44300/";
            #endif
        }
    }
}