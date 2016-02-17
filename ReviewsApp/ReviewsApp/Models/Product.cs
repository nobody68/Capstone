using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Product
    {
        public enum ProductType
        {
            Books,
            DVD,
            Music,
            Apparel,
            Video,
            Jewlery,
            Automotive,
            Watch,
            Electronics
        };

        //[Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Key]
        public string ASIN { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string Rating { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}