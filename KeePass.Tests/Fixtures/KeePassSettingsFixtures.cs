namespace KeePass.Tests.Fixtures
{
    public static class KeePassSettingsFixtures
    {
        public static KeePassSettings GetProperKeePassSettings()
        {
            return new()
            {
                BaseAddress = "http://www.proper.base.address.com/",
                Password = "valid_password",
                Username = "valid_username",
                RestEndpoint = "api/v4/rest/credential/",
                TokenEndpoint = "OAuth2/Token"
            };
        }
    }
}