using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DataContract;

namespace ExperienceInfo.Test.Fakes
{
	class FakeCategoryRepository : ICategoryRepository 
	{
		private List<Category> catsList;

		public FakeCategoryRepository(List<Category> cats)
		{
			catsList = cats;
		}

		public IEnumerable<Category> GetAllCategories()
		{
			return catsList;
		}

		public Category GetCategoryById(int id)
		{
			return catsList.FirstOrDefault(c => c.CategoryId == id);
		}

		public void DeleteCategory(int id)
		{
			throw new NotSupportedException();
		}

		public void Save()
		{
			throw new NotSupportedException();
		}

		public void UpdateCategory(Category category)
		{
			throw new NotSupportedException();
		}

		public void InsertCategory(Category category)
		{
			throw new NotSupportedException();
		}
	}
}
