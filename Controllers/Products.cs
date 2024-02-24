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
