using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DataContract;

namespace ExperienceInfo.Test.Fakes
{
	class FakeSkillRepository : ISkillRepository
	{
		public IEnumerable<IGrouping<string, UserSkill>> GetAllSkillsByUser(int currentUser)
		{
			throw new NotImplementedException();
		}

		public List<Skill> GetSkillsByCategoryId(int categoryId)
		{
			throw new NotImplementedException();
		}

		public Skill GetSkillById(int skillId)
		{
			throw new NotImplementedException();
		}

		public UserSkill GetUserSkill(int skillId, int userId)
		{
			throw new NotImplementedException();
		}

		public List<UserSkill> GetAllUserSkills()
		{
			throw new NotImplementedException();
		}

		public List<UserSkill> GetUserSkillsByUsers(List<int> userIds)
		{
			throw new NotImplementedException();
		}

		public void AddUserSkill(int skillId, int userId, int mark)
		{
			throw new NotImplementedException();
		}

		public void UpdateUserSkill(int skillId, int userId, int mark)
		{
			throw new NotImplementedException();
		}

		public void DeleteUserSkill(int userId, int skillId)
		{
			throw new NotImplementedException();
		}

		public void UpdateSkill(Skill skill)
		{
			throw new NotImplementedException();
		}

		public void DeleteSkill(int skillId)
		{
			throw new NotImplementedException();
		}

		public void AddSkill(Skill skill)
		{
			throw new NotImplementedException();
		}
	}
}
