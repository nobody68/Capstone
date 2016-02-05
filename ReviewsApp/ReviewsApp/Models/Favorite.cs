using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReviewsApp.Models;

namespace ReviewsApp.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}