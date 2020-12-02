using System;
using System.Text.Json.Serialization;

namespace KeePass.Tests.Fixtures.Models
{
    /// <summary>
    /// Record used only in unit testing
    /// </summary>
    sealed record TokenFixtureModel
    {
        /// <summary>
        /// String representation of received <see cref="TokenFixtureModel"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        /// <summary>
        /// Type of received <see cref="TokenFixtureModel"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("token_type")]
        public string Type { get; init; }

        /// <summary>
        /// Number of seconds this <see cref="TokenFixtureModel"/> is valid since being received
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("expires_in")]
        public int ExpirationTime { get; init; }

        /// <summary>
        /// Provides error code if an error occurred when asking for this <see cref="TokenFixtureModel"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("error")]
        public string Error { get; init; }

        /// <summary>
        /// Provides error text if an error occurred when asking for this <see cref="TokenFixtureModel"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; init; }

        /// <summary>
        /// When this instance of <see cref="TokenFixtureModel"/> has been created
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedAt { get; init; } = DateTime.Now;

        /// <summary>
        /// <inheritdoc cref="AccessToken"/>
        /// </summary>
        /// <returns>String representation of received <see cref="TokenFixtureModel"/></returns>
        public override string ToString() => AccessToken;

        public static implicit operator string(TokenFixtureModel token) => token.ToString();
    }
}