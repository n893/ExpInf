using System.Collections.Generic;
using System.Web.Mvc;
using DataContract;
using ExperienceInfo.Controllers;
using ExperienceInfo.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceInfo.Test
{
	[TestClass]
	public class SkillManagingControllerTest
	{
		[TestMethod]
		public void SmControllerTest_Ok()
		{
			// Arrange
			var controller = CreateSmController();
			// Act
			var result = controller.Edit(1);
			// Assert
			Assert.IsInstanceOfType(result, typeof (ViewResult));
		}

		[TestMethod]
		public void SmControllerTest_NotFound()
		{
			// Arrange
			var controller = CreateSmController();
			// Act
			var result = controller.Edit(6);
			// Assert
			Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
		}

		private SkillManagingController CreateSmController()
		{
			var catRepository = new FakeCategoryRepository(createTestCats());
			var skillRepository = new FakeSkillRepository();
			return new SkillManagingController(skillRepository, catRepository);
		}

		private List<Category> createTestCats()
		{
			var cats = new List<Category>
			           {
				           new Category {CategoryId = 1, CategoryName = "Foo"},
				           new Category {CategoryId = 2, CategoryName = "Bar"}
			           };
			return cats;
		}
	}
}
