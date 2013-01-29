#Secure Pages#

Secure pages is a simple way to manage https and non-https for asp.net sites.  Specify specific urls, use regular expressions, define custom rules and force all non-specified urls to http.  

#Usage#

##Url Match##

Specify a specific relative url to be forced to https

```C#
SecurePagesConfiguration.Urls.Add("/cart");
```