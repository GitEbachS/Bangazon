using Bangazon.DTOs;
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

           
            app.MapGet("/api/users/sellers", (BangazonDbContext db) =>
            {
                return db.Users.Where(u => u.IsSeller == true).ToList();
            });

            //Create new user
            app.MapPost("/api/addUser", (BangazonDbContext db, UserDto userObj) =>
            {
                User newUser = new()
                {
                    Name = userObj.Name,
                    Email = userObj.Email,
                    IsSeller = userObj.IsSeller,
                    Uid = userObj.Uid
                };

                db.Users.Add(newUser);
                db.SaveChanges();
                return Results.Created($"/api/users/{newUser.Id}", newUser);
            });

            //Update User
            app.MapPut("/api/updateUser/{userId}", (BangazonDbContext db, int userId, UserUpdateDto user) =>
            {
                User userToUpdate = db.Users.SingleOrDefault(user => user.Id == userId);
                if (userToUpdate == null)
                {
                    return Results.NotFound();
                }
                userToUpdate.Name = user.Name;
                userToUpdate.Email = user.Email;
                userToUpdate.IsSeller = user.IsSeller;

                db.SaveChanges();
                return Results.NoContent();
            });

            app.MapGet("/api/checkUser/{uid}", (BangazonDbContext db, string uid) =>
            {
                User checkUser = db.Users.FirstOrDefault(user => user.Uid == uid);
                if (checkUser == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(checkUser);
            });

        }
    }
}
