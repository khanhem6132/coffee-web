using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeWeb.Models.EF
{
    public class Enum
    {
        public enum OrderStatus
        {
            Wait_for_confirmation =1,
            Confirmed = 2,    // Xác nhận
            Delivering = 3,   // Đang giao
            Delivered = 4,
            Completed=5// Giao thành công
        }
    }
}