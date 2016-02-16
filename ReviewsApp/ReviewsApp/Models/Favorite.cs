using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ReviewsApp.Models;

namespace ReviewsApp.Models
{
    public class Favorite
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ASIN { get; set; }
        [ForeignKey("ProductId, ASIN")]
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}