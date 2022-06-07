using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebScrapingData.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_product")]
        [JsonProperty("id_product")]
        public int IdProduct {get;set;}
        
        [Required]
        [Column("product_asin")]
        [JsonProperty("product_asin")]
        public string ProductAsin {get;set;}
        
        
        [Column("product_name")]
        [JsonProperty("product_name")]
        public string ProductName{get;set;}
        
        [Column("last_scraping")]
        [JsonProperty("last_scraping")]
        public DateTime LastScraping{get;set;}
        
        [Column("enable")]
        [JsonProperty("enable")]
        public bool Enable{get;set;}
        
        [JsonIgnore]
        public List<Review> Reviews { get; set; }
    }
}