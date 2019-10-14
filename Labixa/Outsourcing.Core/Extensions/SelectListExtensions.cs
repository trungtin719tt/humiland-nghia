﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Outsourcing.Core;
using System.ComponentModel;
using Outsourcing.Data.Models;


namespace Outsourcing.Core.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<User> users, string selectedId)
        {
            return

                users.OrderBy(user => user.Id)
                      .Select(user =>
                          new SelectListItem
                          {
                              Selected = (user.Id == selectedId),
                              Text = user.DisplayName,
                              Value = user.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<BlogCategory> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.CategoryParentId)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
            this IEnumerable<Vendor> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
           this IEnumerable<Location> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
           this IEnumerable<Color> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
           this IEnumerable<Promotion> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<TypeNotify> blogCategory, int selectedId)
        {
            return

                blogCategory.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Product> product, int selectedId)
        {
            return

                product.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<ProductCategory> productCategories, int selectedId)
        {
            return

                productCategories.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
             this IEnumerable<ProductCategory> productCategories, int selectedId, int? type = null)
        {
            if (type != null)
            {
                return

              productCategories.OrderBy(f => f.Id)
                    .Select(f =>
                        new SelectListItem
                        {
                            Selected = (f.Id == selectedId),
                            Text = f.Name,
                            Value = f.Id.ToString()
                        });
            }
            else
            {
                return

              productCategories.OrderBy(f => f.Id)
                    .Select(f =>
                        new SelectListItem
                        {
                            Selected = (f.Id == selectedId),
                            Text = f.Name,
                            Value = f.Id.ToString()
                        });
            }
          
        }
        //public static IEnumerable<SelectListItem> ToSelectListItems(
        //      this IEnumerable<GoalStatus> goalStatus, int selectedId)
        //{
        //    return

        //        goalStatus.OrderBy(gs => gs.GoalStatusType)
        //              .Select(gs =>
        //                  new SelectListItem
        //                  {
        //                      Selected = (gs.GoalStatusId == selectedId),
        //                      Text = gs.GoalStatusType,
        //                      Value = gs.GoalStatusId.ToString()
        //                  });
        //}
    }
}
