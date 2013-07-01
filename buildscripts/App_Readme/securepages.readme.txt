#Secure Pages#

Secure pages is a simple way to manage https and non-https for asp.net MVC and Web Forms sites.  Specify specific urls, use regular expressions, define custom rules and force all non-specified urls to http.  

**What it adds to your solution**

Two Files:

- App_Start/SecurePagesConfig.cs
- App_Readme/securepages.readme.txt

One Web.config alteration:

```
<configuration>
  <system.webServer>
    <modules>
      <add name="SecurePageModule" type="SecurePages.Infrastructure.SecurePageModule" />
    </modules>
  </system.webServer>
</configuration>
```

#Usage#

##Add Url##

Specify a specific relative url to be forced to https

```C#
SecurePagesConfiguration.Urls.AddUrl("/cart");
```

By default, specifying a url will be case insensitive.  To specify case sensitivity, pass `caseInsensitive:false` as the optional second argument

```C#
SecurePagesConfiguration.Urls.AddUrl("/cart", caseInsensitive:false );
```

##Add Regex##

Specify a regex pattern to be forced to https

```C#
SecurePagesConfiguration.Urls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
```

##Ignore URLs##

To ignore URLs, such as JavaScript or CSS files you can call `IgnoreUrl` on the URL Collection

```C#
secureUrls.IgnoreUrl(@"(.*)\.css");
secureUrls.IgnoreUrl(@"(.*)\.js");
```

`IgnoreUrl` takes a regex string and optionally as a second argument RegexOptions.

##Custom Secure Request Rules##

Specify a custom https match rule.  This is an example of checking the request headers to determine if the originating request was secure (needed for AppHarbor).

```C#
SecurePagesConfiguration.RegisterCustomMatchRule(c =>
                {
                    return string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https",
                                  StringComparison.InvariantCultureIgnoreCase);
                });
```

The delegate signature is Func<HttpContextBase, bool> and should return true if the request is https.

##Specify secure and unsecure root URLs

By default, secure pages will use the current URL and just alter the protocol too and from http(s). If your secure domain is different (https://secure.mydomain.com/), you will need to explicitly set the secure and nonsecure URLs.

```C#
SecurePagesConfiguration.HttpRootUrl = "http://yourdomain.com/";
SecurePagesConfiguration.HttpsRootUrl = "https://yourdomain.com/";
```

##For Local Testing##

```C#
SecurePagesConfiguration.IgnoreLocalRequests = true;
```

By default, secure pages will ignore all request from localhost






