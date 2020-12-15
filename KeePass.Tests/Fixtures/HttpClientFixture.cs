using JustEat.HttpClientInterception;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KeePass.Tests.Fixtures
{
    public static class HttpClientFixture
    {
        private static string ProperRequestContentForToken()
        {
            var content = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", KeePassSettingsFixtures.GetProperKeePassSettings().Username},
                {"password", KeePassSettingsFixtures.GetProperKeePassSettings().Password}
            };

            return new FormUrlEncodedContent(content).ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public static void HandleTokenNormally(HttpClientInterceptorOptions options, int delay = 0)
        {
            var pol = new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForPost().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().TokenEndpoint)
                .ForContent(ctx =>
               {
                   var context = ctx.ReadAsStringAsync().GetAwaiter().GetResult();
                   return context == ProperRequestContentForToken();
               });

            if (delay > 0)
            {
                pol.WithInterceptionCallback(async (_) => await Task.Delay(delay));
            }

            pol.Responds()
            .WithStatus(HttpStatusCode.OK)
            .WithSystemTextJsonContent(new
            {
                access_token = KeePassTokenFixtures.GetProperToken().AccessToken,
                token_type = KeePassTokenFixtures.GetProperToken().Type,
                expires_in = KeePassTokenFixtures.GetProperToken().ExpirationTime,
                error = KeePassTokenFixtures.GetProperToken().Error,
                error_description = KeePassTokenFixtures.GetProperToken().ErrorDescription
            })
            .RegisterWith(options);

            var pol2 = new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForPost().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().TokenEndpoint)
                .ForContent(ctx =>
                {
                    var context = ctx.ReadAsStringAsync().GetAwaiter().GetResult();
                    return !context.Contains(
                               $"password={KeePassSettingsFixtures.GetProperKeePassSettings().Password}") ||
                           !context.Contains(
                               $"username={KeePassSettingsFixtures.GetProperKeePassSettings().Username}");
                });

            if (delay > 0)
            {
                pol2.WithInterceptionCallback(async (_) => await Task.Delay(delay));
            }
            
            pol2.Responds()
            .WithStatus(HttpStatusCode.Unauthorized)
            .WithSystemTextJsonContent(new
            {
                access_token = KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials().AccessToken,
                token_type = KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials().Type,
                expires_in = KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials().ExpirationTime,
                error = KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials().Error,
                error_description = KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials().ErrorDescription
            })
            .RegisterWith(options);
        }

        public static void RespondWithExpiredToken(HttpClientInterceptorOptions options)
        {
            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForPost().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().TokenEndpoint)
                .Responds()
                .WithStatus(HttpStatusCode.OK)
                .WithSystemTextJsonContent(new
                {
                    access_token = KeePassTokenFixtures.GetExpiredToken().AccessToken,
                    token_type = KeePassTokenFixtures.GetExpiredToken().Type,
                    expires_in = KeePassTokenFixtures.GetExpiredToken().ExpirationTime,
                    error = KeePassTokenFixtures.GetExpiredToken().Error,
                    error_description = KeePassTokenFixtures.GetExpiredToken().ErrorDescription
                })
                .RegisterWith(options);
        }

        public static void HandleGuidAsTokenUnauthorizedExpired(string guid, HttpClientInterceptorOptions options)
        {
            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid)
                .Responds()
                .WithStatus(HttpStatusCode.Unauthorized)
                .WithJsonContent(ServiceResponseFixture.ValidDataResponse(guid))
                .RegisterWith(options);

            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid + "/password")
                .Responds()
                .WithStatus(HttpStatusCode.Unauthorized)
                .RegisterWith(options);
        }

        public static void HandleGuidAsNotFound(string guid, HttpClientInterceptorOptions options)
        {
            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid)
                .Responds()
                .WithStatus(HttpStatusCode.NotFound)
                .WithJsonContent(ServiceResponseFixture.ValidDataResponse(guid))
                .RegisterWith(options);

            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid + "/password")
                .Responds()
                .WithStatus(HttpStatusCode.NotFound)
                .RegisterWith(options);
        }

        public static void HandleGuidAsFound(string guid, HttpClientInterceptorOptions options)
        {
            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid)
                .Responds()
                .WithStatus(HttpStatusCode.OK)
                .WithJsonContent(ServiceResponseFixture.ValidDataResponse(guid))
                .RegisterWith(options);

            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid + "/password")
                .Responds()
                .WithStatus(HttpStatusCode.OK)
                .WithJsonContent(ServiceResponseFixture.ValidPasswordResponse())
                .RegisterWith(options);
        }

        public static void HandleGuidAsFoundWithDelay(string guid, HttpClientInterceptorOptions options, int delay)
        {
            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid)
                .WithInterceptionCallback(async (_) => await Task.Delay(delay))
                .Responds()
                .WithStatus(HttpStatusCode.OK)
                .WithJsonContent(ServiceResponseFixture.ValidDataResponse(guid))
                .RegisterWith(options);

            new HttpRequestInterceptionBuilder()
                .Requests().ForHttps().ForGet().ForAnyHost()
                .ForPath(KeePassSettingsFixtures.GetProperKeePassSettings().RestEndpoint + guid + "/password")
                .WithInterceptionCallback(async (_) => await Task.Delay(delay))
                .Responds()
                .WithStatus(HttpStatusCode.OK)
                .WithJsonContent(ServiceResponseFixture.ValidPasswordResponse())
                .RegisterWith(options);
        }
    }
}