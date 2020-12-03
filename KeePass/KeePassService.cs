﻿using KeePass.Extensions;
using KeePass.Models;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KeePass
{
    /// <summary>
    /// <inheritdoc cref="IKeePassService"/>
    /// </summary>
    public class KeePassService : IKeePassService
    {
        private const string Name = "KeePass";

        private readonly HttpClient _client;
        private readonly KeePassSettings _setting;
        private readonly ILogger<IKeePassService> _logger;

        private readonly IAsyncPolicy<HttpResponseMessage> _retryIfUnauthorizedAsyncPolicy = KeePassPolicies.UnauthorizedRetryAsyncPolicy(Name);
        private readonly SemaphoreSlim _getTokenLock = new(1);
        private Token _token = new();

        /// <summary>
        /// <inheritdoc cref="KeePassService"/>
        /// </summary>
        /// <param name="client">This <see cref="HttpClient"/> instance will be used for all communication in this service</param>
        /// <param name="settings"><see cref="KeePassSettings"/> containing all necessary options for this service</param>
        /// <param name="logger"><see cref="ILogger{KeePassService}"/> - responsible for all logging inside this service, optional</param>
        public KeePassService(HttpClient client, KeePassSettings settings, ILogger<IKeePassService> logger)
        {
            _logger = logger;
            _setting = settings ?? throw new ArgumentNullException(nameof(settings), "Settings object must not be null.");
            _client = client;
        }

        /// <summary>
        /// <inheritdoc cref="IKeePassService"/>
        /// </summary>
        /// <exception cref="HttpRequestException">Service was unavailable, provided KeePass service credentials were incorrect or received responses were incorrect</exception>
        /// <exception cref="Exception">Service was unable to deserialize response from API into a <see cref="Secret"/> or <see cref="Token"/> object</exception>
        /// <exception cref="AuthenticationException">Service received and deserialized <see cref="Token"/> properly, but it failed internal validation</exception>
        /// <param name="guid"></param>
        /// <returns>Valid <see cref="Secret"/> if data was received successfully or empty object</returns>
        public async Task<Secret> AskForSecret(string guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(guid))
                    throw new ArgumentOutOfRangeException(nameof(guid), "Passed in guid cannot be null or empty!");

                _logger?.LogInformation("{0}: Asking for secret of {1}", Name, guid);

                await GetToken().ConfigureAwait(false);

                var dataTask = AskForDataResponse(guid);
                var passwordTask = AskForPasswordResponse(guid);

                var dataResponse = await dataTask.ConfigureAwait(false);
                var passwordResponse = await passwordTask.ConfigureAwait(false);

                dataResponse.EnsureSuccessStatusCode();
                passwordResponse.EnsureSuccessStatusCode();

                _logger?.LogInformation("{0}: Secret received.", Name);
                return await CreateSecret(dataResponse, passwordResponse).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "{0}: Was unable to get secret for \"{1}\" guid - response code - {2}", Name, guid,
                    ex.StatusCode);
                throw;
            }
        }

        private async Task<HttpResponseMessage> AskForDataResponse(string guid)
        {
            return await _retryIfUnauthorizedAsyncPolicy.ExecuteAsync(async _ =>
            {
                _logger?.LogDebug("{0}: Trying for data", Name);
                var requestForData = new HttpRequestMessage(HttpMethod.Get, _setting.RestEndpoint + guid);
                AddRequiredHeadersTo(requestForData, _token);

                return await _client.SendAsync(requestForData).ConfigureAwait(false);
            }, ContextForSecretsRequest("Data")).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> AskForPasswordResponse(string guid)
        {
            return await _retryIfUnauthorizedAsyncPolicy.ExecuteAsync(async _ =>
            {
                _logger?.LogDebug("{0}: Trying for password", Name);
                var requestForPassword = new HttpRequestMessage(HttpMethod.Get, _setting.RestEndpoint + guid + "/password");
                AddRequiredHeadersTo(requestForPassword, _token);

                return await _client.SendAsync(requestForPassword).ConfigureAwait(false);
            }, ContextForSecretsRequest("Password")).ConfigureAwait(false);
        }

        private async Task<Secret> CreateSecret(HttpResponseMessage dataResponse, HttpResponseMessage passwordResponse)
        {
            try
            {
                var secret = await JsonSerializer
                    .DeserializeAsync<Secret>(await dataResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    .ConfigureAwait(false);

                var password = GetPasswordFrom(await passwordResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

                return secret with { Password = password };
            }
            catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
            {
                _logger?.LogError(ex, "{0}: Service was unable to deserialize response from api.");
                throw;
            }
        }

        private static string GetPasswordFrom(string passwordMessage)
        {
            if (string.IsNullOrEmpty(passwordMessage))
                return string.Empty;

            if (passwordMessage.StartsWith('"') && passwordMessage.EndsWith('"'))
                return passwordMessage[1..^1]; // removal of " from beginning and end.

            return passwordMessage;
        }

        private async Task<Token> GetToken()
        {
            try
            {
                await _getTokenLock.WaitAsync().ConfigureAwait(false);
                return _token.IsCorrect() && !_token.IsExpired()
                    ? _token
                    : await RenewToken().ConfigureAwait(false);
            }
            finally
            {
                _getTokenLock.Release();
            }
        }

        private async Task<Token> RenewToken()
        {
            _logger?.LogInformation("{0}: Asking remote service for token...", Name);

            var response = await _client
                .SendAsync(RequestForToken())
                .ConfigureAwait(false);

            var freshToken = await DeserializeToken(response).ConfigureAwait(false);

            EnsureCorrectnessOf(response, freshToken);

            _logger?.LogInformation("{0}: Token received.", Name);
            return _token = freshToken;
        }

        private HttpRequestMessage RequestForToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _setting.TokenEndpoint);
            var content = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", _setting.Username},
                {"password", _setting.Password}
            };
            request.Content = new FormUrlEncodedContent(content);

            var pollyContext = new Context().WithLogger(_logger);
            request.SetPolicyExecutionContext(pollyContext);

            return request;
        }

        private async Task<Token> DeserializeToken(HttpResponseMessage response)
        {
            Token freshToken = null;
            try
            {
                freshToken = JsonSerializer.Deserialize<Token>(await response?.Content?.ReadAsStringAsync() ?? string.Empty);
            }
            catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
            {
                _logger?.LogError(ex, "{0}: Unable to deserialize Token from API response - possibly response format changed?", Name);
            }

            return freshToken;
        }

        private void EnsureCorrectnessOf(HttpResponseMessage response, Token freshToken)
        {
            try
            {
                response.EnsureSuccessStatusCode();
                if (!freshToken.IsCorrect() || freshToken.IsExpired())
                    throw new HttpRequestException();
            }
            catch (HttpRequestException)
            {
                _logger?.LogError("{0}: Error - could not create security token - API response: Error: {1}, Description: {2}",
                    Name, freshToken?.Error ?? "not provided", freshToken?.ErrorDescription ?? "not provided");
                throw;
            }
        }

        private static void AddRequiredHeadersTo(HttpRequestMessage request, Token token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(token.Type, token);
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
        }

        private Context ContextForSecretsRequest(string nameOfRequestedPart)
        {
            var context = new Context().WithLogger(_logger);
            context.TryAdd("RenewToken", new Func<Task<Token>>(async () => await RenewToken().ConfigureAwait(false)));
            context.TryAdd("Requesting", nameOfRequestedPart);
            return context;
        }
    }
}