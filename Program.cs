using HHPW_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using HHPW_BackEnd;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:5169")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<hhpwDbContext>(builder.Configuration["hhpwDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
var app = builder.Build();
//Add for Cors 
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//User Endpoints
app.MapGet("/users", (hhpwDbContext db) =>
{
    return db.Users.ToList();
});

app.MapGet("/user/{id}", (hhpwDbContext db, int id) =>
{
    var user = db.Users.Where(u => u.Id == id);
    return user;
});

app.MapGet("/user/{uid}", (hhpwDbContext db, string uid) =>
{
    var user = db.Users.Where(u => u.uid == uid);
    return user;
});
//Check User
app.MapGet("/checkuser/{uid}", (hhpwDbContext db, string uid) =>
{
    var user = db.Users.Where(x => x.uid == uid).ToList();
    if (uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(user);
    }
});

//Comment Endpoints
app.MapGet("/comments", (hhpwDbContext db) =>
{
    return db.Comments.ToList();
});
//create comment
app.MapPost("/comment", (hhpwDbContext db, Comment comment) =>
{

    db.Comments.Add(comment);
    db.SaveChanges();
    return Results.Ok(comment);
});
//update comment
app.MapPut("/comment/{id}", (hhpwDbContext db, int id, Comment comment) =>
{
    Comment commentToUpdate = db.Comments.FirstOrDefault(c => c.Id == id);
    if (commentToUpdate == null)
    {
        return Results.NotFound();
    }
    commentToUpdate.Content = comment.Content;
    db.SaveChanges();
    return Results.Ok(comment);
});
// delete comment
app.MapDelete("/comment/{id}", (hhpwDbContext db, int id) =>
{
    Comment commentToDelete = db.Comments.FirstOrDefault(c => c.Id == id);
    if (commentToDelete == null)
    {
        return Results.NotFound(id);
    }
    db.Comments.Remove(commentToDelete);
    db.SaveChanges();
    return Results.NoContent();
});

// MenuItem Endpoints 
// get menuItems
app.MapGet("api/menuItem", async (hhpwDbContext db) =>
{
    var products = await db.MenuItems.ToListAsync();

    return Results.Ok(products);
});
//get MenuItem by id
app.MapGet("api/menuItem/{id}", (hhpwDbContext db, int id) =>
{
    return db.MenuItems.Include(p => p.Id).ToList();
});
// create a MenuItem 
app.MapPost("api/menuItem", (hhpwDbContext db, MenuItem item) =>
{
    db.MenuItems.Add(item);
    db.SaveChanges();
    return Results.Created($"/api/menuItem/{item.Id}", item);
});
//update MenuItem 
app.MapPut("api/menuItem/{id}", (hhpwDbContext db, int id, MenuItem item) =>
{
    MenuItem itemToUpdate = db.MenuItems.SingleOrDefault(p => p.Id == id);
    if (itemToUpdate == null)
    {
        return Results.NotFound();
    }
    itemToUpdate.ImageUrl = item.ImageUrl;
    itemToUpdate.Description = item.Description;
    itemToUpdate.Price = item.Price;
    itemToUpdate.Category = item.Category;
    itemToUpdate.Name = item.Name;
    db.SaveChanges();
    return Results.NoContent();
});
//delete a MenuItem 
app.MapDelete("api/menuItem/{id}", (hhpwDbContext db, int id) =>
{
    MenuItem itemToDelete = db.MenuItems.SingleOrDefault(p => p.Id == id);
    if (itemToDelete == null)
    {
        return Results.NotFound();
    }
    db.MenuItems.Remove(itemToDelete);
    db.SaveChanges();
    return Results.NoContent();
});

//Order Endpoints
// get items on a order 
app.MapGet("/orderItems/{id}", (hhpwDbContext db, int id) =>
{
    var orderItemsToGet = db.Orders.Where(o => o.Id == id).Include(m => m.Items).ToList();
    return orderItemsToGet;
});
// add a item to a order 
app.MapPost("/api/MenuItemsOrder", (int itemId, int orderId, hhpwDbContext db) =>
{
    var item = db.MenuItems.Include(m => m.Orders).FirstOrDefault(m => m.Id == itemId);

    if (item == null)
    {
        return Results.NotFound();
    }

    var orderToAdd = db.Orders.FirstOrDefault(o => o.Id == orderId);

    if (orderToAdd == null)
    {
        return Results.NotFound();
    }

    item.Orders.Add(orderToAdd);
    db.SaveChanges();

    return Results.NoContent();
});
// get orders
app.MapGet("api/orders", async (hhpwDbContext db) =>
{
    var orders = await db.Orders.ToListAsync();

    return Results.Ok(orders);
});
// get orders by id
app.MapGet("api/orders/{id}", (hhpwDbContext db, int id) =>
{
    return db.Orders.Include(o => o.Id).ToList();
});
//update a order
app.MapPut("api/orders/{id}", (int id, hhpwDbContext db, Order order) =>
{
    Order orderToUpdate = db.Orders.SingleOrDefault(p => p.Id == id);
    if (orderToUpdate == null)
    {
        return Results.NotFound();
    }
    orderToUpdate.DatePlaced = order.DatePlaced;
    orderToUpdate.OrderStatusId = order.OrderStatusId;
    orderToUpdate.CustomerPhoneNumber = order.CustomerPhoneNumber;
    orderToUpdate.CustomerName = order.CustomerName;
    orderToUpdate.OrderTypeId = order.OrderTypeId;
    orderToUpdate.PaymentTypeId = order.PaymentTypeId;
    orderToUpdate.Tip = order.Tip;
    db.SaveChanges();
    return Results.NoContent();
});
// Create a order 
app.MapPost("api/orders", (hhpwDbContext db, Order order) =>
{
    db.Orders.Add(order);
    db.SaveChanges();
    return Results.Created($"api/orders/{order.Id}", order);
});
// delete a product from an order 
app.MapDelete("/api/MenuItemsOrder", (int orderId, int itemId, hhpwDbContext db) =>
{
    var item = db.MenuItems.Include(m => m.Orders).FirstOrDefault(m => m.Id == itemId);

    if (item == null)
    {
        return Results.NotFound();
    }

    var orderToRemove = db.Orders.FirstOrDefault(o => o.Id == orderId);

    if (orderToRemove == null)
    {
        return Results.NotFound();
    }

    item.Orders.Remove(orderToRemove);
    db.SaveChanges();

    return Results.Ok("Item removed from order successfully");
});

//Misc Endpoints 
//get Order Status
app.MapGet("/orderStatus", (hhpwDbContext db) =>
{
    var status = db.OrderStatus.ToList();
    return Results.Ok(status);
});
//get payment Types
app.MapGet("/paymentType", (hhpwDbContext db) =>
{
    var type = db.PaymentTypes.ToList();
    return Results.Ok(type);
});
//get order types
app.MapGet("/OrderType", (hhpwDbContext db) =>
{
    var type = db.OrderTypes.ToList();
    return Results.Ok(type);
});
app.Run();

