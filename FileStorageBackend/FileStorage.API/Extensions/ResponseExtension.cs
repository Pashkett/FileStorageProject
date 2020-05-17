using System.Text.Json;
using Microsoft.AspNetCore.Http;
using FileStorage.Domain.PagingHelpers;

namespace FileStorage.API.Extensions
{
    public static class ResponseExtension
    {
        public static void AddPagination(this HttpResponse response, PaginationHeader paginationHeader)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, serializeOptions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}


