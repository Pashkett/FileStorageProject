namespace FileStorage.Domain.RequestModels
{
    /// <summary>
    /// Base class for request from customer that provides paging functionality.
    /// </summary>
    public abstract class RequestParameters
    {
        public const int MaxPageSize = 50;
        public const int MinPageSize = 1;

        private int pageSize = 5;
        
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value > MaxPageSize)
                {
                    pageSize = MaxPageSize;
                }
                else if (value <= 0)
                {
                    pageSize = MinPageSize;
                }
                else
                {
                    pageSize = value;
                }
            }
        }
    }
}


