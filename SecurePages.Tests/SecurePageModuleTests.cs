namespace SecurePages.Tests {
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    using SecurePages.Infrastructure;

    using Xunit;

    public class SecurePageModuleTests {
        #region Constants and Fields

        public const string AppPathModifier = "/$(SESSION)";

        #endregion

        #region Public Methods and Operators

        public static void SetHttpContext(Dictionary<object, object> items = null, string url = null) {
            url = url ?? "http://mockurl.com/secure?ignore=true";
            var httpRequest = new HttpRequest("", url, "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpConextMock = new HttpContext(httpRequest, httpResponce);

            HttpContext.Current = httpConextMock;
            if (items != null) {
                foreach (var item in items) {
                    HttpContext.Current.Items.Add(item.Key, item.Value);
                }
            }
        }

        [Fact]
        public void ContextBeginRequest_ShouldReturnWhen_ThereIsAnIgnoreUrlMatch() {
            //arrange
            SetHttpContext();
            SecurePagesConfiguration.Urls.IgnoreUrl(@"ignore\=true");
            bool isSecureWasCalled = false;
            bool isIgnoreWasCalled = false;
            SecurePageModule.IsIgnoreUrlFunc = (u, f) => isIgnoreWasCalled = true;
            SecurePageModule.IsSecureUrlFunc = (u, f) => isSecureWasCalled = true;

            //act
            SecurePageModule.ContextBeginRequest(null, null);

            Assert.True(isIgnoreWasCalled);
            Assert.False(isSecureWasCalled);
        }

        #endregion
    }
}