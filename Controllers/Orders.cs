using Bangazon.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class Orders
    {
        public static void Map(WebApplication app)
        {
            //Get all orders
            app.MapGet("/api/orders", (BangazonDbContext db) =>
            {
                return db.Orders.ToList();
            });

            //Get orders via id
            app.MapGet("/api/orders/{id}", (BangazonDbContext db, int id) =>
            {
                Order singleOrder = db.Orders.SingleOrDefault(x => x.Id == id);
                if (singleOrder == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleOrder);
            });

            //get single order by customer id with the product details
            app.MapGet("/api/productOrders/customers{id}", (BangazonDbContext db, int id) =>
            {
                Order singleOrder = db.Orders.SingleOrDefault(x => x.CustomerId == id);
                if (singleOrder == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleOrder);
            });

            app.MapPost("/api/order/{id}/newProduct/{ProductId}", (BangazonDbContext db, int id, int ProductId) =>
            {
                var singleOrderToUpdate = db.Orders
                .Include(o => o.Products)
                .FirstOrDefault(o => o.Id == id);
                var productToAdd = db.Products.FirstOrDefault(p => p.Id == ProductId);

                try
                {
                    singleOrderToUpdate.Products.Add(productToAdd);
                    db.SaveChanges();
                    return Results.NoContent();

                }
                catch (DbUpdateException)
                {
                    return Results.BadRequest("Invalid data submitted");
                }
            });
            


        }
    }
}
