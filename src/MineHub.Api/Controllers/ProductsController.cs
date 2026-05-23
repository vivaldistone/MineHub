using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.Products;
using MineHub.Application.Products.Commands.AddProduct;
using MineHub.Application.Products.Commands.DeleteProduct;
using MineHub.Application.Products.Commands.UpdateProduct;
using MineHub.Application.Products.Queries.GetProduct;
using MineHub.Application.Products.Queries.GetProducts;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly GetProductsQueryHandler _getProductsHandler;
    private readonly GetProductQueryHandler _getProductHandler;
    private readonly AddProductCommandHandler _addProductCommandHandler;
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;
    private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

    public ProductsController(
        GetProductsQueryHandler queryHandler, 
        GetProductQueryHandler getProductHandler, 
        AddProductCommandHandler addProductCommandHandler, 
        UpdateProductCommandHandler updateProductCommandHandler,
        DeleteProductCommandHandler deleteProductCommandHandler)
    {
        _getProductsHandler = queryHandler;
        _getProductHandler = getProductHandler;
        _addProductCommandHandler = addProductCommandHandler;
        _updateProductCommandHandler = updateProductCommandHandler;
        _deleteProductCommandHandler = deleteProductCommandHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _getProductsHandler.HandleAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _getProductHandler.HandleAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var command = new AddProductCommand(request.Name, request.Description, request.Price);
        
        var result = await _addProductCommandHandler.HandleAsync(command);

        return CreatedAtAction(nameof(GetById), new { id = result.ProductId }, result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price);

        await _updateProductCommandHandler.HandleAsync(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteProductCommandHandler.HandleAsync(id);

        return NoContent();
    }
}
