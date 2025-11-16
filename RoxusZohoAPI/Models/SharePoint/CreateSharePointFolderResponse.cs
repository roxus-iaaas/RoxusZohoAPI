using System;

namespace RoxusZohoAPI.Models.SharePoint
{
    public class CreateSharePointFolderResponse
    {
        public string odatacontext { get; set; }

        public string odataetag { get; set; }

        public DateTime? createdDateTime { get; set; }

        public string eTag { get; set; }

        public string id { get; set; }

        public DateTime? lastModifiedDateTime { get; set; }

        public string name { get; set; }

        public decimal? size { get; set; }

        public string webUrl { get; set; }

        public string cTag { get; set; }

        public CommentSettings commentSettings { get; set; }

        public CreatedBy createdBy { get; set; }

        public LastModifiedBy lastModifiedBy { get; set; }

        public ParentReference parentReference { get; set; }

        public FileSystemInfo fileSystemInfo { get; set; }

        public SharePointFolder folder { get; set; }

        public Shared shared { get; set; }
    }

    public class CommentSettings
    {
        public CommentingDisabled commentingDisabled { get; set; }
    }

    public class CommentingDisabled
    {
        public bool? isDisabled { get; set; }
    }

    public class Application
    {
        public string displayName { get; set; }
        public string id { get; set; }
    }

    public class SharepointIds
    {
        public string listId { get; set; }

        public string listItemUniqueId { get; set; }

        public string siteId { get; set; }

        public string siteUrl { get; set; }

        public string tenantId { get; set; }

        public string webId { get; set; }

    }

    public class SharePointFolder
    {
        public int? childCount { get; set; }

        public View view { get; set; }

    }

    public class View
    {
        public string sortBy { get; set; }

        public string sortOrder { get; set; }

        public string viewType { get; set; }

    }

    public class Shared
    {
        public string scope { get; set; }

    }
}
