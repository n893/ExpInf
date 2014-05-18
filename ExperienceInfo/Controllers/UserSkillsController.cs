using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DAL;
using ExperienceInfo.Filters;
using ExperienceInfo.Models;
using DataContract;
using WebMatrix.WebData;

namespace ExperienceInfo.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class UserSkillsController : Controller
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ICategoryRepository _categoryRepository;

        //private readonly SkillInfoContext _context = new SkillInfoContext();

        public UserSkillsController(ISkillRepository skillRepository, ICategoryRepository categoryRepository)
        {
            _skillRepository = skillRepository; //new SkillRepository(_context);
            _categoryRepository = categoryRepository; //new CategoryRepository(_context);
        }

        public ActionResult Index()
        {
            var skills = new List<SkillsModel>();
            var currentUserId = WebSecurity.CurrentUserId;

            IEnumerable<IGrouping<string, UserSkill>> groupedSkills = _skillRepository.GetAllSkillsByUser(currentUserId);
            foreach (IGrouping<string, UserSkill> category in groupedSkills)
            {
                var c = new SkillsModel
                            {
                                CategoryName = category.Key,
                                Skills = new List<SkillModel>()
                            };
                foreach (UserSkill skill in category)
                {
                    c.Skills.Add(new SkillModel
                                     {
                                         Mark = skill.Mark,
                                         SkillId = skill.SkillId,
                                         SkillName = skill.Skill.SkillName
                                     });
                }
                skills.Add(c);
            }
            return View(skills);
        }

        [HttpPost]
        public ActionResult SelectCategory(int? selectedCategory)
        {
            var skillCatalogModel = new SkillCatalogModel
                                        {
                                            Skills = new List<SkillModel>()
                                        };
            var skillModels = new List<SkillModel>();
            var skills = new List<Skill>();
            if (selectedCategory.HasValue)
            {
                skills = _skillRepository.GetSkillsByCategoryId((int) selectedCategory);
            }
            foreach (var s in skills)
            {
                skillModels.Add(new SkillModel
                                    {
                                        SkillId = s.SkillID,
                                        SkillName = s.SkillName
                                    });
            }
            skillCatalogModel.Skills = skillModels;
            return PartialView("_SkillsPartial", skillCatalogModel);
        }

        public ActionResult Add()
        {
            var skillCatalogModel = new SkillCatalogModel();
            var categoriesList = _categoryRepository.GetAllCategories();
            var categories = new List<CategoryModel>();
            foreach (var category in categoriesList)
            {
                categories.Add(new CategoryModel
                                   {
                                       CategoryId = category.CategoryId,
                                       CategoryName = category.CategoryName
                                   });
            }
            skillCatalogModel.Categories = categories;
            ViewBag.Title = TempData["Message"] ?? "Adding new skill";
            return View(skillCatalogModel);
        }

        [HttpPost]
        public ActionResult Add(SkillCatalogModel skillCatalogModel) // ??
        {
            if (ModelState.IsValid && skillCatalogModel.SelectedSkill != null && skillCatalogModel.SelectedMark != null)
            {
                if (WebSecurity.IsAuthenticated)
                {
                    var userID = WebSecurity.CurrentUserId;
                    UserSkill existedUserSkill = _skillRepository.GetUserSkill((int) skillCatalogModel.SelectedSkill, userID);
                    if (existedUserSkill != null)
                    {
                        if (existedUserSkill.Mark != skillCatalogModel.SelectedMark)
                        {
                            var submittingUserSkill = new SubmittingUserSkill
                                        {
                                            SkillName = existedUserSkill.Skill.SkillName,
                                            ExistedMark = existedUserSkill.Mark,
                                            NewMark = (int) skillCatalogModel.SelectedMark,
                                            SkillId = (int) skillCatalogModel.SelectedSkill
                                        };
                            return View("SubmitReplase", submittingUserSkill);
                        }
                        TempData["Message"] = string.Format("Skill {0} with mark {1} already exists. Want to add another?",
                                                      existedUserSkill.Skill.SkillName, existedUserSkill.Mark);
                        return RedirectToAction("Add");
                    }
                    _skillRepository.AddUserSkill((int) skillCatalogModel.SelectedSkill, userID, (int) skillCatalogModel.SelectedMark);
                }      
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SubmitSkillReplace(SubmittingUserSkill userSkill)
        {
            if (WebSecurity.IsAuthenticated)
            {
                var userId = WebSecurity.CurrentUserId;
                _skillRepository.UpdateUserSkill(userSkill.SkillId, userId, userSkill.NewMark);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            var userId = WebSecurity.CurrentUserId;
            _skillRepository.DeleteUserSkill(userId, id);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var userId = WebSecurity.CurrentUserId;
            var userSkill = _skillRepository.GetUserSkill(id, userId);
            if (userSkill != null)
            {
                var editMark = new EditMark
                            {
                                Mark = userSkill.Mark,
                                SkillName = userSkill.Skill.SkillName,
                                SkillId = id
                            };
                return View(editMark);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditMark editMark)
        {
            var userId = WebSecurity.CurrentUserId;
            _skillRepository.UpdateUserSkill(editMark.SkillId, userId, editMark.Mark);
            return RedirectToAction("Index");
        }
    }
}