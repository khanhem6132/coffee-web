using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static CoffeeWeb.Models.EF.Enum;

namespace CoffeeWeb.Models.EF
{
    [Table("ORDERS")]
    public class Order : CommonDT
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "Customer name must not be left blank.")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không để trống")]
        public string Address { get; set; }
        public string Email { get; set; }
        public long TotalAmount { get; set; }
        
        public int TypePayment { get; set; }

        [Required][ StringLength(128)]
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}