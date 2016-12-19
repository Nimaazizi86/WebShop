using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopV1.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int cartNumber { get; set; }

        public string ownerName { get; set; }
          
        public int expiredYear { get; set; }

        public int expiredMonth { get; set; }

        public int securityCode { get; set; }


    }
}