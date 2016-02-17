using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Review
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 2)]
        public string Source { get; set; }
        public int ProductId { get; set; }
        public string ASIN { get; set; }
        public string ProductReview { get; set; }
        public string Link { get; set; }

        [ForeignKey("ASIN")]
        public virtual Product Product { get; set; }
    }
}