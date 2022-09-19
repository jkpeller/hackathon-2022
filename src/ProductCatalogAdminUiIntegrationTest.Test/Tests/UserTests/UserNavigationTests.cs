using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.UserTests
{
	[TestFixture]
	public class UserNavigationTests : BaseTest
	{
		private BasePage _page;

		public UserNavigationTests() : base(nameof(UserNavigationTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new BasePage();
		}

		[Test]
		[Category("User")]
		[Category("Readonly")]
		public void UserNavigation_ValidateUserMenu_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Click to open the user menu
				_page.ButtonUserSection.Click();
				Log("Clicked the user menu section.");

				//Validate that the user email and logout buttons are displayed
				Assert.IsTrue(_page.DivCurrentUserEmail.IsDisplayed());
				Assert.IsTrue(_page.ButtonLogout.IsDisplayed());
			});
		}

		//TODO: when we have a service account set up, write a test to validate the email display is correct
	}
}
