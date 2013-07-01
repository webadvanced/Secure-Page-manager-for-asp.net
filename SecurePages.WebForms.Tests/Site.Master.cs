namespace SecurePages.WebForms.Tests {
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    public partial class SiteMaster : MasterPage {
        #region Constants and Fields

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";

        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";

        private string _antiXsrfTokenValue;

        #endregion

        #region Methods

        protected void Page_Init(object sender, EventArgs e) {
            // The code below helps to protect against XSRF attacks
            HttpCookie requestCookie = this.Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue)) {
                // Use the Anti-XSRF token from the cookie
                this._antiXsrfTokenValue = requestCookie.Value;
                this.Page.ViewStateUserKey = this._antiXsrfTokenValue;
            }
            else {
                // Generate a new Anti-XSRF token and save to the cookie
                this._antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                this.Page.ViewStateUserKey = this._antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey) {
                    HttpOnly = true,
                    Value = this._antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && this.Request.IsSecureConnection) {
                    responseCookie.Secure = true;
                }
                this.Response.Cookies.Set(responseCookie);
            }

            this.Page.PreLoad += this.master_Page_PreLoad;
        }

        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void master_Page_PreLoad(object sender, EventArgs e) {
            if (!this.IsPostBack) {
                // Set Anti-XSRF token
                this.ViewState[AntiXsrfTokenKey] = this.Page.ViewStateUserKey;
                this.ViewState[AntiXsrfUserNameKey] = this.Context.User.Identity.Name ?? String.Empty;
            }
            else {
                // Validate the Anti-XSRF token
                if ((string)this.ViewState[AntiXsrfTokenKey] != this._antiXsrfTokenValue
                    || (string)this.ViewState[AntiXsrfUserNameKey] != (this.Context.User.Identity.Name ?? String.Empty)) {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        #endregion
    }
}