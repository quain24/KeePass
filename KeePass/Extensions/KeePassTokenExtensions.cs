using KeePass.Models;
using System;

namespace KeePass.Extensions
{
    internal static class KeePassTokenExtensions
    {
        /// <summary>
        /// Checks if <see cref="Token"/> is correct based on its error status and if it's null.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>If <see cref="Token"/> is valid</returns>
        internal static bool IsCorrect(this Token token)
        {
            return token is not null &&
                   token.Error is null &&
                   string.IsNullOrWhiteSpace(token.AccessToken) is false &&
                   token.ExpirationTime > 0;
        }

        /// <summary>
        /// Checks if <see cref="Token"/> is not expired
        /// </summary>
        /// <param name="token"></param>
        /// <returns>True if <see cref="Token"/> is expired</returns>
        internal static bool IsExpired(this Token token)
        {
            _ = token ?? throw new ArgumentNullException(nameof(token), "Tried to check expiration state on a null object.");
            return (DateTime.Now - token.CreatedAt).Seconds > (token.ExpirationTime - 60); // one minute earlier so wont timeout when httpRequest is in progress
        }
    }
}