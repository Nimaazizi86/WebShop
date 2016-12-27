using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopV1.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int totalCost { get; set; }

        public int totalCount { get; set; }

        public string customerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime orderDate { get; set; }

        public virtual List<Product> productList { get; set; }
        //public int productId { get; set; }

        public Order()
        {
            productList = new List<Product>();
        }
    }
}