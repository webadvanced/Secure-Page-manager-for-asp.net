namespace SecurePages.WebForms.Tests.Account {
    using System;
    using System.Web;
    using System.Web.UI;

    public partial class Login : Page {
        #region Methods

        protected void Page_Load(object sender, EventArgs e) {
            this.RegisterHyperLink.NavigateUrl = "Register.aspx";
            this.OpenAuthLogin.ReturnUrl = this.Request.QueryString["ReturnUrl"];

            string returnUrl = HttpUtility.UrlEncode(this.Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl)) {
                this.RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        #endregion
    }
}