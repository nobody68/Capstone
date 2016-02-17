using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Price
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public String ASIN { get; set; }
        public decimal ProductPrice { get; set; }
        public string Link { get; set; }

        [ForeignKey("ASIN")]
        public virtual Product Product { get; set; }
    }
}