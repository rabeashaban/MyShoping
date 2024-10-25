using System.ComponentModel.DataAnnotations;

namespace myshop.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "الاسم")]
        public string Name { get; set; }
        [Display(Name = "الوصف")]
        public string Description { get; set; }
        [Display(Name = "تاريخ الانشاء")]

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
