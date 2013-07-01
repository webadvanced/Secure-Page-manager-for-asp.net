namespace SecurePages.WebForms.Tests.Account {
    using System;
    using System.Web.Security;
    using System.Web.UI;

    using Microsoft.AspNet.Membership.OpenAuth;

    public partial class Register : Page {
        #region Methods

        protected void Page_Load(object sender, EventArgs e) {
            this.RegisterUser.ContinueDestinationPageUrl = this.Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e) {
            FormsAuthentication.SetAuthCookie(this.RegisterUser.UserName, createPersistentCookie: false);

            string continueUrl = this.RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl)) {
                continueUrl = "~/";
            }
            this.Response.Redirect(continueUrl);
        }

        #endregion
    }
}