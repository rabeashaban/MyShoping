using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [DisplayName("الاسم")]

        public string Name { get; set; }
        [DisplayName("الوصف")]

        public string Description { get; set; }

        [DisplayName("الصورة")]
        [ValidateNever]
        public string Img { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [DisplayName("السعر")]

        public decimal Price { get; set; }

        [Required(ErrorMessage = "فئة المنتج مطلوبة")]
        [DisplayName("الفئة")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }
}
