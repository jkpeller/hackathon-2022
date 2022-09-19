namespace ProductCatalogAdminUiIntegrationTest.Core.Utility
{
    public static class MessagePublishUtility
    {
        public static class UpcTopic
        {
            public const string Upc_BatchRequest = "Upc_BatchRequest";
        }

        public static class UpcMessage
        {
            public const string ProductReviewsPriceRating = @"{'eventId':'EVENTID',
                            'eventVersion':'v1',
                            'eventSender':'Reviews',
                            'eventReceiver':'Reviews',
                            'entityName':'ProductReviewsPriceRating',
                            'entityVersion':'v1',
                            'entityOperation':'Upsert',
                            'entityDetail': {
                                   'id': 'PRODUCTID',
                                   'reviewsCount': COUNT,
                                   'averageRelativePriceRating': 2.456
                                             }
                            }";
        }
    }
}
