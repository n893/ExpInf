using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DAL;
using ExperienceInfo.Models;

namespace ExperienceInfo.Controllers
{
    [Authorize(Roles = "manager")]
    public class SearchController : Controller
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SearchController(ISkillRepository skillRepository, ICategoryRepository categoryRepository)
        {
            _skillRepository = skillRepository;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            var categories = _categoryRepository.GetAllCategories();
            var sm = new SearchModel
                         {
                             Categories = new List<CategoryModel>()
                         };
            foreach (var category in categories)
            {
                var c = new CategoryModel
                            {
                                CategoryName = category.CategoryName,
                                Skills = new List<SkillModel>()
                            };
                foreach (var skill in category.Skills)
                {
                    c.Skills.Add(new SkillModel
                                     {
                                         SkillId = skill.SkillID,
                                         SkillName = skill.SkillName
                                     });
                }
                sm.Categories.Add(c);
            }
            return View(sm);
        }

        [HttpPost]
        public ActionResult Search(SearchModel model)
        {
            var skills = new List<SkillToSearch>();
            foreach (var cat in model.Categories)
            {
                foreach (var s in cat.Skills)
                {
                    if (s.Mark > 0)
                    {
                        skills.Add(new SkillToSearch
                                       {
                                           SkillId = s.SkillId,
                                           MinMark = s.Mark
                                       });
                    }
                }
            }
            var userSkills = _skillRepository.GetAllUserSkills();
            IEnumerable<int> users =
                userSkills.Join(skills, skill => skill.SkillId, search => search.SkillId,
                                (skill, src) => new {skill, src.MinMark})
                          .Where(arg => arg.skill.Mark >= arg.MinMark)
                          .GroupBy(grp => grp.skill.UserId)
                          .Where(grouping => grouping.Count() == skills.Count)
                          .Select(grouping => grouping.Key);
            var foundUsers = new List<FoundUser>();

            var dbContext = new UsersContext();
            foreach (var userId in users)
            {
                var u = dbContext.UserProfiles.Find(userId);
                foundUsers.Add(new FoundUser
                                   {
                                       UserId = u.UserId,
                                       Email = u.Email,
                                       UserName = u.UserName
                                   });
            }
            return View(foundUsers);
        }
    }
}
