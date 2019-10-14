﻿using System;
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

namespace Labixa.Areas.Admin.Controllers
{

    public partial class ProductController : BaseController
    {
        #region Field

        readonly IProductService _productService;
        readonly IProductCategoryService _productCategoryService;

        readonly IProductAttributeService _productAttributeService;
        readonly IProductAttributeMappingService _productAttributeMappingService;

        readonly IPictureService _pictureService;
        readonly IVendorService _VendorService;
        readonly ILocationService _LocationService;
        readonly IPromotionService _PromotionService;
        readonly IColorService _ColorService;

        readonly IProductPictureMappingService _productPictureMappingService;
        readonly IProductCategoryMappingService _ProductCategoryMappingService;



        #endregion

        #region Ctor
        public ProductController(IProductService productService, IProductCategoryService productCategoryService,
            IProductAttributeService productAttributeService, IProductAttributeMappingService productAttributeMappingService,
            IPictureService pictureService, IProductPictureMappingService productPictureMappingService,  IVendorService _VendorService,
         ILocationService _LocationService,
         IPromotionService _PromotionService, IProductCategoryMappingService _ProductCategoryMappingService, 
            IColorService _ColorService
           )
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._productAttributeService = productAttributeService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._pictureService = pictureService;
            this._productPictureMappingService = productPictureMappingService;
            this._VendorService = _VendorService;
            this._PromotionService = _PromotionService;
            this._LocationService = _LocationService;
            this._ProductCategoryMappingService = _ProductCategoryMappingService;
            this._ColorService = _ColorService;
        }
        #endregion
       
        public ActionResult Index()
        {
            var products = _productService.GetProducts().ToList();

            //foreach (var item in products)
            //{
            //    ProductAttributeMapping obj = new ProductAttributeMapping();
            //    obj.IsRequired = false;
            //    obj.Value = "true";
            //    obj.DisplayOrder = 0;
            //    obj.ProductId = item.Id;
            //    obj.ProductAttributeId = 13;
            //    _productAttributeMappingService.CreateProductAttributeMapping(obj);
                ////adddata(item.Id);
            //}
            return View(model: products);
        }
        //ua trang nao quen roi
        public ActionResult Create()
        {
            //Get the list category
            var listProductCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
            Product product = new Product();
            ProductFormModel model = new ProductFormModel();

            model.ListProductCategory = listProductCategory;
            model.product = product;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public  ActionResult Create(ProductFormModel newProduct, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //Mapping to domain
                //Product product = Mapper.Map<ProductFormModel, Product>(newProduct.pro);
                Product product = newProduct.product;
                if (String.IsNullOrEmpty(product.Slug))
                {
                    product.Slug = StringConvert.ConvertShortName(product.Name);
                }

                //Create Product
                _productService.CreateProduct(product);
                product.Slug = product.Slug + "-" + product.Id;
                //if (newProduct.CategoryId != 0)
                //{
                //    ProductCategoryMapping obj = new ProductCategoryMapping();
                //    obj.ProductId = product.Id;
                //    obj.ProductCategoryId = newProduct.CategoryId;
                //    _ProductCategoryMappingService.CreateProductCategoryMapping(obj);
                //}
                if (newProduct.CategoryId != 0)
                {
                    ProductCategoryMapping obj = new ProductCategoryMapping();
                    obj.ProductId = product.Id;
                    obj.ProductCategoryId = newProduct.CategoryId;
                    _ProductCategoryMappingService.CreateProductCategoryMapping(obj);
                }
                //Add ProductAttribute after product created
                product.ProductAttributeMappings = new Collection<ProductAttributeMapping>();
                var listAttributeId = _productAttributeService.GetProductAttributes().Select(p => p.Id);
                foreach (var id in listAttributeId)
                {
                    product.ProductAttributeMappings.Add(
                        new ProductAttributeMapping() { ProductAttributeId = id, ProductId = product.Id });

                }
                //nhap thong tin nguoi tao
                var users = GetInfo();
                product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 19).FirstOrDefault().Value = users.UserName;
                product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 20).FirstOrDefault().Value = users.FirstName;
                product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 21).FirstOrDefault().Value = users.PhoneNumber;
                product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 22).FirstOrDefault().Value = users.Email;
                product.ProductAttributeMappings.Where(p => p.ProductAttributeId == 23).FirstOrDefault().Value = users.Skype;
                
                //Add Picture default for Labixa
                product.ProductPictureMappings = new Collection<ProductPictureMapping>();
                for (int i = 0; i < 6; i++)
                {
                    var newPic = new Picture();
                    bool ismain = i == 0;
                    _pictureService.CreatePicture(newPic);
                    product.ProductPictureMappings.Add(
                        new ProductPictureMapping()
                        {
                            PictureId = newPic.Id,
                            ProductId = product.Id,
                            IsMainPicture = ismain,
                            DisplayOrder = 0,
                        });
                }
                _productService.EditProduct(product);


                //create product relation

                //Save all after edit
                return RedirectToAction("Index", "Product");
            }
            else
            {
                Product product = new Product();
                ProductFormModel model = new ProductFormModel();

                model.product = product;
                return View("Create", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int productId)
        {

            Product product = _productService.GetProductById(productId);
           
            var listProductCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
            //var vendorid = product.VendorId != null ? -1 : product.VendorId;
            var listVendor = _VendorService.GetVendors().ToSelectListItems(product.VendorId);
            //var promotionid = product.PromotionId != null ? -1 : product.VendorId;
            var ListPromotion = _PromotionService.GetPromotions().ToSelectListItems(product.PromotionId);
            //var locationid = product.LocationId != null ? -1 : product.VendorId;
            //var ListLocation = _LocationService.GetLocations().ToSelectListItems(product.LocationId);
            var ListColor = _ColorService.GetColors().ToSelectListItems(product.ColorId);

            ProductFormModel model = new ProductFormModel();
            model.CategoryId = product.ProductCategoryMappings.FirstOrDefault().ProductCategoryId;
            model.CategoryId2 = product.ProductCategoryMappings.LastOrDefault().ProductCategoryId;
            model.ListProductCategory = listProductCategory;
            //model.Location = ListLocation;
            model.Promotion = ListPromotion;
            model.Vendor = listVendor;
            model.ListColors = ListColor;
            model.product = product;
            return View(model: model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public ActionResult Edit(ProductFormModel productToEdit, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //Mapping to domain
                Product product = productToEdit.product;
                product.Slug = product.Slug + "-" + product.Id;
                if (productToEdit.CategoryId != 0)
                {
                    var obj = _ProductCategoryMappingService.GetProductCategoryMappings().Where(p=>p.ProductId==productToEdit.product.Id);
                    obj.FirstOrDefault().ProductCategoryId = productToEdit.CategoryId;
                    _ProductCategoryMappingService.EditProductCategoryMapping(obj.FirstOrDefault());
                }
                //if (productToEdit.CategoryId2 != 0)
                //{
                //    var obj = _ProductCategoryMappingService.GetProductCategoryMappings().Where(p => p.ProductId == productToEdit.product.Id);
                //    obj.LastOrDefault().ProductCategoryId = productToEdit.CategoryId;
                //    _ProductCategoryMappingService.EditProductCategoryMapping(obj.LastOrDefault());
                //}
                //Product product = Mapper.Map<ProductFormModel, Product>(productToEdit.product);
                if (String.IsNullOrEmpty(product.Slug))
                {
                    product.Slug = StringConvert.ConvertShortName(product.Name);
                }
                //this funcion not update all relationship value.
                _productService.EditProduct(product);
                //update attribute
                foreach (var mapping in product.ProductAttributeMappings)
                {
                    _productAttributeMappingService.EditProductAttributeMapping(mapping);
                }
                //update productpicture URL
                foreach (var picture in product.ProductPictureMappings)
                {
                    _productPictureMappingService.EditProductPictureMapping(picture);
                    _pictureService.EditPicture(picture.Picture);
                }
                //add tour relation
                return continueEditing ? RedirectToAction("Edit", "Product", new { productId = product.Id })
                      : RedirectToAction("Index", "Product");
            }
            else
            {
                var listProductCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
                productToEdit.ListProductCategory = listProductCategory;
                return RedirectToAction("Edit",new{productId = productToEdit.product.Id});
            }
        }


        public ActionResult Delete(int productId)
        {
            _productService.DeleteProduct(productId);
            return RedirectToAction("Index");
        }
    }
}