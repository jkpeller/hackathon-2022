using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using OpenQA.Selenium;
using System.Threading;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryTags
{
    [TestFixture]
    [Category("Category")]
    public class CategoryTagsTest : BaseTest
    {
        private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
        private Model.Pages.CategoriesPage _categoriesPage;
        private string _productName = RequestUtility.GetRandomString(9);
        private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";

        public CategoryTagsTest() : base(nameof(CategoryTagsTest))
        {
        }

        [SetUp]
        public void SetUp()
        {
            _categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
            _categoriesPage = new Model.Pages.CategoriesPage();
        }

        [Test]
        public void CategoryTags_TagTypeDefaultsToCategoryTag_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and select the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Thread.Sleep(2000);

                //Assert that Tag Type defaults to Category tag			
                Assert.AreEqual("Category Tag", _categoryDetailsPage.InputTagTypesTextValue.GetText());
                Assert.IsTrue(_categoryDetailsPage.SelectTagType.IsDisabled());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AttemptNewTagAboveMaximumTagNameLength_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(201);
            var tagDefinition = RequestUtility.GetRandomString(500);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and select the add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a 201 characters tag name 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                //Type in a 500 characters tag definition
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag defiiton value typed on the card was { tagDefinition }");

                //Assert that up to 200 characters were typed in the input tagname field 
                Assert.AreEqual(tagName.Substring(0, tagName.Length - 1), _categoryDetailsPage.InputTagName.GetTextValue());

                //Assert that the Add button is enabled
                Assert.IsTrue(_categoryDetailsPage.ButtonAddNewTag.IsEnabled());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_ByMissingTagNameValue_Succeeds()
        {
            var tagDefinition = RequestUtility.GetRandomString(500);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and select the add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag definition value 
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definition value typed on the card was { tagDefinition }");

                //Assert that the message "Tag Name is required" is displayed
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageRequiredTagName.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_ByMissingTagDefinitionValue_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(201);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button");

                //Click on the ellipse button and select the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName } ");
                _categoryDetailsPage.InputTagDefinition.SendKeys(Keys.Tab);

                //Assert that the message "Tag Definition is required" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageRequiredTagDefinition.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AttemptNewTagAboveMaximumTagDefinitionLength_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(100);
            var tagDefinition = RequestUtility.GetRandomString(1001);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and select the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a Tag Name 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName } ");

                //Type in a 1001 characters tag definition 
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definiton value typed on the card was { tagDefinition } ");

                //Assert that up to 1000 characters can be typed in the tag definition field 
                Assert.AreEqual(tagDefinition.Substring(0, tagDefinition.Length - 1), _categoryDetailsPage.InputTagDefinition.GetTextValue());

                //Assert that the Add button is enabled
                Assert.IsTrue(_categoryDetailsPage.ButtonAddNewTag.IsEnabled());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_CreateAndSaveNewTag_Succeeds()
        {
            var tagDefinition = RequestUtility.GetRandomString(20);
            var tagName = RequestUtility.GetRandomString(100);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and click on the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name value 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                //Type in a tag definition value 
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definition value typed on the card was { tagDefinition }");

                //Click the Add button 
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Log("The Add button was clicked");
                Thread.Sleep(1500);

                //Assert the Category Tags count. A count of 1 indicates that a new tag was created.
                Assert.AreEqual("1", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());

                // Filter by new tag
                _categoryDetailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                //Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
                //_categoryDetailsPage.SelectCategoryTagFilter.SendKeys();
                Assert.AreEqual(tagName, Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).GetText());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_CancelAddNewTagDialog_Succeeds()
        {
            var tagDefinition = RequestUtility.GetRandomString(20);
            var tagName = RequestUtility.GetRandomString(500);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and click on the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name value
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                //Type in a tag definition value
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definition value typed on the card was { tagDefinition }");

                //Click the Cancel button 
                _categoryDetailsPage.ButtonCancelNewTag.Click();
                Log("The Add button was clicked");

                //Assert the Category Tags count. A count of 0 indicates that a new tag was not created.
                Assert.AreEqual("0", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddNewCategoryTagFromCategoryDetailsPage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Assert that a new category tag was added
                Assert.IsTrue(Model.Pages.CategoryDetailsPage.GetAssociatedTagsChipByCategoryName(tagName).GetText().Contains(tagName));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_RemoveExistingCategoryTagForSelectedCategory_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Assert that the new tag was added
                var getResponseResult = CategoryAdminApiService.GetCategoryTagsById(categoryId.ToString()).Result;
                var associatedTagName = getResponseResult.Single(x => x.IsSelected).Name;
                Assert.AreEqual(tagName, associatedTagName);

                //Click on the remove icon to remove the category tag chip
                Model.Pages.CategoryDetailsPage.GetRemoveAssociatedTagsChipByCategoryName(tagName).Click();
                _categoryDetailsPage.ButtonSaveTags.Click();
                Thread.Sleep(1000);

                //Assert that the category tag was removed from the category
                getResponseResult = CategoryAdminApiService.GetCategoryTagsById(categoryId.ToString()).Result;
                Assert.IsTrue(getResponseResult.All(x => x.IsSelected == false));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_UndoCategoryTagsAssociations_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a category tag
                var (tagId, tagName) = CreateTag(TagType.CategoryTag);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                // Add associated tag to the selected category
                _categoryDetailsPage.InputAssociatedTags.SendKeys(tagName, sendEscape: false);
                Thread.Sleep(1000);
                Model.Pages.CategoryDetailsPage.GetAssociatedTagsOptionByTagName(tagName).Click();
                Thread.Sleep(1000);
                _categoryDetailsPage.ButtonSaveTags.Click();
                Thread.Sleep(1000);

                //Assert that the new tag was added
                var getResponseResult = CategoryAdminApiService.GetCategoryTagsById(categoryId.ToString()).Result;
                var associatedTagName = getResponseResult.Single(x => x.IsSelected).Name;
                Assert.AreEqual(tagName, associatedTagName);

                //Click on the remove icon to remove the category tag chip
                Model.Pages.CategoryDetailsPage.GetRemoveAssociatedTagsChipByCategoryName(tagName).Click();
                _categoryDetailsPage.ButtonSaveTags.Click();
                Thread.Sleep(1000);

                //Assert that the category tag was removed from the category
                getResponseResult = CategoryAdminApiService.GetCategoryTagsById(categoryId.ToString()).Result;
                Assert.IsTrue(getResponseResult.All(x => x.IsSelected == false));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
                CategoryAdminApiService.DeleteTag(tagId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDuplicateTagFromCategoryPageErrorMessage_Succeeds()
        {
            var tagDefinition = RequestUtility.GetRandomString(20);
            var tagName = RequestUtility.GetRandomString(100);

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and click on the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name value 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                //Type in a tag definition value 
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definition value typed on the card was { tagDefinition }");

                //Click the Add button 
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Log("The Add button was clicked");
                Thread.Sleep(1500);

                // Try to add the same tag one more time 
                //Click on the ellipse button and click on the Add new tag option
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name value 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                Thread.Sleep(1500);


                //Assert that the message "Duplicate tag detected" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageDuplicateTag.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddTagFromCategoryPageSuccessMessage_Succeeds()
        {
            var tagDefinition = RequestUtility.GetRandomString(20);
            var tagName = RequestUtility.GetRandomString(100);

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create a new category
                var (categoryId, categoryName) = CreateCategory();

                //Search for the new category
                _categoryDetailsPage.InputEditCategoryName.SendKeys(categoryName);

                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Click on the ellipse button and click on the Add new tag option
                var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
                tableCell.HoverOver();
                Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
                _categoryDetailsPage.AddNewTagMenuOption.Click();
                Log("Selected the Add New Tag option.");

                //Type in a tag name value 
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                Log($"The tag name value typed on the card was { tagName }");

                //Type in a tag definition value 
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                Log($"The tag definition value typed on the card was { tagDefinition }");

                Thread.Sleep(1500);

                //Assert that the message "No duplicates detected  is displayed" is displayed 
                Assert.IsTrue(_categoryDetailsPage.MessageNonDuplicateTag.IsDisplayed());

                //Click the Add button 
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Log("The Add button was clicked");
                Thread.Sleep(1500);

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDuplicateCategoryTagFromCategoryDetailsPageErrorMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Try to add the same tag one more time
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                //Assert that the message "Duplicate tag detected" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageDuplicateTag.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddCategoryTagFromCategoryDetailsPageSuccessMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                Thread.Sleep(1000);
                //Assert that the message "No duplicates detected  is displayed" is displayed 
                Assert.IsTrue(_categoryDetailsPage.MessageNonDuplicateTag.IsDisplayed());

                Thread.Sleep(1000);
                _categoryDetailsPage.ButtonAddNewTag.Click();

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDuplicateDataInsightsTagFromCategoryDetailsPageErrorMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "data insights";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Try to add the same tag one more time
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                //Assert that the message "Duplicate tag detected" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageDuplicateTag.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDataInsightsTagFromCategoryDetailsPageSuccessMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "data insights";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                Thread.Sleep(1000);
                //Assert that the message "No duplicates detected  is displayed" is displayed 
                Assert.IsTrue(_categoryDetailsPage.MessageNonDuplicateTag.IsDisplayed());

                Thread.Sleep(1000);
                _categoryDetailsPage.ButtonAddNewTag.Click();

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDuplicatePpcTagFromCategoryDetailsPageErrorMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "ppc";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Try to add the same tag one more time
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                //Assert that the message "Duplicate tag detected" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageDuplicateTag.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddPpcTagFromCategoryDetailsPageSuccessMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "ppc";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                Thread.Sleep(1000);
                //Assert that the message "No duplicates detected  is displayed" is displayed 
                Assert.IsTrue(_categoryDetailsPage.MessageNonDuplicateTag.IsDisplayed());

                Thread.Sleep(1000);
                _categoryDetailsPage.ButtonAddNewTag.Click();

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddDuplicatePplTagFromCategoryDetailsPageErrorMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "ppl";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);
                _categoryDetailsPage.ButtonAddNewTag.Click();
                Thread.Sleep(1000);

                //Try to add the same tag one more time
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                //Assert that the message "Duplicate tag detected" is displayed 
                Assert.IsTrue(_categoryDetailsPage.ErrorMessageDuplicateTag.IsDisplayed());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

        [Test]
        public void CategoryTags_AddPplTagFromCategoryDetailsPageSuccessMessage_Succeeds()
        {
            var tagName = RequestUtility.GetRandomString(10);
            var tagDefinition = RequestUtility.GetRandomString(30);
            string tagTypeOption = "ppl";

            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                //Create category
                var (categoryId, categoryName) = CreateCategory();

                //Navigates to category detail page
                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                //Click the Add button and add a new category tag
                _categoryDetailsPage.ButtonEditCategoryTag.Click();
                _categoryDetailsPage.InputTagTypes.Click();
                Model.Pages.CategoryDetailsPage.GetOptionTagTypeByName(tagTypeOption).Click();
                _categoryDetailsPage.InputTagTypes.SendKeys();
                _categoryDetailsPage.InputTagName.SendKeys(tagName);
                _categoryDetailsPage.InputTagDefinition.SendKeys(tagDefinition);

                Thread.Sleep(1000);
                //Assert that the message "No duplicates detected  is displayed" is displayed 
                Assert.IsTrue(_categoryDetailsPage.MessageNonDuplicateTag.IsDisplayed());

                Thread.Sleep(1000);
                _categoryDetailsPage.ButtonAddNewTag.Click();

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
            });
        }

    }
}