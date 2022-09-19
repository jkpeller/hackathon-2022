using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
	[TestFixture]
	[Category("ListingRequest")]
	public class ListingRequestNotesTests : BaseTest
	{
		private Model.Pages.NewListingRequestPage _page;
		private string _companyName;
		private string _companyWebsiteUrl;

		public ListingRequestNotesTests() : base(nameof(ListingRequestNotesTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.NewListingRequestPage();
			_companyName = RequestUtility.GetRandomString(12);
			_companyWebsiteUrl = $"https://www.{_companyName}.com/test";
		}

		[Test]
		public void ListingRequestNotes_ValidateEmptyNotes_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the page and then go to the page for the newly-created listing request
				_page.OpenPage();

				//go to the page for the newly-created listing request
				NavigateToListingRequest(result.ListingRequestId);
				Thread.Sleep(1000);

				//validate listing request notes on the page ar not set
				Assert.AreEqual("(Notes not yet set)", _page.ListingRequestNotes.GetText());
			});
		}

		[Test]
		public void EditListingRequestNotes_ByValidNotes_Succeeds()
		{
			const string updatedNotes = "Listing request notes for testing";
			ExecuteTimedTest(() =>
			{
				//setup
				SetUpDataAndPage();

				//enter notes
				_page.InputListingRequestNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the listing request notes text area.");

				//click the save button
				_page.ButtonSaveListingRequestNotes.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//assert that the updated values are shown on the listing request readonly page
				Assert.AreEqual(updatedNotes, _page.ListingRequestNotes.GetText());
			});
		}

		[Test]
		public void EditListingRequestNotes_ByAboveMaximumNotesValue_Succeeds()
		{
			var updatedNotes = RequestUtility.GetRandomString(1001);
			ExecuteTimedTest(() =>
			{
				//setup
				SetUpDataAndPage();

				//enter notes
				_page.InputListingRequestNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the listing request notes text area.");

				//click the save button
				_page.ButtonSaveListingRequestNotes.ClickAndWaitForPageToLoad();
				Log("Clicked the save button.");
				Thread.Sleep(1000);

				//assert that the name has updated to only the first 100 characters of the above maximum value name
				Assert.AreEqual(updatedNotes.Substring(0, updatedNotes.Length - 1), _page.ListingRequestNotes.GetText());
			});
		}

		[Test]
		public void EditListingRequestNotes_CancelEditNotes_Succeeds()
		{
			const string updatedNotes = "These notes are not what the listing request should have.";
			ExecuteTimedTest(() =>
			{
				//setup
				SetUpDataAndPage();

				//enter notes
				_page.InputListingRequestNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the listing request notes text area.");

				//click the cancel button
				_page.ButtonCancelListingRequestNotes.ClickAndWaitForPageToLoad();
				Log("Clicked the cancel button.");
				Thread.Sleep(1000);

				//validate listing request notes on the page ar not set
				Assert.AreEqual("(Notes not yet set)", _page.ListingRequestNotes.GetText());
			});
		}

		private void SetUpDataAndPage()
		{
			var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

			//open the page
			OpenPage(_page);

			NavigateToListingRequest(listingRequest.ListingRequestId);
			_page.ButtonEditListingRequestNotes.HoverOver();
			_page.ButtonEditListingRequestNotes.ClickAndWaitForPageToLoad();
			Log("Clicked the edit listing request notes button");
		}

		private static void NavigateToListingRequest(int listingRequestId)
		{
			//open the page for the newly-created listing request
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"{BrowserUtility.ListingRequestPageName}/{listingRequestId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
		}
	}
}
