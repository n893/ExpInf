using System.Collections.Generic;
using DataContract;

namespace DAL
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void DeleteCategory(int id);
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void Save();
    }
}
