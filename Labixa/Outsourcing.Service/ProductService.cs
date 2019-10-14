﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outsourcing.Core.Common;
using Outsourcing.Data.Models;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Repository;
using Outsourcing.Service.Properties;

namespace Outsourcing.Service
{
    public interface IProductService
    {

        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetHomePageProducts(bool showOnHomePage);
        IEnumerable<Product> GetDailyTour();
        IEnumerable<Product> GetLongTour();
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int productId);

        Product GetProductBySlug(string slug);

        void CreateProduct(Product product);
        void EditProduct(Product productToEdit);
        void DeleteProduct(int productId);
        void SaveProduct();
        IEnumerable<ValidationResult> CanAddProduct(Product product);

        #region [Manage PhotoAlbum]
        Product GetPhoto();
        void EditPhoto(Product Photo);
        void DeletePhoto(int PhotoId); 
        #endregion

    }
    public class ProductService : IProductService
    {
        #region Field
        private readonly IProductRepository productRepository;
        private readonly IProductAttributeMappingRepository productAttributeRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public ProductService(IProductRepository productRepository,IProductAttributeMappingRepository productAttributeRepository, IUnitOfWork unitOfWork)
        {
            this.productRepository = productRepository;
            this.productAttributeRepository = productAttributeRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region BaseMethod
        
        public IEnumerable<Product> GetProducts()
        {
            var products = productRepository.GetMany(p => !p.Deleted).OrderBy(p => p.Position) ;
            return products;
        }

        public IEnumerable<Product> GetHomePageProducts(bool showOnHomePage)
        {
            var products = productRepository.GetMany(p => !p.Deleted && p.IsPublic );
            if (showOnHomePage)
            {
                products = products.Where(p => p.IsHomePage).OrderBy(p=>p.Position);
            }
            return products;
        }
        
        public Product GetProductById(int productId)
        {
            var product = productRepository.GetById(productId);
            return product;
        }
        public Product GetProductBySlug(string slug)
        {
            var product = productRepository.Get(p => !p.Deleted && p.Slug.Equals(slug));
            return product;
        }

        public void CreateProduct(Product product)
        {
            productRepository.Add(product);
            SaveProduct();
        }

        public void EditProduct(Product productToEdit)
        {
            productToEdit.LastEditedTime = DateTime.Now;
            productRepository.Update(productToEdit);
            SaveProduct();
        }

        public void DeleteProduct(int productId)
        {
            //Get product by id.
            var product = productRepository.GetById(productId);
            if (product != null)
            {
                product.Deleted = true;
                product.LastEditedTime = DateTime.Now;
                //productRepository.Delete(product);
                productRepository.Update(product);
                SaveProduct();
            }
        }

        public void SaveProduct()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<ValidationResult> CanAddProduct(Product product)
        {
        
            //    yield return new ValidationResult("Product", "ErrorString");
            return null;
        }

        #endregion


        public IEnumerable<Product> GetAllProducts()
        {
            var list = productRepository.GetAll().Where(p => p.IsPublic == true && p.Deleted==false);
            return list;
        }


        public IEnumerable<Product> GetDailyTour()
        {
            var listDailyTour = new List<Product>();
            var listProduct = productRepository.GetMany(p=>p.Deleted==false);
            foreach (var product in listProduct)
            {
                if (productAttributeRepository.GetAll().Where(p => p.ProductId == product.Id && p.ProductAttributeId == 13 && p.Value.Equals("true")).Count() >0)
                {
                    listDailyTour.Add(product);
                } 
            }
            //foreach (var daily in listDaily)
            //{
            //    if (daily.ProductAttributeMappings.FirstOrDefault(p=>p.ProductAttributeId==13).Value.Equals("true"))
            //    {
            //        listDailyTour.Add(daily);
            //    }
            //}
            return listDailyTour.OrderBy(p=>p.Position);
        }

        public IEnumerable<Product> GetLongTour()
        {
            var listLongTour = new List<Product>();
            var listProduct = productRepository.GetMany(p => p.Deleted == false);
            foreach (var product in listProduct)
            {
                if (productAttributeRepository.GetAll().Where(p => p.ProductId == product.Id && p.ProductAttributeId == 13 && p.Value.Equals("false")).Count()>0)
                 {
                    listLongTour.Add(product);
                }
            }
            return listLongTour.OrderBy(p=>p.Position);
        }

        #region [Manage Photo]
      

        public Product GetPhoto()
        {
            return productRepository.Get(p => p.Id == 50);
        }

       
        public void EditPhoto(Product Photo)
        {
            Photo.LastEditedTime = DateTime.Now;
            productRepository.Update(Photo);
            SaveProduct();
        }

        public void DeletePhoto(int PhotoId)
        {
            var photo = productRepository.GetById(PhotoId);
            photo.Deleted = true;
            EditPhoto(photo);
        }
        #endregion
    }
}
