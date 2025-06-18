using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add other services, repositories, etc.
// builder.Services.AddScoped<IYourService, YourService>();

var app = builder.Build();
