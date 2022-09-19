using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryProducts
{
	[Category("Category")]
	public class TableTest : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public TableTest() : base(nameof(TableTest))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}

		[Test]
		public void CategoryProducts_VerifyLinkToProductPage_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(8);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product, category and their relationship and navigate to details page
				var (categoryId, productId, _) = CreateCategoryProductRelationship(productName);

				//Type a valid product name into the product name filter box
				_categoryDetailsPage.InputFilterProductCategoryName.SendKeys(productName);
				Log($"{productName} was typed into the product name filter box.");
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Apply filters button was clicked.");
				Thread.Sleep(2000);

				//Assert that the returned product has correct vendor url on the vendor name column
				Assert.AreEqual($"{BrowserUtility.BaseUri}products/{productId}", BasePage.GetProductLinkByRowNumber(1).GetHref());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());

			});
		}
		
		[Test]
		public void CategoryProducts_VerifySorting_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var productName1 = $"Z_{partialName}";
			var productName2 = $"A_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product1
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName1,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product1 { productName1 } created.");

				//Create product2
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName2,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product2 { productName2 } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId }
				}).Result.Select(pc => pc.ProductCategoryId).SingleOrDefault();
				Log($"{categoryName} is assigned to {productName1}.");

				//Insert product-category relationship 2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId2,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {productName2}.");

				//Archive product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});
				Log($"{categoryName} is archived for {productName1}.");

				NavigateToCategoryDetailsPage(categoryId.ToString());

				//Filter on both status filter values
				_categoryDetailsPage.SelectCategoryStatus.Click();
				Log("Clicked into the status filter box.");
				_categoryDetailsPage.SelectCategoryStatusArchived.Check();
				Log("Selected the archived status value.");
				_categoryDetailsPage.SelectCategoryStatus.SendKeys();

				//Apply filters
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Validate that the Name column is sorted in ascending order
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.Name, rows: 2);
				Log("Name column is sorted.");

				//Validate that the Category Status column is sorted in ascending order
				_categoryDetailsPage.ButtonApplyFilters.HoverOver();
				_categoryDetailsPage.CategoryStatus.ClickAndWaitForPageToLoad();
				Log("Clicked on the Category Status column.");
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus, rows: 2);
				Log("Category Status column is sorted.");

				//Validate that the Date Added column is sorted
				_categoryDetailsPage.ButtonApplyFilters.HoverOver();
				_categoryDetailsPage.DateAdded.ClickAndWaitForPageToLoad();
				Log("Clicked the Date Added column.");
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.DateAdded, rows: 2);
				Log("Date Added column is sorted.");
				
				//Validate that the Added By column is sorted
				_categoryDetailsPage.ButtonApplyFilters.HoverOver();
				_categoryDetailsPage.AddedBy.ClickAndWaitForPageToLoad();
				Log("Clicked on the Added By column");
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.AddedBy, rows: 2);
				Log("Date Added By column is sorted.");

				//Validate that the Date Changed column is sorted
				_categoryDetailsPage.ButtonApplyFilters.HoverOver();
				_categoryDetailsPage.DateChanged.ClickAndWaitForPageToLoad();
				Log("Clicked the Date Changed column.");
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.DateChanged, rows: 2);
				Log("Date Changed column is sorted.");

				//Validate that the Changed By column is sorted
				_categoryDetailsPage.ButtonApplyFilters.HoverOver();
				_categoryDetailsPage.ChangedBy.ClickAndWaitForPageToLoad();
				Log("Clicked on the Changed By column");
				Thread.Sleep(3000);
				AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.ChangedBy, rows: 2);
				Log("Changed By column is sorted.");

               // Validate that the Verification Status column is sorted in ascending order
                //_categoryDetailsPage.ButtonApplyFilters.HoverOver();
                //_categoryDetailsPage.VerificationStatus.ClickAndWaitForPageToLoad();
                //Log("Clicked on the Verification Status column.");
                //Thread.Sleep(3000);
                //AssertColumnIsSorted(Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus, rows: 2);
                //Log("Verification Status column is sorted.");

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId1.ToString());
				ProductAdminApiService.DeleteProduct(productId2.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void CategoryProducts_VerifyTableHeaders_Succeeds()
		{
			const string productName = "Name";
			const string publishedOn = "Published On";
			const string categoryStatus = "Category Status";
			const string dateAdded = "Date Added";
			const string addedBy = "Added By";
			const string dateChanged = "Date Changed";
			const string changedBy = "Changed By";
			const string verificationStatus = "Verification Status";
			var productName1 = RequestUtility.GetRandomString(10);

			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				////Create category
				//var(categoryId, _) = CreateCategory();

				//Create product1
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName1,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product1 { productName1 } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId }
				}).Result.Select(pc => pc.ProductCategoryId).SingleOrDefault();
				Log($"{categoryName} is assigned to {productName1}.");


				NavigateToCategoryDetailsPage(categoryId.ToString());
				
				//Assert that column headers are correct
				Assert.AreEqual(productName, _categoryDetailsPage.Name.GetText());
				Log($"Text displayed in the Product Name column is {_categoryDetailsPage.Name.GetText()}");

				Assert.AreEqual(publishedOn, _categoryDetailsPage.PublishedOn.GetText());
				Log($"Text displayed in the PublishedOn column is {_categoryDetailsPage.PublishedOn.GetText()}");

				Assert.AreEqual(categoryStatus, _categoryDetailsPage.CategoryStatus.GetText());
				Log($"Text displayed in the Category Status column is {_categoryDetailsPage.CategoryStatus.GetText()}");

				Assert.AreEqual(dateAdded, _categoryDetailsPage.DateAdded.GetText());
				Log($"Text displayed in the Date Added column is {_categoryDetailsPage.DateAdded.GetText()}");

				Assert.AreEqual(addedBy, _categoryDetailsPage.AddedBy.GetText());
				Log($"Text displayed in AddedBy column is {_categoryDetailsPage.AddedBy.GetText()}");

				Assert.AreEqual(dateChanged, _categoryDetailsPage.DateChanged.GetText());
				Log($"Text displayed in Date Changed column is {_categoryDetailsPage.DateChanged.GetText()}");

				Assert.AreEqual(changedBy, _categoryDetailsPage.ChangedBy.GetText());
				Log($"Text displayed in the ChangedBy column is {_categoryDetailsPage.ChangedBy.GetText()}");

				Assert.AreEqual(verificationStatus, _categoryDetailsPage.VerificationStatus.GetText());
				Log($"The text displayed in the Verification Status column is { _categoryDetailsPage.VerificationStatus.GetText()}");

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void CategoryProducts_StandardPaginationResults_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create category
				var (categoryId, _) = CreateCategory();

				NavigateToCategoryDetailsPage(categoryId.ToString());

				//the default items per page is set to 50
				_categoryDetailsPage.PaginatorDisplayRange.Click();
				Log($"The Category page defaults to {_categoryDetailsPage.PaginatorDropDownItemsPerPage.GetText()} results");
				Assert.AreEqual(PagingUtility.DefaultPageSize.ToString(), _categoryDetailsPage.PaginatorDropDownItemsPerPage.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
	}
}
