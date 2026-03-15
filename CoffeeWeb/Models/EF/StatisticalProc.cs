using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeWeb.Models.EF
{
    [Table("STATISTICAL")]
    public class StatisticalProc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime D_TIME { get; set; }
        public long PERSONVISIT { get; set; }
    }
}