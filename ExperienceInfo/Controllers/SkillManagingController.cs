using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DAL;
using ExperienceInfo.Models;
using DataContract;

namespace ExperienceInfo.Controllers
{
    [Authorize(Roles = "manager")]
    public class SkillManagingController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISkillRepository _skillRepository;

        public SkillManagingController(ISkillRepository skillRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _skillRepository = skillRepository;
        }

        public ActionResult Index()
        {
            IEnumerable<Category> categoryList = _categoryRepository.GetAllCategories().ToList();
            var categories = new List<CategoryModel>();
            if (categoryList.Any())
            {
                foreach (var category in categoryList)
                {
                    var c = new CategoryModel
                                {
                                    CategoryId = category.CategoryId,
                                    CategoryName = category.CategoryName,
                                    Skills = new List<SkillModel>()
                                };
                    foreach (Skill skill in category.Skills)
                    {
                        c.Skills.Add(new SkillModel
                                         {
                                             SkillId = skill.SkillID,
                                             SkillName = skill.SkillName
                                         });
                    }
                    categories.Add(c);
                }
            }
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryModel categorymodel)
        {
            try
            {
                var category = new Category();
                if (categorymodel != null)
                {
                    category.CategoryName = categorymodel.CategoryName;
                }
                _categoryRepository.InsertCategory(category);
                _categoryRepository.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var categoryDetails = _categoryRepository.GetCategoryById(id);
            var category = new CategoryModel();
            if (categoryDetails != null)
            {
                category.CategoryName = categoryDetails.CategoryName;
				return View(category);
            }
			return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int id, Category categoryModel)
        {
            try
            {
                var category = _categoryRepository.GetCategoryById(id);
                category.CategoryName = categoryModel.CategoryName;
                _categoryRepository.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddSkill(int id)
        {
            var categoryDetails = _categoryRepository.GetCategoryById(id);
            var skill = new SkillAddModel();
            if (categoryDetails != null)
            {
                skill.CategoryId = categoryDetails.CategoryId;
            }
            return View(skill);
        }

        [HttpPost]
        public ActionResult AddSkill(SkillAddModel skillAddModel)
        {
            if (ModelState.IsValid)
            {
                var skill = new Skill
                              {
                                  CategoryId = skillAddModel.CategoryId,
                                  SkillName = skillAddModel.SkillName
                              };
                _skillRepository.AddSkill(skill);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var categoryDetails = _categoryRepository.GetCategoryById(id);
            var category = new CategoryModel();

            if (categoryDetails != null)
            {
                category.CategoryName = categoryDetails.CategoryName;
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = _categoryRepository.GetCategoryById(id);
                if (category != null)
                {
                    _categoryRepository.DeleteCategory(id);
                    _categoryRepository.Save();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditSkill(int id)
        {
            var skill = _skillRepository.GetSkillById(id);
            if (skill != null)
            {
                var skillModel = new SkillModel
                                     {
                                         SkillName = skill.SkillName
                                     };
                return View(skillModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditSkill(int id, Skill skillModel)
        {
            try
            {
                var skill = _skillRepository.GetSkillById(id);
                skill.SkillName = skillModel.SkillName;
                _skillRepository.UpdateSkill(skill);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteSkill(int id)
        {
            var skill = _skillRepository.GetSkillById(id);
            if (skill != null)
            {
                _skillRepository.DeleteSkill(id);
            }
            return RedirectToAction("Index");
        }
    }
}