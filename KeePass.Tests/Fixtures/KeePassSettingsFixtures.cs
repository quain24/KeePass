namespace KeePass.Tests.Fixtures
{
    public static class KeePassSettingsFixtures
    {
        public static KeePassSettings GetProperKeePassSettings()
        {
            return new(
                username: "valid_username",
                password: "valid_password",
                baseAddress: "https://www.proper.base.address.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/");
        }

        public static KeePassSettings GetInvalidPasswordSettings()
        {
            return new(
                username: "valid_username",
                password: "bad_password",
                baseAddress: "https://www.proper.base.address.com/",
                tokenEndpoint: "OAuth2/Token",
                restEndpoint: "api/v4/rest/credential/");
        }
    }
}