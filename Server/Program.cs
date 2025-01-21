using Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the Razor Pages service.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Register the DbContext service before `app.Build()`.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

 // Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:4200") // Frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddScoped<UpdateTripStatus>();

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseCors("AllowFrontend");  
// Map Razor Pages.
app.MapRazorPages();

app.MapControllers();

app.Run();
