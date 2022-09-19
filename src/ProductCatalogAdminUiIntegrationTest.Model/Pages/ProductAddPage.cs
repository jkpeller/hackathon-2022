using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class ProductAddPage : BasePage
	{
		#region Variables and constructor

		public ProductAddPage()
		{
		}

		#endregion

		#region Single Elements

		//add product
		public readonly Control InputProductName = new Control(ControlUtility.GetElementSelector("input-product-name"));
		public readonly Control InputProductWebsite = new Control(ControlUtility.GetElementSelector("input-product-website-url"));

		public readonly Control ButtonCancelSubmitProductForm = new Control(ControlUtility.GetElementSelector("button-cancel-edit"));
		public readonly Control ButtonSubmitProductForm = new Control(ControlUtility.GetElementSelector("button-save-changes"));

		public readonly Control ErrorMessageProductName = new Control(ControlUtility.GetElementSelector("mat-error-required-product-name"));
		public readonly Control ErrorMessageRequiredWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-required-product-url"));
		public readonly Control ErrorMessageInvalidWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-product-url"));
		
		//duplicate vendor product check
		public readonly Control DuplicateVendorProductSearchInProgress = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-search-in-progress"));
		public readonly Control DuplicateVendorProductDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-detected"));
		public readonly Control DuplicateVendorProductNoneDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-none"));

		#endregion
	}
}