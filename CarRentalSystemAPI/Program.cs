using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Contracts;
using DataAccessLayer.Repositories;
using BusinessAccessLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<Car>, RepositoryCar>();
builder.Services.AddScoped<IServiceCar<Car>, ServiceCar>();
builder.Services.AddDbContext<CarRentalDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon") ?? throw new InvalidOperationException("Connection string have some issues.")));

builder.Services.AddMemoryCache();

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
