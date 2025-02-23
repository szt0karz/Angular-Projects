using Microsoft.EntityFrameworkCore;
using PortfolioManagerApi.Data;
using PortfolioManagerApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodaj DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj politykę CORS, która pozwala na połączenia z lokalnej aplikacji Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Adres Twojej aplikacji frontendowej (Angular)
               .AllowAnyHeader() // Zezwól na dowolne nagłówki
               .AllowAnyMethod(); // Zezwól na dowolne metody HTTP (GET, POST, PUT, DELETE)
    });
});

// Dodaj kontrolery
builder.Services.AddControllers();

var app = builder.Build();

// Automatyczne migracje
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Tylko migracje, bez dodatkowego seedowania
}

// Włącz CORS
app.UseCors("AllowLocalhost"); // Zezwól na połączenia z aplikacji Angular

// Włącz routing i mapowanie kontrolerów
app.MapControllers();

app.Run();
