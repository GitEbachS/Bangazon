using Microsoft.EntityFrameworkCore;
using Bangazon.Models;
using System.Runtime.CompilerServices;

public class BangazonDbContext : DbContext
{

    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders);
            

        // seed data with orders
        modelBuilder.Entity<Order>().HasData(new Order[]
        {
            new Order { Id = 1, CustomerId = 1, PaymentType = "Visa", DateCreated = DateTime.Now, Shipping = "UPS", IsClosed = false },
            new Order { Id = 2, CustomerId = 2, PaymentType = "Cash", DateCreated = DateTime.Now, Shipping = "FedEx", IsClosed = false },
            new Order { Id = 3, CustomerId = 3, PaymentType = "Check", DateCreated = DateTime.Now, Shipping = "Prime", IsClosed = false },
            new Order { Id = 4, CustomerId = 4, PaymentType = "Visa", DateCreated = DateTime.Now, Shipping = "FedEx", IsClosed = false }
        });

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.SellerId);


        modelBuilder.Entity<Product>().HasData(new Product[]
       {
            new Product { Id = 1, SellerId = 1, Name = "Vietnamese Cinnamon", Description = "Bright, fragrant, and sweet", Price = 14.00M, Quantity = 1400, CategoryId = 1 },
            new Product { Id = 2, SellerId = 2, Name = "Garam Masala", Description = "Nutty and colorful", Price = 11.00M, Quantity = 1560, CategoryId = 2 },
            new Product { Id = 3, SellerId = 3, Name = "Curry Powder", Description = "Colorgul, fragrant, and sweet", Price = 12.00M, Quantity = 1660, CategoryId = 2 },
            new Product { Id = 4, SellerId = 4, Name = "Smoked Paprika", Description = "Smoky, dark tomato flaver, and slightly sweet", Price = 10.00M, Quantity = 1500, CategoryId = 4 },
       });

       
        modelBuilder.Entity<Category>().HasData(new Category[]
       {
            new Category { Id = 1, Name = "Asian" },
            new Category { Id = 2, Name = "Indian" },
            new Category { Id = 3, Name = "Mexican" },
            new Category { Id = 4, Name = "BBQ" }
       });
        modelBuilder.Entity<User>().HasData(new User[]
       {
            new User { Id = 1, Name = "Barry Clark", Email = "Barry@barry.com", IsSeller = false, Uid = "lkjfdg95vmfD9geKVPpzydRjhUc2" },
            new User { Id = 2, Name = "Mary Apple", Email = "Mary@mary.com", IsSeller = false, Uid = "kXzfdg95vmfD9geKVPpzydRrthgf" },
            new User { Id = 3, Name = "Steve Lair", Email = "Steve@steve.com", IsSeller = false, Uid = "jkhfdg95vmfDytuKVPpzydRjh333" },
            new User { Id = 4, Name = "Jake Stellar", Email = "Jake@jake.com", IsSeller = false, Uid = "fghfdg95vmfD9geKVPpzydRdfgrt" }

       });

    }
    public BangazonDbContext(DbContextOptions<BangazonDbContext> context) : base(context)
    {

    }
}