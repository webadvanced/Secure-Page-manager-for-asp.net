namespace SecurePages.Mvc.Tests.Tests {
    using SecurePages.Infrastructure;
    using SecurePages.Mvc.Tests.App_Start;
    using SecurePages.TestHelpers;

    using Xunit;

    public class SecureUrlTests {

        public SecureUrlTests() {
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
}