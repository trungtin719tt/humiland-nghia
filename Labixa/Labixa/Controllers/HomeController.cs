using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Labixa.Models;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using PagedList;
using Labixa.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using Goheer.EXIF;

namespace Labixa.Controllers
{

    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IWebsiteAttributeService _websiteAttributeService;
        private readonly IStaffService _staffService;
        private readonly IPictureService _pictureService;
        private readonly IOrderService _orderService;
        private readonly ITypeNotifyService _typeNotifyService;
        private readonly IProductAttributeMappingService _productAttributeMappingService;
        private readonly IProductCategoryService _productCategoryService;


        public HomeController(IPictureService _pictureService, IProductService productService, IBlogService blogService,
            IWebsiteAttributeService websiteAttributeService, IBlogCategoryService blogCategoryService,
            IStaffService staffService, IProductAttributeMappingService productAttributeMappingService, IVendorService _vendorService
           , IOrderService _orderService, ITypeNotifyService _typeNotifyService, IProductCategoryService _productCategoryService)
        {
            this._productCategoryService = _productCategoryService;
            this._productService = productService;
            this._blogService = blogService;
            this._websiteAttributeService = websiteAttributeService;
            this._blogCategoryService = blogCategoryService;
            this._vendorService = _vendorService;
            this._staffService = staffService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._orderService = _orderService;
            this._typeNotifyService = _typeNotifyService;
            this._pictureService = _pictureService;
        }
        [ValidateInput(false)]
        public ActionResult FindProduct(int? page, string keyword = "", int place = 0, string type = "", string direction = "", string minArea = "", string maxArea = "",
                                        float minPrice = 0, float maxPrice = 50000000000)
        {
            ViewBag.keyword = keyword;
            ViewBag.place = place;
            ViewBag.type = type;
            ViewBag.direction = direction;
            ViewBag.minArea = minArea;
            ViewBag.maxArea = maxArea;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;

            int pageSize = 6;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Product> productsList = null;
            List<Product> ListProduct = _productService.GetAllProducts().Where(p => p.IsPublic && !p.Deleted && p.IsHomePage).ToList();
            List<Product> result = new List<Product>();

            //check keyword
            if (keyword.Length > 0)
            {
                ListProduct = ListProduct.Where(p => p.Name.ToLower().Contains(keyword.ToLower())).ToList();
                //ListProduct = ListProduct.Where(p => p.Description.ToLower().Contains(keyword.ToLower())).ToList();
                //result.addrange(listproduct.where(p => p.description.tolower().contains(keyword.tolower())).tolist());
            }

            //check place
            if (place != 0)
            {
                //result.AddRange(ListProduct.Where(p => p.ProductCategoryMappings.FirstOrDefault().ProductCategory.Name.Equals(place)).ToList());
                ListProduct = ListProduct.Where(p => p.ProductCategoryMappings.FirstOrDefault().ProductCategory.Id == place).ToList();
            }

            //check type
            if (!type.Equals("Loại nhà đất"))
            {
                //result.AddRange(ListProduct.Where(p => p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 24).FirstOrDefault().Value.Equals(type)).ToList());
                ListProduct = ListProduct.Where(p => p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 24).FirstOrDefault().Value.Equals(type)).ToList();
            }

            //check direction
            if (!direction.Equals("Hướng nhà"))
            {
                //result.AddRange(ListProduct.Where(p => p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 8).FirstOrDefault().Value.Equals(direction)).ToList());
                ListProduct = ListProduct.Where(p => p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 8).FirstOrDefault().Value.Equals(direction)).ToList();
            }

            //check area
            double min = 0, max = 0;

            if (minArea.Length > 0)
            {
                min = double.Parse(minArea);
                if (maxArea.Length == 0)
                {
                    max = 9999999999999;
                }
            }

            if (maxArea.Length > 0)
            {
                max = double.Parse(maxArea);

            }
            if (min > max)
            {
                double t = min;
                min = max;
                max = t;
            }

            if (min > 0 || max > 0)
            {
                //result.AddRange(ListProduct.Where(p => double.Parse(p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 3).FirstOrDefault().Value) >= min &&
                //            double.Parse(p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 3).FirstOrDefault().Value) <= max).ToList());
                ListProduct = ListProduct.Where(p => double.Parse(p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 3).FirstOrDefault().Value) >= min &&
                            double.Parse(p.ProductAttributeMappings.Where(x => x.ProductAttributeId == 3).FirstOrDefault().Value) <= max).ToList();
            }

            //check price

            maxPrice = maxPrice * 1000000000;

            if (minPrice != 0)
            {
                minPrice = minPrice * 1000000000;
            }

            if (minPrice != 0 || maxPrice != 50000000000)
            {
                ListProduct = ListProduct.Where(p => p.OrginalPrice > minPrice && p.OrginalPrice < maxPrice).ToList();
            }


            //productsList = result.Distinct().ToPagedList(pageIndex,pageSize);
            //if (keyword.Length == 0 && place.Equals("Địa điểm") && type.Equals("Loại nhà đất") && direction.Equals("Hướng nhà") && maxArea.Length == 0 && minArea.Length == 0 
            //    /*&& bedrooms.Equals("Số phòng ngủ") && bathrooms.Equals("Số phòng tắm") && minPrice == 0 && maxPrice == 50000000000*/)
            //{
            //    productsList = ListProduct.Distinct().ToPagedList(pageIndex, pageSize);
            //}

            productsList = ListProduct.Distinct().ToPagedList(pageIndex, pageSize);

            return View(productsList);
        }

        public ActionResult nhaban()
        {
            MenuModelsView model = new MenuModelsView();
            model.listCategory = _productCategoryService.GetProductCategories().Where(p => p.IsPublish == true).ToList();

            model.listProduct = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.ProductAttributeId == 24 && p.Value.Equals("true")).Take(4).ToList();


            return PartialView("nhabanNav", model);
        }
        public ActionResult nhathue()
        {
            MenuModelsView model = new MenuModelsView();
            model.listCategory = _productCategoryService.GetProductCategories().Where(p => p.IsPublish == true).ToList();

            model.listProduct = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.ProductAttributeId == 24 && p.Value.Equals("false")).Take(4).ToList();

            return PartialView("nhathueNav", model);
        }

        public ActionResult Index()
        {
            ProductViewModel model = new ProductViewModel();

            model.NowProduct = _productService.GetAllProducts().Take(12).ToList();
            model.ThueProduct = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals("false") && p.ProductAttributeId == 24).Select(p => p.Product).ToList().OrderByDescending(p => p.Id).Take(12).ToList();
            model.BanProduct = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals("true") && p.ProductAttributeId == 24).Select(p => p.Product).ToList().OrderByDescending(p => p.Id).Take(12).ToList();

            model.Category = _productCategoryService.GetProductCategories().ToList();

            return View(model);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Banner()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Banner");
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult AboutUs()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("AboutUs");
            return PartialView(list);
        }
        public ActionResult ContactUs()
        {
            //var list = _websiteAttributeService.GetWebsiteAttributeByType("ContactUs");
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Info");
            return PartialView(list);
        }

        //action nhận phản hồi từ trang liên hệ
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PhanHoi(string name, string phone, string email, string message)
        {
            if (ModelState.IsValid)
            {
                Vendor vendor = new Vendor();
                vendor.Name = name;
                vendor.Phone = phone;
                vendor.Address = email;
                vendor.Description = message;
                vendor.Type = "0";
                //Mapping to domain
                //Create Blog
                _vendorService.CreateVendor(vendor);
                return Redirect("/lien-he/");
                //chua thong bao gui tin nhan thanh cong
            }
            else
            {
                //return RedirectToAction("SanPham", "ProductHome");
                return Redirect("/lien-he/");
            }
        }

        public ActionResult HomeBanner()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Banner");
            return PartialView(list);
        }
        public ActionResult HomePartner()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Partner");
            return PartialView(list);
        }
        public ActionResult HomeStaff()
        {
            var list = _staffService.GetStaffs();
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult AboutUs2()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("AboutUsDetail").FirstOrDefault();
            return View(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Social()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Social");
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Slogan()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Slogan");
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Info()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Info");
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult DetailInfo()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Info"); 
            return PartialView(list);
        }
        public ActionResult sodienthoai()
        {
            var list = _websiteAttributeService.GetWebsiteAttributes().Where(p => p.Name.Equals("Số Điện Thoại"));
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Info2()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Info");
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult HocVien()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("HocVien");
            ViewBag.image = _websiteAttributeService.GetWebsiteAttributeByType("Background").FirstOrDefault().Value;
            return PartialView(list);
        }

        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Team()
        {
            var list = _staffService.GetStaffs().Where(p => !p.Deleted);
            return PartialView(list);
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Work()
        {
            var list = _blogService.GetBlogsByCategory(4);
            if (list.Count() > 6)
            {
                return PartialView(list.Take(6));
            }
            else
            {
                return PartialView(list);

            }
        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult TinTuc()
        {
            var list = _blogService.GetBlogsByCategory(3);
            if (list.Count() > 6)
            {
                return PartialView(list.Take(6));
            }
            else
            {
                return PartialView(list);

            }
        }

        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult KhoaHoc()
        {
            var list = _vendorService.GetVendors().OrderByDescending(o => o.Id);
            return PartialView(list);
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            var list = _websiteAttributeService.GetWebsiteAttributeByType("Info");
            return View();
        }
        public ActionResult DangKyHoc(int id, string name, string phone, string email, string job)
        {
            var i = _vendorService.GetVendorById(id);
            if (i == null)
            {
                return RedirectToAction("Index");
            }
            string message = "<strong>Tên:</strong> " + name + " <br /><strong>Phone:</strong> " + phone + " <br /><strong>email:</strong> " + email + " <br /><strong>job:</strong> " + job + " <br /> <strong>Môn :</strong> " + i.Name;
            sendMail.SendEmail("truonglongkt12@gmail.com", "Học Viên Đăng ký", message.Replace("\"", null).Replace(".", null));
            var item = _vendorService.GetVendorById(id);
            Order order = new Order();
            order.CustomerName = name;
            order.CustomerPhone = phone;
            order.CustomerEmail = email;
            order.Description = job;
            order.Note = item.Name;
            order.Deleted = false;
            order.Deadline = DateTime.Now;
            order.DateCreated = DateTime.Now;
            _orderService.CreateOrder(order);
            return RedirectToAction("VendorDetail", "BlogHome", new { id = id, confirm = 1 });
        }

        public ActionResult Message(string name, string email, string message, string phone)
        {
            string message2 = "<strong>Tên:</strong> " + name + " <br /><strong>Phone:</strong> " + phone + " <br /><strong>email:</strong> " + email + " <br /> Nội dung: " + message;
            sendMail.SendEmail("truonglongkt12@gmail.com", "Học viên Liên Hệ", message2);
            TypeNotify item = new TypeNotify();
            item.Name = name;
            item.Description = email;
            item.Note = message;
            item.Phone = phone;
            item.IsDelete = false;
            _typeNotifyService.CreateTypeNotify(item);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChoThueQuan()
        {
            return View();
        }
        public ActionResult ChoBanQuan()
        {
            return View();
        }
        public ActionResult GetNum()
        {
            var list = _productAttributeMappingService.GetProductAttributeMappings().Where(p => !p.ProductAttributeId.Equals(User.Identity.Name) && !p.Product.IsPublic && p.Product.IsHomePage && !p.Product.Deleted).Select(p => p.Product).Distinct().Count();
            return PartialView("_GetNum", list);
        }
        public ActionResult imagehihi()
        {
            var listImage = _pictureService.GetPictures();
            foreach (var item in listImage)
            {
                #region [chinh sua hinh anh de khong bi xoay hinh]
                var path = "";
                var bmp = new Bitmap(200, 200);
                try
                {
                 path = @"D:\Cong Ty\humiland\humiland\Labixa\Labixa" + item.Url.Replace("/",@"\");
                 bmp = new Bitmap(path);

                }
                catch (Exception)
                {

                    continue;
                }
                var exif = new EXIFextractor(ref bmp, "n"); // get source from http://www.codeproject.com/KB/graphics/exifextractor.aspx?fid=207371

                if (exif["Orientation"] != null)
                {
                    RotateFlipType flip = OrientationToFlipType(exif["Orientation"].ToString().Substring(0, 1).Trim(), path);

                    if (flip != RotateFlipType.RotateNoneFlipNone) // don't flip of orientation is correct
                    {
                        bmp.RotateFlip(flip);
                        exif.setTag(0x112, "1"); // Optional: reset orientation tag
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);

                        bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        // Dispose of the image files.
                        bmp.Dispose();
                    }
                }
                #endregion
            }
            return View();
        }
        #region [ham tich hop chinh kich thuoc]
        private static RotateFlipType OrientationToFlipType(string orientation, string path)
        {
            switch (int.Parse(orientation))
            {
                case 1:
                    return RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }
        #endregion

    }
    public static class sendMail
    {
        public static void SendEmail(string ToEmail, string Title, string Message)
        {
            string email = "lilotech03@gmail.com";
            string password = "zaq@1234";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(ToEmail));
            msg.Subject = Title;
            msg.Body = Message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);

        }
    }
}