using System;
using System.Collections.Generic;
using System.Linq;

namespace FileStorage.Domain.PagingHelpers
{
    public static class PagingManager<T>
    {
        public static (IEnumerable<T> resultedCollection, PaginationHeader paginationHeader) 
            ProcessPaging(IEnumerable<T> source, int currentPage, int pageSize)
        {
            var resultedCollection = source
                                        .Skip((currentPage - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            var totalCount = source.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginationHeaders =
                new PaginationHeader(currentPage, pageSize, totalCount, totalPages);

            return (resultedCollection, paginationHeaders);
        }
    }
}