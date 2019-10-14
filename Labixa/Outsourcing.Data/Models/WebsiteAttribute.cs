using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class WebsiteAttribute : BaseEntity
    {
        [DisplayName(@"Tên")]
        public string Name { get; set; }
        [DisplayName(@"Tên Sản Phẩm")]
        public string Tieu_De { get; set; }
        [DisplayName(@"Mô Tả")]
        public string Description { get; set; }
        [DisplayName(@"Giá")]
        public string Content { get; set; }
        [DisplayName(@"Link Bài Viết")]
        public string Url { get; set; }
        public string Image { get; set; }
        public string ControlType { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsPublic { get; set; }
        public bool Deleted { get; set; }

    }
}
