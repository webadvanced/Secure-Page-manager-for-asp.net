namespace SecurePages.WebForms.Tests.Account {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    using Microsoft.AspNet.Membership.OpenAuth;

    public partial class OpenAuthProviders : UserControl {
        #region Public Properties

        public string ReturnUrl { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ProviderDetails> GetProviderNames() {
            return OpenAuth.AuthenticationClients.GetAll();
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e) {
            if (this.IsPostBack) {
                string provider = this.Request.Form["provider"];
                if (provider == null) {
                    return;
                }

                string redirectUrl = "~/Account/RegisterExternalLogin.aspx";
                if (!String.IsNullOrEmpty(this.ReturnUrl)) {
                    string resolvedReturnUrl = this.ResolveUrl(this.ReturnUrl);
                    redirectUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(resolvedReturnUrl);
                }

                OpenAuth.RequestAuthentication(provider, redirectUrl);
            }
        }

        #endregion
    }
}