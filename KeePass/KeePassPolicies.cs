using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KeePass.Extensions;
using KeePass.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace KeePass
{
    internal static class KeePassPolicies
    {
        internal static IAsyncPolicy<HttpResponseMessage> WaitAndRetryAsyncPolicy(string serviceName, int retryAttempts)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .Or<HttpRequestException>()
                .OrResult(msg => !msg.IsSuccessStatusCode &&
                                 msg.StatusCode != HttpStatusCode.BadRequest && // Wrong login / password when asking for token
                                 msg.StatusCode != HttpStatusCode.Unauthorized && // Invalid token string used
                                 msg.StatusCode != HttpStatusCode.NotFound) // asked about invalid guid or no guid
                .WaitAndRetryAsync(retryAttempts, retryAttempt => TimeSpan.FromSeconds(1 * retryAttempt),
                    (response, _, retryNumber, context) =>
                    {
                        var message = string.Format("{0}: request failed, retrying: {1}/{2}.", serviceName, retryNumber, retryAttempts);
                        if (response?.Result?.StatusCode is not null)
                        {
                            message += string.Format(" Response code: {0} ({1}).", response.Result.StatusCode, (int)response.Result.StatusCode);
                            if (retryNumber == retryAttempts)
                                message += string.Format("\nFailed request url: {0}", response?.Result?.RequestMessage?.RequestUri);
                        }

                        if (response?.Exception is not null)
                        {
                            message += string.Format(" Exception type: {0}. Exception Message: {1}", response.Exception.GetType().Name, response.Exception.Message);
                        }
                        context.GetLogger()?.LogWarning(message);
                    });
        }

        internal static IAsyncPolicy<HttpResponseMessage> UnauthorizedRetryAsyncPolicy(string serviceName)
        {
            return Policy<HttpResponseMessage>
                .HandleResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(3),
                    async (response, _, retryNumber, context) =>
                    {
                        context.GetLogger()?.LogInformation("{0}: Service reports that access token is invalid / expired - trying to get new one...", serviceName);

                        if (context["RenewToken"] is Func<Task<Token>> renewToken)
                            await renewToken();
                    });
        }
    }
}