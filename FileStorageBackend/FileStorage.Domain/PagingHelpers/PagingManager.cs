using FileStorage.Domain.RequestModels;
using System;

namespace FileStorage.Domain.PagingHelpers
{
    /// <summary>
    /// Utility class for construction pagination header.
    /// </summary>
    public static class PagingManager
    {
        public static PaginationHeader PrepareHeader<T>(int totalCount, T parameters)
                where T : RequestParameters, new()
        {
            if (parameters == null)
                parameters = new T();

            var totalPages = 
                (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            return new PaginationHeader(
                parameters.PageNumber,
                parameters.PageSize,
                totalCount,
                totalPages);
        }
    }
}