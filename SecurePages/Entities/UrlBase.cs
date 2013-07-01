namespace SecurePages.Entities {
    using System.Text.RegularExpressions;

    public abstract class UrlBase : IUrlBase {
        #region Public Properties

        public SecureUrlMatchType MatchType { get; set; }

        public RegexOptions RegexOptions { get; set; }

        public string Url { get; set; }

        #endregion
    }
}