using System.Threading;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
    [Category("Category")]
    public class FeatureCounterTests: BaseTest
    {
        private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
        private Model.Pages.CategoriesPage _categoriesPage;

        public FeatureCounterTests() : base(nameof(FeatureCounterTests))
        {
        }

        [SetUp]
        public void SetUp()
        {
            _categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
            _categoriesPage = new Model.Pages.CategoriesPage();
        }
        
        [Test]
		public void CategoryFeatures_FeatureCountMatchesTheTotalOfActiveFeaturesAssociatedToTheCategories_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryName = RequestUtility.GetRandomString(9);
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, categoryName);
				
				var featureName1 = RequestUtility.GetRandomString(9);
				var featureDescription1 = RequestUtility.GetRandomString(9);
				PostFeature(featureName1, featureDescription1);
				
				var featureName2 = RequestUtility.GetRandomString(9);
				var featureDescription2 = RequestUtility.GetRandomString(9);
				PostFeature(featureName2, featureDescription2);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 1 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName1);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 1 to the category");

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 2 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName2);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 2 to the category");

				var featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("2", featureCount);
					
				//Cleanup
				DeleteCategory(categoryId);
			});
		}
		
		[Test]
		public void CategoryFeatures_VerifyFeatureCountWhenAFeatureIsRemoved_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				
				var categoryName = RequestUtility.GetRandomString(9);
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, categoryName);
				
				var featureName1 = RequestUtility.GetRandomString(9);
				var featureDescription1 = RequestUtility.GetRandomString(9);
				PostFeature(featureName1, featureDescription1);
				
				var featureName2 = RequestUtility.GetRandomString(9);
				var featureDescription2 = RequestUtility.GetRandomString(9);
				PostFeature(featureName2, featureDescription2);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 1 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName1);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 1 to the category");

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 2 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName2);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 2 to the category");

				var featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("2", featureCount);
				
				Model.Pages.CategoryDetailsPage.GetFeatureTableFeatureActionsByRowNumber(1).Click();
				Model.Pages.CategoryDetailsPage.GetFeatureActionRemoveButtonByFeatureName(featureName1).Click();
				_categoriesPage.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();
				
				featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("1", featureCount);

				//Cleanup
				DeleteCategory(categoryId);
			});
		}
		
	    [Test]
		public void CategoryFeatures_VerifyFeatureCountWhenANewFeatureIsAdded_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryName = RequestUtility.GetRandomString(9);
				
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, categoryName);
				
				var featureName1 = RequestUtility.GetRandomString(9);
				var featureDescription1 = RequestUtility.GetRandomString(9);
				PostFeature(featureName1, featureDescription1);
				
				var featureName2 = RequestUtility.GetRandomString(9);
				var featureDescription2 = RequestUtility.GetRandomString(9);
				PostFeature(featureName2, featureDescription2);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 1 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName1);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 1 to the category");

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 2 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName2);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 2 to the category");

				var featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("2", featureCount);
				
				var featureName3 = RequestUtility.GetRandomString(9);
				var featureDescription3 = RequestUtility.GetRandomString(9);
				PostFeature(featureName3, featureDescription3);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 3 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName3);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 3 to the category");

				featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("3", featureCount);

				//Cleanup
				DeleteCategory(categoryId);
			});
		}
		
	    [Test]
		public void CategoryFeatures_VerifyFeatureCountAfterAFeatureIsModified_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryName = RequestUtility.GetRandomString(9);
				
				var categoryId = PostCategoryAndNavigateToDetailsPage(_categoriesPage, categoryName);
				
				var featureName1 = RequestUtility.GetRandomString(9);
				var featureDescription1 = RequestUtility.GetRandomString(9);
				PostFeature(featureName1, featureDescription1);
				
				var featureName2 = RequestUtility.GetRandomString(9);
				var featureDescription2 = RequestUtility.GetRandomString(9);
				PostFeature(featureName2, featureDescription2);

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 1 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName1);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 1 to the category");

				//click to add a feature to the category on the features card
				_categoryDetailsPage.FeatureCardButtonAddFeature.ClickAndWaitForPageToLoad();
				Log("Clicked to add a feature 2 to the category");

				//type the name of the set up feature and select it from the autocomplete
				SelectFeatureFromDropDown(_categoryDetailsPage, featureName2);

				//select 'Common' as the feature type
				SelectFeatureTypeFromDropDown(_categoryDetailsPage, FeatureTypeUtility.CommonName);
				
				//click to add the feature to the category
				_categoryDetailsPage.DialogButtonAddFeature.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked to add the feature 2 to the category");

				var featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("2", featureCount);

				Model.Pages.CategoryDetailsPage.GetFeatureTableFeatureActionsByRowNumber(1).Click();
				Model.Pages.CategoryDetailsPage.GetFeatureActionEditButtonByFeatureName(featureName1).Click();
				var newFeatureDescription = RequestUtility.GetRandomString(9);
				_categoryDetailsPage.InputFeatureDefinition.SendKeys(newFeatureDescription);
				_categoryDetailsPage.DialogButtonSaveFeatureUpdate.ClickAndWaitPageToLoadAndOverlayToDisappear();
				
				featureCount = _categoryDetailsPage.FeatureCount.GetText();
				Assert.AreEqual("2", featureCount);

				//Cleanup
				DeleteCategory(categoryId);
			});
		}
    }
}