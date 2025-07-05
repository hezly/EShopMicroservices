﻿using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

//public record DeleteOrderRequest(Guid Id);

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid Id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(Id);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteOrderResponse>();
            return Results.Ok(response);
        })
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete an existing order")
            .WithDescription("Delete an existing order in the ordering system");
    }
}