namespace SecurePages.Tests {
    using System;
    using System.Linq;
    using System.Web;

    using SecurePages.Infrastructure;

    using Xunit;

    public class SecurePagesConfigurationTests {
        #region Public Methods and Operators

        [Fact]
        public void ConstructorShouldSetDefaults() {
            Assert.True(SecurePagesConfiguration.IgnoreLocalRequests);
            Assert.NotNull(SecurePagesConfiguration.Urls);
            Assert.NotNull(SecurePagesConfiguration.CustomMatchRules);
        }

        [Fact]
        public void RegisterCustomMatchRule_ShouldIncrementRuleCollection() {
            // arrange
            SecurePagesConfiguration.CustomMatchRules.Clear();
            Func<HttpContextBase, bool> func = c => false;

            // assert
            Assert.Equal(0, SecurePagesConfiguration.CustomMatchRules.Count());

            // act
            SecurePagesConfiguration.RegisterCustomMatchRule(func);

            // assert
            Assert.Equal(1, SecurePagesConfiguration.CustomMatchRules.Count());
        }

        [Fact]
        public void RegisterCustomMatchRule_ShouldThrowArgumentNullException_WhenArgumentIsNull() {
            // assert
            Assert.Throws<ArgumentNullException>(() => SecurePagesConfiguration.RegisterCustomMatchRule(null));
        }

        #endregion
    }
}