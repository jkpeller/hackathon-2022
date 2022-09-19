using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[TestFixture]
	public class EditCategoryFeatureTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;
		private string _categoryName;
		private string _featureName;
		private string _featureDefinition;
		private const string CommonFeatureTypeName = FeatureTypeUtility.CommonName;
		private const string OptionalFeatureTypeName = FeatureTypeUtility.OptionalName;

		public EditCategoryFeatureTests() : base(nameof(EditCategoryFeatureTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
			_categoryName = RequestUtility.GetRandomString(10);
			_featureName = RequestUtility.GetRandomString(12);
			_featureDefinition = RequestUtility.GetRandomString(35);
		}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void EditCategoryFeatureType_ByValidFeatureTypeNameAndSave_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);
				var featureId = PostFeature(_featureName, _featureDefinition);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, _featureName);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//click to edit the feature type
				Model.Pages.CategoryDetailsPage.GetFeatureTableFeatureActionsByRowNumber(1).Click();
				Log("Clicked to open the actions menu for the feature");
				Model.Pages.CategoryDetailsPage.GetEditFeatureTypeButtonByFeatureName(_featureName).Click();
				Log($"Clicked to edit the feature type for the feature {_featureName}");

				//change the feature type and save
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, OptionalFeatureTypeName);
				_categoryDetailsPage.DialogButtonSaveFeatureUpdate.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Saved the feature type update");

				//validate the new category feature was updated successfully
				AssertNewCategoryFeatureInUiCard(_featureName, OptionalFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, OptionalFeatureTypeName, _featureDefinition, featureId);
				DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void EditCategoryFeatureType_ByValidFeatureTypeNameAndCancel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);
				var featureId = PostFeature(_featureName, _featureDefinition);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, _featureName);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//click to edit the feature type
				Model.Pages.CategoryDetailsPage.GetFeatureTableFeatureActionsByRowNumber(1).Click();
				Log("Clicked to open the actions menu for the feature");
				Model.Pages.CategoryDetailsPage.GetEditFeatureTypeButtonByFeatureName(_featureName).Click();
				Log($"Clicked to edit the feature type for the feature {_featureName}");

				//change the feature type and cancel
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, OptionalFeatureTypeName);
				_categoryDetailsPage.DialogButtonCancelFeatureUpdate.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Cancelled the feature type update");

				//validate the category feature has the correct value
				AssertNewCategoryFeatureInUiCard(_featureName, CommonFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, featureId);
				DeleteCategory(categoryId);
			});
		}
	}
}
