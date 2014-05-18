using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContract
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
