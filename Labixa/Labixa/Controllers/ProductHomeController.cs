using Labixa.Models;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Outsourcing.Core.Extensions;
namespace Labixa.Controllers
{
    public class ProductHomeController : BaseHomeController
    {  private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IWebsiteAttributeService _websiteAttributeService;
        private readonly IStaffService _staffService;
        private readonly IOrderService _orderService;
        private readonly ITypeNotifyService _typeNotifyService;
        private readonly IProductAttributeMappingService _productAttributeMappingService;
        private readonly IProductCategoryMappingService _productCategoryMappingService;
        private readonly IProductCategoryService _productCategory;


        public ProductHomeController(IProductService productService, IBlogService blogService,
            IWebsiteAttributeService websiteAttributeService, IBlogCategoryService blogCategoryService,
            IStaffService staffService, IProductAttributeMappingService productAttributeMappingService, IVendorService _vendorService
           , IOrderService _orderService, ITypeNotifyService _typeNotifyService,
            IProductCategoryMappingService _productCategoryMappingService, IProductCategoryService _productCategory)
        {
            this._productService = productService;
            this._blogService = blogService;
            this._websiteAttributeService = websiteAttributeService;
            this._blogCategoryService = blogCategoryService;
            this._vendorService = _vendorService;
            this._staffService = staffService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._orderService = _orderService;
            this._typeNotifyService = _typeNotifyService;
            this._productCategoryMappingService = _productCategoryMappingService;
            this._productCategory = _productCategory;
        }

        
        public ActionResult SanPham(string slug)
        {

            var item = _productAttributeMappingService.GetProductAttributeMappings();
            ProductDetailsModel model = new ProductDetailsModel();
            model.product = _productService.GetProductBySlug(slug);
            double productPrice = model.product.OrginalPrice;
            try
            {
                if (model.product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 24).FirstOrDefault().Value.Equals("true"))
                {
                    model.listRelated = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals("true") && p.ProductAttributeId == 24).Select(p => p.Product).Where(e => (e.OrginalPrice >= productPrice - 1000000000) && (e.OrginalPrice <= productPrice + 1000000000)).Take(6).ToList();
                }
                else
                {
                    model.listRelated = _productAttributeMappingService.GetProductAttributeMappings().Where(p => !p.Value.Equals("true") && p.ProductAttributeId == 24).Select(p => p.Product).Where(e => (e.OrginalPrice >= productPrice - 2000000) && (e.OrginalPrice <= productPrice + 2000000)).Take(6).ToList();
                }
            }
            catch
            {
                model.listRelated = null;
            }
            
            
            //if (model.li)
            //{

            //}
            var moigioi = GetInfo(model.product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 19).FirstOrDefault().Value);
            model.user = moigioi;
            model.products = _productService.GetAllProducts().ToList();
            return View("SanPham", model);
        }
        public ActionResult Search()
        {
            SearchModelView search = new SearchModelView();
            SelectListItem list = new SelectListItem();
            list.Value = "Địa điểm";
            list.Text = "Địa điểm";
            List<SelectListItem> listCate = new List<SelectListItem>();
            listCate.Add(list);
            listCate.AddRange(_productCategory.GetProductCategories().ToSelectListItems(-1, 1));
            ViewBag.categoryList = listCate;
            return PartialView("Search", search);
        }

        public ActionResult ChoBanQuan(string slug, int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.slug = slug;

            ViewBag.category = _productCategory.GetProductCategories();

           
            //ProductViewModel model = new ProductViewModel();

            int categoryId = _productCategory.GetProductCategories().Where(q => q.Slug.Equals(slug)).Select(k => k.Id).FirstOrDefault();
            var model = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals("true") && p.ProductAttributeId == 24 && p.Product.OldPrice == categoryId && p.Product.IsPublic).Select(p => p.Product).OrderByDescending(p => p.Id).ToPagedList(pageIndex, pageSize);
            //model.products = _productCategoryMappingService.GetProductCategoryMappings().Where(p => p.ProductCategoryId == categoryId).ToPagedList(pageIndex, pageSize);

            return View("ChoBanQuan", model);
        }
        
        public ActionResult ChoThueQuan(string slug, int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.slug = slug;
            //ProductViewModel model = new ProductViewModel();
            int categoryId = _productCategory.GetProductCategories().Where(q => q.Slug.Equals(slug)).Select(k => k.Id).FirstOrDefault();
            var model = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals("false") && p.ProductAttributeId == 24 && p.Product.OldPrice == categoryId && p.Product.IsPublic).Select(p => p.Product).OrderByDescending(p => p.Id).ToPagedList(pageIndex, pageSize);
            return View("ChoThueQuan", model);
        }
        
        public ActionResult popularProduct()
        {
            //var list = _blogService.GetBlogs().OrderByDescending(p => p.DateCreated).Take(4);
            
            var list = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.ProductAttributeId == 28).OrderByDescending(d => d.Value).Take(3);
            
            return PartialView("_popularProduct", list);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContactNow( string UserName,string Id, string name, string phone, string email, string message, string slug)
        {
            if (ModelState.IsValid)
            {
                Vendor vendor = new Vendor();
                vendor.Content = UserName;
                vendor.Note = Id;
                vendor.Name = name;
                vendor.Phone = phone;
                vendor.Address = email;
                vendor.Description = message;
                vendor.Type = "1";
                //Mapping to domain
                //Create Blog
                _vendorService.CreateVendor(vendor);
                Console.WriteLine("ok");
                return Redirect("/san-pham/" + slug);
                //chua thong bao gui tin nhan thanh cong
                
            }
            else
            {
                //return RedirectToAction("SanPham", "ProductHome");
                return Redirect("/san-pham/" + slug);
            }
        }
	}
}