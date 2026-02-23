using Microsoft.EntityFrameworkCore;
using Acts.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Configure DbContext to use SQLite from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ActsDbContext>(options =>
    options.UseSqlite(connectionString));

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAngular");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Map controllers
app.MapControllers();

//Perform db migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ActsDbContext>();
    db.Database.Migrate();
}

// Run the app
app.Run();