using System;
using Xunit;

namespace KeePass.Tests
{
    public class KeePassSettingsTests
    {
        [Fact]
        public void Will_be_created_if_given_all_proper_data()
        {
            KeePassSettings correctSettings = new(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/");

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
            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: null,
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));

            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));
        }

        [Fact]
        public void Will_throw_ArgumentException_if_rest_endpoint_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: null));

            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: ""));
        }

        [Fact]
        public void Will_throw_ArgumentException_if_token_endpoint_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: null,
                restEndpoint: "api/v4/rest/credential/"));

            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "",
                restEndpoint: "api/v4/rest/credential/"));
        }

        [Fact]
        public void Will_throw_ArgumentException_if_username_is_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: null,
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));

            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "",
                password: "valid_password",
                baseAddress: "https://www.correct.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));
        }

        [Fact]
        public void Will_throw_ArgumentException_if_two_properties_are_null_or_empty()
        {
            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: null,
                password: "valid_password",
                baseAddress: null,
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));

            Assert.Throws<ArgumentException>(() => new KeePassSettings(
                username: "",
                password: "valid_password",
                baseAddress: "",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/"));
        }
    }
}