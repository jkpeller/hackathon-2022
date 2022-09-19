using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.UserTests
{
	[TestFixture]
	public class UserRedirectTests : BaseTest
	{
		private BasePage _page;

		public UserRedirectTests() : base(nameof(UserRedirectTests))
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
		public void UserRedirect_RootUrlToProductMappingPage_Succeeds()
		{
			const string expectedPageTitle = "Products";
			ExecuteTimedTest(() =>
			{
				//Open the application using the base page url
				OpenPage(_page);
				
				//Validate that the application is now on the site products page
				Assert.AreEqual(expectedPageTitle, _page.PageTitle.GetText());
				Log("User redirect from the base url is correct.");
			});
		}
	}
}
