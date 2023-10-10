using HHPW_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace HHPW_BackEnd
{
    public class hhpwDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public hhpwDbContext(DbContextOptions<hhpwDbContext> context) : base(context) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User
                {
                    Id = 1,
                    FirstName ="Cole",
                    LastName = "Amantea",
                    Email = "cole.ama@email.com",
                    IsCashier = true,
                    uid = "vyiiRAsqs6YvGQg6U8RtAoFW8iF2",
                    Address = "this is an address",
                }
            });
            modelBuilder.Entity<OrderType>().HasData(new OrderType[]
            {
                new OrderType { Id = 1, Type = "In Person"},
                new OrderType { Id = 2, Type = "Online"},
                new OrderType { Id = 3, Type = "Phone"}
            });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus[]
            {
                new OrderStatus { Id = 1, Status = "Open"},
                new OrderStatus { Id = 2, Status = "Closed"}
            });
            modelBuilder.Entity<PaymentType>().HasData(new PaymentType[]
            {
                new PaymentType { Id = 1, Type = "Cash"},
                new PaymentType { Id = 2, Type = "Credit Card"},
                new PaymentType { Id = 3, Type = "Apple Pay"}
            });
            modelBuilder.Entity<Comment>().HasData(new Comment[]
           {
                new Comment
                { Id = 1,
                  UserId = 1,
                  MenuItemId = 1,
                  Content = "this is content",
                }
           });
            modelBuilder.Entity<MenuItem>().HasData(new MenuItem[]
            {
                new MenuItem { Id = 1, Category = "Apps", Description = "Mmmm Cheese", Name = "CheeseSticks", ImageUrl = "https://www.foxvalleyfoodie.com/wp-content/uploads/2015/07/homemade-mozzarella-sticks-320x320.jpg", Price = 9.99m, },
            });
            modelBuilder.Entity<Order>().HasData(new Order[]
            {
                new Order { Id = 1, CustomerName = "Billy", CustomerPhoneNumber = "867-5309", DatePlaced = DateTime.Now, OrderStatusId = 1, UserId = 1, Tip = 4.01m, OrderTypeId = 1, PaymentTypeId = 1,}
            });
        }
    }
}
