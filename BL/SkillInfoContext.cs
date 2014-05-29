using System.Data.Entity;

namespace DataContract
{
    public class SkillInfoContext : DbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }

		public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
