namespace FileStorage.Data.QueryModels
{
    public class StorageItemsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long MinSize { get; set; }
        public long MaxSize { get; set; }
        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }
    }
}
