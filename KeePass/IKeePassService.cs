using KeePass.Models;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace KeePass
{
    /// <summary>
    /// Pleasant Password Server connection service, used for retrieval of username / password data pair by given guid
    /// </summary>
    public interface IKeePassService
    {
        /// <summary>
        /// Asks KeePass service for login and password to service corresponding with given <paramref name="guid"/>
        /// </summary>
        /// <exception cref="HttpRequestException">Service was unavailable, provided KeePass service credentials were incorrect or received responses were incorrect</exception>
        /// <exception cref="Exception">Service was unable to deserialize response from API into a <see cref="Secret"/> or <see cref="Token"/> object</exception>
        /// <exception cref="AuthenticationException">Service received and deserialized <see cref="Token"/> properly, but it failed internal validation</exception>
        /// <param name="guid"></param>
        /// <returns>Valid <see cref="Secret"/> if data was received successfully or empty object</returns>
        Task<Secret> AskForSecret(string guid);
    }
}