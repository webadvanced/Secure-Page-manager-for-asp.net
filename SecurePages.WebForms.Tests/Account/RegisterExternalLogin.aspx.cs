namespace SecurePages.WebForms.Tests.Account {
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    using DotNetOpenAuth.AspNet;

    using Microsoft.AspNet.Membership.OpenAuth;

    public partial class RegisterExternalLogin : Page {
        #region Properties

        protected string ProviderDisplayName {
            get {
                return (string)this.ViewState["ProviderDisplayName"] ?? String.Empty;
            }
            private set {
                this.ViewState["ProviderDisplayName"] = value;
            }
        }

        protected string ProviderName {
            get {
                return (string)this.ViewState["ProviderName"] ?? String.Empty;
            }
            private set {
                this.ViewState["ProviderName"] = value;
            }
        }

        protected string ProviderUserId {
            get {
                return (string)this.ViewState["ProviderUserId"] ?? String.Empty;
            }
            private set {
                this.ViewState["ProviderUserId"] = value;
            }
        }

        protected string ProviderUserName {
            get {
                return (string)this.ViewState["ProviderUserName"] ?? String.Empty;
            }
            private set {
                this.ViewState["ProviderUserName"] = value;
            }
        }

        #endregion

        #region Methods

        protected void Page_Load() {
            if (!this.IsPostBack) {
                this.ProcessProviderResult();
            }
        }

        protected void cancel_Click(object sender, EventArgs e) {
            this.RedirectToReturnUrl();
        }

        protected void logIn_Click(object sender, EventArgs e) {
            this.CreateAndLoginUser();
        }

        private void CreateAndLoginUser() {
            if (!this.IsValid) {
                return;
            }

            CreateResult createResult = OpenAuth.CreateUser(
                this.ProviderName, this.ProviderUserId, this.ProviderUserName, this.userName.Text);
            if (!createResult.IsSuccessful) {
                this.ModelState.AddModelError("UserName", createResult.ErrorMessage);
            }
            else {
                // User created & associated OK
                if (OpenAuth.Login(this.ProviderName, this.ProviderUserId, createPersistentCookie: false)) {
                    this.RedirectToReturnUrl();
                }
            }
        }

        private void ProcessProviderResult() {
            // Process the result from an auth provider in the request
            this.ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (String.IsNullOrEmpty(this.ProviderName)) {
                this.Response.Redirect(FormsAuthentication.LoginUrl);
            }

            // Build the redirect url for OpenAuth verification
            string redirectUrl = "~/Account/RegisterExternalLogin.aspx";
            string returnUrl = this.Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(returnUrl)) {
                redirectUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
            }

            // Verify the OpenAuth payload
            AuthenticationResult authResult = OpenAuth.VerifyAuthentication(redirectUrl);
            this.ProviderDisplayName = OpenAuth.GetProviderDisplayName(this.ProviderName);
            if (!authResult.IsSuccessful) {
                this.Title = "External login failed";
                this.userNameForm.Visible = false;

                this.ModelState.AddModelError(
                    "Provider", String.Format("External login {0} failed.", this.ProviderDisplayName));

                // To view this error, enable page tracing in web.config (<system.web><trace enabled="true"/></system.web>) and visit ~/Trace.axd
                this.Trace.Warn(
                    "OpenAuth",
                    String.Format("There was an error verifying authentication with {0})", this.ProviderDisplayName),
                    authResult.Error);
                return;
            }

            // User has logged in with provider successfully
            // Check if user is already registered locally
            if (OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false)) {
                this.RedirectToReturnUrl();
            }

            // Store the provider details in ViewState
            this.ProviderName = authResult.Provider;
            this.ProviderUserId = authResult.ProviderUserId;
            this.ProviderUserName = authResult.UserName;

            // Strip the query string from action
            this.Form.Action = this.ResolveUrl(redirectUrl);

            if (this.User.Identity.IsAuthenticated) {
                // User is already authenticated, add the external login and redirect to return url
                OpenAuth.AddAccountToExistingUser(
                    this.ProviderName, this.ProviderUserId, this.ProviderUserName, this.User.Identity.Name);
                this.RedirectToReturnUrl();
            }
            else {
                // User is new, ask for their desired membership name
                this.userName.Text = authResult.UserName;
            }
        }

        private void RedirectToReturnUrl() {
            string returnUrl = this.Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(returnUrl) && OpenAuth.IsLocalUrl(returnUrl)) {
                this.Response.Redirect(returnUrl);
            }
            else {
                this.Response.Redirect("~/");
            }
        }

        #endregion
    }
}