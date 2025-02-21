using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, List<string> Category, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Category = command.Category,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // Save product to database

            return new CreateProductResult(Guid.NewGuid());
        }
    }
}
