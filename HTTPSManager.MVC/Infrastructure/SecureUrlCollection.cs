namespace SecurePages.Infrastructure {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using SecurePages.Entities;

    public class SecureUrlCollection : IEnumerable<SecureUrl> {
        #region Constants and Fields

        private readonly List<SecureUrl> urls = new List<SecureUrl>();

        #endregion

        #region Public Methods and Operators

        public void Add(SecureUrl url) {
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            this.urls.Add(url);
        }

        public void Add(string url, SecureUrlMatchType matchType = SecureUrlMatchType.CaseInsensitive) {
            if(string.IsNullOrEmpty(url)) {
                throw new ArgumentException("url");
            }
            
            this.urls.Add(new SecureUrl { MatchType = matchType, Url = url });
        }

        public void AddRegex(string pattern, RegexOptions regexOptions = RegexOptions.None) {
            if(string.IsNullOrEmpty(pattern)) {
                throw new ArgumentException("pattern");
            }

            this.urls.Add(
                new SecureUrl { MatchType = SecureUrlMatchType.Regex, Url = pattern, RegexOptions = regexOptions });
        }

        public IEnumerator<SecureUrl> GetEnumerator() {
            return this.urls.GetEnumerator();
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion

        public void Clear() {
            urls.Clear();
        }
    }
}