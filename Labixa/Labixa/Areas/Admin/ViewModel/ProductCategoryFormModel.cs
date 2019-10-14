﻿using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using FluentValidation.Mvc;
//using FluentValidation.Validators;
//using FluentValidation;

namespace Labixa.Areas.Admin.ViewModel
{
    //[FluentValidation.Attributes.Validator(typeof(PersonValidator))]
    public class ProductCategoryFormModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool Deleted { get; set; }
        public bool IsPublish { get; set; }
        public bool IsHome { get; set; }
    }
    //public class PersonValidator : AbstractValidator<ProductCategoryFormModel>
    //{
    //    public PersonValidator()
    //    {
    //        RuleFor(x => x.Name).NotNull().WithMessage("Tên Không Được Để Trống");
    //        RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");
    //    }
    //}
}