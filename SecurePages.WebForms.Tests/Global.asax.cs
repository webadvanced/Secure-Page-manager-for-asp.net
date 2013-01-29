﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SecurePages.WebForms.Tests;

namespace SecurePages.WebForms.Tests
{
    using System.Text.RegularExpressions;

    using SecurePages.Infrastructure;

    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();

            SecurePagesConfiguration.Urls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

            //Secure Cart
            SecurePagesConfiguration.Urls.AddUrl("/cart", caseInsensitive:false);

            //Custom rules
            SecurePagesConfiguration.RegisterCustomMatchRule(c =>
                {
                    return string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https",
                                  StringComparison.InvariantCultureIgnoreCase);
                });

            //For testing only
#if DEBUG
            SecurePagesConfiguration.IgnoreLocalRequests = false;
#endif

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
