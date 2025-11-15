using System;

namespace RoxusZohoAPI.Models.SharePoint
{

    public class SearchSharePointFolderResponse
    {

        public string odatacontext { get; set; }

        public Value[] value { get; set; }

    }

    public class Value
    {

        public DateTime? createdDateTime { get; set; }

        public string id { get; set; }

        public DateTime? lastModifiedDateTime { get; set; }

        public string name { get; set; }

        public string webUrl { get; set; }

        public long? size { get; set; }

        public CreatedBy createdBy { get; set; }

        public LastModifiedBy lastModifiedBy { get; set; }

        public ParentReference parentReference { get; set; }

        public FileSystemInfo fileSystemInfo { get; set; }

        public SharepointFolder folder { get; set; }

        public SearchResult searchResult { get; set; }

        public File file { get; set; }

    }

    public class CreatedBy
    {

        public User user { get; set; }

    }

    public class User
    {

        public string email { get; set; }

        public string displayName { get; set; }

    }

    public class LastModifiedBy
    {
        public User1 user { get; set; }
    }

    public class User1
    {

        public string email { get; set; }

        public string displayName { get; set; }

    }

    public class ParentReference
    {

        public string driveType { get; set; }

        public string driveId { get; set; }

        public string id { get; set; }

        public string siteId { get; set; }

        public string path { get; set; }

    }

    public class FileSystemInfo
    {

        public DateTime? createdDateTime { get; set; }

        public DateTime? lastModifiedDateTime { get; set; }

    }

    public class SharepointFolder
    {

        public int? childCount { get; set; }

    }

    public class SearchResult
    {
    }

    public class File
    {

        public string mimeType { get; set; }

        public Hashes hashes { get; set; }

    }

    public class Hashes
    {

    }

}
