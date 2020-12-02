using System;
using System.Text.Json.Serialization;

namespace KeePass.Models
{
    internal sealed record Token
    {
        /// <summary>
        /// String representation of received <see cref="Token"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        /// <summary>
        /// Type of received <see cref="Token"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("token_type")]
        public string Type { get; init; }

        /// <summary>
        /// Number of seconds this <see cref="Token"/> is valid since being received
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("expires_in")]
        public int ExpirationTime { get; init; }

        /// <summary>
        /// Provides error code if an error occurred when asking for this <see cref="Token"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("error")]
        public string Error { get; init; }

        /// <summary>
        /// Provides error text if an error occurred when asking for this <see cref="Token"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; init; }

        /// <summary>
        /// When this instance of <see cref="Token"/> has been created
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedAt { get; } = DateTime.Now;

        /// <summary>
        /// <inheritdoc cref="AccessToken"/>
        /// </summary>
        /// <returns>String representation of received <see cref="Token"/></returns>
        public override string ToString() => AccessToken;

        public static implicit operator string(Token token) => token.ToString();
    }
}