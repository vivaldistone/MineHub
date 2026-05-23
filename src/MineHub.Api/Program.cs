using Microsoft.EntityFrameworkCore;
using MineHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using MineHub.Infrastructure.Identity;
using MineHub.Api.Extensions;
using MineHub.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(builderOptions => builderOptions.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<AuthUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabaseAsync();

app.MapControllers();

app.Run();
