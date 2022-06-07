using System;
using Newtonsoft.Json;

namespace WebScrapingAPI.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public PagedResponse()
        {
        }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }

        [JsonProperty("page_number")] public int PageNumber { get; set; }

        [JsonProperty("page_size")] public int PageSize { get; set; }

        [JsonProperty("first_page")] public Uri FirstPage { get; set; }

        [JsonProperty("last_page")] public Uri LastPage { get; set; }

        [JsonProperty("total_pages")] public int TotalPages { get; set; }

        [JsonProperty("total_records")] public int TotalRecords { get; set; }

        [JsonProperty("next_page")] public Uri NextPage { get; set; }

        [JsonProperty("previous_page")] public Uri PreviousPage { get; set; }
    }
}