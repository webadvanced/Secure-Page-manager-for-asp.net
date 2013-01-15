namespace SecurePages.Entities {
    using System.Text.RegularExpressions;

    public class SecureUrl {
        #region Public Properties

        public SecureUrlMatchType MatchType { get; set; }

        public RegexOptions RegexOptions { get; set; }

        public string Url { get; set; }

        #endregion
    }
}