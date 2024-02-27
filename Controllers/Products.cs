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
                    var sellerProductsInAllOrders = db.Products
                    .Include(p => p.Orders)
                    .Where(p => p.SellerId == sellerId && p.Orders.Any(o => o.IsClosed))
                    .ToList();

                    return sellerProductsInAllOrders;
                });

            //get seller's products that are in closed or open orders
            app.MapGet("/api/sellerProducts/allOrders/{sellerId}", (BangazonDbContext db, int sellerId) =>
            {
                var sellerProductsInAllOrders = db.Products
                .Include(p => p.Orders)
                .Where(p => p.SellerId == sellerId && p.Orders.All(o => o.IsClosed))
                .ToList();

                return sellerProductsInAllOrders;
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
                var results = db.Products.Where(p => p.CategoryId == categoryId && p.SellerId == sellerId);



                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });

            app.MapGet("/api/twentyProducts", (BangazonDbContext db) =>
            {
                var results = db.Products.OrderByDescending(p => p.Id).Take(20).ToList();



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
