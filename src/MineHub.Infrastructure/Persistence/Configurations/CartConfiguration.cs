using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId);

        builder.Property(c => c.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(c => c.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.Ignore(c => c.TotalPrice);

        builder.OwnsMany(c => c.CartItems, itemsBuilder =>
        {
            itemsBuilder.ToTable("cart_items");

            itemsBuilder.WithOwner().HasForeignKey("cart_id");

            itemsBuilder.Property<Guid>("id");
            
            itemsBuilder.HasKey("id");

            itemsBuilder.Property(i => i.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            itemsBuilder.Property(i => i.ProductName)
                .HasColumnName("product_name")    
                .HasMaxLength(255)
                .IsRequired();

            itemsBuilder.Property(i => i.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired();

            itemsBuilder.Property(i => i.Quantity)
                .HasColumnName("quantity")    
                .IsRequired();

            itemsBuilder.Property(i => i.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            itemsBuilder.Ignore(i => i.TotalPrice);
        });
    }
}
