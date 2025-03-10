namespace Ordering.Application.Orders.EventHandlers;
public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("OrderCreatedEvent: {OrderId} at {Time}", notification.Order.Id, DateTime.Now);
        return Task.CompletedTask;
    }
}
