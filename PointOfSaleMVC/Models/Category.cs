using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PointOfSaleMVC.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter category name")]
        [DisplayName("Name")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please enter category code")]
        [DisplayName("Code")]
        public string CategoryCode { get; set; }

        [Required(ErrorMessage = "Please enter category description")]
        [DisplayName("Description")]
        public string CategoryDescription { get; set; }
        //public string ImagePath { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [DisplayName("Parent Category")]
        public int? RootCategoryId { get; set; }
        public string CategoryType { get; set; }
        public Category RootCategory { get; set; }
    }
}