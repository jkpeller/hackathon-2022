using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductPricingPlanUpdateRequest
    {
		public PaymentFrequencyType? PaymentFrequencyTypeId { get; set; }
        [StringLength(ProductPricingUtility.MaximumPlanDescriptionLength)]
        public string Description { get; set; }
        public PricingModelType? PricingModelTypeId { get; set; }
        public decimal? StartingPrice { get; set; }
        public string Name { get; set; }
        public PriceType PriceTypeId { get; set; }
        public string CustomPrice { get; set; }
        public decimal? EndingPrice { get; set; }
        public int DisplayOrder { get; set; }
	}
}