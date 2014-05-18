using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataContract;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SkillInfoContext _context;

        public CategoryRepository(SkillInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public void DeleteCategory(int id)
        {
            Category category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
        }

        public void InsertCategory(Category category)
        {
            _context.Categories.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
