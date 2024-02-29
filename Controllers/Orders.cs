using Bangazon.Models;
using Bangazon.DTOs;
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

            //get customer's open order with the product details
            app.MapGet("/api/productOrders/customers/{id}", (BangazonDbContext db, int id) =>
            {
                var allOrders = db.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Seller)
                .Where(o => o.IsClosed == false && o.CustomerId == id)
                .ToList();
                if (allOrders == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(allOrders);
            });

            //get cutomer's completed orders with the details, by customer id
            app.MapGet("/api/closedOrders/customers/{id}", (BangazonDbContext db, int id) =>
            {
                return db.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Seller)
                .Where(o => o.IsClosed == true && o.CustomerId == id)
                .ToList();
               
            });

          
            //addOrder
            app.MapPost("/api/addOrder", (BangazonDbContext db, OrderDto orderObj) =>
            {
                Order addOrder = new()
                {
                    CustomerId = orderObj.CustomerId,
                    PaymentType = orderObj.PaymentType,
                    DateCreated = orderObj.DateCreated,
                    Shipping = orderObj.Shipping,
                };

                db.Orders.Add(addOrder);
                db.SaveChanges();
                return Results.Created($"/api/users/{addOrder.Id}", addOrder);
            });

            app.MapPost("/api/order/addProduct", (BangazonDbContext db, OrderProductDto orderProduct) =>
            {
                var singleOrderToUpdate = db.Orders
                .Include(o => o.Products)
                .FirstOrDefault(o => o.Id == orderProduct.OrderId);
                var productToAdd = db.Products.FirstOrDefault(p => p.Id == orderProduct.ProductId);

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

            //Update Order
            app.MapPut("/api/updateOrder/{orderId}", (BangazonDbContext db, int orderId, OrderUpdateDto order) =>
            {
                Order orderToUpdate = db.Orders.SingleOrDefault(order => order.Id == orderId);
                if (orderToUpdate == null)
                {
                    return Results.NotFound();
                }
                orderToUpdate.PaymentType = order.PaymentType;
                orderToUpdate.Shipping = order.Shipping;
                orderToUpdate.IsClosed = order.IsClosed;

                db.SaveChanges();
                return Results.NoContent();
            });

            //delete product from cart
            app.MapDelete("/api/order/{orderId}/deleteProduct/{productId}", (BangazonDbContext db, int orderId, int productId) =>
            {
                var singleOrderToUpdate = db.Orders
                .Include(o => o.Products)
                .FirstOrDefault(o => o.Id == orderId);
                var productToDelete = db.Products.FirstOrDefault(p => p.Id == productId);

                try
                {
                    singleOrderToUpdate.Products.Remove(productToDelete);
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
