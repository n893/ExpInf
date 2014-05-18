using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExperienceInfo.Models
{
    public class SearchModel
    {
        public List<CategoryModel> Categories { get; set; }
    }

    public class SkillToSearch
    {
        public int SkillId { get; set; }
        public int MinMark { get; set; }
    }

    public class FoundUser
    {
        public int UserId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}