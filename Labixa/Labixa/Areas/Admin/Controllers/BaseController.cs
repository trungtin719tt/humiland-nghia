﻿using Labixa.Helpers;
using Microsoft.AspNet.Identity;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //string cultureName = null;
            //cultureName = "vi";
            //// Validate culture name
            //HttpCookie cultureCookie = Request.Cookies["_culture"];
            //if (cultureCookie != null)
            //{                //cultureName = cultureCookie.Value;
            //    cultureName = "vi";
            //}
            ////else
            ////    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
            ////            Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
            ////            null;
            //// Validate culture name
            //cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            //cultureName = "vi";
            //// Modify current thread's cultures            
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("vi");
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
        public User GetInfo()
        {
            var manager = new UserManager<User>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(new Outsourcing.Data.OutsourcingEntities()));
            var user = manager.FindByName(User.Identity.Name);
            return user;
        }
    }
}