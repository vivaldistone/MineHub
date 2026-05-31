using Microsoft.AspNetCore.Authorization;
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
    private readonly AddProductCommandHandler _addProductHandler;
    private readonly UpdateProductCommandHandler _updateProductHandler;
    private readonly DeleteProductCommandHandler _deleteProductHandler;

    public ProductsController(
        GetProductsQueryHandler queryHandler, 
        GetProductQueryHandler getProductHandler, 
        AddProductCommandHandler addProductCommandHandler, 
        UpdateProductCommandHandler updateProductCommandHandler,
        DeleteProductCommandHandler deleteProductCommandHandler)
    {
        _getProductsHandler = queryHandler;
        _getProductHandler = getProductHandler;
        _addProductHandler = addProductCommandHandler;
        _updateProductHandler = updateProductCommandHandler;
        _deleteProductHandler = deleteProductCommandHandler;
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var command = new AddProductCommand(request.Name, request.Description, request.Price);
        
        var result = await _addProductHandler.HandleAsync(command);

        return CreatedAtAction(nameof(GetById), new { id = result.ProductId }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price);

        await _updateProductHandler.HandleAsync(command);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteProductHandler.HandleAsync(id);

        return NoContent();
    }
}
