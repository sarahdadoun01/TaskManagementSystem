var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// **
// Home "http://localhost:<PORT>/"
app.MapGet("/", () =>
{
    return "API is working";
})
.WithName("GetHome");

// Price Calculation Endpoint "http://localhost:<PORT>/40.50/0.15"
app.MapGet("/{price:double}/{tax:double}", (double price, double tax) =>
{
    if (price < 0 || tax < 0)
    {
        return Results.BadRequest(new { error = "Price and tax must be positive numbers." });
    }

    double taxAmount = price * tax;
    double finalPrice = price + taxAmount;

    return Results.Json(new
    {
        price = price.ToString("0.00"),
        tax = tax.ToString("0.00"),
        final = finalPrice.ToString("0.00")
    });
})
.WithName("CalculatePrice");



// Weather Forcast "http://localhost:<PORT>/weatherforcast"
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
