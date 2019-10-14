using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Labixa.Areas.Admin.Models;

namespace Labixa.Controllers
{
    public class NhanvienController : Controller
    {
        #region Field

        readonly IProductService _productService;
        readonly IProductCategoryService _productCategoryService;

        readonly IProductAttributeService _productAttributeService;
        readonly IProductAttributeMappingService _productAttributeMappingService;

        readonly IPictureService _pictureService;
        readonly IVendorService _VendorService;
        readonly IProductPictureMappingService _productPictureMappingService;
        readonly IInventoryLogService _inventoryLogService;
        readonly IInventoryService _inventoryService;



        #endregion

        #region Ctor
        public NhanvienController(IProductService productService, IProductCategoryService productCategoryService,
            IProductAttributeService productAttributeService, IProductAttributeMappingService productAttributeMappingService,
            IPictureService pictureService, IProductPictureMappingService productPictureMappingService,
            IVendorService _VendorService, IInventoryService _inventoryService, IInventoryLogService _inventoryLogService
           )
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._productAttributeService = productAttributeService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._pictureService = pictureService;
            this._productPictureMappingService = productPictureMappingService;
            this._VendorService = _VendorService;
            this._inventoryLogService = _inventoryLogService;
            this._inventoryService = _inventoryService;
        }
        #endregion
        //
        //
        // GET: /Nhanvien/
        public ActionResult Index()
        {
            var list = _inventoryLogService.GetInventoryLogs().OrderByDescending(p => p.Id);
            return View(list);
        }
        public ActionResult Danhsach()
        {
            var list = _inventoryService.GetInventorys().OrderByDescending(p => p.Id);
            return View(list);
        }
        public JsonResult CheckAttend(string UDID, int port)
        {
            var obj = _inventoryService.GetInventorys().Where(p => p.Note.Equals(UDID)).FirstOrDefault();
            if (obj!=null)
            {
                var inven = new InventoryLog();
                inven.DateCreated = DateTime.Now;
                //0 o trong, 1 o ngoai
                inven.IsImport = port == 0 ? true : false;
                inven.IsDelete = false;
                inven.InventoryId = obj.Id;
                inven.LocationId = 1;
                _inventoryLogService.CreateInventoryLog(inven);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}