using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace WebScrapingData.Model
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_review")]
        public long IdReview{get;set;}
        
        [Column("review_card")]
        public string Card {get;set;}
        
        [Column("review_title")]
        public string ReviewTitle {get;set;}
        
        [Column("review_comment")]
        public string ReviewComment {get;set;}
        
        [Column("review_stars")]
        public double? ReviewStars {get;set;}
        
        [Column("review_country")]
        public string ReviewCountry {get;set;}
        
        [Column("review_date")]
        public DateTime? ReviewDate {get;set;}
        
        [Column("review_verified")]
        public bool ReviewVerified {get;set;}
        
        [Column("review_validation")]
        public long? ReviewValidation {get;set;}
        
        [Column("review_profile")]
        public string ReviewProfile {get;set;}
    }
}