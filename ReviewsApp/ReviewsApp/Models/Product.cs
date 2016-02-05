using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Product
    {
        public enum ProductType
        {
            Movie, 
            Game,
            Music
        };

        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string Rating { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}