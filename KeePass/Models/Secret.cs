using System.Text.Json.Serialization;

namespace KeePass.Models
{
    /// <summary>
    /// Object containing single Username, Password and GUID received from password service.
    /// </summary>
    public record Secret
    {
        /// <summary>
        /// KeePass guid ID
        /// </summary>
        [JsonInclude]
        public string Id { get; init; }

        /// <summary>
        /// Retrieved username corresponding to given <see cref="Secret.Id"/>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Username")]
        public string Username { get; init; }

        /// <summary>
        /// Retrieved password corresponding to given <see cref="Secret.Id"/>
        /// </summary>
        public string Password { get; init; }
    }
}