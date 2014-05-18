using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataContract;

namespace DAL
{
    public class SkillRepository : ISkillRepository
    {
        private readonly SkillInfoContext _context;

        public SkillRepository(SkillInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<IGrouping<string, UserSkill>> GetAllSkillsByUser(int currentUser)
        {
            return _context.UserSkills.Where(userSkill => userSkill.UserId == currentUser)
                          .ToList()
                          .GroupBy(skill => skill.Skill.Category.CategoryName);
        }

        public Skill GetSkillById(int skillId)
        {
            return _context.Skills.Find(skillId);
        }

        public List<Skill> GetSkillsByCategoryId(int categoryId)
        {
            List<Skill> skills = _context.Skills.Where(skill => skill.CategoryId == categoryId).ToList();
            return skills;
        }

        public UserSkill GetUserSkill(int skillId, int userId)
        {
            return _context.UserSkills.FirstOrDefault(skill => skill.SkillId == skillId && skill.UserId == userId);
        }

        public void AddSkill(Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();
        }

        public void AddUserSkill(int skillId, int userId, int mark)
        {
            _context.UserSkills.Add(new UserSkill {Mark = mark, SkillId = skillId, UserId = userId});
            _context.SaveChanges();
        }

        public void UpdateUserSkill(int skillId, int userId, int mark)
        {
            var skill = new UserSkill {Mark = mark, SkillId = skillId, UserId = userId}; 
            _context.UserSkills.Attach(skill);
            _context.Entry(skill).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUserSkill(int userId, int skillId)
        {
            //todo: add if s != null
            var s = _context.UserSkills.Find(userId, skillId);
            _context.UserSkills.Remove(s);
            _context.SaveChanges();
        }

        public void UpdateSkill(Skill skill)
        {
            _context.Skills.Attach(skill);
            _context.Entry(skill).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteSkill(int skillId)
        {
            var s = _context.Skills.Find(skillId);
            _context.Skills.Remove(s);
            _context.SaveChanges();
        }

        public List<UserSkill> GetAllUserSkills()
        {
            return _context.UserSkills.ToList();
        }

        public List<UserSkill> GetUserSkillsByUsers(List<int> userIds)
        {
            return _context.UserSkills.Where(u => userIds.Contains(u.UserId)).ToList();
        }
    }
}
