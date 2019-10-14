using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Models
{
    public class submitPropertyModel
    {
        public string Lat { get; set; }
        public string Long { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> listCategory { get; set; }
        public IEnumerable<SelectListItem> ListColor { get; set; }

    }
}