using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ReviewsApp.Models;

namespace ReviewsApp.Code
{
    public class AmazonProduct
    {
        [Required]
        public string Asin { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
    }
}