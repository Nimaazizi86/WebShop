﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopV1.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        
        public string userId { get; set; }

        [Required]
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string adress { get; set; }

        public ShoppingCart cartDetail { get; set; }

        public virtual List<Order> orderHistory { get; set; }

        public Customer()
        {
            orderHistory = new List<Order>();
        }
    }

}