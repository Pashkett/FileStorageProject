namespace FileStorage.Domain.RequestModels
{
    /// <summary>
    /// Class for specification additional parameters for request for StorageItems.
    /// Provides specification for size, ordering and search.
    /// </summary>
    public class StorageItemsRequestParameters : RequestParameters
    {
        private const long MinLength = 0;
        private const long MaxLength = 500 * 1024 * 1024;

        private long minSize = 0;
        private long maxSize = 500 * 1024 * 1024;

        public bool IsValidSizeRange => MaxSize > MinSize;
        public long MinSize 
        { 
            get => minSize; 
            set => minSize = (value < MinLength) ? MinLength : value; 
        }

        public long MaxSize
        {
            get => maxSize;
            set => maxSize = (value > MaxLength) ? MaxLength : value;
        }

        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }
    }
}


