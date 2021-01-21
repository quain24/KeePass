using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KeePass
{
    /// <summary>
    /// If <see cref="IKeePassService"/> is used without dependency injection, this class should be passed as a constructor parameter.
    /// <br/>It returns a singleton <see cref="HttpClient"/> configured using options from <see cref="KeePassSettings"/>
    /// </summary>
    public sealed class NoDiHttpClientFactory : IHttpClientFactory
    {
        private static HttpClient _httpClient;
        private readonly KeePassSettings _settings;
        private readonly object _lock = new object();

        /// <summary>
        /// <inheritdoc cref="NoDiHttpClientFactory"/>
        /// </summary>
        /// <param name="settings"><see cref="KeePassSettings"/> options used to configure client that will be provided by this class</param>
        public NoDiHttpClientFactory(KeePassSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Creates a singleton <see cref="HttpClient"/> which will be used by <see cref="IKeePassService"/> when no <see cref="IHttpClientFactory"/>
        /// <br/>from dependency container can be provided.
        /// <br/><paramref name="name"/> is left only for compatibility.
        /// </summary>
        /// <param name="name">ignored, left for interface compatibility</param>
        /// <returns>A singleton <see cref="HttpClient"/> configured by options from constructor</returns>
        public HttpClient CreateClient(string name)
        {
            lock (_lock)
            {
                if (_httpClient is not null)
                    return _httpClient;

                _httpClient = new HttpClient { BaseAddress = new Uri(_settings.BaseAddress) };
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return _httpClient;
            }
        }
    }
}