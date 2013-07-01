namespace HTTPSManager.MVC.Web.Tests.Controllers {
    using System.Web.Mvc;

    public class HomeController : Controller {
        #region Public Methods and Operators

        public ActionResult About() {
            this.ViewBag.Message = "Your app description page.";

            return this.View();
        }

        public ActionResult Contact() {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }

        public ActionResult Index() {
            this.ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return this.View();
        }

        #endregion
    }
}