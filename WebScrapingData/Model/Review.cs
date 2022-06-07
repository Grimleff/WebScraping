using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebScrapingData.Model
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_review")]
        [JsonProperty("id_review")]
        public long IdReview { get; set; }

        [Column("review_card")]
        [JsonProperty("review_card")]
        public string Card { get; set; }

        [Column("review_title")]
        [JsonProperty("review_title")]
        public string ReviewTitle { get; set; }

        [Column("review_comment")]
        [JsonProperty("review_comment")]
        public string ReviewComment { get; set; }

        [Column("review_stars")]
        [JsonProperty("review_stars")]
        public double? ReviewStars { get; set; }

        [Column("review_country")]
        [JsonProperty("review_country")]
        public string ReviewCountry { get; set; }

        [Column("review_date")]
        [JsonProperty("review_date")]
        public DateTime? ReviewDate { get; set; }

        [Column("review_verified")]
        [JsonProperty("review_verified")]
        public bool ReviewVerified { get; set; }

        [Column("review_validation")]
        [JsonProperty("review_validation")]
        public long? ReviewValidation { get; set; }

        [Column("review_profile")]
        [JsonProperty("review_profile")]
        public string ReviewProfile { get; set; }

        [Column("product_id")] public int ProductId { get; set; }

        [JsonIgnore] public Product Product { get; set; }
    }
}