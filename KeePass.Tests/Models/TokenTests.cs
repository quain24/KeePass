using KeePass.Tests.Fixtures;
using Xunit;

namespace KeePass.Tests.Models
{
    public class TokenTests
    {
        [Fact]
        public void Will_return_valid_token_string_if_implicit_conversion_to_string_was_used()
        {
            var token = KeePassTokenFixtures.GetProperToken();

            Assert.Equal(token.AccessToken, token);
        }

        [Fact]
        public void Will_return_valid_token_string_if_ToString_conversion_was_used()
        {
            var token = KeePassTokenFixtures.GetProperToken();

            Assert.Equal(token.AccessToken, token.ToString());
        }
    }
}