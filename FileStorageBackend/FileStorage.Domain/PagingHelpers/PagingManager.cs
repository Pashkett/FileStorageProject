using FileStorage.Domain.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileStorage.Domain.PagingHelpers
{
    public static class PagingManager
    {
        public static PaginationHeader PrepareHeader(int totalCount,
            StorageItemsRequestParameters requestParameters)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)requestParameters.PageSize);

            return new PaginationHeader(
                requestParameters.PageNumber,
                requestParameters.PageSize,
                totalCount,
                totalPages);
        }
    }
}