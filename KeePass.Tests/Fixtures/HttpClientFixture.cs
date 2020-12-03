using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KeePass.Tests.Fixtures
{
    public static class HttpClientFixture
    {
        public static HttpClient GetValidResponseClient()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var data = KeePassSettingsFixtures.GetProperKeePassSettings();
                    var guid = string.Empty;

                    // Ugly, but beats regex solution
                    if ((request.RequestUri?.AbsoluteUri.StartsWith(data.BaseAddress + data.RestEndpoint) ?? false) &&
                        (!request.RequestUri?.AbsoluteUri.EndsWith("password", StringComparison.InvariantCultureIgnoreCase) ?? false))
                    {
                        guid = request.RequestUri.AbsoluteUri.Replace(data.BaseAddress + data.RestEndpoint, "");
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content = new StringContent(ServiceResponseFixture.ValidDataResponse(guid));
                        return response;
                    }
                    if ((request.RequestUri?.AbsoluteUri.StartsWith(data.BaseAddress + data.RestEndpoint) ?? false) &&
                             (request.RequestUri?.AbsoluteUri.EndsWith("password", StringComparison.InvariantCultureIgnoreCase) ?? false))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content = new StringContent(ServiceResponseFixture.ValidPasswordResponse());
                        return response;
                    }

                    if (request.RequestUri.AbsoluteUri.StartsWith(data.BaseAddress + data.TokenEndpoint))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content =
                            new StringContent(JsonSerializer.Serialize(KeePassTokenFixtures.GetProperToken()));
                        return response;
                    }

                    throw new InvalidOperationException("Error when generating response in http client from fixture.");
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress)
            };
        }

        public static HttpClient GetOnlyInvalidTokenClient()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var data = KeePassSettingsFixtures.GetProperKeePassSettings();

                    if (request.RequestUri.AbsoluteUri.StartsWith(data.BaseAddress + data.TokenEndpoint))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.RequestMessage = request;
                        response.Content =
                            new StringContent(JsonSerializer.Serialize(KeePassTokenFixtures.GetInvalidTokenCausedByWrongCredentials()));
                        return response;
                    }

                    throw new InvalidOperationException("Error when generating response in http client from fixture.");
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress)
            };
        }

        public static HttpClient GetOnlyTimeoutTokenClient()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var data = KeePassSettingsFixtures.GetProperKeePassSettings();

                    if (request.RequestUri.AbsoluteUri.StartsWith(data.BaseAddress + data.TokenEndpoint))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content =
                            new StringContent(JsonSerializer.Serialize(KeePassTokenFixtures.GetExpiredToken()));
                        return response;
                    }

                    throw new InvalidOperationException("Error when generating response in http client from fixture.");
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress)
            };
        }

        public static HttpClient GetGuidNotFoundClient()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var data = KeePassSettingsFixtures.GetProperKeePassSettings();
                    var guid = string.Empty;

                    if (request.RequestUri.AbsoluteUri.StartsWith(data.BaseAddress + data.TokenEndpoint))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content =
                            new StringContent(JsonSerializer.Serialize(KeePassTokenFixtures.GetProperToken()));
                        return response;
                    }
                    else
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.RequestMessage = request;
                        response.Content = new StringContent("");
                        return response;
                    }
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress)
            };
        }

        public static HttpClient GetValidTokenButUnauthorizedResponseClient()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var data = KeePassSettingsFixtures.GetProperKeePassSettings();
                    var guid = string.Empty;

                    if ((request.RequestUri?.AbsoluteUri.StartsWith(data.BaseAddress + data.RestEndpoint) ?? false) &&
                        (!request.RequestUri?.AbsoluteUri.EndsWith("password", StringComparison.InvariantCultureIgnoreCase) ?? false))
                    {
                        guid = request.RequestUri.AbsoluteUri.Replace(data.BaseAddress + data.RestEndpoint, "");
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content = new StringContent(ServiceResponseFixture.ValidDataResponse(guid));
                        return response;
                    }
                    if ((request.RequestUri?.AbsoluteUri.StartsWith(data.BaseAddress + data.RestEndpoint) ?? false) &&
                             (request.RequestUri?.AbsoluteUri.EndsWith("password", StringComparison.InvariantCultureIgnoreCase) ?? false))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        response.RequestMessage = request;
                        response.Content = new StringContent(ServiceResponseFixture.ValidPasswordResponse());
                        return response;
                    }

                    if (request.RequestUri.AbsoluteUri.StartsWith(data.BaseAddress + data.TokenEndpoint))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.RequestMessage = request;
                        response.Content =
                            new StringContent(JsonSerializer.Serialize(KeePassTokenFixtures.GetProperToken()));
                        return response;
                    }

                    throw new InvalidOperationException("Error when generating response in http client from fixture.");
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri(KeePassSettingsFixtures.GetProperKeePassSettings().BaseAddress)
            };
        }
    }
}