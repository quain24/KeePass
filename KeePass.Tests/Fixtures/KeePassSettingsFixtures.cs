namespace KeePass.Tests.Fixtures
{
    public static class KeePassSettingsFixtures
    {
        public static KeePassSettings GetProperKeePassSettings()
        {
            return new()
            {
                BaseAddress = "https://www.proper.base.address.com/",
                Password = "valid_password",
                Username = "valid_username",
                RestEndpoint = "api/v4/rest/credential/",
                TokenEndpoint = "OAuth2/Token"
            };
        }

        public static KeePassSettings GetInvalidPasswordSettings()
        {
            return new()
            {
                BaseAddress = "https://www.proper.base.address.com/",
                Password = "bad_password",
                Username = "valid_username",
                RestEndpoint = "api/v4/rest/credential/",
                TokenEndpoint = "OAuth2/Token"
            };
        }
    }
}