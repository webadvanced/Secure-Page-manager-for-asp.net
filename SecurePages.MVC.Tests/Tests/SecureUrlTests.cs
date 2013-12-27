namespace SecurePages.Mvc.Tests.Tests {
    using SecurePages.Infrastructure;
    using SecurePages.Mvc.Tests.App_Start;
    using SecurePages.TestHelpers;

    using Xunit;

    public class SecureUrlTests {
        #region Constructors and Destructors

        public SecureUrlTests() {
            SecurePagesConfig.RegisterUrls(SecurePagesConfiguration.Urls);
        }

        #endregion

        #region Public Methods and Operators

        [Fact]
        public void WhenAccountUrlIsCalledWithHttpItShouldRedirectToHttps() {
            bool result = RequestedUrl.When("http://test.com/account/register").ShouldBeHttps();
            Assert.True(result);
        }

        [Fact]
        public void WhenAccountUrlIsCalledWithHttpsItShouldStayOnHttps() {
            bool result = RequestedUrl.When("https://test.com/account/register").ShouldBeHttps();
            Assert.True(result);
        }

        [Fact]
        public void WhenCssFilesAreCalledTheyShouldBeIgnored() {
            bool result = RequestedUrl.When("http://test.com/main.css").ShouldIgnore();
            Assert.True(result);
        }

        [Fact]
        public void WhenHomePageIsCalledWithHttpItShouldRemainOnHttp() {
            bool result = RequestedUrl.When("http://test.com/").ShouldBeHttp();
            Assert.True(result);
        }

        [Fact]
        public void WhenHomePageIsCalledWithHttpsItShouldRedirectToHttp() {
            bool result = RequestedUrl.When("http://test.com/").ShouldBeHttp();
            Assert.True(result);
        }

        [Fact]
        public void WhenImageFilesAreCalledTheyShouldBeIgnored() {
            Assert.True(RequestedUrl.When("http://test.com/img.jpg").ShouldIgnore());
            Assert.True(RequestedUrl.When("http://test.com/img.png").ShouldIgnore());
            Assert.True(RequestedUrl.When("http://test.com/img.gif").ShouldIgnore());
        }

        #endregion
    }
}