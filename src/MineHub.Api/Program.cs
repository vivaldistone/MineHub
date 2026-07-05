using MineHub.Api.Extensions;
using MineHub.Api.Middlewares;
using FluentValidation;
using MineHub.Api.Contracts.Validators.Products;
using MineHub.Application;
using MineHub.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastucture(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabaseAsync(app.Lifetime.ApplicationStopping);

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
