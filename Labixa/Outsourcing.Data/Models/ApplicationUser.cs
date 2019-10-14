using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            DateCreated = DateTime.Now;
        }

        public string FirstName { get; set; }
        //public string UserName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
        public string Skype { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool Activated { get; set; }

        public Gender Gender { get; set; }

        public SystemRoles RoleId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool Deleted { get; set; }
        public bool IsSubcribe{get;set;}
        public int promotionID { get; set; }

        public string DisplayName
        {
            get { return LastName + " " + FirstName; }
        }
    }
    public enum SystemRoles
    {
        [Description("Admin")]
        Admin = 1,
             [Description("SuperAdmin")]
        SuperAdmin = 2,
             [Description("Guest")]
        Guest = 3
    }
    public enum Gender
    {
        [Description("Nam")]
        Male = 0,
        [Description("Nữ")]
        Female = 1
    }
}
