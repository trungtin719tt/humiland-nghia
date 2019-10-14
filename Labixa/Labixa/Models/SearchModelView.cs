using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Models
{
    public class SearchModelView
    {
        public string keyword { get; set; }
        public string place { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
        public string maxArea { get; set; }
        public string minArea { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }

    }
}