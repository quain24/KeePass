using KeePass.Tests.Fixtures;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace KeePass.Tests
{
    public class KeePassServiceTests
    {
        private ITestOutputHelper Output { get; }
        private KeePassService Service { get; set; }

        public KeePassServiceTests(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public async Task Given_proper_guid_will_return_proper_secret()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            Service = new KeePassService(HttpClientFixture.GetValidResponseClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = "abcdef";

            var response = await Service.AskForSecret(askGuid);

            Assert.Equal(askGuid, response.Id);
            Assert.Equal(response.Username, ServiceResponseFixture.GetNameUsedInValidDataResponse());
            Assert.Equal(response.Password, ServiceResponseFixture.ValidPasswordResponse());
        }

        [Fact]
        public async Task Given_empty_guid_will_throw_ArgumentOutOfRangeException()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetValidResponseClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = string.Empty;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Given_null_guid_will_throw_ArgumentOutOfRangeException()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetValidResponseClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            string askGuid = null;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_bad_credentials_for_token_were_sent()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetOnlyInvalidTokenClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = "not_important";

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_token_is_expired_after_retrying()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetOnlyTimeoutTokenClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = "not_important";

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_searched_guid_is_not_found()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetGuidNotFoundClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = "not_important";

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_trying_to_get_secret_with_invalid_token_for_password_part()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();

            Service = new KeePassService(HttpClientFixture.GetValidTokenButUnauthorizedResponseClient(),
                KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = "not_important";

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }
    }
}