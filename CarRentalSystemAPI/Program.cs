using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using CarRentalSystemAPI.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(CarProfile));
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CarRentalDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon") ?? throw new InvalidOperationException("Connection string have some issues.")));

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
