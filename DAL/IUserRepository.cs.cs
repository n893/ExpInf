using System.Collections.Generic;

namespace DAL
{
	public interface IUserRepository
	{
		Dictionary<string, string> GetBirthdayUsers();
	}
}
