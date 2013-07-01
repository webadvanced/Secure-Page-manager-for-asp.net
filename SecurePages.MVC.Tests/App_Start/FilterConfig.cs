namespace HTTPSManager.MVC.Web.Tests {
    using System.Web.Mvc;

    public class FilterConfig {
        #region Public Methods and Operators

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}