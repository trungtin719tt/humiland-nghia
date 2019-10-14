using Outsourcing.Core.Common;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class BlogHomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IWebsiteAttributeService _websiteAttributeService;
        private readonly IStaffService _staffService;
        private readonly IProductAttributeMappingService _productAttributeMappingService;



        public BlogHomeController(IProductService productService, IBlogService blogService,
            IWebsiteAttributeService websiteAttributeService, IBlogCategoryService blogCategoryService,
            IStaffService staffService, IProductAttributeMappingService productAttributeMappingService, IVendorService _vendorService
           )
        {
            this._productService = productService;
            this._blogService = blogService;
            this._websiteAttributeService = websiteAttributeService;
            this._blogCategoryService = blogCategoryService;
            this._vendorService = _vendorService;
            this._staffService = staffService;
            this._productAttributeMappingService = productAttributeMappingService;

        }
        //
        // GET: /Blog/
        //[OutputCache(Duration = 300, VaryByParam = "page")]
        public ActionResult Index(int? page = 1)
        {
            int pageSize = 6;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Blog> blogs = null;
            blogs = _blogService.GetBlogsByCategory(3).ToPagedList(pageIndex, pageSize);
            return View(blogs);
        }
        //[OutputCache(Duration = 300, VaryByParam = "page")]
        public ActionResult IndexWork(int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Blog> blogs = null;
            blogs = _blogService.GetBlogsByCategory(4).ToPagedList(pageIndex, pageSize);
            return View(blogs);
        }
        //[OutputCache(Duration = 300, VaryByParam = "slug")]
        public ActionResult Detail(string slug="") {
            if (slug!="")
            {
            var item = _blogService.GetBlogByUrlName(slug);
            return View(item);
            }
            else
            {
                return RedirectToAction("Index","BlogHome");
            }
        }
        public ActionResult VendorDetail(int id=0, int confirm = 0 )
        {
            if (confirm==1)
            {
                ViewBag.ok = "<h1 style='color:red'>Bạn Đã Đăng Ký Thành Công</h1>";
            }
            if (id!=0)
            {
                var item = _vendorService.GetVendorById(id);
                return View(item);
            }
            else
            {
                return RedirectToAction("Index", "BlogHome");
            }
        }
       //[OutputCache(Duration = 300, VaryByParam = "page")]
        public ActionResult Vendor(int? page=1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Vendor> blogs = null;
            blogs = _vendorService.GetVendors().ToPagedList(pageIndex, pageSize);
            return View(blogs);
        }
        public ActionResult popularPost()
        {
            var list = _blogService.GetBlogs().OrderByDescending(p => p.DateCreated).Take(4);
            return PartialView("_popularpost",list);
        }
        //bất động sản mới
        public ActionResult newPost()
        {
            var list = _blogService.GetBlogs().OrderByDescending(p => p.DateCreated).Take(2);
            return PartialView("_newpost", list);
        }
    }
}