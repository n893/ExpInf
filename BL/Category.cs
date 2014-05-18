using System.Collections.Generic;

namespace DataContract
{
    public class Category
    {
        private ICollection<Skill> _skills;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Skill> Skills
        {
            get { return _skills ?? (_skills = new HashSet<Skill>()); }
        }
    }
}