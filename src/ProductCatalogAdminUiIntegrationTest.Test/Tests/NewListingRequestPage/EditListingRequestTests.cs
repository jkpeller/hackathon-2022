using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;
using System.Linq;
using System;
using System.Collections.Generic;


namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
	[TestFixture]
	[Category("ListingRequest")]
	public class EditListingRequestTests : BaseTest
	{
		private Model.Pages.NewListingRequestPage _page;
		private Model.Pages.VendorPage _vendorPage;
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private string _updatedCompanyName;
		private string _updatedCompanyWebsiteUrl;
		private string _updatedCompanyPhoneNumber;
		private string _updatedProductName;
		private string _updatedProductWebsiteUrl;
		private string _updatedProductShortDescription;
		private string _updatedApprovedCategoryName;
		private const string UpdatedCountry = "United States of America";
		private string _sugarUserFirstName;
		private string _sugarUserLastName;
		private string _sugarUserEmail;
		private string _companyName;
		private string _companyWebsiteUrl;

		public EditListingRequestTests() : base(nameof(EditListingRequestTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_vendorPage = new Model.Pages.VendorPage();
			_page = new Model.Pages.NewListingRequestPage();
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_updatedCompanyName = RequestUtility.GetRandomString(10);
			_updatedCompanyWebsiteUrl = $"https://www.{_updatedCompanyName}.com";
			_updatedCompanyPhoneNumber = RequestUtility.GetRandomString(15);
			_updatedProductName = RequestUtility.GetRandomString(10);
			_updatedProductWebsiteUrl = $"https://www.{_updatedCompanyName}.com/{_updatedProductName}";
			_updatedProductShortDescription = RequestUtility.GetRandomString(50);
			_updatedApprovedCategoryName = RequestUtility.GetRandomString(11);
			_sugarUserFirstName = RequestUtility.GetRandomString(7);
			_sugarUserLastName = RequestUtility.GetRandomString(10);
			_sugarUserEmail = $"{_sugarUserFirstName}.{_sugarUserLastName}@{SocialMediaUtility.CompanyName}.com";
			_companyName = RequestUtility.GetRandomString(9);
			_companyWebsiteUrl = $"https://www.{_updatedCompanyName}.com/{_companyName}";
		}

		[Test]
		public void EditListingRequest_ByAllValidRequiredFields_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Assert that the updated values are shown on the listing request readonly page
				AssertAllRequiredFields(listingRequest, approvedCategory);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByAllValidFields_Succeeds()
		{
			const string streetAddress1 = "2500 Bee Caves Road";
			const string streetAddress2 = "#1";
			const string city = "Texas City";
			const string state = "Texas";
			const string zipPostalCode = "78746";
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Enter in valid values for the social media urls
				_page.InputTwitter.SendKeys(SocialMediaUtility.TwitterUrl, true);
				_page.InputFacebook.SendKeys(SocialMediaUtility.FacebookUrl, true);
				_page.InputLinkedIn.SendKeys(SocialMediaUtility.LinkedInUrl, true);
				_page.InputYouTube.SendKeys(SocialMediaUtility.YoutubeUrl, true);
				_page.InputInstagram.SendKeys(SocialMediaUtility.InstagramUrl, true);

				//Enter in valid values into the rest of the address fields
				_page.InputAddress1.SendKeys(streetAddress1, true);
				_page.InputAddress2.SendKeys(streetAddress2, true);
				_page.InputCity.SendKeys(city, true);
				_page.InputStateProvince.SendKeys(state, true);
				_page.InputZipPostalCode.SendKeys(zipPostalCode, true);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Assert that the updated values are shown on the listing request readonly page
				Assert.AreEqual($"{city}, {state}, {UpdatedCountry}", _page.ListingRequestAddress.GetText());
				Assert.AreEqual(SocialMediaUtility.TwitterUrl, _page.ListingRequestTwitterUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.FacebookUrl, _page.ListingRequestFacebookUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.LinkedInUrl, _page.ListingRequestLinkedinUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.YoutubeUrl, _page.ListingRequestYoutubeUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.InstagramUrl, _page.ListingRequestInstagram.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByValidCompanyWebsiteUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields except for Company Website Url
				_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
				Log($"Entered {_updatedCompanyName} into the company name input box.");

				_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
				Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

				//Country is a little different because you need to click into it first
				_page.SelectCountry.Click();
				BasePage.GetCountryOptionByName(UpdatedCountry).ClickAndWaitForPageToLoad();
				Log($"Selected {UpdatedCountry} from the country drop down.");

				_page.InputFilterProductName.SendKeys(_updatedProductName, true);
				Log($"Entered {_updatedProductName} into the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				_page.InputProductShortDescription.SendKeys(_updatedProductShortDescription, true);
				Log($"Entered {_updatedProductShortDescription} into the product short description input box.");

				//Approved category will work similarly to country
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Log($"Selected {approvedCategory.Name} from the approved category drop down box.");

				//Enter each of the Valid Urls into the box and assert the save button enables
				foreach (var url in ValidUrls)
				{
					_page.InputCompanyWebsiteUrl.SendKeys(url, true);
					Log($"Entered {url} into the company website url input box.");

					Assert.IsTrue(_page.ButtonSave.IsDisplayed());
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByValidProductWebsiteUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields except for Company Website Url
				_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
				Log($"Entered {_updatedCompanyName} into the company name input box.");

				_page.InputCompanyWebsiteUrl.SendKeys(_updatedCompanyWebsiteUrl, true);
				Log($"Entered {_updatedCompanyWebsiteUrl} into the company website url input box.");

				_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
				Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

				//Country is a little different because you need to click into it first
				_page.SelectCountry.Click();
				BasePage.GetCountryOptionByName(UpdatedCountry).ClickAndWaitForPageToLoad();
				Log($"Selected {UpdatedCountry} from the country drop down.");

				_page.InputFilterProductName.SendKeys(_updatedProductName, true);
				Log($"Entered {_updatedProductName} into the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				_page.InputProductShortDescription.SendKeys(_updatedProductShortDescription, true);
				Log($"Entered {_updatedProductShortDescription} into the product short description input box.");

				//Approved category will work similarly to country
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Log($"Selected {approvedCategory.Name} from the approved category drop down box.");

				//Enter each of the Valid Urls into the box and assert the save button enables
				foreach (var url in ValidUrls)
				{
					_page.InputProductWebsiteUrl.SendKeys(url, true);
					Log($"Entered {url} into the product website url input box.");

					Assert.IsTrue(_page.ButtonSave.IsDisplayed());
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		[Ignore("Ignore test due to environment issue")]
		public void EditListingRequest_ByNewToUnderReviewStatus_Succeeds()
		{
			const string expectedUnderReviewText = "Under Review";
			ExecuteTimedTest(() =>
			{
				//Setup
				var (_, approvedCategory) = SetUpDataAndPage(editListingRequest: false, isCreateVendor: true);

				//Change the status from New to Under Review
				_page.SelectListingRequestStatus.Click();
				Log("Clicked into the listing request status edit box.");
				_page.SelectUnderReviewStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the Under Review status option.");

				//Go back to the listing request table
				_page.ButtonGoBack.ClickAndWaitForPageToLoad();
				Log("Clicked the back arrow on the page.");

				//Assert the latest-created request now has a status of under review
				Assert.AreEqual(expectedUnderReviewText, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByUnderReviewToNewStatus_Succeeds()
		{
			const string expectedNewText = "New";
			ExecuteTimedTest(() =>
			{
				//Setup
				var (_, approvedCategory) = SetUpDataAndPage(editListingRequest: false);

				//Change the status from New to Under Review
				_page.SelectListingRequestStatus.Click();
				Log("Clicked into the listing request status edit box.");
				_page.SelectUnderReviewStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the Under Review status option.");

				//Change the status from Under Review to New
				_page.SelectListingRequestStatus.Click();
				Log("Clicked into the listing request status edit box.");
				_page.SelectNewStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the New status option.");

				//Go back to the listing request table
				_page.ButtonGoBack.ClickAndWaitForPageToLoad();
				Log("Clicked the back arrow on the page.");

				//Assert the latest-created request now has a status of under review
				Assert.AreEqual(expectedNewText, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByExistingVendorAddProduct_Succeeds()
		{
			var updatedApprovedCategoryName = RequestUtility.GetRandomString(13);
			var vendorName = RequestUtility.GetRandomString(10);
			ExecuteTimedTest(() =>
			{
				//Setup
				OpenPage(_page);
				var categoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = updatedApprovedCategoryName }).Result.CategoryId.ToString();
				var vendor = VendorAdminApiService.PostVendor(new VendorInsertRequest
				{
					Name = vendorName,
					WebsiteUrl = $"https://www.test.com/{vendorName}",
					Address = new AddressRequest { CountryId = 237 }
				});
				var vendorIntegrationId = vendor.Result.VendorIntegrationId;

				var postRequest = new ListingRequestInsertRequest
				{
					SugarUserId = Guid.NewGuid(),
					VendorId = vendorIntegrationId,
					TargetSourceSiteVendorId = vendorIntegrationId.ToString(),
					TargetSourceSiteVendorSiteCode = nameof(SiteType.Capterra),
					CompanyCountryCode = "US",
					SourceTypeId = (int)ListingRequestSourceType.SugarUser,
					ProductName = _updatedProductName,
					ProductProposedCategoryName = RequestUtility.GetRandomString(9),
					CreatedByEventId = Guid.NewGuid(),
					SugarUserFirstName = _sugarUserFirstName,
					SugarUserLastName = _sugarUserLastName,
					SugarUserEmail = _sugarUserEmail
				};
				var listingRequest = MessageApiService.PostListingRequest(postRequest).Result;
				Log("Listing request insert complete");

				//Navigate to the new listing request
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString(), 15000);

				//Click edit
				_page.ButtonEditListingRequest.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked on the edit button");

				//Assert that the enabled fields can be interacted with
				_page.InputFilterProductName.SendKeys(_updatedProductName);
				Log($"Typed {_updatedProductName} into the product name input box");
				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl);
				Log($"Typed {_updatedProductWebsiteUrl} into the product website url input box");
				_page.InputProductShortDescription.SendKeys(_updatedProductShortDescription);
				Log($"Typed {_updatedProductShortDescription}");
				_page.SelectApprovedCategory.Click();
				_page.SearchApprovedCategory.SendKeys(updatedApprovedCategoryName, sendEscape: false);
				Log($"Typed {updatedApprovedCategoryName} into the approved category input box");
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(updatedApprovedCategoryName.ToLower()).Click();

				//Click the save button
				Thread.Sleep(1000);
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				

				//Click on the name of the vendor
				_page.ListingRequestCompanyName.ClickAndWaitForPageToLoad();

				//Assert the name of the vendor is correct
				Assert.AreEqual(vendorName, _vendorPage.TitleVendorName.GetText());

				//Cleanup
				VendorAdminApiService.DeleteVendor(vendor.Result.VendorId);
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditListingRequest_ByInvalidProductShortDescription_Fails()
		{
			var invalidProductShortDescription = RequestUtility.GetRandomString(136);
			const string expectedErrorMessage = "Please edit the short description to be 135 characters or less";
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields
				_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
				Log($"Entered {_updatedCompanyName} into the company name input box.");

				_page.InputCompanyWebsiteUrl.SendKeys(_updatedCompanyWebsiteUrl, true);
				Log($"Entered {_updatedCompanyWebsiteUrl} into the company website url input box.");

				_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
				Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

				//Country is a little different because you need to click into it first
				_page.SelectCountry.Click();
				BasePage.GetCountryOptionByName(UpdatedCountry).ClickAndWaitForPageToLoad();
				Log($"Selected {UpdatedCountry} from the country drop down.");

				_page.InputFilterProductName.SendKeys(_updatedProductName, true);
				Log($"Entered {_updatedProductName} into the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				_page.InputProductShortDescription.SendKeys(invalidProductShortDescription, true);
				Log($"Entered {invalidProductShortDescription} into the product short description input box.");

				//Approved category will work similarly to country
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Log($"Selected {approvedCategory.Name} from the approved category drop down box.");

				//Assert that the save button is disabled and an error is present for the product short description
				Assert.IsFalse(_page.ButtonSave.IsDisplayed());
				Assert.AreEqual(expectedErrorMessage, _page.ErrorLengthProductShortDescription.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByInvalidCompanyWebsiteUrl_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields except for Company Website Url
				_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
				Log($"Entered {_updatedCompanyName} into the company name input box.");

				_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
				Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

				//Country is a little different because you need to click into it first
				_page.SelectCountry.Click();
				BasePage.GetCountryOptionByName(UpdatedCountry).ClickAndWaitForPageToLoad();
				Log($"Selected {UpdatedCountry} from the country drop down.");

				_page.InputFilterProductName.SendKeys(_updatedProductName, true);
				Log($"Entered {_updatedProductName} into the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				//Approved category will work similarly to country
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Log($"Selected {approvedCategory.Name} from the approved category drop down box.");

				//Enter each of the Invalid Urls into the box and assert the correct error
				foreach (var url in InvalidUrls)
				{
					_page.InputCompanyWebsiteUrl.SendKeys(url, true);
					Log($"Entered {url} into the company website url input box.");

					Assert.IsFalse(_page.ButtonSave.IsDisplayed());
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByInvalidProductWebsiteUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields except for Company Website Url
				_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
				Log($"Entered {_updatedCompanyName} into the company name input box.");

				_page.InputCompanyWebsiteUrl.SendKeys(_updatedCompanyWebsiteUrl, true);
				Log($"Entered {_updatedCompanyWebsiteUrl} into the company website url input box.");

				_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
				Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

				//Country is a little different because you need to click into it first
				_page.SelectCountry.Click();
				BasePage.GetCountryOptionByName(UpdatedCountry).ClickAndWaitForPageToLoad();
				Log($"Selected {UpdatedCountry} from the country drop down.");

				_page.InputFilterProductName.SendKeys(_updatedProductName, true);
				Log($"Entered {_updatedProductName} into the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
				Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

				_page.InputProductShortDescription.SendKeys(_updatedProductShortDescription, true);
				Log($"Entered {_updatedProductShortDescription} into the product short description input box.");

				//Approved category will work similarly to country
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Log($"Selected {approvedCategory.Name} from the approved category drop down box.");

				//Enter each of the Invalid Urls into the box and assert the correct error
				foreach (var url in InvalidUrls)
				{
					_page.InputProductWebsiteUrl.SendKeys(url, true);
					Log($"Entered {url} into the product website url input box.");

					Assert.IsFalse(_page.ButtonSave.IsDisplayed());
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ByMissingRequiredFields_Succeeds()
		{
			const string expectedErrorCompanyName = "Company Name is required";
			const string expectedErrorCompanyWebsiteUrl = "Company Website is required";
			const string expectedErrorPhoneNumber = "Phone Number is required";
			const string expectedErrorProductName = "Product Name is required";
			const string expectedErrorProductWebsiteUrl = "Product Website is required";
			const string expectedErrorProductShortDescription = "Short Description is required";
			ExecuteTimedTest(() =>
			{
				//Setup
				SetUpDataAndPage(false);

				//Clear out the input from all required fields
				_page.InputCompanyName.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the company name input box.");

				_page.InputCompanyWebsiteUrl.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the company website url input box.");

				_page.InputCompanyPhoneNumber.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the company phone number input box.");

				_page.InputFilterProductName.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the product name input box.");

				_page.InputProductWebsiteUrl.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the product website url input box.");

				_page.InputProductShortDescription.SendKeys(clear: true, sendEscape: false);
				Log("Cleared out the product short description input box.");

				//Assert that errors are present under all required fields
				Assert.AreEqual(expectedErrorCompanyName, _page.ErrorRequiredCompanyName.GetText());
				Assert.AreEqual(expectedErrorCompanyWebsiteUrl, _page.ErrorRequiredCompanyWebsiteUrl.GetText());
				Assert.AreEqual(expectedErrorPhoneNumber, _page.ErrorRequiredPhoneNumber.GetText());
				Assert.AreEqual(expectedErrorProductName, _page.ErrorRequiredProductName.GetText());
				Assert.AreEqual(expectedErrorProductWebsiteUrl, _page.ErrorRequiredProductWebsiteUrl.GetText());
				Assert.AreEqual(expectedErrorProductShortDescription, _page.ErrorRequiredProductShortDescription.GetText());
			});
		}

		[Test]
		public void EditListingRequest_ChangeStatus_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (_, approvedCategory) = SetUpDataAndPage();

				//Assert the status in not editable
				Assert.IsTrue(_page.SelectListingRequestStatus.IsDisabled());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ApprovedCategory_DisplayNoCategorySelectedMessage()
		{
			ExecuteTimedTest(() =>
			{
				//Create listing request and navigate to detail page (edit mode)
				SetUpDataAndPage(false);

				//Validate
				Assert.IsTrue(_page.CategoryNoSelectedToDisplayCoreFeature.GetText().Contains("No Approved Category Selected"));
				Assert.IsNull(_page.CategoryPublishedOn.GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.GetSelectedProposedCategoryCoreFeaturesRowCount(), 0);

				//Clean Up - Can't delete listing request yet :(
			});
		}

		[Test]
		public void EditListingRequest_ApprovedCategory_DisplayCoreFeatures()
		{
			ExecuteTimedTest(() =>
			{
				//Create listing request and navigate to detail page (edit mode)
				var (_, approvedCategory) = SetUpDataAndPage();

				//Select Category with Core Features
				_page.SelectApprovedCategory.Click();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, true, false);
				Thread.Sleep(1000);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).Click();

				//Wait for table to load
				Thread.Sleep(2000);

				//Validate
				Assert.IsNotNull(_page.CategoryPublishedOn);
				Assert.Greater(Model.Pages.NewListingRequestPage.GetSelectedProposedCategoryCoreFeaturesRowCount(), 0);

				//TODO: Clean Up - Can't delete listing request yet :(
			});
		}

		[Test]
		public void EditListingRequest_ApprovedCategory_DisplayMessageNoResultCoreFeatures()
		{
			ExecuteTimedTest(() =>
			{
				//Create listing request and navigate to detail page (edit mode)
				var (_, approvedCategory) = SetUpDataAndPage(setUpCategoryFeature: false);

				//Get new category name
				var categoryName = approvedCategory.Name;

				//Select Category with Core Features
				_page.SelectApprovedCategory.Click();
				_page.SearchApprovedCategory.SendKeys(categoryName, true, false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(categoryName).Click();

				//Wait for table to load
				Thread.Sleep(2000);

				//Validate
				Assert.IsTrue(_page.CategoryCoreFeatureNoResult.GetText().Contains("Core Features have not been configured for this category"));
				Assert.IsNotNull(_page.CategoryPublishedOn);
				Assert.AreEqual(Model.Pages.NewListingRequestPage.GetSelectedProposedCategoryCoreFeaturesRowCount(), 0);

				//TODO: Clean Up - Can't delete listing request yet :(
			});
		}

		[Test]
		public void ApproveListingRequest_NonExistingVendorAndNonExistingProduct_ValidateConfirmationDialog_Succeeds()
		{
			const string expectedApproveListingTitle = "Approve New Listing?";
			const string expectedApproveListingMessage = "Clicking 'Approve Listing' will create a new Vendor and Product.";
			const string expectedApproveListingButtonText = "APPROVE LISTING";
			ExecuteTimedTest(() =>
			{
				//Setup
				var approvedCategory = SetUpDataAndPage().ApprovedCategory;

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Assert the dialog title, message, and the approve button text
				Assert.AreEqual(expectedApproveListingTitle, _page.ConfirmationDialogTitle.GetText());
				Assert.AreEqual(expectedApproveListingMessage, _page.ConfirmationDialogText.GetText());
				Assert.AreEqual(expectedApproveListingButtonText, _page.ButtonConfirmationDialogAction.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				//TODO: delete listing request once listing request DELETE endpoint exists
			});
		}

		[Test]
		public void ApproveListingRequest_ExistingVendorAndNonExistingProduct_ValidateConfirmationDialog_Succeeds()
		{
			const string expectedApproveListingTitle = "Approve New Listing?";
			const string expectedApproveListingButtonText = "APPROVE LISTING";
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(isCreateVendor: true);
				var expectedApproveListingMessage = $"Clicking 'Approve Listing' will create a new product under the vendor {listingRequest.CompanyName}.";

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Assert the dialog title, message, and the approve button text
				Assert.AreEqual(expectedApproveListingTitle, _page.ConfirmationDialogTitle.GetText());
				Assert.AreEqual(expectedApproveListingMessage, _page.ConfirmationDialogText.GetText());
				Assert.AreEqual(expectedApproveListingButtonText, _page.ButtonConfirmationDialogAction.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				//TODO: delete listing request once listing request DELETE endpoint exists
			});
		}

		[Test]
		public void ApproveListingRequest_ByValidFields_Succeeds()
		{
			const string expectedListingStatus = nameof(ListingRequestStatusType.Approved);
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();
				var listingRequestId = listingRequest.ListingRequestId;

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(2500);
				
				//Navigate back to the newly-approved request and assert it shows the approved status
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequestId.ToString(), 15000);
				Assert.AreEqual(expectedListingStatus, _page.ListingStatusDisplay.GetText());

				//Retrieve the new vendorId and productId values and validate that they were both created
				var listingRequestGetResponse = GetListingRequest(listingRequestId.ToString());
				var vendor = GetVendor(listingRequestGetResponse.VendorId.ToString());
				Assert.AreEqual(listingRequestGetResponse.ProductName, GetProductByPartialName(listingRequestGetResponse.ProductName).Name);
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				ProductAdminApiService.DeleteProduct(listingRequestGetResponse.ProductId.ToString());
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
				//TODO: delete listing request once listing request DELETE endpoint exists
			});
		}

		[Test]
		public void ApproveListingRequest_ByNonExistingVendorAndNonExistingProductNavigatesToVendor_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();
				var listingRequestId = listingRequest.ListingRequestId;

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(2500);

				//Retrieve the new vendorId and productId values
				var listingRequestGetResponse = GetListingRequest(listingRequestId.ToString());

				//Assert the page navigated to the newly created vendor
				var expectedUrl = new Uri(BrowserUtility.BaseUri, $"/vendors/{listingRequestGetResponse.VendorId}").ToString();
				Assert.AreEqual(expectedUrl, BrowserUtility.WebDriver.Url);
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				ProductAdminApiService.DeleteProduct(listingRequestGetResponse.ProductId.ToString());
				VendorAdminApiService.DeleteVendor(listingRequestGetResponse.VendorId.ToString());
				//TODO: delete listing request once listing request DELETE endpoint exists
			});
		}

		[Test]
		public void ApproveListingRequest_ByExistingVendorAndNonExistingProductNavigatesToVendor_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(isCreateVendor: true);
				var listingRequestId = listingRequest.ListingRequestId;

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(2500);

				//Retrieve the new vendorId and productId values
				var listingRequestGetResponse = GetListingRequest(listingRequestId.ToString());

				//Assert the page navigated to the newly created vendor
				var expectedUrl = new Uri(BrowserUtility.BaseUri, $"/vendors/{listingRequestGetResponse.VendorId}").ToString();
				Assert.AreEqual(expectedUrl, BrowserUtility.WebDriver.Url);
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				ProductAdminApiService.DeleteProduct(listingRequestGetResponse.ProductId.ToString());
				VendorAdminApiService.DeleteVendor(listingRequestGetResponse.VendorId.ToString());
				//TODO: delete listing request once listing request DELETE endpoint exists
			});
		}

		[Test]
		public void ApproveListingRequest_ByMissingRequiredFields_Fails()
		{
			const string expectedListingStatus = nameof(ListingRequestStatusType.Approved);
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(editListingRequest: false);
				var listingRequestId = listingRequest.ListingRequestId;

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Assert that the confirmation dialog did not appear due to missing required fields
				Assert.IsFalse(_page.ConfirmationDialogTitle.IsDisplayed());

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory, true);

				//Click the save button
				_page.ButtonSave.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(5000);

				//Navigate back to the newly-approved request and assert it shows the approved status
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequestId.ToString(), 15000);
				Assert.AreEqual(expectedListingStatus, _page.ListingStatusDisplay.GetText());

				//Retrieve the new vendorId and productId values and validate that they were both created
				var listingRequestGetResponse = GetListingRequest(listingRequestId.ToString());
				GetVendor(listingRequestGetResponse.VendorId.ToString());
				Assert.AreEqual(listingRequestGetResponse.ProductName, GetProductByPartialName(listingRequestGetResponse.ProductName).Name);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ApprovedCategoryPublishedOn_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();

				//Navigate to category page
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, approvedCategory.CategoryId.ToString());

				//Click Capterra publish button
				BasePage.GetSitePublishButtonBySite("capterra").ClickAndWaitForPageToLoad();
				//Click action button on confirm dialog
				_categoryDetailsPage.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Click GetApp publish button
				BasePage.GetSitePublishButtonBySite("getapp").ClickAndWaitForPageToLoad();
				//Click action button on confirm dialog
				_categoryDetailsPage.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Navigate back to listing request page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString());

				//Click Edit button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				//Select the Approved category
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();

				//Assert that the site icons are correctly displayed based on the site category's publish status
				Assert.That(_categoryDetailsPage.PublishedOnCapterraSiteIcon.GetImageSrc(), Does.Contain(BasePage.ColorIcon)); //published
				Assert.That(_categoryDetailsPage.PublishedOnGetAppSiteIcon.GetImageSrc(), Does.Contain(BasePage.ColorIcon)); //published
				Assert.That(_categoryDetailsPage.PublishedOnSoftwareAdviceSiteIcon.GetImageSrc(), Does.Contain(BasePage.GrayIcon)); //not published

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void EditListingRequest_WithProvidedVendorId_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//create a listing request using the same name/url from the vendor
				var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, false, true);

				//Navigate back to listing request page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString());
				Thread.Sleep(1000);

				//click the edit button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				//validate that the page does not show the duplicate vendor alert if VendorId is provided
				Assert.IsFalse(_page.DuplicateVendorsNoneDetected.IsDisplayed());

			});
		}

		[Test]
		public void ApproveListingRequest_CategoryWithCoreFeatures_VerifyCoreFeaturesAppearsSelected()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(editListingRequest: false);
				var listingRequestId = listingRequest.ListingRequestId;

				//Get category and core features information
				var getResponse = CategoryAdminApiService.GetCategoryFeaturesById(approvedCategory.CategoryId.ToString());
				var coreFeatureName = getResponse.Result.Find(x => x.FeatureTypeName.Equals("Core")).Name;

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory, true);

				//Click the save button
				_page.ButtonSave.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(3000);

				//Get Listing Request Information after Listing Request Approval
				var approvedListingRequest = VendorAdminApiService.GetListingRequestById(listingRequestId.ToString());

				//Navigate to the product detail page for test assertion
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, approvedListingRequest.ProductId.ToString());

				// Assert Verification Status for the product is set to Verified
				Assert.AreEqual(ProductCategoryVerificationType.Verified.ToString(), Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText());
				Log($"Verification Status:  {Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText()}");

				// Assert that core features appears selected 
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(approvedListingRequest.ProductId.Value);
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(coreFeatureName);
				Assert.IsTrue(checkbox.IsSelected());
				var selectedCoreFeature = productFeatures.Result.Single().Features.Exists(x => x.FeatureTypeId == 1 && x.IsSelected);
				Assert.IsTrue(selectedCoreFeature);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				ProductAdminApiService.DeleteProduct(approvedListingRequest.ProductId.ToString());
			});
		}

		[Test]
		public void ApproveListingRequest_CategoryWithNonCoreFeatures_VerifyNonCoreFeatureAppearsUnSelected()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(editListingRequest: false, setUpNonCoreCategoryFeature: true, setUpCategoryFeature: false);
				var listingRequestId = listingRequest.ListingRequestId;

				//Get category and core features information
				var getResponse = CategoryAdminApiService.GetCategoryFeaturesById(approvedCategory.CategoryId.ToString());
				var nonCoreFeatureName = getResponse.Result.Single().Name;

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory, true);

				//Click the save button
				_page.ButtonSave.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(3000);

				// Get Listing Request Information after Listing Request Approval
				var approvedListingRequest = VendorAdminApiService.GetListingRequestById(listingRequestId.ToString());

				//Navigate to the product detail page for test assertion 
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, approvedListingRequest.ProductId.ToString());

				// Assert that Verification Status for the Product is set to Verified
				Assert.AreEqual(ProductCategoryVerificationType.Verified.ToString(), Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText());
				Log($"Verification Status: {Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText()}");

				// Assert that the non core feature is not selected
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(approvedListingRequest.ProductId.Value);
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(nonCoreFeatureName);
				Assert.IsFalse(checkbox.IsSelected());
				var isNonCoreFeatureSelected = productFeatures.Result.Single().Features.Exists(x => x.FeatureTypeId == 2 && x.IsSelected);
				Assert.IsFalse(isNonCoreFeatureSelected);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
				ProductAdminApiService.DeleteProduct(approvedListingRequest.ProductId.ToString());
			});
		}

		[Test]
		public void ApproveListingRequest_CategoryWithNoFeatures_VerifyNoFeatureIsDisplayed()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage(editListingRequest: false, setUpCategory: true, setUpCategoryFeature: false);
				var listingRequestId = listingRequest.ListingRequestId;

				//Click the Approve Listing button
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");

				//Enter valid info into all required fields
				InputValidValuesForAllRequiredFields(approvedCategory, true);

				//Click the save button
				_page.ButtonSave.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//Click the Approve Listing button and confirm
				_page.ButtonApproveListing.Click();
				Log("Clicked the approve listing button");
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(3000);

				// Get Listing Request Information after Listing Request Approval
				var approvedListingRequest = VendorAdminApiService.GetListingRequestById(listingRequestId.ToString());

				//Navigate to product detail page for test assertion
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, approvedListingRequest.ProductId.ToString());

				// Assert that Verification Status for the Product is set to Verified
				Assert.AreEqual(ProductCategoryVerificationType.Verified.ToString(), Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText());
				Log($"Verification Status: {Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText()}");

				// Assert that the no product feature is displayed 
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(approvedListingRequest.ProductId.Value);
				var featuresCount = productFeatures.Result.Single().Features.Count;
				Assert.IsTrue(featuresCount == 0);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_ShortDescriptionValueTruncate_Succeed()
		{
			var companyName = RequestUtility.GetRandomString(10);
			var companyWebsiteUrl = $"http://www.{companyName }.com";
			var expectedShortDescriptionLength = 135;
						
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				// listing request message with 300 characters ProductShortDescription
				ListingRequestInsertRequest postRequest;
				var vendorIntegrationId = PostVendor(companyName, companyWebsiteUrl).VendorIntegrationId;
				postRequest = new ListingRequestInsertRequest
				{
					SugarUserId = Guid.NewGuid(),
					SourceTypeId = (int)ListingRequestSourceType.SugarUser,
					VendorId = vendorIntegrationId,
					SugarUserEmail = $"{SocialMediaUtility.CompanyName}@company.com",
					SugarUserFirstName = RequestUtility.GetRandomString(8),
					SugarUserLastName = RequestUtility.GetRandomString(10),
					ProductName = RequestUtility.GetRandomString(10),
					ProductShortDescription = RequestUtility.GetRandomString(300),
					ProductProposedCategoryName = RequestUtility.GetRandomString(9),
					//SocialMedialUrls = socialMediaRequests,
					CreatedByEventId = Guid.NewGuid()
				};
				var result = MessageApiService.PostListingRequest(postRequest).Result;
			    Log("Listing request insert complete.");

				//Navigate to Listing Request detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, result.ListingRequestId.ToString(), 1500);

				//Click on the edit listing request button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				// Assert that the productshortdescription value displayed in the UI was truncated to 135 characters 
				Assert.AreEqual(expectedShortDescriptionLength, _page.InputProductShortDescription.GetTextValue().Length);
			});
		}

		[Test]
		public void EditListingRequest_ShortDescriptionWith135CharactersIsNotTruncated_Succeed()			
		{
			var companyName = RequestUtility.GetRandomString(10);
			var companyWebsiteUrl = $"http://www.{companyName }.com";
			var expectedShortDescriptionLength = 135;
						
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				// listing request message with 135 characters ProductShortDescription
				ListingRequestInsertRequest postRequest;
				var vendorIntegrationId = PostVendor(companyName, companyWebsiteUrl).VendorIntegrationId;
				postRequest = new ListingRequestInsertRequest
				{
					SugarUserId = Guid.NewGuid(),
					SourceTypeId = (int)ListingRequestSourceType.SugarUser,
					VendorId = vendorIntegrationId,
					SugarUserEmail = $"{SocialMediaUtility.CompanyName}@company.com",
					SugarUserFirstName = RequestUtility.GetRandomString(8),
					SugarUserLastName = RequestUtility.GetRandomString(10),
					ProductName = RequestUtility.GetRandomString(10),
					ProductShortDescription = RequestUtility.GetRandomString(135),
					ProductProposedCategoryName = RequestUtility.GetRandomString(9),
					//SocialMedialUrls = socialMediaRequests,
					CreatedByEventId = Guid.NewGuid()
				};
				var result = MessageApiService.PostListingRequest(postRequest).Result;
				Log("Listing request insert complete.");

				//Navigate to Listing Request detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, result.ListingRequestId.ToString(), 1500);

				//Click on the edit listing request button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				// Assert that the productshortdescription value displayed in the UI is exactly 135 characters 
				Assert.AreEqual(expectedShortDescriptionLength, _page.InputProductShortDescription.GetTextValue().Length);
			});
		}

		[Test]
		public void EditListingRequest_ShortDescriptionWithLessThan135CharactersIsNotTruncated_Succeed()
		{
			var companyName = RequestUtility.GetRandomString(10);
			var companyWebsiteUrl = $"http://www.{companyName }.com";
			var expectedShortDescriptionLength = 50;

			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				// listing request message with 135 characters ProductShortDescription
				ListingRequestInsertRequest postRequest;
				var vendorIntegrationId = PostVendor(companyName, companyWebsiteUrl).VendorIntegrationId;
				postRequest = new ListingRequestInsertRequest
				{
					SugarUserId = Guid.NewGuid(),
					SourceTypeId = (int)ListingRequestSourceType.SugarUser,
					VendorId = vendorIntegrationId,
					SugarUserEmail = $"{SocialMediaUtility.CompanyName}@company.com",
					SugarUserFirstName = RequestUtility.GetRandomString(8),
					SugarUserLastName = RequestUtility.GetRandomString(10),
					ProductName = RequestUtility.GetRandomString(10),
					ProductShortDescription = RequestUtility.GetRandomString(50),
					ProductProposedCategoryName = RequestUtility.GetRandomString(9),
					//SocialMedialUrls = socialMediaRequests,
					CreatedByEventId = Guid.NewGuid()
				};
				var result = MessageApiService.PostListingRequest(postRequest).Result;
				Log("Listing request insert complete.");

				//Navigate to Listing Request detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, result.ListingRequestId.ToString(), 1500);

				//Click on the edit listing request button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				// Assert that the productshortdescription value displayed in the UI is exactly 135 characters 
				Assert.AreEqual(expectedShortDescriptionLength, _page.InputProductShortDescription.GetTextValue().Length);
			});
		}


		[Test]
		public void EditListingRequest_ApprovedCategoryDefinition_Succeed()
		{
			var categoryDefinition = RequestUtility.GetRandomString(10);
			var productWebsiteUrl = $"http://www.{RequestUtility.GetRandomString(10)}.com";
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();

				//Navigate to category page
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, approvedCategory.CategoryId.ToString());

				// Click on Edit Category Details button and type a category definition
				_categoryDetailsPage.ButtonEditCategory.Click();
				_categoryDetailsPage.InputEditCategoryDefinition.SendKeys(categoryDefinition);
				_categoryDetailsPage.ButtonSaveCategoryChanges.Click();
				Thread.Sleep(3000);

				//Navigate back to listing request page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString());

				//Click Edit button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				//Type the product website
				_page.InputProductWebsiteUrl.SendKeys(productWebsiteUrl);
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Thread.Sleep(2000);

				//Select the Approved category
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Thread.Sleep(2000);

				//Assert that the Approved Category Definition is set in the Edit Listing Request page
				Assert.AreEqual(_page.ApprovedCategoryDefinition.GetText(), categoryDefinition);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(2000);

				//Assert that the Approved Category Definition is set in the Edit Listing Request page
				Assert.AreEqual(_page.ListingRequestApprovedCategoryDefinition.GetText(), categoryDefinition);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}

		[Test]
		public void EditListingRequest_CategoryDefinitionNotSet_Succeed()
		{
			var expectedCategoryDefinition = "(not set)";
			var productWebsiteUrl = $"http://www.{RequestUtility.GetRandomString(10)}.com";
			ExecuteTimedTest(() =>
			{
				//Setup
				var (listingRequest, approvedCategory) = SetUpDataAndPage();

				//Navigate to category page
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, approvedCategory.CategoryId.ToString());

				//Navigate back to listing request page
				BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString());

				//Click Edit button
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");

				//Type the product website
				_page.InputProductWebsiteUrl.SendKeys(productWebsiteUrl);
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Thread.Sleep(2000);

				//Select the Approved category
				_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
				_page.SearchApprovedCategory.SendKeys(approvedCategory.Name, sendEscape: false);
				Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(approvedCategory.Name).ClickAndWaitForPageToLoad();
				Thread.Sleep(2000);

				//Assert that Category Definition is not set in the Edit Listing Request page
				Assert.AreEqual(_page.ApprovedCategoryDefinition.GetText(), expectedCategoryDefinition);

				//Click the save button
				_page.ButtonSave.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(2000);

				// Assert that Category Definition is not set in the Listing Request page
				Assert.AreEqual(_page.ListingRequestApprovedCategoryDefinition.GetText(), expectedCategoryDefinition);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(approvedCategory.CategoryId.ToString());
			});
		}


		//TODO: once we have the tests for approving/denying a request, add tests to check for the status here

		private (Data.Dto.V1.MessageApi.ListingRequestDto ListingRequest, CategoryAdminDto ApprovedCategory) SetUpDataAndPage(bool setUpCategory = true, bool editListingRequest = true, bool setUpCategoryFeature = true, bool setUpNonCoreCategoryFeature = false, bool isCreateVendor = false)
		{
			//Open the page
			OpenPage(_page);

			var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, isCreateVendor: isCreateVendor);
			var approvedCategory = new CategoryAdminDto();

			//Setup a new category to be used as a new approved category in the update
			if (setUpCategory)
			{
				approvedCategory = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = _updatedApprovedCategoryName }).Result;
				if (setUpCategoryFeature)
				{
					var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
					{
						Name = RequestUtility.GetRandomString(10),
						Definition = RequestUtility.GetRandomString(10)
					})
						.Result;
					CategoryAdminApiService.PostCategoryFeature(approvedCategory.CategoryId.ToString(),
						new CategoryFeatureInsertRequest
						{
							FeatureId = feature.FeatureId,
							FeatureTypeId = FeatureType.Core
						});
				}

				if (setUpNonCoreCategoryFeature)
				{
					var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
					{
						Name = RequestUtility.GetRandomString(10),
						Definition = RequestUtility.GetRandomString(10)
					})
						.Result;
					CategoryAdminApiService.PostCategoryFeature(approvedCategory.CategoryId.ToString(),
						new CategoryFeatureInsertRequest
						{
							FeatureId = feature.FeatureId,
							FeatureTypeId = FeatureType.Common
						});
				}
			}

			//Navigate to Listing Request detail page
			BrowserUtility.NavigateToPage(BrowserUtility.ListingRequestPageName, listingRequest.ListingRequestId.ToString(), 1500);

			//click the edit button
			if (editListingRequest)
			{
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");
			}

			return (listingRequest, approvedCategory);
		}

		private void InputValidValuesForAllRequiredFields(CategoryAdminDto category, bool editListingRequest = false)
		{
			//click the edit button
			if (editListingRequest)
			{
				_page.ButtonEditListingRequest.ClickAndWaitForPageToLoad();
				Log("Clicked the edit listing request button");
			}

			//Enter valid info into all required fields
			_page.InputCompanyName.SendKeys(_updatedCompanyName, true);
			Log($"Entered {_updatedCompanyName} into the company name input box.");

			_page.InputCompanyWebsiteUrl.SendKeys(_updatedCompanyWebsiteUrl, true);
			Log($"Entered {_updatedCompanyWebsiteUrl} into the company website url input box.");

			_page.InputCompanyPhoneNumber.SendKeys(_updatedCompanyPhoneNumber, true);
			Log($"Entered {_updatedCompanyPhoneNumber} into the company phone number input box.");

			_page.InputFilterProductName.SendKeys(_updatedProductName, true);
			Log($"Entered {_updatedProductName} into the product name input box.");

			_page.InputProductWebsiteUrl.SendKeys(_updatedProductWebsiteUrl, true);
			Log($"Entered {_updatedProductWebsiteUrl} into the product website url input box.");

			_page.InputProductShortDescription.SendKeys(_updatedProductShortDescription, true);
			Log($"Entered {_updatedProductShortDescription} into the product short description input box.");

			//Approved category will work similarly to country
			_page.SelectApprovedCategory.ClickAndWaitForPageToLoad();
			_page.SearchApprovedCategory.SendKeys(category.Name, sendEscape: false);
			Model.Pages.NewListingRequestPage.GetApprovedCategoryOptionByName(category.Name).ClickAndWaitForPageToLoad();
			Log($"Selected {category.Name} from the approved category drop down box.");
		}

		private void AssertAllRequiredFields(Data.Dto.V1.MessageApi.ListingRequestDto listingRequest, CategoryAdminDto category)
		{
			Assert.AreEqual(_updatedCompanyName, _page.ListingRequestCompanyName.GetText());
			Assert.AreEqual(_updatedCompanyWebsiteUrl, _page.ListingRequestCompanyUrl.GetText());
			Assert.AreEqual($"{listingRequest.CompanyCity}, {listingRequest.CompanyStateProvinceRegionName}, {UpdatedCountry}",
				_page.ListingRequestAddress.GetText());
			Assert.AreEqual(_updatedCompanyPhoneNumber, _page.ListingRequestPhoneNumber.GetText());
			Assert.AreEqual(_updatedProductName, _page.ListingRequestProductName.GetText());
			Assert.AreEqual(_updatedProductShortDescription, _page.ListingRequestProductDesc.GetText());
			Assert.AreEqual(_updatedProductWebsiteUrl, _page.ListingRequestProductUrl.GetText());
			Assert.AreEqual(category.Name, _page.ListingRequestApprovedCategory.GetText());
		}
	}
}