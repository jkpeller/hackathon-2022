using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[TestFixture]
	public class AddCategoryFeatureTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;
		private string _categoryName;
		private string _featureName;
		private string _featureDefinition;
		private const string CommonFeatureTypeName = FeatureTypeUtility.CommonName;

		public AddCategoryFeatureTests() : base(nameof(AddCategoryFeatureTests))
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
		public void AddCategoryFeature_ByExistingFeature_Succeeds()
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

				//validate the new categoryFeature was created successfully
				AssertNewCategoryFeatureInUiCard(_featureName, CommonFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, featureId);
				DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void AddCategoryFeature_ByNewFeatureWithValidInformation_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();

				//type the name of a non-existing feature in the autocomplete box
				SelectFeatureFromDropDown(_categoryDetailsPage, _featureName);

				//enter in some valid text for a definition
				_categoryDetailsPage.InputFeatureDefinition.SendKeys(_featureDefinition, sendEscape: false);
				Log($"Typed '{_featureDefinition}' into the feature definition input box");

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//validate the new categoryFeature was created successfully
				AssertNewCategoryFeatureInUiCard(_featureName, CommonFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, null);
				DeleteCategory(categoryId);
			});
		}

		//[Test]
		//[Category("Category")]
		//[Category("Feature")]
		//public void AddCategoryFeature_ByDuplicateFeature_Fails()
		//{
		//	ExecuteTimedTest(() =>
		//	{

		//		//TODO uncomment this story after the bug around it has been fixed
		//		var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);
		//		PostFeature(_featureName, _featureDefinition);

		//		//click to add a feature to the category on the features card
		//		_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();

		//		//type the name of an existing feature with added leading and trailing spaces
		//		_categoryDetailsPage.InputFeatureAutocomplete.SendKeys($" {_featureName} ", sendEscape: false);
		//		Log($"Typed '{_featureName}' into the input feature autocomplete box");
		//		Model.Pages.CategoryDetailsPage.GetFeatureAutocompleteResultByFeatureName(_featureName).Click();
		//		Log($"Clicked on the autocomplete suggestion with the following text: '{_featureName}'");

		//		//enter in some valid text for a definition
		//		_categoryDetailsPage.InputFeatureDefinition.SendKeys(_featureDefinition, sendEscape: false);
		//		Log($"Typed '{_featureDefinition}' into the feature definition input box");

		//		//select 'Common' as the feature type
		//		SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

		//		//click to add the feature to the category
		//		_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
		//		Log("Clicked to add the feature to the category");

		//		//validate the new categoryFeature was created successfully
		//		AssertNewCategoryFeatureInUiCard(_featureName, CommonFeatureTypeName, _featureDefinition);
		//		AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, null);
		//		DeleteCategory(categoryId);
		//	});
		//}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void AddCategoryFeature_ByNewFeatureWithAboveMaximumFeatureName_Fails()
		{
			var featureName = RequestUtility.GetRandomString(101);
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();

				//type a name above the character limit in the box and click on the suggestion that is the first 100 characters of the entered text
				_categoryDetailsPage.InputFeatureAutocomplete.SendKeys(featureName, sendEscape: false);
				Log($"Typed '{featureName}' into the input feature autocomplete box");
				Thread.Sleep(1000);
				Model.Pages.CategoryDetailsPage.GetFeatureAutocompleteResultByFeatureName(featureName.Substring(0, 100)).Click();
				Log($"Clicked on the autocomplete suggestion with the following text: '{featureName.Substring(0, 100)}'");

				//enter in some valid text for a definition
				_categoryDetailsPage.InputFeatureDefinition.SendKeys(_featureDefinition, sendEscape: false);
				Log($"Typed '{_featureDefinition}' into the feature definition input box");

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//validate the new categoryFeature was created successfully
				AssertNewCategoryFeatureInUiCard(featureName.Substring(0, 100), CommonFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, null);
				DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void AddCategoryFeature_ByNewFeatureWithAboveMaximumFeatureDefinition_Fails()
		{
			var featureDefinition = RequestUtility.GetRandomString(136);
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();

				//type the name of a non-existing feature in the autocomplete box
				SelectFeatureFromDropDown(_categoryDetailsPage, _featureName);

				//enter in some valid text for a definition
				_categoryDetailsPage.InputFeatureDefinition.SendKeys(featureDefinition, sendEscape: false);
				Log($"Typed '{featureDefinition}' into the feature definition input box");

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//validate the new categoryFeature was created successfully
				AssertNewCategoryFeatureInUiCard(_featureName, CommonFeatureTypeName, featureDefinition.Substring(0, 135));
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, featureDefinition.Substring(0, 135), null);
				DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Category")]
		[Category("Feature")]
		public void AddCategoryFeature_ByNewFeatureWithMinimumFeatureName_Fails()
		{
			var featureName = RequestUtility.GetRandomString(3);
			ExecuteTimedTest(() =>
			{
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, _categoryName);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();

				//type 3 characters into the autocomplete and select the add new option
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName);

				//enter in some valid text for a definition
				_categoryDetailsPage.InputFeatureDefinition.SendKeys(_featureDefinition, sendEscape: false);
				Log($"Typed '{_featureDefinition}' into the feature definition input box");

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, CommonFeatureTypeName);

				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature to the category");

				//validate the new categoryFeature was created successfully
				AssertNewCategoryFeatureInUiCard(featureName, CommonFeatureTypeName, _featureDefinition);
				AssertNewCategoryFeatureViaApi(categoryId, CommonFeatureTypeName, _featureDefinition, null);
				DeleteCategory(categoryId);
			});
		}
	}
}
