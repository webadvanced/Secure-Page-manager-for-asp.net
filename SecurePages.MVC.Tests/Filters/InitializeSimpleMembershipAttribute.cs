namespace HTTPSManager.MVC.Web.Tests.Filters {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Web.Mvc;

    using HTTPSManager.MVC.Web.Tests.Models;

    using WebMatrix.WebData;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute {
        #region Constants and Fields

        private static SimpleMembershipInitializer _initializer;

        private static object _initializerLock = new object();

        private static bool _isInitialized;

        #endregion

        #region Public Methods and Operators

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        #endregion

        private class SimpleMembershipInitializer {
            #region Constructors and Destructors

            public SimpleMembershipInitializer() {
                Database.SetInitializer<UsersContext>(null);

                try {
                    using (var context = new UsersContext()) {
                        if (!context.Database.Exists()) {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection(
                        "DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex) {
                    throw new InvalidOperationException(
                        "The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588",
                        ex);
                }
            }

            #endregion
        }
    }
}