using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Models
{
    public class MenuModelsView
    {
        public MenuModelsView()
        {
            listCategory = new List<ProductCategory>();
            listProduct = new List<ProductAttributeMapping>();
        }
        public List<ProductCategory>listCategory { get; set; } 
        public List<ProductAttributeMapping> listProduct { get; set; }
    }
}