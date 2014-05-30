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
		private readonly ISearchRepository _searchR;

		public SearchController(ISkillRepository skillRepository, ICategoryRepository categoryRepository, ISearchRepository searchR)
        {
            _skillRepository = skillRepository;
            _categoryRepository = categoryRepository;
			_searchR = searchR;
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
	        var conditions = new List<KeyValuePair<int, int>>();
	        foreach (var cat in model.Categories.Where(cat => cat.Skills != null))
	        {
		        foreach (var s in cat.Skills)
                {
                    if (s.Mark > 0)
                    {
                        conditions.Add(new KeyValuePair<int, int>(s.SkillId, s.Mark));
                    }
                }
	        }
			var userProfiles = _searchR.GetUsers(conditions);

			var foundUsers = new List<FoundUser>();
			foreach (var usr in userProfiles)
			{
				foundUsers.Add(new FoundUser {UserId = usr.UserId, Email = usr.Email, UserName = usr.UserName});
			}
            return View(foundUsers);
        }
    }
}
