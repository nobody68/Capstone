using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public string Link { get; set; }

        public virtual Product Product { get; set; }
    }
}