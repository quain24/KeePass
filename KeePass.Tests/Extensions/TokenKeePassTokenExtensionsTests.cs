using KeePass.Extensions;
using KeePass.Models;
using System.Collections.Generic;
using Xunit;

namespace KeePass.Tests.Extensions
{
    public class TokenKeePassTokenExtensionsTests
    {
        private readonly Token _correctToken = new()
        {
            AccessToken = "correct",
            Error = null,
            ErrorDescription = null,
            ExpirationTime = 1200,
            Type = "bearer"
        };

        public static IEnumerable<object[]> InvalidTokens =>
            new List<object[]>
            {
                new object[] { new Token {Type = "bearer", ExpirationTime = 0, Error = null, ErrorDescription = null, AccessToken = "abcdefgh"}} ,
                new object[] { new Token {Type = "bearer", ExpirationTime = -50, Error = null, ErrorDescription = null, AccessToken = "abcdefgh"}},
                new object[] { new Token {Type = "bearer", ExpirationTime = 120, Error = "invalid_grant", ErrorDescription = "Username is invalid", AccessToken = "abcdefgh"}},
                new object[] { new Token {Type = "bearer", ExpirationTime = 120, Error = null, ErrorDescription = null, AccessToken = null}},
                new object[] { new Token {Type = "bearer", ExpirationTime = 120, Error = "Invalid", ErrorDescription = null, AccessToken = "abcdefgh"}}
            };

        [Fact]
        internal void IsCorrect_return_true_when_token_is_not_null_and_correct()
        {
            Assert.True(_correctToken.IsCorrect());
        }

        [Fact]
        private void IsExpired_returns_false_if_there_is_still_more_than_60_seconds_left_for_token_lifetime()
        {
            Assert.False(_correctToken.IsExpired());
        }

        [Fact]
        private void IsExpired_returns_true_if_token_is_expire_or_has_less_than_60_seconds_left_for_token_lifetime()
        {
            Token expiredToken = new()
            {
                AccessToken = "correct",
                Error = null,
                ErrorDescription = null,
                ExpirationTime = 50,
                Type = "bearer"
            };

            Assert.True(expiredToken.IsExpired());
        }

        [Fact]
        private void IsExpired_returns_false_if_token_has_more_than_60_seconds_left_for_token_lifetime()
        {
            Token expiredToken = new()
            {
                AccessToken = "correct",
                Error = null,
                ErrorDescription = null,
                ExpirationTime = 120,
                Type = "bearer"
            };

            Assert.False(expiredToken.IsExpired());
        }

        [Theory]
        [MemberData(nameof(InvalidTokens))]
        private void IsCorrect_will_return_false_if_any_property_is_incorrect(Token data)
        {
            Assert.False(data.IsCorrect());
        }
    }
}