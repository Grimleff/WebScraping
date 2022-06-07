using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebScrapingAPI.Extensions;
using WebScrapingAPI.Model;
using WebScrapingAPI.Wrappers;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;

namespace WebScrapingAPI.Controllers
{
    public class ScrapingController : ControllerBase
    {
        private readonly ILogger<ScrapingController> _logger;
        private readonly IScrapingRepository _scrapingRepository;

        public ScrapingController(IScrapingRepository scrapingRepository, ILogger<ScrapingController> logger)
        {
            _scrapingRepository = scrapingRepository;
            _logger = logger;
        }

        /// <summary>
        ///     Get all customer review for one product ASIN
        /// </summary>
        /// <param name="asin">ASIN product id (Amazon ref)</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        [HttpGet]
        [Route(@"reviews_from_product", Name = "reviews_from_product")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewFromAsin([FromQuery(Name = "asin")] string asin,
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "page_size")] int? pageSize)
        {
            try
            {
                var urlBase = $"{HttpContext.Request.GetEncodedUrl().Split("?").First()}?asin={asin}";
                var dbReviews = await _scrapingRepository.GetReviewsFromAsinProductAsync(asin);

                var reviews = dbReviews
                    .ToList()
                    .Skip((page.LimitPage() - 1) * pageSize.LimitPageSize())
                    .Take(pageSize.LimitPageSize());

                var pages = dbReviews.Count() / (decimal) pageSize.LimitPageSize();
                var totalPages = (int) Math.Ceiling(pages);

                if (page.LimitPage() > totalPages)
                    return Ok(new PagedResponse<IEnumerable<Review>>
                    {
                        Succeeded = false,
                        TotalRecords = dbReviews.Count(),
                        Data = null,
                        PageSize = pageSize.LimitPageSize(),
                        PageNumber = page.LimitPage(),
                        TotalPages = totalPages,
                        FirstPage = new Uri($"{urlBase}&page=1&page_size={pageSize.LimitPageSize()}"),
                        LastPage = new Uri($"{urlBase}&page={totalPages}&page_size={pageSize.LimitPageSize()}"),
                        PreviousPage = page.LimitPage() - 1 > totalPages
                            ? null
                            : new Uri($"{urlBase}&page={page.LimitPage() - 1}&page_size={pageSize.LimitPageSize()}"),
                        NextPage = null,
                        Errors = new[] {"Page not exist"},
                        Message = "Please query page number between first and last"
                    });

                var response = new PagedResponse<IEnumerable<Review>>
                {
                    Succeeded = true,
                    TotalRecords = dbReviews.Count(),
                    Data = reviews,
                    PageSize = pageSize.LimitPageSize(),
                    PageNumber = page.LimitPage(),
                    TotalPages = totalPages,
                    FirstPage = new Uri($"{urlBase}&page=1&page_size={pageSize.LimitPageSize()}"),
                    LastPage = new Uri($"{urlBase}&page={totalPages}&page_size={pageSize.LimitPageSize()}"),
                    PreviousPage = page.LimitPage() - 1 == 0
                        ? null
                        : new Uri($"{urlBase}&page={page.LimitPage() - 1}&page_size={pageSize.LimitPageSize()}"),
                    NextPage = page.LimitPage() + 1 > totalPages
                        ? null
                        : new Uri($"{urlBase}&page={page.LimitPage() + 1}&page_size={pageSize.LimitPageSize()}")
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                var response = new PagedResponse<IEnumerable<Review>>
                {
                    Succeeded = false,
                    TotalRecords = 0,
                    Data = null,
                    PageSize = pageSize.LimitPageSize(),
                    PageNumber = page.LimitPage(),
                    TotalPages = 0,
                    Errors = new[] {e.Message}
                };
                return Ok(response);
            }
        }

        /// <summary>
        ///     Get reviews of a list of products
        /// </summary>
        /// <param name="asins">List of product ASIN (amazon ref)</param>
        /// <param name="since">Date limit to get customer review</param>
        /// <returns></returns>
        [HttpPost]
        [Route(@"reviews_from_products", Name = "reviews_from_products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsFromAsins([FromBody] string[] asins,
            [FromQuery(Name = "since_date")] DateTime since)
        {
            try
            {
                var dbReviews = await _scrapingRepository.GetReviewsFromAsinsProductAsync(asins, since);
                var response = new SimpleResponse<Review>
                {
                    Success = true,
                    Result = dbReviews.ToList(),
                    Count = dbReviews.Count(),
                    Error = null
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                var response = new SimpleResponse<Review>
                {
                    Success = false,
                    Result = null,
                    Count = 0,
                    Error = e.Message
                };
                return Ok(response);
            }
        }


        /// <summary>
        ///     Add new product for review trackin
        /// </summary>
        /// <param name="asins">Amazon ASIN ref</param>
        /// <returns></returns>
        [HttpPost]
        [Route(@"add_products", Name = "add_products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProductForTrackingReview([FromBody] string[] asins)
        {
            try
            {
                foreach (var asin in asins)
                    await _scrapingRepository.AddOrUpdateProduct(new Product
                    {
                        Enable = true,
                        ProductAsin = asin
                    });
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}