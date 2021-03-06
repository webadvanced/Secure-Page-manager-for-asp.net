﻿namespace HTTPManager.MVC.Tests {
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SecurePages.Entities;
    using SecurePages.Infrastructure;

    using Xunit;

    public class SecureUrlCollectionTests {
        #region Public Methods and Operators

        [Fact]
        public void AddRegex_ShouldAddNewSecureUrlToCollection_WhenPassedValidParams() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddRegex(@"MockRegex");

            // Assert
            Assert.NotEmpty(collection);
            Assert.Equal(1, collection.Count());
        }

        [Fact]
        public void AddRegex_ShouldSetDefaultRegexOptionsToNone_WhenNoRegexOptionsArePassed() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddRegex(@"MockRegex");
            SecureUrl secureUrl = collection.OfType<SecureUrl>().First();

            // Assert
            Assert.Equal(RegexOptions.None, secureUrl.RegexOptions);
        }

        [Fact]
        public void AddRegex_ShouldSetRegexOptionsToPassedRegexOptions() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddRegex(@"MockRegex", RegexOptions.IgnoreCase);
            SecureUrl secureUrl = collection.OfType<SecureUrl>().First();

            // Assert
            Assert.Equal(RegexOptions.IgnoreCase, secureUrl.RegexOptions);
        }

        [Fact]
        public void AddRegex_ShouldThrowArgumentException_WhenPatternIsNullOrEmpty() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act // Assert
            Assert.Throws<ArgumentException>(() => collection.AddRegex(null));
            Assert.Throws<ArgumentException>(() => collection.AddRegex(string.Empty));
        }

        [Fact]
        public void Add_WithSecureUrlParam_ShouldIncramentTotalByOne_WhenPassedValidSecureUrlInstance() {
            // Arrange
            var collection = new SecureUrlCollection();
            var secureUrl = new SecureUrl();

            // Act 
            collection.Add(secureUrl);

            // Assert
            Assert.NotEmpty(collection);
            Assert.Equal(1, collection.Count());
        }

        [Fact]
        public void Add_WithSecureUrlParam_ShouldThrowArgumentNullException_WhenPassedNull() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act // Assert
            Assert.Throws<ArgumentNullException>(() => collection.Add(null));
        }

        [Fact]
        public void Add_WithStringAndMatchTypeParams_ShouldIncramentTotalByOne_WhenPassedValidUrlString() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddUrl("mock/url");

            // Assert
            Assert.NotEmpty(collection);
            Assert.Equal(1, collection.Count());
        }

        [Fact]
        public void Add_WithStringAndMatchTypeParams_ShouldSetMatchTypeToCaseInsensitive_WhenNoMatchTypeIsProvided() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddUrl("mock/url");
            SecureUrl secureUrl = collection.OfType<SecureUrl>().First();

            // Assert
            Assert.Equal(SecureUrlMatchType.CaseInsensitive, secureUrl.MatchType);
        }

        [Fact]
        public void Add_WithStringAndMatchTypeParams_ShouldSetMatchTypeToProvidedMatchType() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act
            collection.AddUrl("mock/url", false);
            SecureUrl secureUrl = collection.OfType<SecureUrl>().First();

            // Assert
            Assert.Equal(SecureUrlMatchType.CaseSensitive, secureUrl.MatchType);
        }

        [Fact]
        public void Add_WithStringAndMatchTypeParams_ShouldThrowArgumentException_WhenStringUrlIsNullOrEmpty() {
            // Arrange
            var collection = new SecureUrlCollection();

            // Act // Assert
            Assert.Throws<ArgumentException>(() => collection.AddUrl(string.Empty, false));
            Assert.Throws<ArgumentException>(() => collection.AddUrl(null, true));
        }

        [Fact]
        public void Clear_ShouldEmptyAllEntriesFromCollection() {
            // arrange
            var collection = new SecureUrlCollection();
            collection.AddUrl("mock/url");
            collection.AddUrl("mock/url2");

            // assert
            Assert.NotEmpty(collection);

            // act
            collection.Clear();

            // assert
            Assert.Empty(collection);
        }

        [Fact]
        public void IgnoreUrls_ShouldReturn2_WhenThereAre2IgnorUrlTypesInCollection() {
            // arrange
            var collection = new SecureUrlCollection();
            var ignoreUrlOne = new IgnoreUrl();
            var ignoreUrlTwo = new IgnoreUrl();
            var secureUrl = new SecureUrl();

            // act
            collection.Add(ignoreUrlOne);
            collection.Add(ignoreUrlTwo);
            collection.Add(secureUrl);

            //assert
            Assert.Equal(2, collection.IgnoreUrls.Count);
            Assert.Equal(3, collection.Count());
        }

        [Fact]
        public void SecureUrls_ShouldReturn1_WhenThereIs1SecureUrlTypeInCollection() {
            // arrange
            var collection = new SecureUrlCollection();
            var ignoreUrlOne = new IgnoreUrl();
            var ignoreUrlTwo = new IgnoreUrl();
            var secureUrl = new SecureUrl();

            // act
            collection.Add(ignoreUrlOne);
            collection.Add(ignoreUrlTwo);
            collection.Add(secureUrl);

            //assert
            Assert.Equal(1, collection.SecureUrls.Count);
            Assert.Equal(3, collection.Count());
        }

        [Fact]
        public void IgnoreUrl_ShouldAddNewIgnoreUrlTypeToCollection() {
            // arrange
            var collection = new SecureUrlCollection();
            collection.IgnoreUrl(@"test");

            // act
            var count = collection.IgnoreUrls.Count;

            //assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void IgnoreUrl_ShouldThorwArgumentException_WhenPatternIsNullOrEmpty() {
            // arrange
            var collection = new SecureUrlCollection();

            //act and assert 
            Assert.Throws<ArgumentException>(() => collection.IgnoreUrl(string.Empty));
            Assert.Throws<ArgumentException>(() => collection.IgnoreUrl(null));
        }

        #endregion
    }
}