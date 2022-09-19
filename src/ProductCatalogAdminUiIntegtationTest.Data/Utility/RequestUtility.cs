using System;
using System.Collections.Generic;
using System.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Utility
{
	public static class RequestUtility
	{
		private const string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		public const string SpecialCharacters = "!&$@#%^*()_-+=";
		private static readonly Random Random = new Random();
		public const string SiteCode = nameof(SiteType.Capterra);

		/// <summary>
		/// Returns a random Guid-based unique string ID (removes hyphens from guid)
		/// </summary>
		/// <returns>Unique alphanumeric identifier</returns>
		public static string GetUniqueId()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}

		/// <summary>
		/// Returns a random string of given length, using specified valid characters
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GetRandomString(int length)
		{
			return new string(Enumerable.Repeat(ValidCharacters, length)
				.Select(s => s[Random.Next(s.Length)]).ToArray());
		}

		#region Categories

		public static CategoryInsertRequest GetCategoryInsertRequest(string name)
		{
			return new CategoryInsertRequest
			{
				Name = name
			};
		}

		#endregion

		#region Site categories

		public static SourceSiteCategorySingleSaveRequest GetSourceSiteCategorySaveRequest(string categoryId)
		{
			return new SourceSiteCategorySingleSaveRequest
			{
				SiteCode = SiteCode,
				CategoryId = categoryId,
				Name = $"CategoryName_{categoryId}",
				IsPublished = true
			};
		}

		public static SiteCategoryToCategoryMapSaveRequest GetSiteCategoryToCategoryMapSaveRequest(int categoryId, int siteCategoryId)
		{
			return new SiteCategoryToCategoryMapSaveRequest
			{
				CategoryId = categoryId,
				SiteCategoryId = siteCategoryId
			};
		}

		#endregion

		#region Site products

		public static SourceSiteProductSaveRequest GetSourceSiteProductSaveRequest(string productId, bool isPublished = true, bool includeVendor = false, string vendorId = "")
		{
			var request = new SourceSiteProductSaveRequest
			{
				SiteCode = nameof(SiteType.Capterra),
				ProductId = productId,
				VendorId = includeVendor ? vendorId : "",
				VendorName = includeVendor ? $"Vendor {vendorId} Name" : "",
				Name = $"ProductName_{productId}",
				LongDescription = $"Long Description for Product_{productId}",
				LogoUrl = "http://www.blah.com/logo.jpg",
				ProductWebsiteUrl = "http://www.testsiteproduct.com/",
				IsPublished = isPublished,
				ListingType = SiteProductListingType.Free,
				PpcStatus = SiteProductListingStatusType.Inactive,
				PplStatus = SiteProductListingStatusType.Inactive,
				DeploymentOptions = new List<SiteProductDeploymentOptionType> { SiteProductDeploymentOptionType.NotDetermined },
				PaymentFrequencies = new List<SiteProductPaymentFrequencyType> { SiteProductPaymentFrequencyType.NotDetermined },
				PriceModels = new List<SiteProductPriceModelType> { SiteProductPriceModelType.NotDetermined }
			};

			return request;
		}

		public static SourceSiteProductSaveRequest GetMaxLengthSourceSiteProductSaveRequest(string productId, bool isPublished = true)
		{
			var request = new SourceSiteProductSaveRequest
			{
				SiteCode = nameof(SiteType.Capterra),
				ProductId = productId,
				Name = $"1234567890123456789012345678901234567890123456789_{productId}",
				LongDescription = $"Long Description for Product_{productId}",
				LogoUrl = "http://www.blah.com/logo.jpg",
				IsPublished = isPublished,
				ListingType = SiteProductListingType.Free,
				PpcStatus = SiteProductListingStatusType.Inactive,
				PplStatus = SiteProductListingStatusType.Inactive,
				DeploymentOptions = new List<SiteProductDeploymentOptionType> { SiteProductDeploymentOptionType.NotDetermined },
				PaymentFrequencies = new List<SiteProductPaymentFrequencyType> { SiteProductPaymentFrequencyType.NotDetermined },
				PriceModels = new List<SiteProductPriceModelType> { SiteProductPriceModelType.NotDetermined }
			};

			return request;
		}

		public static SiteProductToProductMapSaveRequest GetSiteProductToProductMapSaveRequest(int productId, int siteProductId)
		{
			return new SiteProductToProductMapSaveRequest
			{
				ProductId = productId,
				SiteProductId = siteProductId
			};
		}

		#endregion

		#region Vendors

		public static VendorInsertRequest GetVendorInsertRequest(string vendorName, string websiteUrl = "https://www.test.com/", int? countryId = 237,
			string linkedInUrl = null, string twitterUrl = null, string facebookUrl = null, string youtubeUrl = null, string instagramUrl = null,
			string streetAddress1 = null, string streetAddress2 = null, string city = null, int? stateProvinceId = null,
			string stateProvinceRegionName = null, string zipPostalCode = null, string yearFounded = null, string phoneNumber = null,
			string about = null)
		{
			var includeSocialMedias = !string.IsNullOrWhiteSpace(linkedInUrl)
			                          || !string.IsNullOrWhiteSpace(twitterUrl)
			                          || !string.IsNullOrWhiteSpace(facebookUrl)
			                          || !string.IsNullOrWhiteSpace(youtubeUrl)
			                          || !string.IsNullOrWhiteSpace(instagramUrl);
			return new VendorInsertRequest
			{
				Name = vendorName,
				WebsiteUrl = websiteUrl,
				YearFounded = yearFounded,
				PhoneNumber = phoneNumber,
				About = about,
				Address = GetVendorAddressRequest(countryId, streetAddress1, streetAddress2, city, stateProvinceId, stateProvinceRegionName, zipPostalCode),
				SocialMediaUrls = includeSocialMedias ? GetVendorSocialMediaRequests(linkedInUrl, twitterUrl, facebookUrl, youtubeUrl, instagramUrl) : null
			};
		}

		private static AddressRequest GetVendorAddressRequest(int? countryId, string streetAddress1,
			string streetAddress2, string city, int? stateProvinceId,
			string stateProvinceRegionName, string zipPostalCode)
		{
			return new AddressRequest
			{
				CountryId = countryId,
				StreetAddress1 = streetAddress1,
				StreetAddress2 = streetAddress2,
				City = city,
				StateProvinceId = stateProvinceId,
				StateProvinceRegionName = stateProvinceRegionName,
				ZipPostalCode = zipPostalCode
			};
		}

		private static List<SocialMediaRequest> GetVendorSocialMediaRequests(
			string linkedInUrl = null, string twitterUrl = null, string facebookUrl = null, string youtubeUrl = null,
			string instagramUrl = null)
		{
			return new List<SocialMediaRequest>
			{
				new SocialMediaRequest{ SocialMediaTypeId = SocialMediaType.LinkedIn, SocialMediaUrl = linkedInUrl },
				new SocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Twitter, SocialMediaUrl = twitterUrl },
				new SocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Facebook, SocialMediaUrl = facebookUrl },
				new SocialMediaRequest{ SocialMediaTypeId = SocialMediaType.YouTube, SocialMediaUrl = youtubeUrl },
				new SocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Instagram, SocialMediaUrl = instagramUrl }
			};
		}

		#endregion
	}
}
