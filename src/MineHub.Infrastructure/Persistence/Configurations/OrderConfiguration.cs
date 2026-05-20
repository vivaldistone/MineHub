using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;

namespace MineHub.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId);

        builder.Property(o => o.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.OwnsMany(o => o.OrderItems, itemsBuilder =>
        {
            itemsBuilder.ToTable("order_items");

            itemsBuilder.Property<Guid>("id");

            itemsBuilder.WithOwner()
                .HasForeignKey("order_id");

            itemsBuilder.HasKey("id");

            itemsBuilder.Property(o => o.ProductId)
                .HasColumnName("product_id")    
                .IsRequired();

            itemsBuilder.Property(o => o.Name)
                .HasColumnName("name")    
                .HasMaxLength(255)
                .IsRequired();

            itemsBuilder.Property(o => o.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired();

            itemsBuilder.Property(o => o.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            itemsBuilder.Property(o => o.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            itemsBuilder.Ignore(o => o.TotalPrice);

        });


        builder.Property(o => o.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Ignore(o => o.TotalPrice);
    }
}
