using Bangazon.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class Products
    {
        public static void Map(WebApplication app)
        {
            //Get all products
            app.MapGet("/api/products", (BangazonDbContext db) =>
            {
                return db.Products.ToList();
            });

            //Get products via id
            app.MapGet("/api/products/{id}", (BangazonDbContext db, int id) =>
            {
                Product singleProduct = db.Products.SingleOrDefault(x => x.Id == id);
                if (singleProduct == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleProduct);
            });

            //Get products via seller id
            app.MapGet("/api/products/seller/{id}", (BangazonDbContext db, int id) =>
            {
                List<Product> filteredSellerProd = db.Products.Where(p => p.SellerId == id).ToList();
                if (filteredSellerProd == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(filteredSellerProd);
            });

            /*app.MapPost("/api/products", async (BangazonDbContext _context, Product product) =>
            {
                Product newProduct = new()
                {
                    SellerId = 4,
                    Name = "test",
                    Description = "test",
                    Price = 10.00M,
                    Quantity = 1,
                    CategoryId = 1,

                };

                if (Product.Orders?.Count > 0)
                {
                    foreach (var id in Product.Orders)
                    {
                        Order newOrder = await _context.Orders.FirstOrDefault(o => o.Id == id);
                        newProduct.Orders.Add(newOrder);
                    }
                }
                _context.Add(newProduct);
                await _context.SaveChangesAsync();
                return Results.Created($"/api/products/{product.Id}", product);
            });*/

            app.MapGet("/api/productOrders/sellers/{id}", (BangazonDbContext db, int id) =>
            {
                var results = db.Products.Include(o => o.Orders).Where(p => p.SellerId == id).ToList();
               


    if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });
        }
    }
}
