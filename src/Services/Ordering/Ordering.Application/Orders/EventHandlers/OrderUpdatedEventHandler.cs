
namespace Ordering.Application.Orders.EventHandlers;
public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("OrderUpdatedEvent: {OrderId} is updated.", notification.Order.Id);
        return Task.CompletedTask;
    }
}
