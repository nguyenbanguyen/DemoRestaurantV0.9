using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DemoRestaurant.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;

namespace DemoRestaurant.DAL
{
    public class RestaurantDemoContext : DbContext
    {
        //public DbSet<Client> Client { get; set; }
        //public DbSet<ClientDetail> ClientDetail { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        //public DbSet<AccountModel> Account { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public class RestaurantDbInitializer : DropCreateDatabaseIfModelChanges<DAL.RestaurantDemoContext>
        {
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
}