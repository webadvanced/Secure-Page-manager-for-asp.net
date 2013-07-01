namespace SecurePages.Infrastructure {
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class SecurePagesConfiguration {
        #region Constants and Fields

        private static readonly IList<Func<HttpContextBase, bool>> CustomMatchRuleCollection;

        private static readonly SecureUrlCollection SecureUrlCollection;

        #endregion

        #region Constructors and Destructors

        static SecurePagesConfiguration() {
            SecureUrlCollection = new SecureUrlCollection();
            IgnoreLocalRequests = true;
            CustomMatchRuleCollection = new List<Func<HttpContextBase, bool>>();
        }

        #endregion

        #region Public Properties

        public static IList<Func<HttpContextBase, bool>> CustomMatchRules {
            get {
                return CustomMatchRuleCollection;
            }
        }

        public static string HttpRootUrl { get; set; }

        public static string HttpsRootUrl { get; set; }

        public static bool IgnoreLocalRequests { get; set; }

        public static SecureUrlCollection Urls {
            get {
                return SecureUrlCollection;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void RegisterCustomMatchRule(Func<HttpContextBase, bool> customMatchRuleFunc) {
            if (customMatchRuleFunc == null) {
                throw new ArgumentNullException("customMatchRuleFunc");
            }

            CustomMatchRuleCollection.Add(customMatchRuleFunc);
        }

        #endregion
    }
}