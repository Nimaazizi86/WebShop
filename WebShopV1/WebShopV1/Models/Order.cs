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

        public int customerId { get; set; }
        public virtual Customer customer { get; set; }

        [DataType(DataType.Date)]
        public DateTime orderDate { get; set; }

        public List<Product> productList { get; set; }
    }
}