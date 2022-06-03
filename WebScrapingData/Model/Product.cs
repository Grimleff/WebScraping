using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebScrapingData.Model
{
    public class Product
    {
        [KeyAttribute]
        [Column("id_product"),]
        public string IdProduct{get;set;}
        [Column("product_name")]
        public string ProductName{get;set;}
    }
}