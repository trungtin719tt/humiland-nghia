using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.Models
{
    
    public class VendorAndProductModel
    {
        public VendorAndProductModel()
        {
            
        }
        public Product product { get; set; }
        public Vendor vendor { get; set; }
    }

    
}