namespace SecurePages.Entities {
    using System.Text.RegularExpressions;

    public interface IUrlBase {
        #region Public Properties

        SecureUrlMatchType MatchType { get; set; }

        RegexOptions RegexOptions { get; set; }

        string Url { get; set; }

        #endregion
    }
}