using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Labixa.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            NowProduct = new List<Product>();
            ThueProduct = new List<Product>();
            BanProduct = new List<Product>();
        }
        
        public List<Product> NowProduct { get; set; }
        public List<Product> ThueProduct { get; set; }
        public List<Product> BanProduct { get; set; }

        public List<ProductCategory> Category { get; set; }
    }
}