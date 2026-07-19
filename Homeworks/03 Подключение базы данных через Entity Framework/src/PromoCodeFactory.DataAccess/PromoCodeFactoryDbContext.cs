using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess;

public class PromoCodeFactoryDbContext : DbContext
{
    public PromoCodeFactoryDbContext(DbContextOptions<PromoCodeFactoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Preference> Preferences => Set<Preference>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();

    public DbSet<CustomerPromoCode> CustomerPromoCodes => Set<CustomerPromoCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureRole(modelBuilder);
        ConfigureEmployee(modelBuilder);
        ConfigurePreference(modelBuilder);
        ConfigureCustomer(modelBuilder);
        ConfigurePromoCode(modelBuilder);
        ConfigureCustomerPromoCode(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .HasMaxLength(500);
        });
    }

    private static void ConfigureEmployee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Ignore(x => x.FullName);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasOne(x => x.Role)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePreference(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preference>(entity =>
        {
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
    }

    private static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Ignore(x => x.FullName);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasMany(p => p.Preferences)
                .WithMany(c => c.Customers)
                .UsingEntity(j => j.ToTable("CustomerPreferences"));

            entity.HasMany(c => c.CustomerPromoCodes)
                .WithOne()
                .HasForeignKey(cpc => cpc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurePromoCode(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PromoCode>(entity =>
        {
            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ServiceInfo)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(x => x.PartnerName)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(x => x.BeginDate)
                .IsRequired();

            entity.Property(x => x.EndDate)
                .IsRequired();

            entity.HasOne(x => x.PartnerManager)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Preference)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.CustomerPromoCodes)
                .WithOne()
                .HasForeignKey(cpc => cpc.PromoCodeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureCustomerPromoCode(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerPromoCode>(entity =>
        {
            entity.Property(x => x.CustomerId)
                .IsRequired();

            entity.Property(x => x.PromoCodeId)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });
    }
}
