using System;
using System.Collections.Generic;
using System.Linq;
using DataContract;

namespace DAL
{
	public class UserRepository : IUserRepository
	{
		public Dictionary<string, string> GetBirthdayUsers()
		{
			using (var context = new SkillInfoContext())
			{
				var users = context.UserProfiles.Where(
					user =>
						user.Birthday.HasValue &&
						user.Birthday.Value.Year == DateTime.Now.Year &&
						user.Birthday.Value.Month == DateTime.Now.Month &&
						user.Birthday.Value.Day == DateTime.Now.Day)
					.Select(user => new {user.UserName, user.Email});
				return users.ToDictionary(user => user.UserName, user => user.Email);
			}
		}
	}
}
