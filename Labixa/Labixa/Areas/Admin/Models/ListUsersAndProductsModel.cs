using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Labixa.Areas.Admin.Models
{
    public class ListUsersAndProductsModel
    {
        public ListUsersAndProductsModel()
        {
            listProducts = new List<Product>();
        }

        public List<Product> listProducts { get; set; }
        public List<User> listUser { get; set; }
    }
}