namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events
{
    public record OrderStatusChangedToCompleteIntegrationEvent: IntegrationEvent
    {

        public int OrderId { get; }
        public string OrderStatus { get; }
        public string BuyerName { get; }

        public OrderStatusChangedToCompleteIntegrationEvent(int orderId, string orderStatus, string buyerName)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            BuyerName = buyerName;
        }
    }
}
