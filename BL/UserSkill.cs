using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContract
{
    public class UserSkill
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int SkillId { get; set; }

        public int Mark { get; set; }

        public virtual Skill Skill { get; set; }
    }
}