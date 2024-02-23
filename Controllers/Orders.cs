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


            /*app.MapPost("/api/orders", async (BangazonDbContext _context, Order order) =>
            {
                Order newOrder = new()
                {
                    CustomerId = 4,
                    PaymentType = "Visa",
                    DateCreated = DateTime.Now,
                    Shipping = "FedEx",
                    IsClosed = false,
             

                };

                if (Order.Products?.Count > 0)
                {
                    foreach (var id in Order.Products)
                    {
                        Product newProduct = await _context.Products.FirstOrDefault(x => x.Id == id);
                        newOrder.Products.Add(newProduct);
                    }
                }
                _context.Add(newOrder);
                await _context.SaveChangesAsync();
                return Results.Created($"/api/orders/{order.Id}", order);
            });*/

            
            app.MapGet("/api/orders/{id}", (BangazonDbContext db, int id) =>
            {
                var results = db.Orders.Include(c => c.Products).Where(c => c.Id == id);

                if (results == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(results);
            });
        }
    }
}
