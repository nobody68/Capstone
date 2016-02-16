using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ReviewsApp.AWSECommerceService;
using ReviewsApp.Models;

namespace ReviewsApp.Code
{
    public class ReviewSearch
    {
        [Required]
        public Item Item { get; set; }

        public string ReturnUrl { get; set; }
    }
}