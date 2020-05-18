﻿namespace FileStorage.Domain.RequestModels
{
    public abstract class RequestParameters
    {
        public const int MaxPageSize = 50;
        private int pageSize = 5;
        
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}


