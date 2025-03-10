using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(
            x => x.OrderName, namebuilder =>
            {
                namebuilder.Property(n => n.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();
            });

        builder.ComplexProperty(
            x => x.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

                addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

                addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50);

                addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

                addressBuilder.Property(a => a.Country)
                .HasMaxLength(100)
                .IsRequired();

                addressBuilder.Property(a => a.State)
                .HasMaxLength(100)
                .IsRequired();

                addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(10)
                .IsRequired();
            });

        builder.ComplexProperty(
            x => x.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(180)
                .IsRequired();
                addressBuilder.Property(a => a.Country)
                .HasMaxLength(100)
                .IsRequired();
                addressBuilder.Property(a => a.State)
                .HasMaxLength(100)
                .IsRequired();
                addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(10)
                .IsRequired();
            });

        builder.ComplexProperty(
            x => x.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardName)
                .HasMaxLength(50)
                .IsRequired();

                paymentBuilder.Property(p => p.CardNumber)
                .HasMaxLength(24)
                .IsRequired();

                paymentBuilder.Property(p => p.Expiration)
                .HasMaxLength(10)
                .IsRequired();

                paymentBuilder.Property(p => p.CVV)
                .HasMaxLength(3)
                .IsRequired();

                paymentBuilder.Property(p => p.PaymentMethod);
            });

        builder.Property(x => x.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                v => v.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(x => x.TotalPrice);
    }
}
