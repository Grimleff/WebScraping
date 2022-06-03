using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace WebScrapingData.Model
{
    public class Review
    {
        [Key]
        [Column("id_review")]
        public long IdReview{get;set;}
        [Column("comment")]
        public string Comment {get;set;}
        [Column("stars")]
        public int Stars {get;set;}
        [Column("review_date")]
        public DateTime ReviewDate {get;set;}
    }
}