using Domain.Handling;
using Domain.Interfaces;
using Services.Classes;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register the in-memory cache service
builder.Services.AddMemoryCache();

// Register logging service
builder.Services.AddLogging();

// Register services
builder.Services.AddScoped<IShipHandle  , ShipHandle>();
builder.Services.AddScoped<IShootHandle , ShootHandle>();
builder.Services.AddScoped<IShipService , ShipService>();
builder.Services.AddScoped<IShootService, ShootService>();

// Register endpoint and swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    
    // Add support for custom headers
    options.AddSecurityDefinition("custom-header", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name        = "X-consumer", // header name
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Consumer application name"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
