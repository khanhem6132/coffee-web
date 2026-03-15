using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeWeb.Models.EF
{
    [Table("CATEGORY")]
public class Category : CommonDT
{
    [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

    [Required]
    [StringLength(150)]
    
    public string Title { get; set; }
    public string Alias { get; set; } 

    public ICollection<News> News { get; set; }
    public ICollection<Aboutus> About_us { get; set; }
}

}