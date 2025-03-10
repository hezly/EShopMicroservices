namespace Ordering.Application.Orders.Queries.GetOrderByName;
public class GetOrderByNameHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetOrderByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .Where(x => x.OrderName.Value.Contains(request.Name))
            .OrderBy(x => x.OrderName)
            .ToListAsync(cancellationToken);

        return new GetOrderByNameResult(orders.ToOrderDtoList());
    }
}
