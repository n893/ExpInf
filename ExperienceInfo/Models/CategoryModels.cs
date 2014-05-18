using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExperienceInfo.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Name of the Category")]
        public string CategoryName { get; set; }
        public List<SkillModel> Skills { get; set; }
    }

    public class SkillAddModel
    {
        public int CategoryId { get; set; }
        [Required]
        public string SkillName { get; set; }
    }
}