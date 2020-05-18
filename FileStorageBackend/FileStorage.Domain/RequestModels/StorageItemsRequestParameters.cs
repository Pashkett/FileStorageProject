namespace FileStorage.Domain.RequestModels
{
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

        public string OrderBy { get; set; }
    }
}


