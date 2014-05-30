using System.Collections.Generic;
using DataContract;

namespace DAL
{
	public interface ISearchRepository
	{
		List<UserProfile> GetUsers(List<KeyValuePair<int, int>> conditionPairs);
	}
}
