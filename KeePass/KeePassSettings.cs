using System;

namespace KeePass
{
    /// <summary>
    /// Class used when configuring <see cref="IKeePassService"/> instance.
    /// </summary>
    public class KeePassSettings
    {
        private const string DefaultTokenEndpoint = "OAuth2/Token";
        private const string DefaultRestEndpoint = "api/v4/rest/credential/";
        private readonly string _username;
        private readonly string _password;
        private readonly string _baseAddress;
        private readonly string _tokenEndpoint;
        private readonly string _restEndpoint;

        /// <summary>
        /// <inheritdoc cref="KeePassSettings"/>
        /// </summary>
        /// <param name="username">KeePass service username</param>
        /// <param name="password">KeePass service password</param>
        /// <param name="baseAddress">KeePass service base address. Must end with '/' character.</param>
        /// <param name="tokenEndpoint">KeePass service endpoint for token</param>
        /// <param name="restEndpoint">KeePass service rest endpoint for getting usernames / password pairs</param>
        public KeePassSettings(string username, string password, string baseAddress, string tokenEndpoint = DefaultTokenEndpoint, string restEndpoint = DefaultRestEndpoint)
        {
            Username = username;
            Password = password;
            BaseAddress = baseAddress;
            TokenEndpoint = tokenEndpoint;
            RestEndpoint = restEndpoint;
        }

        /// <summary>
        /// Base address for KeePass server, including port. Must end with "/"
        /// <para><example>example: https://server-keepass:900/ </example></para>
        /// </summary>
        public string BaseAddress { get => _baseAddress; init => _baseAddress = ValidateProperty(value, nameof(BaseAddress)); }

        /// <summary>
        /// Endpoint for token retrieval, typically <c>"OAuth2/Token"</c>
        /// </summary>
        public string TokenEndpoint { get => _tokenEndpoint; init => _tokenEndpoint = ValidateProperty(value, nameof(TokenEndpoint)); }

        /// <summary>
        /// Endpoint for REST endpoint, typically <c>"api/v4/rest/credential/"</c>
        /// </summary>
        public string RestEndpoint { get => _restEndpoint; init => _restEndpoint = ValidateProperty(value, nameof(RestEndpoint)); }

        /// <summary>
        /// Username for KeePass server
        /// </summary>
        public string Username { get => _username; init => _username = ValidateProperty(value, nameof(Username)); }

        /// <summary>
        /// Password for KeePass server
        /// </summary>
        public string Password { get => _password; init => _password = ValidateProperty(value, nameof(Password)); }

        private static string ValidateProperty(string value, string name)
        {
            return string.IsNullOrEmpty(value)
                ? throw new ArgumentException($"{name} property cannot be neither null nor empty. Value was not set or, if taken from appsettings.json - group or value might be missing", name)
                : value;
        }
    }
}