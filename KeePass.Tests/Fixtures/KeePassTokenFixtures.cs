using KeePass.Tests.Fixtures.Models;
using System;

namespace KeePass.Tests.Fixtures
{
    internal static class KeePassTokenFixtures
    {
        internal static TokenFixtureModel GetProperToken()
        {
            return new TokenFixtureModel()
            {
                AccessToken = "proper_access_token_string",
                Error = null,
                ErrorDescription = null,
                ExpirationTime = 1200,
                Type = "bearer"
            };
        }

        internal static TokenFixtureModel GetInvalidTokenCausedByWrongCredentials()
        {
            return new TokenFixtureModel()
            {
                AccessToken = null,
                Error = "400",
                ErrorDescription = "Invalid credentials",
                ExpirationTime = 1200,
                Type = "bearer"
            };
        }

        internal static TokenFixtureModel GetExpiredToken()
        {
            return new TokenFixtureModel()
            {
                AccessToken = "proper_token_in_string",
                Error = null,
                ErrorDescription = null,
                ExpirationTime = 1200,
                Type = "bearer",
                CreatedAt = DateTime.Now - TimeSpan.FromSeconds(1201)
            };
        }
    }
}