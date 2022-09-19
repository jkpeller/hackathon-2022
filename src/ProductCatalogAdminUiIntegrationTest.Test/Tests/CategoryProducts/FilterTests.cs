using NUnit.Framework;
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
	[TestFixture]
	[Category("Category")]
	public class FilterTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}
		
		[Test]
		public void FilterCategoryProducts_ByValidProductName_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(8);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product { productName } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();

				//Insert product-category relationship
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {productName}.");

				NavigateToCategoryDetailsPage(categoryId.ToString());
				Log($"href attribute of the product created {Model.Pages.CategoryDetailsPage.GetLinkCategoryDetailsNameByRowNumber(1).GetHref()}");

				//Type a valid product name into the product name filter box
				_categoryDetailsPage.InputFilterProductCategoryName.SendKeys(productName);
				Log($"{productName} was typed into the product name filter box.");
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Apply filters button was clicked.");
				Thread.Sleep(2000);

				//Assert that the returned product name equals the product created for this test
				Assert.AreEqual(productName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.Name).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByAboveMaximumCharacters_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(101);
			var saveProductName = productName.Substring(0, productName.Length - 1);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				var (categoryId, productId, _) = CreateCategoryProductRelationship(saveProductName);

				//Type more than what is allowed into the product name filter box
				_categoryDetailsPage.InputFilterProductCategoryName.SendKeys(productName);
				Log($"Typed {productName} into the product name filter box.");
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Thread.Sleep(2000);
				Log("Clicked the apply filters button.");

				//Assert that the the product is returned because the search cut off the last character of the search
				Assert.AreEqual(saveProductName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.Name).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByBelowMinimumCharacters_Fails()
		{
			const string expectedProductNameErrorText = "You must type at least 3 characters";
			var searchText = RequestUtility.GetRandomString(2);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				var (categoryId, _) = CreateCategory();

				NavigateToCategoryDetailsPage(categoryId.ToString());

				//Type 2 characters into the product name filter box
				_categoryDetailsPage.InputFilterProductCategoryName.SendKeys(searchText);	
				Log($"Typed {searchText} into the product name filter box.");
				_categoryDetailsPage.DisplayCategoryName.Click();
				Log("Clicked the category name.");

				//Assert that the error message is correctly displayed below the filter and that the apply filters button is disabled
				Assert.AreEqual(expectedProductNameErrorText, _categoryDetailsPage.ErrorMessageInputProductNameFilter.GetText());
				Assert.IsFalse(_categoryDetailsPage.ButtonApplyFilters.IsDisplayed());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByActiveStatus_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(8);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				var (categoryId, productId, _) = CreateCategoryProductRelationship(productName);

				//Click Apply filters buttons since Active status is already selected
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Thread.Sleep(2000);
				Log("Clicked the apply filters button.");

				//Validate that the product displays active in the status column
				Assert.AreEqual(nameof(ProductCategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText());
				
				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByArchivedStatus_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(8);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				var (categoryId, productId, productCategory) = CreateCategoryProductRelationship(productName, false);

				//Try to archive product-category
				ProductAdminApiService.PutProductCategory(productCategory.ToString(),new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});

				NavigateToCategoryDetailsPage(categoryId.ToString());
				Thread.Sleep(2000);
				
				//Filter only on the archived status filter
				_categoryDetailsPage.SelectCategoryStatus.Click();
				Log("Clicked into the status filter box.");
				_categoryDetailsPage.SelectCategoryStatusActive.Uncheck();
				Log("Deselected the active status value.");
				_categoryDetailsPage.SelectCategoryStatusArchived.Check();
				Log("Selected the archived status value.");
				_categoryDetailsPage.SelectCategoryStatus.SendKeys();

				//Apply filters
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Thread.Sleep(2000);
				Log("Clicked the apply filters button.");

				//Validate that the product archived in the status column is empty
				Assert.AreEqual(null, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByBothStatuses_Succeeds()
		{
			var productName1 = RequestUtility.GetRandomString(9);
			var productName2 = RequestUtility.GetRandomString(9);
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
				var (category2Id, category2Name) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId, category2Id }
				}).Result.Where(pc => pc.CategoryId == category2Id).FirstOrDefault().ProductCategoryId;
				Log($"{categoryName} and {category2Name} is assigned to {productName1}.");

				//Insert product-category relationship 2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId2,
					CategoryIds = new List<int> { categoryId, category2Id }
				});
				Log($"{categoryName} and {category2Name} is assigned to {productName2}.");

				//Archive product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});
				Log($"{categoryName} is archived for {productName1}.");

				NavigateToCategoryDetailsPage(category2Id.ToString());
				Thread.Sleep(3000);

				//Filter on both status filter values
				_categoryDetailsPage.SelectCategoryStatus.Click();
				Log("Clicked into the status filter box.");
				_categoryDetailsPage.SelectCategoryStatusArchived.Check();
				Log("Selected the archived status value.");
				_categoryDetailsPage.SelectCategoryStatus.SendKeys();
				Thread.Sleep(3000);

				//Apply filters
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");
				Thread.Sleep(3000);

				//Click to sort by the status column so we can guarantee the order
				_categoryDetailsPage.TableHeaderStatus.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the status column.");
				Thread.Sleep(2000);

				//Validate that both products display the correct status values
				Assert.AreEqual(nameof(ProductCategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText());
				Assert.AreEqual("Removed", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1.ToString());
				ProductAdminApiService.DeleteProduct(productId2.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteCategory(category2Id.ToString());
			});
		}
		
		[Test]
		public void FilterCategoryProducts_ByNoStatus_Fails()
		{
			const string expectedErrorText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create category
				var (categoryId, _) = CreateCategory();
				
				NavigateToCategoryDetailsPage(categoryId.ToString());

				//Filter only neither status values
				_categoryDetailsPage.SelectCategoryStatus.Click();
				Log("Clicked into the status filter box.");
				_categoryDetailsPage.SelectCategoryStatusActive.Uncheck();
				Log("Deselected the active status value.");
				_categoryDetailsPage.SelectCategoryStatus.SendKeys();

				//Validate that an error message displays with the correct text
				Assert.AreEqual(expectedErrorText, _categoryDetailsPage.ErrorMessageSelectCategoryStatusFilter.GetText());
			});
		}

		//TODO: Add tests for Verification status filter when we are able to change it
	}
}