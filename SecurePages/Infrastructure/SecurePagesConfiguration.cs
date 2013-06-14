namespace SecurePages.Infrastructure {
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class SecurePagesConfiguration {
        #region Constants and Fields

        private static readonly SecureUrlCollection SecureUrlCollection;

        private static readonly IList<Func<HttpContextBase, bool>> CustomMatchRuleCollection;

        #endregion

        #region Constructors and Destructors

        static SecurePagesConfiguration() {
            SecureUrlCollection = new SecureUrlCollection();
            IgnoreLocalRequests = true;
            HttpValue = "Http://";
            HttpsValue = "Https://";
            CustomMatchRuleCollection = new List<Func<HttpContextBase, bool>>();
        }

        #endregion

        public static void RegisterCustomMatchRule(Func<HttpContextBase, bool> customMatchRuleFunc) {
            if (customMatchRuleFunc == null) {
                throw new ArgumentNullException("customMatchRuleFunc");
            }

            CustomMatchRuleCollection.Add(customMatchRuleFunc);
        }

        #region Public Properties

        public static SecureUrlCollection Urls {
            get {
                return SecureUrlCollection;
            }
        }

        public static IList<Func<HttpContextBase, bool>> CustomMatchRules {
            get {
                return CustomMatchRuleCollection;
            }
        }

        #endregion

        #region Properties

        public static bool IgnoreLocalRequests { get; set; }

        public static string HttpValue { get; set; }

        public static string HttpsValue { get; set; }

        #endregion
    }
}