using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ReviewsApp.Models;

namespace ReviewsApp.Code
{
    public class AmazonSearch
    {
        [Required]
        public string SearchText { get; set; }

        public Product.ProductType ProductType { get; set; }
    }
}