namespace SecurePages.Infrastructure {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SecurePages.Entities;

    public class SecureUrlCollection : IEnumerable<UrlBase> {
        #region Constants and Fields

        private readonly List<UrlBase> urls = new List<UrlBase>();

        #endregion

        #region Public Properties

        public IList<IgnoreUrl> IgnoreUrls {
            get {
                return this.urls.OfType<IgnoreUrl>().ToList();
            }
        }

        public IList<SecureUrl> SecureUrls {
            get {
                return this.urls.OfType<SecureUrl>().ToList();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add(UrlBase url) {
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            this.urls.Add(url);
        }

        public void AddRegex(string pattern, RegexOptions regexOptions = RegexOptions.None) {
            if (string.IsNullOrEmpty(pattern)) {
                throw new ArgumentException("pattern");
            }

            this.urls.Add(
                new SecureUrl { MatchType = SecureUrlMatchType.Regex, Url = pattern, RegexOptions = regexOptions });
        }

        public void AddUrl(string url, bool caseInsensitive = true) {
            if (string.IsNullOrEmpty(url)) {
                throw new ArgumentException("url");
            }

            this.urls.Add(
                new SecureUrl {
                    MatchType =
                        (caseInsensitive) ? SecureUrlMatchType.CaseInsensitive : SecureUrlMatchType.CaseSensitive,
                    Url = url
                });
        }

        public void Clear() {
            this.urls.Clear();
        }

        public IEnumerator<UrlBase> GetEnumerator() {
            return this.urls.GetEnumerator();
        }

        public void IgnoreUrl(string pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase) {
            if (string.IsNullOrEmpty(pattern)) {
                throw new ArgumentException("pattern");
            }

            this.urls.Add(
                new IgnoreUrl { MatchType = SecureUrlMatchType.Regex, Url = pattern, RegexOptions = regexOptions });
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion
    }
}