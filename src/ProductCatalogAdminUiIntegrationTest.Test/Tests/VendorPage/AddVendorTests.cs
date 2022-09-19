using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	public class AddVendorTests : BaseTest
	{
		private Model.Pages.VendorPage _page;
		private string _vendorName;
		private string _vendorWebsiteUrl;
		private string _countryName;

		public AddVendorTests() : base(nameof(AddVendorTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.VendorPage();
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
			_countryName = "United States of America";
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByValidRequiredFields_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, _countryName.ToLower());

				//Click the add vendor button
				_page.ButtonCancelSubmitVendorForm.HoverOver();
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				//Assert the vendor was created and displayed information is correct
				AssertVendorInUi(_vendorName, _vendorWebsiteUrl, _page);

				var vendorId = VendorAdminApiService.SearchVendors(new VendorSearchRequest { TextFilter = _vendorName }).Result.Single().VendorId;

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByValidSocialMediaUrlFields_Succeeds()
		{
			var linkedInUrl = $"https://www.linkedin.com/{_vendorName}";
			var twitterUrl = $"https://www.twitter.com/{_vendorName}";
			var facebookUrl = $"https://www.facebook.com/{_vendorName}";
			var youtubeUrl = $"https://www.youtube.com/{_vendorName}";
			var instagramUrl = $"https://www.instagram.com/{_vendorName}";
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, _countryName.ToLower());

				//Type in valid values for all 5 social media url fields
				_page.InputVendorTwitter.SendKeys(twitterUrl);
				Log($"Typed {twitterUrl} into the Twitter Social Media Profiles input field");
				_page.InputVendorFacebook.SendKeys(facebookUrl);
				Log($"Typed {facebookUrl} into the Facebook Social Media Profiles input field");
				_page.InputVendorLinkedIn.SendKeys(linkedInUrl);
				Log($"Typed {linkedInUrl} into the LinkedIn Social Media Profiles input field");
				_page.InputVendorYouTube.SendKeys(youtubeUrl);
				Log($"Typed {youtubeUrl} into the YouTube Social Media Profiles input field");
				_page.InputVendorInstagram.SendKeys(instagramUrl);
				Log($"Typed {instagramUrl} into the Instagram Social Media Profiles input field");

				//Click the add vendor button
				_page.ButtonCancelSubmitVendorForm.HoverOver();
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				//Assert the vendor was created and displayed information is correct
				AssertVendorInUi(_vendorName, _vendorWebsiteUrl, _page);

				//Assert the social media urls are correct in the database using the api
				var vendorId = VendorAdminApiService.SearchVendors(new VendorSearchRequest { TextFilter = _vendorName }).Result.Single().VendorId;
				var getResponse = VendorAdminApiService.GetVendorById(vendorId);
				Assert.IsTrue(getResponse.SocialMediaUrls.Any(s => s.SocialMediaTypeName == SocialMediaType.Twitter.ToString() && s.SocialMediaUrl == twitterUrl));
				Assert.IsTrue(getResponse.SocialMediaUrls.Any(s => s.SocialMediaTypeName == SocialMediaType.Facebook.ToString() && s.SocialMediaUrl == facebookUrl));
				Assert.IsTrue(getResponse.SocialMediaUrls.Any(s => s.SocialMediaTypeName == SocialMediaType.LinkedIn.ToString() && s.SocialMediaUrl == linkedInUrl));
				Assert.IsTrue(getResponse.SocialMediaUrls.Any(s => s.SocialMediaTypeName == SocialMediaType.YouTube.ToString() && s.SocialMediaUrl == youtubeUrl));
				Assert.IsTrue(getResponse.SocialMediaUrls.Any(s => s.SocialMediaTypeName == SocialMediaType.Instagram.ToString() && s.SocialMediaUrl == instagramUrl));

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByVendorNameWithSpecialCharacters_Succeeds()
		{
			var vendorNameWithSpecialCharacters = $"{RequestUtility.GetRandomString(5)}{RequestUtility.SpecialCharacters}";
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(vendorNameWithSpecialCharacters, _vendorWebsiteUrl, _countryName.ToLower());

				//Click the add vendor button
				_page.ButtonCancelSubmitVendorForm.HoverOver();
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				//Assert the vendor was created and displayed information is correct
				AssertVendorInUi(vendorNameWithSpecialCharacters, _vendorWebsiteUrl, _page);

				var vendorId = GetVendorIdFromUrl();

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByValidVendorWebsiteUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type a valid value into the company name field
				_page.InputVendorCompanyName.SendKeys(_vendorName);
				Log($"Typed {_vendorName} into the Company Name input field.");

				//Select the country from the drop down
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(_countryName, sendEscape: false);
				Log($"Typed {_countryName} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(_countryName.ToLower()).Click();
				Log($"Clicked on the option for {_countryName}.");

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
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByValidSocialMediaUrls_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, _countryName.ToLower());

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
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_CancelAddVendor_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, _countryName.ToLower());

				//Click the add vendor button
				_page.ButtonCancelSubmitVendorForm.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the cancel add vendor button.");

				//Search for the new vendor by name and click the apply filters button
				_page.InputVendorName.SendKeys(_vendorName);
				Log($"Typed {_vendorName} into the Vendor Name filter box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the Apply Filters button.");

				//Assert the new vendor was not returned by the search
				Assert.IsTrue(_page.MessageTableNoResults.IsDisplayed());
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ValidateCountryAutocomplete_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type a valid value into the country field
				_page.SelectVendorCountry.ClickAndWaitForPageToLoad();
				Log("Clicked into the select Country box.");

				_page.InputCountrySearch.SendKeys(_countryName, sendEscape: false);
				Log($"Typed {_countryName} into the Country input field.");

				//Assert that an autocomplete suggestion is displayed for the input
				Assert.AreEqual(_countryName, BasePage.GetCountryOptionByName(_countryName).GetText());
			});
		}

		[Test]
		[Category("Vendor")]
		[TestCaseSource(typeof(AddVendorTestCases), nameof(AddVendorTestCases.AddVendorByValidAddress))]
		public void AddVendor_ByValidAddress_Succeeds(string streetAddress1, string streetAddress2, string city, string zipPostalCode, string countryName, bool isInternational)
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, countryName.ToLower());

				//Type valid values into all Address fields
				var stateProvince = InputOptionalAddressFieldsAndAdd(streetAddress1, streetAddress2, city, zipPostalCode, isInternational);

				//Assert the vendor was created and displayed information is correct
				AssertVendorInformation(_vendorName, _vendorWebsiteUrl, streetAddress1, streetAddress2, stateProvince, city, zipPostalCode, countryName, _page);

				var vendorId = GetVendorIdFromUrl();

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByAboveMaximumVendorName_Fails()
		{
			var aboveMaximumVendorName = RequestUtility.GetRandomString(101);
			var savedVendorName = aboveMaximumVendorName.Substring(0, 100);
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type valid values into all required fields
				InputRequiredFieldValues(aboveMaximumVendorName, _vendorWebsiteUrl, _countryName.ToLower());

				//Click the add vendor button
				_page.ButtonCancelSubmitVendorForm.HoverOver();
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");
				Thread.Sleep(5000);

				//Search for the new vendor by the first 50 characters of the name and click the apply filters button
				AssertVendorInUi(savedVendorName, _vendorWebsiteUrl, _page);

				var vendorId = GetVendorIdFromUrl();

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByInvalidVendorWebsiteUrl_Fails()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type a valid value into the company name field
				_page.InputVendorCompanyName.SendKeys(_vendorName);
				Log($"Typed {_vendorName} into the Company Name input field.");

				//Select the country from the drop down
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(_countryName, sendEscape: false);
				Log($"Typed {_countryName} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(_countryName.ToLower()).Click(false);
				Log($"Clicked on the option for {_countryName}.");

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
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByInvalidSocialMediaUrls_Fails()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

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
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_MissingRequiredFields_Fails()
		{
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Assert the add vendor button is not enabled
				Assert.IsFalse(_page.ButtonSubmitVendorForm.IsDisplayed());
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_ByNonExistingCountry_Fails()
		{
			var invalidCountryName = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
			{
				OpenAddVendorForm();

				//Type text into the country box for a non-existing country
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.Click();
				_page.InputCountrySearch.SendKeys(invalidCountryName, sendEscape: false);
				Log($"Typed {invalidCountryName} into the Country autocomplete box.");

				//Assert no countries appear as suggestions, just the No matches found message
				Assert.IsTrue(_page.SelectCountryNoMatches.IsDisplayed());
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_DuplicateCompanyUrlWarningMessage_Succeeds()
		{
			var vendorName = "";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);
				var vendorList = VendorAdminApiService.SearchVendors(new VendorSearchRequest { TextFilter = vendorName });
				var _vendorWebsiteUrl = vendorList.Result.ElementAt(new Random().Next(vendorList.Result.Count())).WebsiteUrl;

				_page.ButtonAddVendor.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				InputRequiredFieldValues(_vendorName, _vendorWebsiteUrl, _countryName.ToLower());

				//Assert that a duplicate company website url warning message is displayed
				Assert.IsTrue(_page.ErrorMessageDuplicateVendor.IsDisplayed());
				Log($"The duplicate company website warning message {_page.ErrorMessageDuplicateVendor.GetText()} was displayed on the page");

				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				//Assert the vendor was created and the displayed information is correct
				AssertVendorInUi(_vendorName, _vendorWebsiteUrl, _page);

				//Cleanup the vendor
				DeleteVendor(GetVendorIdFromUrl());
			});
		}

		[Test]
		[Category("Vendor")]
		public void AddVendor_VerifyNewVendorAfterAddressingDuplicateCompanyWarningMessage_Succeeds()
		{
			var vendorName = "";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);
				var vendorList = VendorAdminApiService.SearchVendors(new VendorSearchRequest { TextFilter = vendorName });
				var vendorWebsiteUrl = vendorList.Result.ElementAt(new Random().Next(vendorList.Result.Count())).WebsiteUrl;

				_page.ButtonAddVendor.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				InputRequiredFieldValues(_vendorName, vendorWebsiteUrl, _countryName.ToLower());

				//Assert that a duplicate company website url warning message is displayed
				Assert.IsTrue(_page.ErrorMessageDuplicateVendor.IsDisplayed());
				Log($"The duplicate company website warning message {_page.ErrorMessageDuplicateVendor.GetText()} was displayed on the page");

				//Type a valid value into the company website field

				_page.InputVendorCompanyWebsite.SendKeys(clear:true);
				_page.InputVendorCompanyWebsite.SendKeys(_vendorWebsiteUrl);
				Log($"Typed {vendorWebsiteUrl} into the Company Website input field.");
				Thread.Sleep(3000);

				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the add vendor button.");

				//Assert the vendor was created and the displayed information is correct
				AssertVendorInUi(_vendorName, _vendorWebsiteUrl, _page);

				//Cleanup the vendor
				DeleteVendor(GetVendorIdFromUrl());
			});
		}

		private static string GetVendorIdFromUrl()
		{
			var url = BrowserUtility.WebDriver.Url;
			return url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
		}

		private void OpenAddVendorForm()
		{
			//Open the page
			OpenPage(_page);

			//Click the add vendor button
			_page.ButtonAddVendor.ClickAndWaitForPageToLoad();
			Log("Clicked the add vendor button.");
		}

		private void InputRequiredFieldValues(string vendorName, string vendorWebsiteUrl, string countryName)
		{
			//Type a valid value into the company name field
			_page.InputVendorCompanyName.SendKeys(vendorName);
			Log($"Typed {vendorName} into the Company Name input field.");

			//Type a valid value into the company website field
			Thread.Sleep(3000);
			_page.InputVendorCompanyWebsite.SendKeys(vendorWebsiteUrl);
			Log($"Typed {vendorWebsiteUrl} into the Company Website input field.");

			//Select the country from the drop down
			Thread.Sleep(3000);
			_page.SelectVendorCountry.Click();
			Log("Clicked into the select Country box.");
			Thread.Sleep(3000);
			_page.InputCountrySearch.SendKeys(countryName, sendEscape: false);
			Log($"Typed {countryName} into the Country autocomplete box.");
			Thread.Sleep(3000);
			BasePage.GetCountryOptionByName(countryName).Click(false);
			Log($"Clicked on the option for {countryName}.");
		}

		private string InputOptionalAddressFieldsAndAdd(string streetAddress1, string streetAddress2, string city, string zipPostalCode, bool isInternational)
		{
			const string internationalStateProvince = "Mimami-ku";
			string stateProvince;
			//Type valid values into all Address fields
			_page.InputVendorAddress1.SendKeys(streetAddress1);
			Log($"Typed {streetAddress1} into the Street Address 1 input field");

			_page.InputVendorAddress2.SendKeys(streetAddress2);
			Log($"Typed {streetAddress2} into the Street Address 2 input field");

			_page.InputVendorCity.SendKeys(city);
			Log($"Typed {city} into the City input field");
			_page.InputVendorZipPostalCode.SendKeys(zipPostalCode);
			Log($"Typed {zipPostalCode} into the Zip/Postal Code input field");

			if (isInternational)
			{
				stateProvince = internationalStateProvince;
				_page.InputStateProvince.SendKeys(internationalStateProvince);
				Log($"Typed {internationalStateProvince} into the State/Province/Region input field");
			}
			else
			{
				_page.SelectStateProvince.Click();
				stateProvince = _page.OptionFirstStateProvince.GetText();
				_page.OptionFirstStateProvince.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked on the first State Province option.");
			}

			//Click the add vendor button
			_page.ButtonCancelSubmitVendorForm.HoverOver();
			_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
			Log("Clicked the add vendor button.");

			return stateProvince;
		}

		private static class AddVendorTestCases
		{
			public static IEnumerable<TestCaseData> AddVendorByValidAddress
			{
				get 
				{
					yield return new TestCaseData("200 Academy Drive", "Suite 120", "Austin", "47404", "United States of America", false)
					.SetName("AddVendorByValidUsaAddress")
					.SetCategory("Vendor");

					yield return new TestCaseData("200 Academy Drive", "Suite 120", "British Columbia", "VH6 3N1", "Canada", false)
						.SetName("AddVendorByValidCanadaAddress")
						.SetCategory("Vendor");

					yield return new TestCaseData("11-1 Hokotate-cho", "Kamitoba", "Mimami-ku", "Kyoto", "Japan", true)
						.SetName("AddVendorByValidInternationalAddress")
						.SetCategory("Vendor");

				}
			}
		}
	}
}