using Bangazon.Models;
using Bangazon.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class Orders
    {
        public static void Map(WebApplication app)
        {

            //Get orders via id
            app.MapGet("/api/orders/{id}", (BangazonDbContext db, int id) =>
            {
                Order singleOrder = db.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Seller)
                .FirstOrDefault(x => x.Id == id);
                if (singleOrder == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleOrder);
            });

            //get customer's open order with the product details
            app.MapGet("/api/cartOrder/customer/{userId}", (BangazonDbContext db, int userId) =>
            {
                var allOrders = db.Orders
                .Include(o => o.Products)
                     .ThenInclude(p => p.Seller)
                .Include(o => o.Products)
                     .ThenInclude(p => p.Category)
                .SingleOrDefault(o => o.IsClosed == false && o.CustomerId == userId);
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

         

            //an order is opened or checked for a current open order when the cutomer logs in
            app.MapPost("/api/cartOrder/new/{userId}", (BangazonDbContext db, int userId) =>
            {
                Order openOrder = db.Orders.SingleOrDefault(o => o.CustomerId == userId && o.IsClosed == false);

                if (openOrder != null)
                {
                    return Results.Ok(openOrder);
                }
                else 
                {
                    Order cart = new Order();
                    cart.CustomerId = userId;
                    cart.IsClosed = false;
                    cart.PaymentType = "none";
                    cart.Shipping = "";
                    cart.DateCreated = new DateTime();
                    db.Orders.Add(cart);
                    db.SaveChanges();
                    return Results.Created($"/api/cartOrder/{userId}/new/{cart.Id}", cart);
                }
            });

            //the customer's open order is updated and closed via form
            app.MapPut("/api/cartOrder/close", (BangazonDbContext db, CloseCartDto dto) =>
            {
                Order cart = db.Orders
                .Include(o => o.Products)
                .SingleOrDefault(o => o.Id == dto.Id && o.IsClosed == false);

                if (cart == null)
                {
                    return Results.BadRequest("Order not found!");
                }
                if (cart.Products.Count < 1)
                {
                    return Results.BadRequest("Order has no products");
                }
                    cart.IsClosed = true;
                    cart.DateCreated = DateTime.Now;
                    cart.PaymentType = dto.PaymentType;
                    db.SaveChanges();
                    return Results.Ok(cart);
              
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
