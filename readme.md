#Secure Pages#

Secure pages is a simple way to manage https and non-https for asp.net sites.  Specify specific urls, use regular expressions, define custom rules and force all non-specified urls to http.  

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

##Custom Rules##

Specify a custom https match rule.  This is an example of checking the request headers to determine if the originating request was secure (needed for AppHarbor).

```C#
SecurePagesConfiguration.RegisterCustomMatchRule(c =>
                {
                    return string.Equals(c.Request.Headers["X-Forwarded-Proto"], "https",
                                  StringComparison.InvariantCultureIgnoreCase);
                });
```

The delegate signature is Func<HttpContextBase, bool> and should return true if the request is https.





