using System;
using Microsoft.VisualBasic.CompilerServices;

namespace WebScrapingWorker.Extensions
{
    public static class StringExtensions
    {
        public static double? ExtractStars(this string webReviewStars)
        {
            try
            {
                return double.Parse(webReviewStars.Split(" ")[0]);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DateTime? ExtractDateTime(this string webReviewDate)
        {
            try
            {
                return DateTime.Parse(webReviewDate.Split("on")[1]);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string ExtractCountry(this string webReviewCountry)
        {
            try
            {
                return webReviewCountry.Split("on")[0].Trim().Replace("Reviewed in the", "");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static bool CheckVerified(this string webReviewVerified)
        {
            return webReviewVerified.Contains("Verified Purchase");
        }
        public static int ExtractReviewValidations(this string webReviewValidation)
        {
            try
            {
                return int.Parse(webReviewValidation.Split(" ")[0]);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        
    }
}