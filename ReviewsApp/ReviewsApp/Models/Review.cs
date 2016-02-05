using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductReview { get; set; }
        public string Link { get; set; }

        public virtual Product Product { get; set; }
    }
}