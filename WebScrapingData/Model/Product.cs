using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebScrapingData.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_product"),]
        public int IdProduct {get;set;}
        
        [Required]
        [Column("product_asin"),]
        public string ProductAsin {get;set;}
        
        
        [Column("product_name")]
        public string ProductName{get;set;}
    }
}