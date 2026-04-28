using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;
public class Product
{
    public Guid ProductId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }
    
    public Product(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required", "invalid_name");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required", "invalid_description");
        if (price <= 0)
            throw new DomainException("Price must be greater than zero", "invalid_price");
        
        ProductId = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        IsActive = true;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required", "invalid_name");
        Name = name.Trim();
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description is required", "invalid_description");
        Description = description.Trim();
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be greater than zero", "invalid_price");
        Price = price;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
