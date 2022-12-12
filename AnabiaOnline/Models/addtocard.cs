using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnabiaOnline.Models
{
    public class addtocard
    {
      
            [System.ComponentModel.DataAnnotations.Key]
            public int Id { get; set; }
            public int Pro_Id { get; set; }
            public string Pro_Name { get; set; }
            public decimal Pro_Price { get; set; }

            public decimal Ord_Bill { get; set; }
            public int Ord_Quantity { get; set; }
            public decimal DeliveryCharges { get; set; }


        
    }
}