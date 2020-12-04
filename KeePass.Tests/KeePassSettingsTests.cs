using System;
using Xunit;

namespace KeePass.Tests
{
    public class KeePassSettingsTests
    {
        private readonly KeePassSettings _correctSettings = new()
        {
            BaseAddress = "https://www.correct.com/",
            RestEndpoint = "api/v4/rest/credential/",
            Password = "valid_password",
            TokenEndpoint = "OAuth2/Token",
            Username = "valid_username"
        };

        [Fact]
        public void Will_be_created_if_given_all_proper_data()
        {
            KeePassSettings correctSettings = new()
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = "valid_username"
            };

            Assert.NotNull(correctSettings);
            Assert.Equal("https://www.correct.com/", correctSettings.BaseAddress);
            Assert.Equal("api/v4/rest/credential/", correctSettings.RestEndpoint);
            Assert.Equal("valid_password", correctSettings.Password);
            Assert.Equal("OAuth2/Token", correctSettings.TokenEndpoint);
            Assert.Equal("valid_username", correctSettings.Username);
        }

        [Fact]
        public void Will_throw_ArgumentException_if_base_address_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = null,
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = "valid_username"
            });

            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = "valid_username"
            });
        }

        [Fact]
        public void Will_throw_ArgumentException_if_rest_endpoint_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = null,
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = "valid_username"
            });
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = "valid_username"
            });
        }

        [Fact]
        public void Will_throw_ArgumentException_if_token_endpoint_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = null,
                Username = "valid_username"
            });
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "",
                Username = "valid_username"
            });
        }

        [Fact]
        public void Will_throw_ArgumentException_if_username_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = null
            });

            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = ""
            });
        }

        [Fact]
        public void Will_throw_ArgumentException_if_two_properties_are_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "https://www.correct.com/",
                RestEndpoint = string.Empty,
                Password = "valid_password",
                TokenEndpoint = string.Empty,
                Username = "valid_username"
            });

            Assert.Throws<ArgumentException>(() => new KeePassSettings
            {
                BaseAddress = "",
                RestEndpoint = "api/v4/rest/credential/",
                Password = "valid_password",
                TokenEndpoint = "OAuth2/Token",
                Username = ""
            });
        }
    }
}