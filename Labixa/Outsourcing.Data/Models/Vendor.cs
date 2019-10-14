using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Vendor :BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public int Percent { get; set; }
        public bool IsDelete { get; set; }
        //[DisplayName(@"Đường dẫn")]
        public string Slug { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaTitle { get; set; }

        //[DisplayName(@"Thẻ meta Mô tả")]
        public string MetaDescription { get; set; }

        //public ICollection<Product> Products { get; set; }
    }
}
