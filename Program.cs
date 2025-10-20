

using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var orders = new List<Order>
{
   new() { Id = 1, Customer = "Pixel Coworking", Total = 400 },
   new() { Id = 2, Customer = "Katia Barrón", Total = 250 }
};

// GET all
app.MapGet("/api/orders", () => orders);

// GET by Id
app.MapGet("/api/orders/{id:int}", (int id) =>
{ 
  var order = orders.FirstOrDefault(o => o.Id == id);
  return order is null ? Results.NotFound() : Results.Ok(order);
});

// POST new
app.MapPost("/api/orders", (Order order) =>
{
    order.Id = orders.Max(o => o.Id) + 1;
    orders.Add(order);
    return Results.Created($"/api/orders/{order.Id}", order);
});

// PUT update
app.MapPut("/api/orders/{id:int}", (int id, Order updated) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order is null) return Results.NotFound();
    order.Customer = updated.Customer;
    order.Total = updated.Total;
    return Results.NoContent();
});

// DELETE
app.MapDelete("/api/orders/{id:int}", (int id) =>
{
    var order = orders.FirstOrDefault((o) => o.Id == id);
    if (order is null) return Results.NotFound();
    orders.Remove(order);
    return Results.NoContent();

});

app.Run();

class Order
{
    public int Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

