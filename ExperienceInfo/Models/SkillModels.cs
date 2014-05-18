using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExperienceInfo.Models
{
    public class SkillModel
    {
        [Key]
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Mark { get; set; }
        public CategoryModel Category { get; set; }
    }

    public class SkillsModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SkillModel> Skills { get; set; }
        
        public int? SelectedMark { get; set; }
    }

    public class SubmittingUserSkill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int ExistedMark { get; set; }
        public int NewMark { get; set; }
    }

    public class EditMark
    {
        public int SkillId { get; set; }
        [Display(Name = "Skill name")]
        public string SkillName { get; set; }
        public int Mark { get; set; }
    }

    public class SkillCatalogModel
    {
        public int? SelectedCategory { get; set; }
        [Required]
        public int? SelectedSkill { get; set; }
        [Required]
        public int? SelectedMark { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<SkillModel> Skills { get; set; }
    }

    public class UserSkills
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<SkillModel> Skills { get; set; } 
    }
}