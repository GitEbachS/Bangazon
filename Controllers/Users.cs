using Bangazon.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class Users
    {
        public static void Map(WebApplication app)
        {
            //Get all users
            app.MapGet("/api/users", (BangazonDbContext db) =>
            {
                return db.Users.ToList();
            });

            //Get users via id
            app.MapGet("/api/users/{id}", (BangazonDbContext db, int id) =>
            {
                User singleUser = db.Users.SingleOrDefault(x => x.Id == id);
                if (singleUser == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleUser);
            });


        
        }
    }
}
