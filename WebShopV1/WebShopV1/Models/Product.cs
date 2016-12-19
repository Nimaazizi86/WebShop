using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopV1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        public decimal cost { get; set; }

        public string description { get; set; }

        public int quantity { get; set; } 

    }
}