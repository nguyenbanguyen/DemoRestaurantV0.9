namespace DemoRestaurant.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using DemoRestaurant.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DemoRestaurant.DAL.RestaurantDemoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DemoRestaurant.DAL.RestaurantDemoContext";
        }

        protected override void Seed(DemoRestaurant.DAL.RestaurantDemoContext context)
        {
            var category = new List<Category>
            {
                new Category {CategoryId= 1, CategoryName="Food"},
                new Category {CategoryId =2 ,CategoryName="Drink" },
            };
            //check by id, need to be fixed
            category.ForEach(s => context.Category.AddOrUpdate(p => p.CategoryId, s));
            context.SaveChanges();
            var product = new List<Product>
            {   new Product {ProductId =1,ProductName="Bánh Mì",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },
                new Product {ProductId =1,ProductName="Xôi",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },
                new Product {ProductId =1,ProductName="Bún chả",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },
                new Product {ProductId =1,ProductName="Phở",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },
                new Product {ProductId =1,ProductName="Hủ tiếu",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },

                new Product {ProductId =1,ProductName="pizzar",Description="Fast food",CategoryID=1,Price=50000,Discount=0,MaximunQuantity=400,TotalSold=0 },
                new Product {ProductId =2,ProductName="Coca cola",Description="Gas Drink",CategoryID=1,Price=10000,Discount=0,MaximunQuantity=400,TotalSold=0 },
            };
            //check by id, need to be fixed
            product.ForEach(s => context.Product.AddOrUpdate(p => p.ProductId, s));
            context.SaveChanges();

        }
    }

}
