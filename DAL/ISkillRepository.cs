using System.Collections.Generic;
using System.Linq;
using DataContract;

namespace DAL
{
    public interface ISkillRepository
    {
        IEnumerable<IGrouping<string, UserSkill>> GetAllSkillsByUser(int currentUser);
        List<Skill> GetSkillsByCategoryId(int categoryId);
        Skill GetSkillById(int skillId);
        UserSkill GetUserSkill(int skillId, int userId);
        List<UserSkill> GetAllUserSkills();
        List<UserSkill> GetUserSkillsByUsers(List<int> userIds);
        void AddUserSkill(int skillId, int userId, int mark);
        void UpdateUserSkill(int skillId, int userId, int mark);
        void DeleteUserSkill(int userId, int skillId);
        void UpdateSkill(Skill skill);
        void DeleteSkill(int skillId);
        void AddSkill(Skill skill);
    }
}
