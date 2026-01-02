using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParallelQueries.CoreApp.Domain.Customers;

namespace ParallelQueries.CoreApp.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.OwnsOne(x => x.Person, cb =>
        {
            cb.Property(p => p.Gender).HasColumnName("Gender").HasMaxLength(30);
            cb.Property(p => p.Title).HasColumnName("Title").HasMaxLength(20);
            cb.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(50);
            cb.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(50);
        });

        builder.Property(x => x.Created).HasDefaultValue(DateTime.UtcNow);


    }
}