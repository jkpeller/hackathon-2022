using Newtonsoft.Json.Linq;
using NLog;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Logging;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi;
using ProductCatalogAdminUiIntegrationTest.Data.Service;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Pages;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ContentType = ProductCatalogAdminUiIntegrationTest.Data.Shared.ContentType;
using Control = ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore.Control;
using ProductUpdateRequest = ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi.ProductUpdateRequest;

namespace ProductCatalogAdminUiIntegrationTest.Test.Shared
{
	public abstract class BaseTest
	{
		//finds all dashes, example the "-" from "https://www.foo-bar.com"
		private static readonly Regex DashRegex = new Regex("[-]");
		//finds all IDs wrapped in parentheses, example the "(5671)" from "My Product Name (5671)"
		private static readonly Regex IdRegex = new Regex(@".+?(?=\((.*)\))");
		protected readonly BrandedResearchAdminApiService BrandedResearchAdminApiService;
		protected readonly CategoryAdminApiService CategoryAdminApiService;
		protected readonly FileManagementApiService FileManagementApiService;
		protected readonly IntegrationApiService IntegrationApiService;
		protected readonly MessageApiService MessageApiService;
		protected readonly MessagePublishService MessagePublishService;
		protected readonly ProductAdminApiService ProductAdminApiService;
		protected readonly VendorAdminApiService VendorAdminApiService;
		private readonly Logger _logger;
		private readonly LogService _logService;

		public const string ColorBlackRgba = "rgba(0, 0, 0, 1)";
		public const string ColorCssPropertyName = "color";
		public const string ActiveTabClassName = "mat-tab-label-active";
		protected const string SoftwareAdviceLogoFileName = "softwareAdviceLogo.jpg";
		protected const string SoftwareAdviceLogo50x50FileName = "softwareAdviceLogo50x50.jpg";
		protected const string CapterraLogoFileName = "capterraLogo.jpg";
		protected const string LargeFileName = "5MbImage.jpg";

		protected readonly List<string> ValidUrls = new List<string>
		{
			"http://foo.com/blah_blah",
			"http://foo.com/blah_blah/",
			"http://foo.com/blah_blah_(wikipedia)",
			"http://foo.com/blah_blah_(wikipedia)_(again)",
			"http://www.example.com/wpstyle/?p=364",
			"https://www.example.com/foo/?bar=baz&inga=42&quux",
			"http://142.42.1.1/",
			"http://142.42.1.1:8080/",
			"http://foo.com/blah_(wikipedia)#cite-1",
			"http://foo.com/blah_(wikipedia)_blah#cite-1",
			"http://foo.com/(something)?after=parens",
			"http://code.google.com/events/#&product=browser",
			"http://j.mp",
			"http://foo.bar/?q=Test%20URL-encoded%20stuff",
			"http://1337.net",
			"http://a.b-c.de",
			"http://223.255.255.254",
			"http://a.b--c.de/",
			"http://0.0.0.0",
			"http://10.1.1.0",
			"http://10.1.1.255",
			"http://224.1.1.1",
			"http://10.1.1.1",
			"http://-a.b.co",
			"http://a.b-.co",
			"http://-error-.valid/"
		};

		protected readonly List<string> InvalidUrls = new List<string>
		{
			"http://",
			"http://.",
			"http://..",
			"http://../",
			"http://?",
			"http://??",
			"http://??/",
			"http://#",
			"http://##",
			"http://##/",
			"http://foo.bar?q=Spaces should be encoded",
			"//",
			"//a",
			"///a",
			"///",
			"http:///a",
			"foo.com",
			"rdar://1234",
			"h://test",
			"http:// shouldfail.com",
			":// should fail",
			"http://foo.bar/foo(bar)baz quux",
			"ftps://foo.bar/",
			"http://.www.foo.bar/",
			"http://.www.foo.bar./",
			"http://123.123.123",
			"http://3628126748",
			"http://1.1.1.1.1"
		};

		protected readonly List<string> SocialMedias = new List<string>
		{
			"twitter",
			"facebook",
			"linkedin",
			"youtube",
			"instagram"
		};

		protected readonly List<string> MobileApps = new List<string>
		{
			"android",
			"ios"
		};


		protected readonly List<string> ScreenshotUploadFiles = new List<string> {
			"ImageFile1.jpg",
			"ImageFile2.jpg",
			"ImageFile3.jpg"
		};
		protected BaseTest(string loggerName)
		{
			_logger = LogManager.GetLogger(loggerName);
			_logService = new LogService(_logger);
			BrandedResearchAdminApiService = new BrandedResearchAdminApiService();
			CategoryAdminApiService = new CategoryAdminApiService(_logService);
			FileManagementApiService = new FileManagementApiService();
			IntegrationApiService = new IntegrationApiService();
			MessageApiService = new MessageApiService();
			MessagePublishService = new MessagePublishService();
			ProductAdminApiService = new ProductAdminApiService(_logService);
			VendorAdminApiService = new VendorAdminApiService();
		}

		protected void Log(string message)
		{
			_logService.Log(message);
		}

		private void Log(Exception ex)
		{
			_logService.Log(ex);
		}

		protected void ExecuteTimedTest(Action testAction)
		{
			try
			{
				var stopWatch = new Stopwatch();
				stopWatch.Start();

				Assert.Multiple(() =>
				{
					testAction();
					BrowserUtility.WebDriver.Quit();
					stopWatch.Stop();
					Log($"ExecuteTimedTest. Time: {stopWatch.ElapsedMilliseconds}");
				});
			}
			catch (Exception e)
			{
				Log(e);
				BrowserUtility.WebDriver.Quit();
				throw;
			}
		}

        private List<string> validateNotNullAndLog(string columnName, int rows = 10)
        {
			var cellTextValues = new List<string>();
			for (var i = 1; i <= rows; i++)
			{
				var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
				Log($"Validate that the value is not null. Row: {i}, Column: {columnName}, Text Value: {cellText}");
				cellTextValues.Add(cellText);
			}
			return cellTextValues;
		}

		#region Assertions

		protected void AssertColumnIsSorted(string columnName, SortDirectionType sortDirection = SortDirectionType.Ascending, int rows = 10)
		{
			//get cell text values from all Product Name cells in the 1st page of results
			var cellTextValues = validateNotNullAndLog(columnName, rows);

			//sort the list separately and compare the 2 to validate the sorting worked correctly
			var result = sortDirection == SortDirectionType.Ascending
				? cellTextValues.OrderBy(value => value)
				: cellTextValues.OrderByDescending(value => value);

			CollectionAssert.AreEqual(result, cellTextValues);
		}

		protected void AssertColumnDateTypeIsSorted(string columnName, SortDirectionType sortDirection = SortDirectionType.Ascending)
		{
			//get cell text values from all the date type column cells in the 1st page of results
			var cellTextValues = validateNotNullAndLog(columnName);

			//sort the list separately and compare the 2 to validate the sorting worked correctly
			var result = SortTimeAgoDateUtility.SortDateType(cellTextValues, sortDirection);

			CollectionAssert.AreEqual(result, cellTextValues);
		}

		protected void AssertNumericColumnIsSorted(string columnName, SortDirectionType sortDirection = SortDirectionType.Ascending)
		{
			//get cell text values from all Product Name cells in the 1st page of results
			var cellTextValues = validateNotNullAndLog(columnName);

			//sort the list separately and compare the 2 to validate the sorting worked correctly
			var result = sortDirection == SortDirectionType.Ascending
				? cellTextValues.OrderBy(int.Parse).ToList()
				: cellTextValues.OrderByDescending(int.Parse).ToList();

			CollectionAssert.AreEqual(result, cellTextValues);
		}

		protected void AssertColumnIsSortedWithoutDashes(string columnName, SortDirectionType sortDirection = SortDirectionType.Ascending)
		{
			//setup list of tuples to hold the value pairs
			var valueList = new List<Tuple<string, string>>();

			for (var i = 1; i <= 10; i++)
			{
				//get cell text, removing the ([ItemId]) where necessary
				var cellText = IdRegex.Match(BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText()).ToString();
				//use string.Replace to get rid of the dashes
				var cellTextWithoutDashes = DashRegex.Replace(cellText, string.Empty);
				Log($"Validate that the value is not null. Row: {i}, Original Value: {cellText}, Value Without Dashes: {cellTextWithoutDashes}");
				//add the values to the valueList
				valueList.Add(new Tuple<string, string>(cellText, cellTextWithoutDashes));
			}

			//sort the list appropriately depending on the sortDirection (same as AssertColumnIsSorted())
			var sortedValueList = sortDirection == SortDirectionType.Ascending
				? valueList.OrderBy(t => t.Item2).ToList()
				: valueList.OrderByDescending(t => t.Item2).ToList();

			//assert that the 2 tuple lists are equal
			CollectionAssert.AreEqual(valueList, sortedValueList);
		}

		protected void AssertThumbnailHeightAndWidth(Control image)
		{
			const string defaultHeightAndWidth = "200";
			Assert.AreEqual(defaultHeightAndWidth, image.GetImageSrcHeightQueryParamValue());
			Assert.AreEqual(defaultHeightAndWidth, image.GetImageSrcWidthQueryParamValue());
		}

		#endregion

		#region Categories

		protected void DeleteCategory(string categoryId)
		{
			CategoryAdminApiService.DeleteCategory(categoryId);
			Log($"Category deleted. CategoryId: {categoryId}");
		}

		protected (int, int, int) CreateCategoryProductRelationship(string productName, bool navigateToCategoryDetailsPage = true)
		{
			//Create Product
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
			{
				Name = productName,
				ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
			}).Result.ProductId;
			Log($"Product { productName } created.");

			var (categoryId, categoryName) = CreateCategory();

			//Assign Category to Product
			var productCategory = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
			{
				ProductId = productId,
				CategoryIds = new List<int> { categoryId }
			});
			Log($"{categoryName} is assigned to {productName}.");

			//Navigate to category Details page
			if (navigateToCategoryDetailsPage)
			{
				NavigateToCategoryDetailsPage(categoryId.ToString());
				Log($"href attribute of the product created {CategoryDetailsPage.GetLinkCategoryDetailsNameByRowNumber(1).GetHref()}");
			}
				
			return (categoryId, productId, productCategory.Result.Select(pc => pc.ProductCategoryId).SingleOrDefault());
		}

        protected (int, int) CreateCategoryProductWithCoreFeature(int _productId, string _categoryName)
		{
			//create category1
			var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest  { Name = _categoryName });
			var categoryName = category.Result.Name;
			var categoryId = category.Result.CategoryId;
		    Log($" Category { categoryName } created.");

			// Create Category Core Feature
			var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
			{
				Name = RequestUtility.GetRandomString(10),
				Definition = RequestUtility.GetRandomString(10)
			}).Result;

			CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
				new CategoryFeatureInsertRequest
				{
					FeatureId = feature.FeatureId,
					FeatureTypeId = FeatureType.Core
				});

			//Assign Category to Product
			var productCategory = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
			{
				ProductId = _productId,
				CategoryIds = new List<int> { categoryId }
			});
			Log($"{categoryName} is assigned to  {_productId}.");
			
			return (categoryId, productCategory.Result.Select(pc => pc.ProductCategoryId).SingleOrDefault());
		}



		protected (int, string) CreateCategory(string prefix = null)
		{
			//Create category
			var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = $"{prefix}Architecture{RequestUtility.GetRandomString(9)}" });
			var categoryName = category.Result.Name;
			var categoryId = category.Result.CategoryId;
			Log($" Category { categoryName } created.");

			return (categoryId, categoryName);
		}
        protected (int, string) CreateTag(TagType tagType)
        {
			//Create tag
			var tag = CategoryAdminApiService.PostTag(
                new TagInsertRequest
                {
                    Name = $"{RequestUtility.GetRandomString(9)}",
                    TagTypeId = tagType,
                    Definition = $"{RequestUtility.GetRandomString(9)}"
				});
            var tagName = tag.Result.Name;
            var tagId = tag.Result.TagId;
            Log($" Category { tagName } created.");

            return (tagId, tagName);
        }

		protected void NavigateToCategoryDetailsPage(string categoryId)
		{
			Log("Navigate to page for the new category");
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(2000);
			Log("Navigate to page for the new category");
		}

		#endregion

		#region Features

		protected string PostCategoryAndNavigateToDetailsPage(CategoriesPage categoriesPage, string categoryName)
		{
			//open the page
			OpenPage(categoriesPage);

			//setup data by creating a new category
			var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = categoryName });
			Log($"Category created. CategoryId: {categoryPostResponse.Result.CategoryId}, Response: {categoryPostResponse.ToJson()}");

			//navigate to the page for the new category
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryPostResponse.Result.CategoryId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(1000);

			return categoryPostResponse.Result.CategoryId.ToString();
		}

		protected int PostFeature(string featureName, string featureDefinition)
		{
			//create a new feature
			var featurePostResponse = CategoryAdminApiService.PostFeature(new FeatureInsertRequest { Name = featureName, Definition = featureDefinition });
			Log($"Feature created. FeatureId: {featurePostResponse.Result.FeatureId}, Response: {featurePostResponse.ToJson()}");

			return featurePostResponse.Result.FeatureId;
		}

		protected void SelectFeatureFromDropDown(CategoryDetailsPage categoryDetailsPage, string featureName)
		{
			categoryDetailsPage.InputFeatureAutocomplete.SendKeys(featureName, sendEscape: false);
			Log($"Typed '{featureName}' into the input feature autocomplete box");
			Thread.Sleep(1000);
			CategoryDetailsPage.GetFeatureAutocompleteResultByFeatureName(featureName).Click();
			Log($"Clicked on the autocomplete suggestion with the following text: '{featureName}'");
		}

		protected void SelectFeatureTypeFromDropDown(CategoryDetailsPage categoryDetailsPage, string featureTypeName)
		{
			categoryDetailsPage.SelectFeatureType.Click();
			Log("Clicked to open the select feature type drop down");
			CategoryDetailsPage.GetFeatureTypeDropdownOptionByTypeName(featureTypeName).ClickAndWaitForPageToLoad();
			Log($"Selected '{featureTypeName}' as the feature type");
		}

		protected static void AssertNewCategoryFeatureInUiCard(string featureName, string featureTypeName, string featureDefinition)
		{
			Assert.AreEqual(featureName, CategoryDetailsPage.GetFeatureTableFeatureNameByRowNumber(1).GetText());
			Assert.AreEqual(featureTypeName, CategoryDetailsPage.GetFeatureTableFeatureTypeByRowNumber(1).GetText());
			Assert.AreEqual(featureDefinition, CategoryDetailsPage.GetFeatureTableFeatureDefinitionByRowNumber(1).GetText());
		}

		protected void AssertNewCategoryFeatureViaApi(string categoryId, string featureTypeName, string featureDefinition, int? featureId)
		{
			var getResponse = CategoryAdminApiService.GetCategoryFeaturesById(categoryId);
			Log($"Category features retrieved. CategoryId: {categoryId}, Response: {getResponse.ToJson()}");
			Assert.AreEqual(featureTypeName, getResponse.Result.Single().FeatureTypeName);
			Assert.AreEqual(featureDefinition, getResponse.Result.Single().Definition);
			if (featureId != null)
			{
				Assert.AreEqual(featureId, getResponse.Result.Single().FeatureId);
			}
		}

		#endregion

		#region Open page and filters

		protected void OpenPage(BasePage page)
		{
			//open the page
			page.OpenPage();
			Log($"Open the page. Page: {page.GetType()}");
		}

		protected void FilterByAllFilters(BasePage page, string siteProductName)
		{
			//Filter on the site product name
			page.InputFilterProductName.SendKeys(siteProductName);

			//select all options from the mapping status filter box
			page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened.");
			page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			page.SelectOptionMappingStatusMapped.SendKeys(hoverOver: false);

			//select all options from the publish status filter box
			page.SelectPublishStatus.Click();
			Log("Publish status filter drop down clicked.");
			page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			page.SelectOptionPublishStatusUnpublished.SendKeys(hoverOver: false);

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
		}

		#endregion

		#region Products

		protected ProductInsertRequest GetProductInsertRequest(string name)
		{
			return new ProductInsertRequest
			{
				Name = name,
				ProductWebsiteUrl = $"http://www.product.upc.com/{name}"
			};
		}

		protected ProductAutocompleteDto GetProductByPartialName(string partialName)
		{
			var getResponse = ProductAdminApiService.GetProductsByPartialName(partialName);
			Log($"Product retrieved successfully. ProductId: {partialName}");

			return getResponse.Result.Single();
		}

		protected (string ProductId, string ProductFileId) PostProduct(string productName, string productWebsiteUrl, bool includeLogo = false, int? vendorId = null)
		{
			//Setup data by creating a new product
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName, ProductWebsiteUrl = productWebsiteUrl, VendorId = vendorId }).Result.ProductId.ToString();

			//Setup logo if needed
			string productFileId = null;
			if (includeLogo)
			{
				productFileId = ProductAdminApiService.PostProductLogo(productId, new FileRequest
				{
					Content = "VGVzdCBsb2dv",
					Extension = "png"
				})
					.Result
					.ProductFileId
					.ToString();
			}

			return (productId, productFileId);
		}

		#endregion

		#region Vendors

		protected VendorDto PostVendor(string vendorName, string vendorWebsiteUrl)
		{
			//Setup a new vendor
			var postRequest = RequestUtility.GetVendorInsertRequest(
				vendorName,
				vendorWebsiteUrl,
				linkedInUrl: "http://www.linkedin.com/test",
				twitterUrl: "http://www.twitter.com/test",
				facebookUrl: "http://www.facebook.com/test",
				youtubeUrl: "http://www.youtube.com/test",
				instagramUrl: "http://www.instagram.com/test",
				streetAddress1: "200 Academy Drive",
				streetAddress2: "Suite 120",
				city: "Austin",
				zipPostalCode: "47404",
				stateProvinceId: 43,
				yearFounded: "1993",
				phoneNumber: "(890) 123-4567",
				about: "This is the vendor about section.");

			return VendorAdminApiService.PostVendor(postRequest).Result;
		}

		protected void DeleteVendor(string vendorId)
		{
			VendorAdminApiService.DeleteVendor(vendorId);
			Log($"Deleted vendor successfully. Id: {vendorId}");
		}

        protected VendorDto GetVendor(string vendorId)
		{
			var getResponse = VendorAdminApiService.GetVendorById(vendorId);
			Log($"Vendor retrieved successfully. VendorId: {vendorId}");

			return getResponse;
		}

		#endregion

		#region Listing request

		protected Data.Dto.V1.MessageApi.ListingRequestDto SetupNewListingRequestWithAllValidFields(string companyName, string companyWebsiteUrl, bool includeSocialMediaUrls = true, bool isCreateVendor = false)
		{
			var socialMediaRequests = includeSocialMediaUrls
				? new List<ListingRequestSocialMediaRequest>
				{
					new ListingRequestSocialMediaRequest{ SocialMediaTypeId = SocialMediaType.LinkedIn, SocialMediaUrl = SocialMediaUtility.LinkedInUrl },
					new ListingRequestSocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Twitter, SocialMediaUrl = SocialMediaUtility.TwitterUrl },
					new ListingRequestSocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Facebook, SocialMediaUrl = SocialMediaUtility.FacebookUrl },
					new ListingRequestSocialMediaRequest{ SocialMediaTypeId = SocialMediaType.YouTube, SocialMediaUrl = SocialMediaUtility.YoutubeUrl },
					new ListingRequestSocialMediaRequest{ SocialMediaTypeId = SocialMediaType.Instagram, SocialMediaUrl = SocialMediaUtility.InstagramUrl }
				}
				: null;

			ListingRequestInsertRequest postRequest;
			if (isCreateVendor)
			{
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
					SocialMedialUrls = socialMediaRequests,
					CreatedByEventId = Guid.NewGuid()
				};
			}
			else
			{
				postRequest = new ListingRequestInsertRequest
				{
					SugarLeadId = Guid.NewGuid(),
					SugarUserId = Guid.NewGuid(),
					SourceTypeId = (int)ListingRequestSourceType.Web,
					CompanyWebsiteUrl = companyWebsiteUrl,
					CompanyName = companyName,
					CompanyPhoneNumber = RequestUtility.GetRandomString(14),
					CompanyContactEmail = $"{SocialMediaUtility.CompanyName}@company.com",
					CompanyContactFirstName = RequestUtility.GetRandomString(8),
					CompanyContactLastName = RequestUtility.GetRandomString(10),
					CompanyCountryCode = "US",
					CompanyCity = "Austin",
					CompanyStreetAddress1 = "200 Academy Drive",
					CompanyStreetAddress2 = "Suite 120",
					CompanyStateProvinceRegionName = "Texas",
					ProductName = RequestUtility.GetRandomString(10),
					ProductShortDescription = RequestUtility.GetRandomString(50),
					ProductProposedCategoryName = RequestUtility.GetRandomString(9),
					SocialMedialUrls = socialMediaRequests,
					CreatedByEventId = Guid.NewGuid()
				};
			}

			var result = MessageApiService.PostListingRequest(postRequest).Result;
			Log("Listing request insert complete.");

			return result;
		}

		protected Data.Dto.V1.MessageApi.ListingRequestDto SetupNewListingRequestWithExistingVendor(string vendorId, string productName = null)
		{
			ListingRequestInsertRequest postRequest;
			var vendorIntegrationId = GetVendor(vendorId).VendorIntegrationId;

			postRequest = new ListingRequestInsertRequest
			{
				SugarUserId = Guid.NewGuid(),
				SourceTypeId = (int)ListingRequestSourceType.SugarUser,
				VendorId = vendorIntegrationId,
				SugarUserEmail = $"{SocialMediaUtility.CompanyName}@company.com",
				SugarUserFirstName = RequestUtility.GetRandomString(8),
				SugarUserLastName = RequestUtility.GetRandomString(10),
				ProductName = string.IsNullOrEmpty(productName)
					? RequestUtility.GetRandomString(10)
					: productName,
				ProductShortDescription = RequestUtility.GetRandomString(50),
				ProductProposedCategoryName = RequestUtility.GetRandomString(9),
				SocialMedialUrls = null,
				CreatedByEventId = Guid.NewGuid()
			};

			var result = MessageApiService.PostListingRequest(postRequest).Result;
			Log("Listing request insert complete.");

			return result;
		}

		protected ListingRequestDto GetListingRequest(string listingRequestId)
		{
			var getResponse = VendorAdminApiService.GetListingRequestById(listingRequestId);
			Log($"Listing request retrieved successfully. ListingRequestId: {listingRequestId}");

			return getResponse;
		}

		#endregion

		#region Message API

		protected (string ProductId, string categoryId) SetUpProductWithMessageApiPut(string productName, string categoryName)
		{
			//Post new product, category, and link the product with the category
			//Post a new product via the ProductAdminApi
			var postProductResponse = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName });
			Log($"Product posted successfully. ProductId: {postProductResponse.Result.ProductId}, ProductIntegrationId: {postProductResponse.Result.ProductIntegrationId}");

			//Post a new category via the CategoryAdminApi
			var postCategoryResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = categoryName });
			Log($"Category created successfully. CategoryId: {postCategoryResponse.Result.CategoryId}");

			//Link the product to the category
			ProductAdminApiService.UpsertProductCategoriesByProductId(
				new ProductCategoriesUpsertRequest
				{
					ProductId = postProductResponse.Result.ProductId,
					CategoryIds = new List<int> { postCategoryResponse.Result.CategoryId }
				});
			Log("ProductCategory created successfully.");

			//Use the message api put to add valid values for all fields
			var putRequest = new ProductUpdateRequest
			{
				SupportOptionIds = new JArray
				{
					SupportOptionType.EmailHelpDesk,
					SupportOptionType.FaqForum
				},
				TrainingOptionIds = new JArray
				{
					TrainingOptionType.InPerson,
					TrainingOptionType.LiveOnline
				},
				DeploymentOptionIds = new JArray
				{
					DeploymentOptionType.CloudSaaSWebBased,
					DeploymentOptionType.OnPremiseWindows
				},
				TargetCompanySizeIds = new JArray
				{
					CompanySizeType.SelfEmployed,
					CompanySizeType.TwoToTen
				},
				TargetNumberUserIds = new JArray
				{
					NumberUserType.One,
					NumberUserType.TwoToTen
				},
				TargetIndustryCodes = new JArray
				{
					47,
					94
				},
				IsPricingVisible = false,
				Sites = new JArray
				{
					new ProductSiteUpdateRequest
					{
						SiteCode = nameof(SiteType.Capterra),
						Overrides = new JArray
						{
							new OverrideUpdateRequest
							{
								Categories = new JArray
								{
									new CategoryOverrideUpdateRequest
									{
										Id = postCategoryResponse.Result.CategoryIntegrationId.ToString(),
										DestinationUrls = new JArray
										{
											new ProductDestinationUrlUpdateRequest
											{
												Id = (int)UrlType.Product,
												Url = "https://www.testoverride.com/original",
												Name = null
											}
										},
										Descriptions = new JArray
										{
											new Data.Request.MessageApi.DescriptionUpdateRequest
											{
												Id = (int)ContentType.ShortDescription,
												Text = "This is a short override description."
											},
											new Data.Request.MessageApi.DescriptionUpdateRequest
											{
												Id = (int)ContentType.LongDescription,
												Text = "This is a long override description."
											}
										}
									}
								}
							}
						}
					}
				}
			};
			MessageApiService.PutProduct(postProductResponse.Result.ProductIntegrationId, putRequest);
			Log("Product updated successfully.");

			BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, postProductResponse.Result.ProductId.ToString());

			return (postProductResponse.Result.ProductId.ToString(), postCategoryResponse.Result.CategoryId.ToString());
		}

		#endregion
	}
}