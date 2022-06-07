using System;
using System.Linq;
using HtmlAgilityPack;

namespace WebScrapingWorker.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static string Title(this HtmlNode htmlReviewTitleNode)
        {
            try
            {
                return htmlReviewTitleNode == null
                    ? string.Empty
                    : htmlReviewTitleNode.InnerText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string Review(this HtmlNode htmlReviewNode)
        {
            try
            {
                return htmlReviewNode == null
                    ? string.Empty
                    : htmlReviewNode
                        .InnerText
                        .Trim()
                        .Replace(@"\n","");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string Card(this HtmlNode htmlReviewCardNode)
        {
            try
            {
                return htmlReviewCardNode
                    .Id
                    .Split("-")
                    .ToList()
                    .Last();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static double? Star(this HtmlNode htmlReviewStarNode)
        {
            try
            {
                return htmlReviewStarNode == null
                    ? 0
                    : htmlReviewStarNode
                        .InnerText
                        .ExtractStars();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Country(this HtmlNode htmlReviewCountryNode)
        {
            try
            {
                return htmlReviewCountryNode == null
                    ? string.Empty
                    : htmlReviewCountryNode
                        .InnerText
                        .ExtractCountry();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static DateTime? Date(this HtmlNode htmlReviewDateNode)
        {
            try
            {
                return htmlReviewDateNode?
                    .InnerText
                    .ExtractDateTime();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsVerified(this HtmlNode htmlReviewVerifiedNode)
        {
            try
            {
                return htmlReviewVerifiedNode != null && htmlReviewVerifiedNode
                    .InnerText
                    .CheckVerified();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static long Validations(this HtmlNode htmlReviewValidationsNode)
        {
            try
            {
                return htmlReviewValidationsNode == null 
                    ? 0
                    : htmlReviewValidationsNode
                        .InnerText
                        .ExtractReviewValidations();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string Profile(this HtmlNode htmlReviewProfileName)
        {
            try
            {
                return htmlReviewProfileName == null
                    ? string.Empty
                    : htmlReviewProfileName
                        .InnerText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}