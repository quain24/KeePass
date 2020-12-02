namespace KeePass.Tests.Fixtures
{
    internal static class ServiceResponseFixture
    {
        internal static string ValidPasswordResponse() => "valid_password_response";

        internal static string ValidDataResponse(string askedGuid)
        {
            return $"{{\"CustomUserFields\":{{}},\"CustomApplicationFields\":{{}}," +
                   $"\"Attachments\":[],\"Tags\":[],\"HasModifyEntriesAccess\":false,\"HasViewEntryContentsAccess\":true," +
                   $"\"CommentPrompts\":{{\"AskForCommentOnViewPassword\":false,\"AskForCommentOnViewOffline\":false," +
                   $"\"AskForCommentOnModifyEntries\":false,\"AskForCommentOnMoveEntries\":false,\"AskForCommentOnMoveFolders\":false," +
                   $"\"AskForCommentOnModifyFolders\":false}},\"UsageComment\":null,\"Id\":\"{askedGuid}\"," +
                   $"\"Name\":\"DHL Parcel Status Updater APP\",\"Username\":\"{GetNameUsedInValidDataResponse()}\",\"Password\":null," +
                   $"\"Url\":\"\",\"Notes\":\"\",\"GroupId\":\"0907c691-3d1d-4f6f-a2f0-d852283aaf98\",\"Created\":\"2020-11-13T11:44:11+01:00\"," +
                   $"\"Modified\":\"2020-11-13T11:44:11+01:00\",\"Expires\":null}}";
        }

        internal static string GetNameUsedInValidDataResponse() => "a_name_corresponding_to_asked_guid";
    }
}