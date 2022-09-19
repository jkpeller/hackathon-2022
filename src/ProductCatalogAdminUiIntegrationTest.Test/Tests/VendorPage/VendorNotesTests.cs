using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	[Category("Vendor")]
	public class VendorNotesTests : BaseTest
	{
		private Model.Pages.VendorPage _page;
		private string _vendorName;
		private string _vendorWebsiteUrl;

		private const string NotSetNotesText = "(Notes not yet set)";


		public VendorNotesTests() : base(nameof(VendorNotesTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.VendorPage();
			_page.OpenPage();
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
		}

		[Test]
		public void VendorNotes_ValidateEmptyNotes_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);
				
				//validate vendor notes on the page are not set
				Assert.AreEqual(NotSetNotesText, _page.Notes.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorNotes_ByValidNotes_Succeeds()
		{
			const string updatedNotes = "Vendor notes for testing";
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//enter notes
				_page.ButtonEditVendorNotes.Click();
				_page.TextAreaNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the vendor notes text area.");

				//click the save button
				_page.ButtonSaveVendorNotes.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-vendor-notes");


				//assert that the updated value is shown
				Assert.AreEqual(updatedNotes, _page.Notes.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorNotes_ByAboveMaximumNotesValue_Succeeds()
		{
			var updatedNotes = RequestUtility.GetRandomString(4001);
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//enter notes
				_page.ButtonEditVendorNotes.Click();
				_page.TextAreaNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the vendor notes text area.");

				//click the save button
				_page.ButtonSaveVendorNotes.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-vendor-notes");

				//assert that the notes has updated to only the first 4000 characters of the above maximum value notes
				Assert.AreEqual(updatedNotes.Substring(0, updatedNotes.Length - 1), _page.Notes.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorNotes_CancelEditNotes_Succeeds()
		{
			const string updatedNotes = "These notes are not what the vendor should have.";
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//enter notes
				_page.ButtonEditVendorNotes.Click();
				_page.TextAreaNotes.SendKeys(updatedNotes, true);
				Log($"Entered {updatedNotes} into the vendor notes text area.");

				//click the cancel button
				_page.ButtonCancelVendorNotes.Click();
				Log("Clicked the cancel button.");

				//validate vendor notes on the page ar not set
				Assert.AreEqual(NotSetNotesText, _page.Notes.GetText());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditVendorNotes_UnsavedChanges_DisplayDialog()
		{
			ExecuteTimedTest(() =>
			{
				var notes = "Unsaved changes";
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//enter notes
				_page.ButtonEditVendorNotes.Click();
				_page.TextAreaNotes.SendKeys(notes, true);
				Log($"Entered {notes} into the vendor notes text area.");

				//trying to go back
				_page.ButtonGoBack.Click();

				//validate unsaved changes dialog is displayed
				Assert.IsTrue(_page.UnsavedChangesDialogSupportingText.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});

		}
	}
}
