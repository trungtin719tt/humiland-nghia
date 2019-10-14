using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using Outsourcing.Core.Common;
using Outsourcing.Core.Extensions;
using WebGrease.Css.Extensions;
using Outsourcing.Core.Framework.Controllers;
using Microsoft.AspNet.Identity;
using Labixa.Areas.Admin.Models;

namespace Labixa.Areas.Admin.Controllers
{
    public class StaffController : BaseController
    {
        private UserManager<User> _userManager;
        private IUserRoleStore<User> _userRoleManager;
        #region Field
        readonly IStaffService _staffService;
        readonly IProductService _productService;
        #endregion

        #region Ctor
        public StaffController(IStaffService staffService, UserManager<User> userManager, IProductService _productService)
        {
            _staffService = staffService;
            _userManager = userManager;
            this._productService = _productService;
        }
        #endregion
        //
        // GET: /Admin/Staff/
        public ActionResult Index()
        {
            //var list = _userManager.Users.ToList();
            var model = new ListUsersAndProductsModel();
            if (User.IsInRole("SuperAdmin"))
            {
            model.listUser = _userManager.Users.Where(u => u.Deleted == false && !u.UserName.Equals("longt") && !u.UserName.Equals("admin") && !u.UserName.Equals("editor")).ToList();
            }
            else
            {
                model.listUser = _userManager.Users.Where(u => u.Deleted == false && !u.UserName.Equals("longt") && !u.UserName.Equals("admin") && !u.UserName.Equals("editor") && u.RoleId== SystemRoles.Guest).ToList();
            }

            model.listProducts = _productService.GetProducts().ToList();

            return View(model);
        }
        public ActionResult Create()
        {
            var model = new StaffFormModel();
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(StaffFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                obj.Deleted = false;
                if (obj.Name != null)
                {
                    obj.Rename = StringConvert.ConvertShortName(obj.Name);
                }
                Staff item = Mapper.Map<StaffFormModel, Staff>(obj);
                _staffService.CreateStaff(item);
                return continueEditing ? RedirectToAction("Edit", "Staff", new { id = item.Id })
                                 : RedirectToAction("Index", "Staff");
            }
            else return View("Create", obj);
        }
        public ActionResult Edit(string id = "", string username = "")
        {
            if (username != "" && username !="longt")
            {
                var item = _userManager.Users.ToList().Where(p => p.UserName.Equals(username)).FirstOrDefault();
                if (item != null)
                {
                    return View(item);
                }
            }
            else
            {
                var item = _userManager.Users.ToList().Where(p => p.Id.Equals(id)).FirstOrDefault();
                if (item != null)
                {
                    return View(item);
                }

            }
            return RedirectToAction("Index", "Staff");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<ActionResult> Edit(FormCollection collection)
        {
            bool role = false;
            bool activated = false;
            if (!string.IsNullOrEmpty(collection["roles"]))
            {
                string checkResp = collection["roles"];
                role = Convert.ToBoolean(checkResp);
            }
            if (!string.IsNullOrEmpty(collection["Activated"]))
            {
                string checkResp = collection["Activated"];
                activated = Convert.ToBoolean(checkResp);
            }
            //return RedirectToAction("Index", "Staff");
            //if (ModelState.IsValid)
            //{
            //    Staff item = Mapper.Map<StaffFormModel, Staff>(obj);
            //    if (obj.Name != null)
            //    {
            //        item.Rename = StringConvert.ConvertShortName(obj.Name);
            //    }
            //    _staffService.EditStaff(item);
            //    return continueEditing ? RedirectToAction("Edit", "Staff", new { id = item.Id })
            //        : RedirectToAction("Index", "Staff");
            //}
            //else
            //return View("Edit", obj);
            //var user = _userManager.Users.ToList().Where(p => p.Id.Equals(obj.Id)).FirstOrDefault();
            var manager = new UserManager<User>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(new Outsourcing.Data.OutsourcingEntities()));
            var user = await manager.FindByIdAsync(collection["Id"]);
            user.Activated = activated;
            // var role = roles.Equals("True")?true:false;
            if (role)
            {
                //await _userRoleManager.AddToRoleAsync(user, "Admin");
                    _userManager.RemoveFromRole(user.Id, "Admin");
                _userManager.RemoveFromRole(user.Id, "Guest");
                _userManager.AddToRole(user.Id, "Admin");
                user.RoleId = SystemRoles.Admin;
            }
            else
            {
                //await _userRoleManager.AddToRoleAsync(user, "Guest");
                 _userManager.RemoveFromRole(user.Id, "Admin");
                 _userManager.RemoveFromRole(user.Id, "Guest");
                _userManager.AddToRole(user.Id, "Guest");
                user.RoleId = SystemRoles.Guest;
            }
            IdentityResult result = await manager.UpdateAsync(user);
            return RedirectToAction("Index", "Staff");
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Delete(string id)
        {
            var manager = new UserManager<User>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(new Outsourcing.Data.OutsourcingEntities()));
            var user = await manager.FindByIdAsync(id);

            user.Deleted = true;
            IdentityResult result = await manager.UpdateAsync(user);
            return RedirectToAction("Index", "Staff");
        }
        [HttpGet]
        public ActionResult List(string UserName)
        {
            var list = _productService.GetProducts().Where(p => p.ProductAttributeMappings.Where(d => d.ProductAttributeId == 19).FirstOrDefault().Value.Equals(UserName)).ToList();
            return View(list);
        }

    }
}