using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Models.EF
{
    [Table("PRODUCT")]
    public class Product : CommonDT
    {
        public Product()
        {
            this.ProductImage = new HashSet<ProductImage>();
            this.OrderDetail = new HashSet<OrderDetail>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        public string Alias { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        public int OriginalPrice { get; set; }
        public int Price { get; set; }
        public int? PriceSale { get; set; }
        public int Quantity { get; set; }
        public int ViewCount { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsHot { get; set; }
        public bool IsFeature { get; set; }
        public bool IsActive { get; set; }
        public int ProductCategoryId { get; set; }
        [StringLength(250)]
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }


    }
}