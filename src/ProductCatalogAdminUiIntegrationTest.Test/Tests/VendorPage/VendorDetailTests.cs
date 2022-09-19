using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	[Category("Vendor")]
	public class VendorDetailTests : BaseTest
	{
		private readonly Model.Pages.VendorPage _page;
		private readonly string _vendorName;
		private readonly string _vendorWebsiteUrl;

		public VendorDetailTests() : base(nameof(VendorDetailTests))
		{
			_page = new Model.Pages.VendorPage();
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
		}

		[Test]
		public void ValidateVendorDetailsPage_Succeeds()
		{
			const string linkedInUrl = "http://www.linkedin.com/test";
			const string twitterUrl = "http://www.twitter.com/test";
			const string facebookUrl = "http://www.facebook.com/test";
			const string youtubeUrl = "http://www.youtube.com/test";
			const string instagramUrl = "http://www.instagram.com/test";
			const string countryName = "United States of America";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Navigate to vendors detail page
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId.ToString());

				//Assert that all of the correct information is shown on screen
				Assert.IsNull(_page.DetailsVendorStatus.GetText());
				Assert.AreEqual(_vendorName, _page.DetailsVendorName.GetText());
				Assert.IsTrue(_page.DetailsVendorAddress.GetText().Contains(vendor.Address.City));
				Assert.IsTrue(_page.DetailsVendorAddress.GetText().Contains("Texas"));
				Assert.IsTrue(_page.DetailsVendorAddress.GetText().Contains(countryName));

				//Assert that all of the links on the page are pointing to the correct urls
				Assert.AreEqual(_vendorWebsiteUrl.ToLower(), _page.DetailsVendorWebsiteUrl.GetHref());
				Assert.AreEqual(linkedInUrl, _page.DetailsVendorLinkedinUrl.GetHref());
				Assert.AreEqual(twitterUrl, _page.DetailsVendorTwitterUrl.GetHref());
				Assert.AreEqual(facebookUrl, _page.DetailsVendorFacebookUrl.GetHref());
				Assert.AreEqual(youtubeUrl, _page.DetailsVendorYoutubeUrl.GetHref());
				Assert.AreEqual(instagramUrl, _page.DetailsVendorInstagramUrl.GetHref());
				Assert.AreEqual(vendor.YearFounded, _page.DetailsYearFounded.GetText());
				Assert.AreEqual(vendor.PhoneNumber, _page.DetailsPhoneNumber.GetText());
				Assert.AreEqual(vendor.About, _page.DetailsAbout.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void VendorDetailsPage_VendorStatusChangeToArchived_Succeeds()
		{
			string vendorStatus = "ARCHIVED";
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//Update vendor status to Archived
				_page.ButtonEditVendor.Click();
				_page.DetailsVendorSelectVendorStatus.Click();
				_page.DetailsVendorArchivedVendorStatus.Click();
				_page.ButtonSubmitVendorForm.Click();
				Log($"Vendor status was changed to Archived");

				//Assert Archived vendor status
				BrowserUtility.WaitForElementToAppear("vendor-status");
				Assert.AreEqual(vendorStatus, _page.DetailsVendorStatus.GetText(false));

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}
		

		[Test]
		public void VendorDetailsPage_VendorStatusChangeToActive_Succeeds()
		{
			string archivedVendorStatus = "Archived";
			string activeVendorStatus = "Active";
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//Update vendor status to Archived
				_page.ButtonEditVendor.Click();
				_page.DetailsVendorSelectVendorStatus.Click();
				_page.DetailsVendorArchivedVendorStatus.Click();
				_page.ButtonSubmitVendorForm.Click();
				BrowserUtility.WaitForElementToAppear("vendor-status");
				Log($"Vendor status was changed to Archived");

				//Assert Archived vendor status
				var getResponse = VendorAdminApiService.GetVendorById(vendor.VendorId);
				Assert.AreEqual(archivedVendorStatus, getResponse.VendorStatusTypeName);

				//Change vendor status to Active
				_page.ButtonEditVendor.Click();
				_page.DetailsVendorSelectVendorStatus.Click();
				_page.DetailsVendorActiveVendorStatus.Click();
				_page.ButtonSubmitVendorForm.Click();
				Log($"Vendor status was set to active");

				//Assert that Vendor Status is Active
				Thread.Sleep(1000);
				getResponse = VendorAdminApiService.GetVendorById(vendor.VendorId);
				Assert.AreEqual(activeVendorStatus, getResponse.VendorStatusTypeName);

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ValidateVendorProducts_ByNoProducts_Succeeds()
		{
			const string productsCardTitle = "Products";
			const string noProductsCreatedText = "No products created";
			const string clickAddButtonText = "Click the 'Add' button to create a product";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendorId = PostVendor(_vendorName, _vendorWebsiteUrl).VendorId;

				//Navigate to vendors detail page
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId);

				//Assert that the products card appears with the correct information with no products
				Assert.AreEqual(productsCardTitle, _page.VendorProductsCardTitle.GetText());
				Assert.IsTrue(_page.ButtonAddVendorProduct.IsDisplayed());
				Assert.IsTrue(_page.VendorProductsNoResult.GetText().Contains(noProductsCreatedText));
				Assert.IsTrue(_page.VendorProductsNoResult.GetText().Contains(clickAddButtonText));

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		//TODO: when we can add vendor-product relationship via API, add tests for table results

		[Test]
		public void EditVendorDetails_ByValidName_Succeeds()
		{
			var updatedName = RequestUtility.GetRandomString(10);
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Change the name of the vendor
				_page.InputVendorCompanyName.SendKeys(updatedName, true);
				Log($"Typed {updatedName} into the vendor company name input box.");

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					updatedName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					vendor.Address.StateProvinceRegionName,
					vendor.Address.City,
					vendor.Address.ZipPostalCode,
					vendor.Address.CountryName,
					vendor.YearFounded,
					vendor.PhoneNumber,
					vendor.About,
					_page);

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ValidateEditMode_Succeeds()
		{
			const string expectedEditTitle = "Edit Vendor Details";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Assert that the title has updated correctly
				var actualEditTitle = _page.EditVendorDetailsTitle.GetText();
				Assert.AreEqual(expectedEditTitle, actualEditTitle);
				Log($"Validated Edit Vendor title. Text: {actualEditTitle}");

				//Assert that the cancel and save buttons are present
				Assert.IsTrue(_page.ButtonCancelSubmitVendorForm.IsDisplayed());
				Assert.IsTrue(_page.ButtonSubmitVendorForm.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendor_ByValidSocialMediaUrls_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Assert that valid Urls do not show an error message for any of the fields
				foreach (var url in ValidUrls)
				{
					//Type in a url value from the valid urls list
					_page.InputVendorTwitter.SendKeys(url, true);
					Log($"Typed {url} into the Twitter Social Media Profiles input field");
					_page.InputVendorFacebook.SendKeys(url, true);
					Log($"Typed {url} into the Facebook Social Media Profiles input field");
					_page.InputVendorLinkedIn.SendKeys(url, true);
					Log($"Typed {url} into the LinkedIn Social Media Profiles input field");
					_page.InputVendorYouTube.SendKeys(url, true);
					Log($"Typed {url} into the YouTube Social Media Profiles input field");
					_page.InputVendorInstagram.SendKeys(url, true);
					Log($"Typed {url} into the Instagram Social Media Profiles input field");

					//Assert that the add vendor button is enabled
					Assert.IsTrue(_page.ButtonSubmitVendorForm.IsDisplayed());
				}

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendor_ByValidVendorWebsiteUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Assert valid Urls do not show an error message
				foreach (var url in ValidUrls)
				{
					//Type in a url value from the valid urls list
					_page.InputVendorCompanyWebsite.SendKeys(url, true);
					Log($"Typed {url} into the Company Website input field.");

					//Assert that the add vendor button is enabled
					Log($"Submit form IsDisplayed value for {url}");
					Assert.IsTrue(_page.ButtonSubmitVendorForm.IsDisplayed());
				}

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByValidInternationalAddress_Succeeds()
		{
			const string updatedCountryName = "Mexico";
			const string stateProvinceRegionName = "Mexico City";
			const string city = "Mexico";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Change the country and clear out the stateProvince
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(updatedCountryName, sendEscape: false);
				Log($"Typed {updatedCountryName} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(updatedCountryName).Click(false);
				Log($"Clicked on the option for {updatedCountryName}.");

				//Select international province
				_page.SelectInputStateProvince.Click();
				BasePage.GetStateProvinceByName(stateProvinceRegionName.ToLower()).Click();

				//Type international state
				_page.InputVendorCity.SendKeys(Keys.Control + "a");
				_page.InputVendorCity.SendKeys(Keys.Delete);
				_page.InputVendorCity.SendKeys(city);

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					_vendorName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					stateProvinceRegionName,
					city,
					vendor.Address.ZipPostalCode,
					updatedCountryName,
					vendor.YearFounded,
					vendor.PhoneNumber,
					vendor.About,
					_page);

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByUsaToCanadaAddress_Succeeds()
		{
			const string updatedCountryName = "Canada";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Change the country
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(updatedCountryName, sendEscape: false);
				Log($"Typed {updatedCountryName} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(updatedCountryName).Click(false);
				Log($"Clicked on the option for {updatedCountryName}.");

				//Select the first state province in the drop down
				_page.SelectStateProvince.Click();
				var updatedStateProvince = _page.OptionFirstStateProvince.GetText();
				_page.OptionFirstStateProvince.ClickAndWaitForPageToLoad();
				Log("Clicked on the first State Province option.");

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					_vendorName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					updatedStateProvince,
					vendor.Address.City,
					vendor.Address.ZipPostalCode,
					updatedCountryName,
					vendor.YearFounded,
					vendor.PhoneNumber,
					vendor.About,
					_page);

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByValidAdditionalFields_Succeeds()
		{
			const string updatedYearFounded = "2019";
			var updatedPhoneNumber = RequestUtility.GetRandomString(15);
			var updatedAbout = RequestUtility.GetRandomString(100);
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Edit the values of the Year Founded, Phone Number, and About fields
				_page.InputYearFounded.SendKeys(updatedYearFounded, true);
				Log($"Entered {updatedYearFounded} into the Year Founded input field.");
				_page.InputPhoneNumber.SendKeys(updatedPhoneNumber, true);
				Log($"Entered {updatedPhoneNumber} into the Phone Number input field.");
				_page.InputAbout.SendKeys(updatedAbout, true);
				Log($"Entered {updatedAbout} into the About input field.");

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					_vendorName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					vendor.Address.StateProvinceRegionName,
					vendor.Address.City,
					vendor.Address.ZipPostalCode,
					vendor.Address.CountryName,
					updatedYearFounded,
					updatedPhoneNumber,
					updatedAbout,
					_page);
				Assert.AreEqual(updatedYearFounded, _page.DetailsYearFounded.GetText(false));
				Assert.AreEqual(updatedPhoneNumber, _page.DetailsPhoneNumber.GetText(false));
				Assert.AreEqual(updatedAbout, _page.DetailsAbout.GetText(false));

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByNullAdditionalFields_Succeeds()
		{
			const string expectedEmptyText = "(not set)";
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Clear the values from the yearFounded, phoneNumber and about fields
				for (var i = 0; i < vendor.YearFounded.Length; i++)
				{
					_page.InputYearFounded.SendKeys(Keys.Backspace);
				}
				_page.InputYearFounded.SendKeys(Keys.Tab);
				Log("Cleared out the Year Founded input field.");

				for (var i = 0; i < vendor.PhoneNumber.Length; i++)
				{
					_page.InputPhoneNumber.SendKeys(Keys.Backspace);
				}
				_page.InputPhoneNumber.SendKeys(Keys.Tab);
				Log("Cleared out the Phone Number input field.");

				for (var i = 0; i < vendor.About.Length; i++)
				{
					_page.InputAbout.SendKeys(Keys.Backspace);
				}
				_page.InputAbout.SendKeys(Keys.Tab);
				Log("Cleared out the About input field.");

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					_vendorName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					vendor.Address.StateProvinceRegionName,
					vendor.Address.City,
					vendor.Address.ZipPostalCode,
					vendor.Address.CountryName,
					null,
					"",
					"",
					_page);
				Assert.IsTrue(_page.DetailsYearFounded.GetText().Contains(expectedEmptyText));
				Assert.IsTrue(_page.DetailsPhoneNumber.GetText().Contains(expectedEmptyText));
				Assert.IsTrue(_page.DetailsAbout.GetText().Contains(expectedEmptyText));

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByAboveMaximumAdditionalFields_Succeeds()
		{
			const string updatedYearFounded = "20001";
			var updatedPhoneNumber = RequestUtility.GetRandomString(101);
			var updatedAbout = RequestUtility.GetRandomString(501);
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Edit the values of the Year Founded, Phone Number, and About fields
				_page.InputYearFounded.SendKeys(updatedYearFounded, true);
				Log($"Entered {updatedYearFounded} into the Year Founded input field.");
				_page.InputPhoneNumber.SendKeys(updatedPhoneNumber, true);
				Log($"Entered {updatedPhoneNumber} into the Phone Number input field.");
				_page.InputAbout.SendKeys(updatedAbout, true);
				Log($"Entered {updatedAbout} into the About input field.");

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the vendor was updated both in the UI and API
				AssertVendorDetails(
					_vendorName,
					_vendorWebsiteUrl,
					vendor.Address.StreetAddress1,
					vendor.Address.StreetAddress2,
					vendor.Address.StateProvinceRegionName,
					vendor.Address.City,
					vendor.Address.ZipPostalCode,
					vendor.Address.CountryName,
					updatedYearFounded.Substring(0, 4),
					updatedPhoneNumber.Substring(0, 100),
					updatedAbout.Substring(0, 500),
					_page);
				Assert.AreEqual(updatedYearFounded.Substring(0, 4), _page.DetailsYearFounded.GetText());
				Assert.AreEqual(updatedPhoneNumber.Substring(0, 100), _page.DetailsPhoneNumber.GetText());
				Assert.AreEqual(updatedAbout.Substring(0, 500), _page.DetailsAbout.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorDetails_ByNoChanges_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button.");

				//Assert that the card returned to edit mode
				Assert.IsTrue(_page.ButtonEditVendor.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendor_ByInvalidSocialMediaUrls_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Assert that invalid Urls show an error message
				foreach (var url in InvalidUrls)
				{
					//Type in a url value from the invalid urls list for each social media field
					_page.InputVendorTwitter.SendKeys(url, true);
					Log($"Typed {url} into the Twitter Social Media Profiles input field");
					_page.InputVendorFacebook.SendKeys(url, true);
					Log($"Typed {url} into the Facebook Social Media Profiles input field");
					_page.InputVendorLinkedIn.SendKeys(url, true);
					Log($"Typed {url} into the LinkedIn Social Media Profiles input field");
					_page.InputVendorYouTube.SendKeys(url, true);
					Log($"Typed {url} into the YouTube Social Media Profiles input field");
					_page.InputVendorInstagram.SendKeys(url, true);
					Log($"Typed {url} into the Instagram Social Media Profiles input field");
					_page.ErrorMessageTwitterUrl.Click();

					//Assert that the error message is visible for each social media field
					Assert.IsTrue(_page.ErrorMessageTwitterUrl.IsDisplayed());
					Assert.IsTrue(_page.ErrorMessageFacebookUrl.IsDisplayed());
					Assert.IsTrue(_page.ErrorMessageLinkedInUrl.IsDisplayed());
					Assert.IsTrue(_page.ErrorMessageYoutubeUrl.IsDisplayed());
					Assert.IsTrue(_page.ErrorMessageInstagramUrl.IsDisplayed());
				}

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendor_ByInvalidVendorWebsiteUrl_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Assert that invalid Urls show an error message
				foreach (var url in InvalidUrls)
				{
					//Type in a new name for the product
					_page.InputVendorCompanyWebsite.SendKeys(url, true);
					_page.InputVendorCompanyWebsite.SendKeys(Keys.Tab);
					Log($"Typed {url} into the Company Website input field.");

					//Assert that the error message is visible for each social media field
					Assert.IsTrue(_page.ErrorMessageInvalidWebsiteUrl.IsDisplayed());
				}

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendor_MissingRequiredFields_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenEditVendorScreen(vendor.VendorId);

				//Clear the selection out of the name and website fields
				for (var i = 0; i < _vendorName.Length; i++)
				{
					_page.InputVendorCompanyName.SendKeys(Keys.Backspace);
				}
				_page.InputVendorCompanyName.SendKeys(Keys.Tab);
				Log("Cleared out the Company Website input field.");

				for (var i = 0; i < _vendorWebsiteUrl.Length; i++)
				{
					_page.InputVendorCompanyWebsite.SendKeys(Keys.Backspace);
				}
				_page.InputVendorCompanyWebsite.SendKeys(Keys.Tab);
				Log("Cleared out the Company Website input field.");
				Thread.Sleep(500);

				//Assert the error messages are displayed for each required field
				Assert.IsTrue(_page.ErrorMessageCompanyName.IsDisplayed());
				Assert.IsTrue(_page.ErrorMessageRequiredWebsiteUrl.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		private void OpenEditVendorScreen(string vendorId)
		{
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId, 3000);

			//Click to edit the vendor
			_page.ButtonEditVendor.ClickAndWaitForPageToLoad();
			Log("Clicked the edit vendor button.");
		}
	}
}