#Secure Pages#

Secure pages is a simple way to manage https and non-https for asp.net MVC and Web Forms sites.  Specify specific urls, use regular expressions, define custom rules and force all non-specified urls to http.  


##Changelog:##

###Version 1.0.5109.19336 - December 27 2013###
- Removed majority of string usage and replaced with Uri.
- Fixed small bugs in TestHelpers.RequestedUrl 

###Version 1.0.5007.19696 - September 16 2013###
- Added TestHelpers.RequestedUrl - [more here](https://github.com/webadvanced/Secure-Page-manager-for-asp.net#testing-your-configuration "How to test your config")
- Fixed SecurePagesConfig.cs references 

###Version 1.0.4930.15380 - July 01 2013###

- Added SecureUrlCollection.IgnoreUrl(string, RegexOptions)  providing the ability to ignore URLs 
- Added more test coverage 

###Version 1.0.4927.20371 - June 28 2013###

- Added ability to set HttpRootUrl
- Added ability to set HttpsRootUrl
- Small bug fixes 



#Adding it to your solution#

Install package from nuget: `Install-Package SecurePages`

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
SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.css");
SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.js");
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


##Testing your configuration

SecurePages comes with a test helper class called `RequestedUrl` with 3 helper methods:

- ShouldBeHttp

- ShouldBeHttps

- ShouldIgnore

Here is an example of testing a configuration (the below code is using xUnit for testing but you can use nUnit, MSTest etc)

**The rules**

```C#
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.css");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.js");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.png");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.jpg");
            SecurePagesConfiguration.Urls.IgnoreUrl(@"(.*)\.gif");
			
			//Securing all pages with account/
            SecurePagesConfiguration.Urls.AddRegex(@"(.*)account", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			
			//All other urls will be forced to http

```

**The tests**

```C#

	public class SecureUrlTests {

		public SecureUrlTests() {
			// Make sure you register your configuration rules
			SecurePagesConfig.RegisterUrls(SecurePagesConfiguration.Urls);
		}

		[Fact]
		public void WhenAccountUrlIsCalledWithHttpItShouldRedirectToHttps() {
			var result = RequestedUrl.When("http://test.com/account/register").ShouldBeHttps();
			Assert.True(result);
		}

		[Fact]
		public void WhenAccountUrlIsCalledWithHttpsItShouldStayOnHttps() {
			var result = RequestedUrl.When("https://test.com/account/register").ShouldBeHttps();
			Assert.True(result);
		}

		[Fact]
		public void WhenHomePageIsCalledWithHttpsItShouldRedirectToHttp() {
			var result = RequestedUrl.When("https://test.com/").ShouldBeHttp();
			Assert.True(result);
		}

		[Fact]
		public void WhenHomePageIsCalledWithHttpItShouldRemainOnHttp() {
			var result = RequestedUrl.When("http://test.com/").ShouldBeHttp();
			Assert.True(result);
		}

		[Fact]
		public void WhenCssFilesAreCalledTheyShouldBeIgnored() {
			var result = RequestedUrl.When("http://test.com/main.css").ShouldIgnore();
			Assert.True(result);
		}

		[Fact]
		public void WhenImageFilesAreCalledTheyShouldBeIgnored() {
			Assert.True(RequestedUrl.When("http://test.com/img.jpg").ShouldIgnore());
			Assert.True(RequestedUrl.When("http://test.com/img.png").ShouldIgnore());
			Assert.True(RequestedUrl.When("http://test.com/img.gif").ShouldIgnore());
		}
	}
	
```





