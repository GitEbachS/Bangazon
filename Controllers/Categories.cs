﻿using Bangazon.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class Categories
    {
        public static void Map(WebApplication app)
        {
            //Get all users
            app.MapGet("/api/category", (BangazonDbContext db) =>
            {
                return db.Users.ToList();
            });

            //Get users via id
            app.MapGet("/api/category/{id}", (BangazonDbContext db, int id) =>
            {
                Category singleCategory = db.Categories.SingleOrDefault(x => x.Id == id);
                if (singleCategory == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleCategory);
            });

            //get all categories with products and take the first three
            app.MapGet("/api/category/displayProducts", (BangazonDbContext db) =>
            {
                var categoriesWithProducts = db.Categories
                .Include(c => c.Products.OrderBy(p => p.Id).Take(3))
                .OrderBy(c => c.Name)
                .ToList();

                return Results.Ok(categoriesWithProducts);
            });



        }
    }
}
