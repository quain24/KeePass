using JustEat.HttpClientInterception;
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
            var askGuid = "abcdef";

            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsFound(askGuid, options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);
            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);

            var response = await Service.AskForSecret(askGuid);

            Assert.Equal(askGuid, response.Id);
            Assert.Equal(response.Username, ServiceResponseFixture.GetNameUsedInValidDataResponse);
            Assert.Equal(response.Password, ServiceResponseFixture.ValidPasswordResponse());
        }

        [Fact]
        public async Task Given_empty_guid_will_throw_ArgumentOutOfRangeException()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsFound(string.Empty, options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = string.Empty;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Given_whitespace_guid_will_throw_ArgumentOutOfRangeException()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsFound("       ", options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            var askGuid = string.Empty;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Given_null_guid_will_throw_ArgumentOutOfRangeException()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsFound(string.Empty, options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);
            string askGuid = null;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_bad_credentials_for_token_were_sent()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetInvalidPasswordSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetInvalidPasswordSettings(), logger);
            var askGuid = "not_important";

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_token_is_expired()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var askGuid = "not_important";
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsTokenUnauthorizedExpired(askGuid, options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }

        [Fact]
        public async Task Will_throw_HttpRequestException_when_searched_guid_is_not_found()
        {
            using var logger = Output.BuildLoggerFor<IKeePassService>();
            var askGuid = "not_important";
            var options = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
            HttpClientFixture.HandleTokenNormally(options);
            HttpClientFixture.HandleGuidAsNotFound(askGuid, options);
            var client = options.CreateHttpClient(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress);

            Service = new KeePassService(client, KeePassSettingsFixtures.GetProperKeePassSettings(), logger);

            await Assert.ThrowsAsync<HttpRequestException>(() => Service.AskForSecret(askGuid));
        }
    }
}