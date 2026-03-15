using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CoffeeWeb.Models.EF.Enum;

namespace CoffeeWeb.Models
{
    public class OrderViewModel
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string Email { get; set; }
        public long TotalAmount { get; set; }
        public int TypePayment { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }

    }
}