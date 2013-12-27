namespace SecurePages.Tests {
    using System;
    using System.Collections.Specialized;
    using System.Web;

    using Moq;

    using SecurePages.Entities;
    using SecurePages.Infrastructure;
    using SecurePages.Services;

    using Xunit;

    public class SecurePagesServiceTests : SecurePagesTestBase {
        #region Public Methods and Operators

        [Fact]
        public void HandleRequest_ShouldNotCallResponseHandler_WhenIsLocalRequestAndConfiguredtoIgnore() {
            // arrange
            bool isSecureRequest = true;
            bool isSecureUrl = false;
            var context = new Mock<HttpContextBase>();
            bool flag = false;
            Action<HttpContextBase, string> responseHandler = (c, u) => flag = true;
            SecurePagesConfiguration.IgnoreLocalRequests = true;
            context.Setup(x => x.Request.IsLocal).Returns(() => true);

            // act
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context.Object, responseHandler);

            // assert
            Assert.False(flag);
        }

        [Fact]
        public void HandleRequest_ShouldNotCallResponseHandler_WhenIsSecureRequestAndIsSecureUrlAreFalse() {
            // arrange
            bool isSecureRequest = false;
            bool isSecureUrl = false;
            var context = new Mock<HttpContextBase>();
            bool flag = false;
            Action<HttpContextBase, string> responseHandler = (c, u) => flag = true;
            SecurePagesConfiguration.IgnoreLocalRequests = false;

            // act
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context.Object, responseHandler);

            // assert
            Assert.False(flag);
        }

        [Fact]
        public void HandleRequest_ShouldNotCallResponseHandler_WhenIsSecureRequestAndIsSecureUrlAreTrue() {
            // arrange
            bool isSecureRequest = true;
            bool isSecureUrl = true;
            var context = new Mock<HttpContextBase>();
            bool flag = false;
            Action<HttpContextBase, string> responseHandler = (c, u) => flag = true;
            SecurePagesConfiguration.IgnoreLocalRequests = false;

            // act
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context.Object, responseHandler);

            // assert
            Assert.False(flag);
        }

        [Fact]
        public void
            HandleRequest_WhenRequestIsNotSecureAndUrlIsSupposedToBeSecureResponseHandlerIsCalledAndPassedSecureVersionOfUrl() {
            // arrange
            bool isSecureRequest = false;
            bool isSecureUrl = true;
            SecurePagesConfiguration.HttpRootUrl = "http://www.webadvanced.com/";
            var context = new Mock<HttpContextBase>();
            string responseUrl = string.Empty;
            Action<HttpContextBase, string> responseHandler = (c, u) => responseUrl = u;
            SecurePagesConfiguration.IgnoreLocalRequests = false;
            context.Setup(x => x.Request.IsLocal).Returns(() => false);
            context.Setup(x => x.Request.Url).Returns(() => new Uri("http://www.webadvanced.com/"));

            // act
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context.Object, responseHandler);
            
            // assert
            Assert.NotEmpty(responseUrl);
            Assert.Equal(responseUrl, "https://www.webadvanced.com/");
        }

        [Fact]
        public void
            HandleRequest_WhenRequestIsSecureAndUrlIsNotSupposedToBeSecureResponseHandlerIsCalledAndPassedNonSecureVersionOfUrl
            () {
            // arrange
            bool isSecureRequest = true;
            bool isSecureUrl = false;
            var context = new Mock<HttpContextBase>();
            string responseUrl = string.Empty;
            Action<HttpContextBase, string> responseHandler = (c, u) => responseUrl = u;
            SecurePagesConfiguration.IgnoreLocalRequests = false;
            SecurePagesConfiguration.HttpRootUrl = string.Empty;
            SecurePagesConfiguration.HttpsRootUrl = string.Empty;
            context.Setup(x => x.Request.IsLocal).Returns(() => false);
            context.Setup(x => x.Request.Url).Returns(() => new Uri("https://www.webadvanced.com/"));

            // act
            SecurePagesService.HandelRequest(isSecureRequest, isSecureUrl, context.Object, responseHandler);

            // assert
            Assert.NotEmpty(responseUrl);
            Assert.Equal(responseUrl, "http://www.webadvanced.com/");
        }

        [Fact]
        public void IsSecureRequest_ShouldReturnIsSecureConnection_WhenCustomMatchRulesDoesNotContainASecureRule() {
            // arrange
            var headers = new NameValueCollection();
            headers.Add("Test-Secure-Connection", "http");

            var context = new Mock<HttpContextBase>();
            context.Setup(x => x.Request.Headers).Returns(() => headers);
            context.Setup(x => x.Request.IsSecureConnection).Returns(() => false);

            SecurePagesConfiguration.CustomMatchRules.Clear();
            SecurePagesConfiguration.RegisterCustomMatchRule(
                c =>
                string.Equals(
                    c.Request.Headers["Test-Secure-Connection"], "https", StringComparison.InvariantCultureIgnoreCase));

            // act
            bool isSecureConnection = SecurePagesService.IsSecureRequest(context.Object);

            // assert
            Assert.False(isSecureConnection);
        }

        [Fact]
        public void IsSecureRequest_ShouldReturnIsSecureConnection_WhenCustomMatchRulesIsEmpty() {
            // arrange
            var context = new Mock<HttpContextBase>();
            context.Setup(x => x.Request.IsSecureConnection).Returns(() => false);
            SecurePagesConfiguration.CustomMatchRules.Clear();

            // act
            bool isSecureConnection = SecurePagesService.IsSecureRequest(context.Object);

            // assert
            Assert.False(isSecureConnection);
        }

        [Fact]
        public void IsSecureRequest_ShouldReturnTrue_WhenCustomMatchRulesContainsASecureRule() {
            // arrange            
            var headers = new NameValueCollection();
            headers.Add("Test-Secure-Connection", "https");

            var context = new Mock<HttpContextBase>();
            context.Setup(x => x.Request.Headers).Returns(() => headers);
            context.Setup(x => x.Request.IsSecureConnection).Returns(() => false);

            SecurePagesConfiguration.CustomMatchRules.Clear();
            SecurePagesConfiguration.RegisterCustomMatchRule(
                c =>
                string.Equals(
                    c.Request.Headers["Test-Secure-Connection"], "https", StringComparison.InvariantCultureIgnoreCase));

            // act
            bool isSecureConnection = SecurePagesService.IsSecureRequest(context.Object);

            // assert
            Assert.True(isSecureConnection);
        }

        [Fact]
        public void IsSecureUrl_ShouldBreakOnFirstMatch() {
            // arrange
            SecurePagesConfiguration.Urls.Clear();
            SecurePagesConfiguration.Urls.AddUrl("/mock/url");
            SecurePagesConfiguration.Urls.AddUrl("/url/mocking");
            var uri = AbsoluteUri("mock/url");

            // act
            bool result = SecurePagesService.IsSecureUrl(uri);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsSecureUrl_ShouldCallRegexMatch_WhenSecureUrlMatchTypeIsRegex() {
            // arrange
            SecurePagesConfiguration.Urls.Clear();
            SecurePagesConfiguration.Urls.AddRegex("mock/url");
            var uri = AbsoluteUri("mock/url");
            bool wasCalled = false;
            Func<Uri, IUrlBase, bool> regexMatchFunc = (s, su) => {
                wasCalled = true;
                return false;
            };

            // act
            SecurePagesService.IsSecureUrl(uri, regexMatchFunc);

            // assert
            Assert.True(wasCalled);
        }

        [Fact]
        public void IsSecureUrl_ShouldIgnoreCase_WhenMatchTypeIsCaseInsensitive() {
            // arrange
            SecurePagesConfiguration.Urls.Clear();
            SecurePagesConfiguration.Urls.AddUrl("/mock/url");
            var lowerCaseUri = AbsoluteUri("mock/url");
            var upperCaseUri = AbsoluteUri("Mock/URL");

            // act
            bool lowerCaseUrlResult = SecurePagesService.IsSecureUrl(lowerCaseUri);
            bool upperCaseUrlResult = SecurePagesService.IsSecureUrl(upperCaseUri);

            // assert
            Assert.True(lowerCaseUrlResult);
            Assert.True(upperCaseUrlResult);
        }

        [Fact]
        public void IsSecureUrl_ShouldMatchCase_WhenMatchTypeIsCaseSensitive() {
            // arrange
            SecurePagesConfiguration.Urls.Clear();
            SecurePagesConfiguration.Urls.AddUrl("/mock/url", false);
            var lowerCaseUri = AbsoluteUri("mock/url");
            var upperCaseUri = AbsoluteUri("Mock/URL");

            // act
            bool lowerCaseUrlResult = SecurePagesService.IsSecureUrl(lowerCaseUri);
            bool upperCaseUrlResult = SecurePagesService.IsSecureUrl(upperCaseUri);

            // assert
            Assert.True(lowerCaseUrlResult);
            Assert.False(upperCaseUrlResult);
        }

        [Fact]
        public void IsSecureUrl_ShouldReturnFalse_WhenUriIsNull() {
            // arrange
            SecurePagesConfiguration.Urls.Clear();
            SecurePagesConfiguration.Urls.AddUrl("mockUrl");

            // act
            bool nullUrlResult = SecurePagesService.IsSecureUrl(null);

            // assert
            Assert.False(nullUrlResult);
        }

        [Fact]
        public void NonSecureUrl_ShouldUseCurrentUrl_WhenHttpRootUrlIsNotSpecified() {
            // arrange
            SecurePagesConfiguration.HttpRootUrl = string.Empty;
            var uri = new Uri("https://mocksite.com/test?q=param");

            string result = SecurePagesService.NonSecureUrl(uri);

            Assert.Equal("http://mocksite.com/test?q=param", result);
        }

        [Fact]
        public void NonSecureUrl_ShouldUseHttpRootUrl_WhenSpecified() {
            // arrange
            SecurePagesConfiguration.HttpRootUrl = "http://mock.com/";
            var uri = new Uri("https://mocksite.com/test?q=param");

            string result = SecurePagesService.NonSecureUrl(uri);

            Assert.Equal("http://mock.com/test?q=param", result);
        }

        [Fact]
        public void RedirectPermanent_ShouldThrowArgumentException_WhenUrlIsEmptyOrNull() {
            var context = new Mock<HttpContextBase>();

            Assert.Throws<ArgumentException>(() => SecurePagesService.RedirectPermanent(context.Object, string.Empty));

            Assert.Throws<ArgumentException>(() => SecurePagesService.RedirectPermanent(context.Object, null));
        }

        [Fact]
        public void RedirectPermanent_ShouldThrowArgumentNullException_WhenContextIsNull() {
            Assert.Throws<ArgumentNullException>(
                () => SecurePagesService.RedirectPermanent(null, "http://www.webadvanced.com"));
        }

        [Fact]
        public void SecureUrl_ShouldUseCurrentUrl_WhenHttpsRootUrlIsNotSpecified() {
            // arrange
            SecurePagesConfiguration.HttpsRootUrl = string.Empty;
            var uri = new Uri("http://mocksite.com/test?q=param");

            string result = SecurePagesService.SecureUrl(uri);

            Assert.Equal("https://mocksite.com/test?q=param", result);
        }

        [Fact]
        public void SecureUrl_ShouldUseHttpsRootUrl_WhenSpecified() {
            // arrange
            SecurePagesConfiguration.HttpsRootUrl = "https://mock.com/";
            var uri = new Uri("http://mocksite.com/test?q=param");

            string result = SecurePagesService.SecureUrl(uri);

            Assert.Equal("https://mock.com/test?q=param", result);
        }

        #endregion
    }
}