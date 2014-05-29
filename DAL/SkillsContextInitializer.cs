using System.Collections.Generic;
using System.Data.Entity;
using DataContract;
using WebMatrix.WebData;

namespace DAL
{
	public class SkillsContextInitializer : DropCreateDatabaseAlways<SkillInfoContext>
	{
		protected override void Seed(SkillInfoContext context)
		{
			var categories = new List<Category>
			                 {
				                 new Category {CategoryName = "Data bases"},
				                 new Category {CategoryName = "Administering"},
				                 new Category {CategoryName = "Languages"},
				                 new Category {CategoryName = "Web development"}
			                 };
			categories.ForEach(c => context.Categories.Add(c));

			var skills = new List<Skill>
			             {
				             new Skill {Category = categories[0], SkillName = "Slq"},
				             new Skill {Category = categories[0], SkillName = "Oracle"},
				             new Skill {Category = categories[0], SkillName = "MS Sql"},
				             new Skill {Category = categories[0], SkillName = "Migrations"},
				             new Skill {Category = categories[1], SkillName = "Unix"},
				             new Skill {Category = categories[2], SkillName = "Windows"}
			             };
			skills.ForEach(s => context.Skills.Add(s));

			var userSkills = new List<UserSkill>
			                 {
				                 new UserSkill {Mark = 3, Skill = skills[0], UserId = 1},
				                 new UserSkill {Mark = 5, Skill = skills[1], UserId = 1},
				                 new UserSkill {Mark = 4, Skill = skills[0], UserId = 2},
				                 new UserSkill {Mark = 1, Skill = skills[1], UserId = 2},
				                 new UserSkill {Mark = 2, Skill = skills[2], UserId = 1},
				                 new UserSkill {Mark = 4, Skill = skills[4], UserId = 1},
				                 new UserSkill {Mark = 4, Skill = skills[5], UserId = 1}
			                 };
			userSkills.ForEach(us => context.UserSkills.Add(us));

			base.Seed(context);
			SeedMembership();
		}

		private void SeedMembership()
		{
			WebSecurity.InitializeDatabaseConnection("DataContract.SkillInfoContext",
				"UserProfile", "UserId", "UserName", autoCreateTables: true);

			WebSecurity.CreateUserAndAccount("n893", "1", new {Email = "kon0n@ukr.net"});
			WebSecurity.CreateUserAndAccount("t1", "1", new {Email = "t1@gmail.com"});
			WebSecurity.CreateUserAndAccount("t2", "1", new {Email = "t2@gmail.com"});
			WebSecurity.CreateUserAndAccount("t3", "1", new {Email = "t3@gmail.com"});
		}
	}
}
