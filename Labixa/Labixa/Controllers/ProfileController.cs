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
using System.Drawing;
using Goheer.EXIF;

namespace Labixa.Controllers
{
    [Authorize]
    public class ProfileController : BaseHomeController
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

        public ProfileController(UserManager<User> userManager, IProductService _productService,
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
        // GET: /Profile/
        public ActionResult Index()
        {
            var user = GetInfo();
            return View(user);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> UpdateProfile(User user)
        {
            var manager = new UserManager<User>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(new Outsourcing.Data.OutsourcingEntities()));
            var users = await manager.FindByNameAsync(User.Identity.Name);

            users.Address = user.Address;
            users.Description = user.Description;
            users.Email = user.Email;
            users.FirstName = user.FirstName;
            users.Image = user.Image;
            users.LastName = user.LastName;
            users.PhoneNumber = user.PhoneNumber;
            users.Skype = user.Skype;
            IdentityResult result = await manager.UpdateAsync(users);
            return RedirectToAction("Index", "Profile");
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult WaitingProperty(int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<ProductAttributeMapping> list = _productAttributeMappingService.GetProductAttributeMappings().Where(p => !p.Value.Equals(User.Identity.Name) && p.ProductAttributeId==19 && !p.Product.IsPublic && p.Product.IsHomePage && p.Product.IsHomePage && !p.Product.Deleted).ToPagedList(pageIndex, pageSize);
            return View(list);
        }
        public ActionResult MyProperty(int? page = 1)
        {
            int pageSize = 12;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var user = GetInfo();

            IPagedList<Product> list = null;
            if (User.IsInRole("SuperAdmin"))
            {
                list = _productAttributeMappingService.GetProductAttributeMappings().Where(p => !p.Product.Deleted && p.Product.IsHomePage).Select(p => p.Product).Distinct().ToList().OrderByDescending(p => p.Id).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                list = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals(user.UserName) && !p.Product.Deleted && p.Product.IsHomePage).Select(p => p.Product).Distinct().ToList().OrderByDescending(p => p.Id).ToPagedList(pageIndex, pageSize);
            }
            var item = list.ToList();
            return View(list);
        }
        public ActionResult SubmitProperty(int? Id,string place)
        {
            var user = GetInfo();
            Product product = new Product();
            
            submitPropertyModel model = new submitPropertyModel();
            ViewBag.place = place==""?"mp":place;
            if (User.IsInRole("SuperAdmin"))
            {
                model.ListColor = _colorService.GetColors().Where(p => !p.isDelete).OrderBy(h => h.Name).ToSelectListItems(-1);
            }
            else
            {
                model.ListColor = _colorService.GetColors().Where(p => !p.isDelete && p.StaffUserName.Equals(user.UserName)).OrderBy(h => h.Name).ToSelectListItems(-1);
            }
            
            var existed = _productAttributeMappingService.GetProductAttributeMappings().Where(p => p.Value.Equals(User.Identity.Name)).Where(p=>!p.Product.IsHomePage).FirstOrDefault();
            if (Id != null)//edit product
            {
                product = _productService.GetProductById(int.Parse(Id.ToString()));
                //model.ListColor = _colorService.GetColors().Where(p => p.StaffUserName.Equals(user.UserName)).ToSelectListItems(product.ColorId);
                if (User.IsInRole("SuperAdmin"))
                {
                    model.ListColor = _colorService.GetColors().Where(p => !p.isDelete).OrderBy(h => h.Name).ToSelectListItems(product.ColorId);
                }
                else
                {
                    model.ListColor = _colorService.GetColors().Where(p => p.StaffUserName.Equals(user.UserName)).OrderBy(h => h.Name).ToSelectListItems(product.ColorId);
                }
                model.listCategory = _productCategoryService.GetProductCategories().ToSelectListItems(product.ProductCategoryMappings.FirstOrDefault().ProductCategoryId);
                model.Product = product;
                model.Product.OrginalPrice = model.Product.OrginalPrice;
                model.CategoryId = product.ProductCategoryMappings.FirstOrDefault().ProductCategoryId;
                model.Lat = product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 26).FirstOrDefault().Value;
                model.Long = product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 27).FirstOrDefault().Value;
                return View(model);
            }
            if (existed!=null)
            {
                model.listCategory = _productCategoryService.GetProductCategories().ToSelectListItems(existed.Product.ProductCategoryMappings.FirstOrDefault().ProductCategoryId);
                model.Product = existed.Product;
                model.Product.OrginalPrice = model.Product.OrginalPrice;
                model.CategoryId = model.Product.ProductCategoryMappings.FirstOrDefault().ProductCategoryId;
                model.Lat = model.Product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 26).FirstOrDefault().Value;
                model.Long = model.Product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 27).FirstOrDefault().Value;
               
                return View(model);
            }
            model.listCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
            product.Deleted = false;
            product.IsHomePage = false;
            //Add ProductAttribute after product created
            product.ProductAttributeMappings = new Collection<ProductAttributeMapping>();
            var listAttributeId = _productAttributeService.GetProductAttributes().Select(p => p.Id);
            foreach (var id in listAttributeId)
            {
                product.ProductAttributeMappings.Add(
                    new ProductAttributeMapping() { ProductAttributeId = id, ProductId = product.Id,Value="0"});

            }
            //nhap thong tin chu so huu mac dinh
            if (model.ListColor.ToList().FirstOrDefault()==null)
            {
                product.ColorId = 1;
                ViewBag.noti = "Bạn Vui Lòng Tạo Chủ sở hữu trước khi tạo Bất Động Sản";
                return RedirectToAction("Index", "Owner");
            }
            else
            {
            product.ColorId = int.Parse(model.ListColor.ToList().FirstOrDefault().Value);
            }

            //nhap thong tin nguoi tao
            var users = GetInfo();
            product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 19).FirstOrDefault().Value = users.UserName;
            product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 20).FirstOrDefault().Value = users.FirstName == null? users.UserName: users.FirstName;
            product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 21).FirstOrDefault().Value = users.PhoneNumber == null ? users.UserName : users.PhoneNumber;
            product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 22).FirstOrDefault().Value = users.Email == null ? users.UserName : users.Email;
            product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 23).FirstOrDefault().Value = users.Skype == null ? users.UserName : users.Skype;
            _productService.CreateProduct(product);
            model.Product = product;
            model.Lat = "10.778557";
            model.Long = "106.622344";
            //tao category

            ProductCategoryMapping obj = new ProductCategoryMapping();
            obj.ProductId = product.Id;
            obj.ProductCategoryId = 1;
            _productCategoryMappingService.CreateProductCategoryMapping(obj);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitProperty(submitPropertyModel model, string place="mp")
        {
            var temp = _productService.GetProductById(model.Product.Id);
            temp.ProductCategoryMappings.FirstOrDefault().ProductCategoryId = model.CategoryId;
            try
            {
                temp.Slug = StringConvert.ConvertShortName(model.Product.Name) + "-" + model.Product.Id;
            }
            catch (Exception)
            {

                return RedirectToAction("SubmitProperty", new { Id = temp.Id, place = place });
            }
            temp.ColorId = model.Product.ColorId;
            temp.CodeName = model.Product.CodeName;
            temp.OldPrice = model.CategoryId;
            temp.Content = model.Product.Content;
            temp.Description = model.Product.Description;
            temp.IsHomePage = true;
            temp.IsNew = model.Product.IsNew;
            temp.IsPublic = model.Product.IsPublic;
            temp.Name = model.Product.Name;
            temp.Deleted = false;
            temp.OrginalPrice = model.Product.OrginalPrice;
            foreach (var item in model.Product.ProductAttributeMappings)
            {
                if(item.Value != null)
                {
                    temp.ProductAttributeMappings.Where(p => p.Id == item.Id).FirstOrDefault().Value = item.Value;
                }
                else
                {
                    temp.ProductAttributeMappings.Where(p => p.Id == item.Id).FirstOrDefault().Value = "0";
                }
                
            }

            temp.ProductAttributeMappings.Where(m => m.ProductAttributeId == 30).FirstOrDefault().Value = model.Product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 30).FirstOrDefault().Value;
            temp.LastEditedTime = DateTime.Now;
            if (!temp.ProductPictureMappings.Any())
            {
                var pic = new Picture();
                pic.IsDeleted = false;
                pic.Url = "/Images/vin_hom_tan_cang.jpg";
                _pictureService.CreatePicture(pic);
                ProductPictureMapping mapping = new ProductPictureMapping();
                mapping.ProductId = temp.Id;
                mapping.PictureId = pic.Id;
                temp.ProductPictureMappings.Add(mapping);
            }
            //foreach (var item in temp.ProductPictureMappings)
            //{
            //model.Product.ProductPictureMappings.Add(item);
            //}
                try
            {

                _productService.EditProduct(temp);
            }
            catch (Exception)
            {

                return RedirectToAction("SubmitProperty", new { Id = temp.Id, place = place });
            }
            if (place.Equals("wp"))
            {
                return RedirectToAction("WaitingProperty");
            }
            else if (place.Equals("mp"))
            {
                return RedirectToAction("MyProperty");

            }
            else if (place.Equals("fe"))
            {
                return Redirect("/san-pham/" + temp.Slug);
            }
            else if (place.Equals("am"))
            {
                return Redirect("/Admin/Product");
            }
            else if(place!="")
            {
                return Redirect(place);
            }
            return RedirectToAction("MyProperty");



        }

        public ActionResult DeleteProduct(int? Id, string place)
        {
            
            //var item = _productService.GetProductById(Id);
            _productService.DeleteProduct(int.Parse(Id.ToString()));
            if (place.Equals("wp"))
            {
                return RedirectToAction("WaitingProperty");
            }
            else if(place.Equals("am"))
            {
            return Redirect("/Admin/Product");
            }
            else
            {
                return RedirectToAction("MyProperty");

            }
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
        [HttpPost]
        public JsonResult UploadImage(FormCollection collection)
        {
            bool isSavedSuccessfully = true;
            var id = collection["Product.Id"];
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Product\\" + User.Identity.Name.ToString()+ id , Server.MapPath(@"\")));

                        string pathString = originalDirectory.ToString();// System.IO.Path.Combine(originalDirectory.ToString(), User.Identity.Name.ToString());

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        //
                      
                        file.SaveAs(path);
                        #region [chinh sua hinh anh de khong bi xoay hinh]
                        var bmp = new Bitmap(path);
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
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                var product = _productService.GetProductById(int.Parse(id));
                var pic = new Picture();
                pic.IsDeleted = false;
                pic.Url = "/Images/Product/" + User.Identity.Name.ToString()+ product.Id + "/" + fName;
                _pictureService.CreatePicture(pic);
                ProductPictureMapping mapping = new ProductPictureMapping();
                mapping.ProductId = product.Id;
                mapping.PictureId = pic.Id;
                product.ProductPictureMappings.Add(mapping);
                _productService.EditProduct(product);
                return Json(new { Message = "/Images/Product/" + User.Identity.Name.ToString()+ product.Id + "/" + fName, Id =mapping.Id });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }

        }
        [HttpPost]
        public JsonResult UploadImageProfile(FormCollection collection)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\ProfileMember\\" + User.Identity.Name.ToString(), Server.MapPath(@"\")));

                        string pathString = originalDirectory.ToString();// System.IO.Path.Combine(originalDirectory.ToString(), User.Identity.Name.ToString());

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = "/Images/ProfileMember/" + User.Identity.Name.ToString() + "/" + fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }

        }

        public ActionResult GetAttachments(int productId)
        {
            var listImage = _productPictureMappingService.GetProductPictureMappings().Where(p => p.ProductId == productId);
            //Get the images list from repository
            var attachmentsList = new List<AttachmentsModel>();
            foreach (var item in listImage)
            {
                AttachmentsModel obj = new AttachmentsModel();
                obj.AttachmentID = item.Id;
                obj.FileName = item.Product.Name;
                obj.Path = item.Picture.Url;
                attachmentsList.Add(obj);
            }
            return Json(new { Data = attachmentsList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteImage(string path, int? id)
        {
            string fullPath = Request.MapPath(path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _productPictureMappingService.DeleteProductPictureMapping(int.Parse(id.ToString()));
            return null;
        }
    }
    public class AttachmentsModel
    {
        public long AttachmentID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}