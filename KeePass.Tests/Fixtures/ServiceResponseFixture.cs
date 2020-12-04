using System;
using System.Collections.Generic;

namespace KeePass.Tests.Fixtures
{
    internal static class ServiceResponseFixture
    {
        internal static string ValidPasswordResponse() => "valid_password_response";

        internal static ApiDataResponse ValidDataResponse(string askedGuid) => new ApiDataResponse(askedGuid);

        internal static string GetNameUsedInValidDataResponse { get; } = "a_name_corresponding_to_asked_guid";
    }

    public class ApiDataResponse
    {
        public ApiDataResponse(string guid)
        {
            Id = guid;
        }

        public Dictionary<string, string> CustomUserFields { get; set; } = new();
        public Dictionary<string, string> CustomApplicationFields { get; set; } = new();
        public List<string> Attachments { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool HasModifyEntriesAccess { get; set; } = false;
        public bool HasViewEntryContentsAccess { get; set; } = true;

        public Dictionary<string, string> CommentPrompts { get; set; } = new()
        {
            { "AskForCommentOnViewPassword", "false" },
            { "AskForCommentOnViewOffline", "false" },
            { "AskForCommentOnModifyEntries", "false" },
            { "AskForCommentOnMoveEntries", "false" },
            { "AskForCommentOnMoveFolders", "false" },
            { "AskForCommentOnModifyFolders", "false" },
        };

        public string UsageComment { get; set; } = null;
        public string Id { get; set; }
        public string Name { get => "a_name_corresponding_to_asked_guid"; }
        public string Username { get => ServiceResponseFixture.GetNameUsedInValidDataResponse; }
        public string Password { get; set; } = null;
        public string Url { get; set; } = null;
        public string Notes { get; set; } = null;
        public string GroupId { get; set; } = "0907c691-3d1d-4f6f-a2f0-d852283aaf98";
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Parse("2020-11-13T11:44:11+01:00");
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.Parse("2020-11-13T11:44:11+01:00");
        public string Expires { get; set; } = null;
    }
}