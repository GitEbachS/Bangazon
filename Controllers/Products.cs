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
