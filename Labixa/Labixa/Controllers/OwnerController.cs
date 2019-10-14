using Labixa.Models;
using Microsoft.AspNet.Identity;
using Outsourcing.Core.Common;
using Outsourcing.Core.Extensions;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Labixa.Controllers
{
    [Authorize]
    public class OwnerController : BaseHomeController
    {
        private UserManager<User> _userManager;
        private IUserRoleStore<User> _userRoleManager;
        private readonly IProductService _productService;
        private readonly IProductAttributeMappingService _productAttributeMappingService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductAttributeService _productAttributeService;
        readonly IProductPictureMappingService _productPictureMappingService;
        readonly IPictureService _pictureService;
        readonly IProductCategoryMappingService _productCategoryMappingService;
        readonly IColorService _colorService;

        public OwnerController(UserManager<User> userManager, IProductService _productService,
            IProductAttributeMappingService _productAttributeMappingService,
         IProductAttributeService _productAttributeService,
            IProductCategoryService _productCategoryService,
            IProductPictureMappingService _productPictureMappingService,
            IPictureService _pictureService, IProductCategoryMappingService _productCategoryMappingService,
            IColorService _colorService)
        {
            _userManager = userManager;
            this._productAttributeMappingService = _productAttributeMappingService;
            this._productCategoryService = _productCategoryService;
            this._productService = _productService;
            this._productAttributeService = _productAttributeService;
            this._productPictureMappingService = _productPictureMappingService;
            this._pictureService = _pictureService;
            this._productCategoryMappingService = _productCategoryMappingService;
            this._colorService = _colorService;
        }
        //
        // GET: /Owner/
        public ActionResult Index(int? page = 1)
        {
            if (ViewBag.noti!=null)
            {
                ViewBag.noti = "Bạn Vui Lòng Tạo Chủ sở hữu trước khi tạo Bất Động Sản";
            }
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var user = GetInfo();
            var listHolder = _colorService.GetColors().Where(p => p.StaffUserName.Equals(user.UserName)).ToPagedList(pageIndex, pageSize);
            if (User.IsInRole("SuperAdmin"))
            {
                 listHolder = _colorService.GetColors().ToPagedList(pageIndex, pageSize);
            }
            return View(listHolder);
        }
        public ActionResult CreateHolder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateHolder(Color obj)
        {
            var user = GetInfo();
            obj.StaffUserName = user.UserName;
            obj.isDelete = false;
            _colorService.CreateColor(obj);
            return RedirectToAction("Index");
        }
        public ActionResult EditHolder(int Id)
        {
            var obj = _colorService.GetColorById(Id);
            if (obj!=null)
            {
            return View(obj);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult EditHolder(Color obj)
        {
            _colorService.EditColor(obj);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult DeleteHolder(int Id)
        {
            var obj = _colorService.GetColorById(Id);
            obj.isDelete = true;
            _colorService.EditColor(obj);
            return RedirectToAction("Index");
        }

        public ActionResult ListProperty(int Id,int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var listProduct = _productService.GetProducts().Where(p => p.ColorId == Id).ToPagedList(pageIndex, pageSize);
            return View(listProduct);
        }

    }
}