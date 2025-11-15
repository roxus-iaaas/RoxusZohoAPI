namespace RoxusZohoAPI.Models.SharePoint
{
    public class SearchFoldersByNameRequest
    {

        public string FolderName { get; set; }

        public string FolderPath { get; set; }

        public string FolderId { get; set; }

        public string CustomerName { get; set; }

        public bool ExactMatch { get; set; }

        public bool FirstLevel { get; set; }

    }
}
