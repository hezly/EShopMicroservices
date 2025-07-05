using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

//public record GetOrderByNameRequest(string Name);

public record GetOrderByNameResult(IEnumerable<OrderDto> Orders);

public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/name/{name}", async (string name, ISender sender) =>
        {
            var query = new GetOrderByNameQuery(name);
            var result = await sender.Send(query);
            var response = result.Adapt<GetOrderByNameResult>();
            return Results.Ok(response);
        })
            .WithName("GetOrderByName")
            .Produces<GetOrderByNameResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get orders by name")
            .WithDescription("Retrieve orders based on the customer's name");
    }
}
