using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeWeb.Models.EF
{
    [Table("PRODUCTCATEGORY")]
    public class ProductCategory : CommonDT
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [Required]
        [StringLength(150)]
        public string Alias { get; set; }
        public string Description { get; set; }
        [StringLength(250)]
        public string Icon { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}