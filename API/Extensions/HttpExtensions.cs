using System.Text.Json;
using API.Helpers;
 
namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void addPaginationHeaders(this HttpResponse response, int currentPage, 
        int totalPages, int totalItems, int ItemsPerPage )
        {
            var paginationHeader = new PaginationHeaders(currentPage,totalPages,ItemsPerPage,totalItems);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
           response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));
           response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}