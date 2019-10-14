using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Models
{
    public class ProductDetailsModel
    {
        public ProductDetailsModel()
        {
            listRelated = new List<Product>();
        }
        public Product product { get; set; }
        public List<Product> listRelated { get; set; }
        public List<Product> products { get; set; }
        public User user = new User();
    }
}