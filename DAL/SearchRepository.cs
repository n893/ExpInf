using System.Collections.Generic;
using System.Linq;
using DataContract;

namespace DAL
{
	public class SearchRepository : ISearchRepository
	{
		private readonly SkillInfoContext _context;

		public SearchRepository(SkillInfoContext context)
        {
            _context = context;
        }

		/// <summary>
		/// Selects users that requires condition
		/// </summary>
		/// <param name="conditionPairs">SkillId - Min mark</param>
		public List<UserProfile> GetUsers(List<KeyValuePair<int, int>> conditionPairs)
		{
			// group userSkills by users
			var query = _context.UserSkills.GroupBy(c => c.UserId, c => c);
			foreach (var kvp in conditionPairs)
			{
				// add WHERE to each mark requirement
				query = query.Where(g => g.Any(us => us.SkillId == kvp.Key && us.Mark > kvp.Value));
			}
			
			var userIds = query.Select(g => g.Key);

			var profiles = from id in userIds
				join user in _context.UserProfiles on id equals user.UserId
				select user;

			return profiles.ToList();
		}
	}
}
