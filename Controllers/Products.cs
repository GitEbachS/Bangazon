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
                return db.Products
                .Include(p => p.Seller)
                .Include(p => p.Orders)
                .Include(p => p.Category)
                .ToList();
            });

            //Get products via id
            app.MapGet("/api/products/{id}", (BangazonDbContext db, int id) =>
            {
                Product singleProduct = db.Products
                .Include(p => p.Seller)
                .FirstOrDefault(s => s.Id == id);

                try
                {
                    return Results.Ok(singleProduct);

                }
                catch (DbUpdateException)
                {
                    return Results.BadRequest("Invalid data submitted");
                }

            });

            //get seller's products that are in closed orders
            app.MapGet("/api/sellerProducts/closedOrders/{sellerId}", (BangazonDbContext db, int sellerId) =>
            {
                var results = db.Orders
                    .Include(o => o.Products)
                    .Where(o => o.Products.Any(p => p.SellerId == sellerId) && o.IsClosed)
                    .Select(o => new Order
                    {
                        Id = o.Id,
                        CustomerId = o.CustomerId,
                        Shipping = o.Shipping,
                        PaymentType = o.PaymentType,
                        DateCreated = o.DateCreated,
                        Products = o.Products.Where(p => p.SellerId == sellerId).ToList(),
                    })
                .ToList();
                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });


            //get seller's products that are in all orders
            app.MapGet("/api/sellerProducts/allOrders/{sellerId}", (BangazonDbContext db, int sellerId) =>
            {
                var results = db.Orders
                .Include(o => o.Products)
                .Where(o => o.Products.Any(p => p.SellerId == sellerId))
                .Select(o => new Order
                 {
                     Id = o.Id,
                     CustomerId = o.CustomerId,
                     Shipping = o.Shipping,
                     PaymentType = o.PaymentType,
                     DateCreated = o.DateCreated,
                     Products = o.Products.Where(p => p.SellerId == sellerId).ToList()
                 })
                 .ToList();
                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });
            app.MapGet("/api/products/sellers/{sellerId}", (BangazonDbContext db, int sellerId) =>
            {
                var results = db.Users.Include(u => u.Products).Where(u => u.Id == sellerId).ToList();



                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });

            //get all of seller's products by category
            app.MapGet("/api/products/categories/{sellerId}/{categoryId}", (BangazonDbContext db, int sellerId, int categoryId) =>
            {
                var results = db.Products.Where(p => p.CategoryId == categoryId && p.SellerId == sellerId).ToList();



                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });

            app.MapGet("/api/twentyProducts", (BangazonDbContext db) =>
            {
                var results = db.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .OrderByDescending(p => p.Id).Take(20).ToList();



                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });

            app.MapGet("/api/products/search/{userInput}", (BangazonDbContext db, string userInput) =>
            {
                string searchTerm = userInput.ToLower();

                return db.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                p.Description.ToLower().Contains(searchTerm) ||
                p.Category.Name.ToLower().Contains(searchTerm) ||
                p.Seller.Name.ToLower().Contains(searchTerm)).ToList();
            });

        }
    }
}
