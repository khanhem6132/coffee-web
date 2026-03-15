using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeWeb.Models.EF
{
    [Table("PRODUCTIMAGE")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [Required]
        public string Image { get; set; }
        public bool IsDefault { get; set; }

        public virtual Product Product { get; set; }
    }
}