using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
    public class ProductPricingPlanDto
    {
        public int PlanId { get; set; }
        public string Name { get; set; }
        public decimal? StartingPrice { get; set; }
        public decimal? EndingPrice { get; set; }
        public string Description { get; set; }
        public string CustomPrice { get; set; }
        public int DisplayOrder { get; set; }
        public List<PriceTypeDto> PriceTypes { get; set; }
        public List<PricingModelDto> PricingModels { get; set; }
        public List<PaymentFrequencyDto> PaymentFrequencies { get; set; }
        public List<AttributeDto> Attributes { get; set; }
	}
}
