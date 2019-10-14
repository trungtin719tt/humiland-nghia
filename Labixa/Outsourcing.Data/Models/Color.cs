using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Phone_1 { get; set; }
        public string Phone_2 { get; set; }
        public string Phone_3 { get; set; }
        public string Email { get; set; }
        public string Temp_1 { get; set; }
        public string Temp_2 { get; set; }
        public string Temp_3 { get; set; }
        public string Temp_4 { get; set; }
        public string Temp_5 { get; set; }
        public string Note { get; set; }
        public string StaffUserName { get; set; }
        public bool isDelete { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
