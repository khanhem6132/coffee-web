using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Models.EF
{
    [Table("NEWS")]
    public class News : CommonDT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Không để trống tiêu đề tin")]
        [StringLength(250)]
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public virtual Category Category { get; set; }
    }
}