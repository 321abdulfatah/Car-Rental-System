using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using CarRentalSystemAPI.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using CarRentalSystemAPI.Swagger;
using BusinessAccessLayer.Services.Interfaces;
using BusinessAccessLayer.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
builder.Services.AddControllersWithViews();

//swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// unit of work
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

// Repos
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

// Services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddTransient<IAuthService, AuthService>();

// cache memory
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheInitializerService, CacheInitializerService>();

// Profiles
builder.Services.AddAutoMapper(typeof(CarProfile));

// Set Database
builder.Services.AddDbContext<CarRentalDBContext>(options =>
    { 
        options.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon") ?? throw new InvalidOperationException("Connection string have some issues."));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    });

// For Identity  
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CarRentalDBContext>()
                .AddDefaultTokenProviders();

// Adding Authentication  

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  

.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/error-local-development");
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var cacheInitializer = scope.ServiceProvider.GetRequiredService<ICacheInitializerService>();
    cacheInitializer.InitializeCacheAsync().Wait();
}
/*
// Initialize the cache when the application starts
var cacheInitializerService = app.Services.GetRequiredService<ICacheInitializerService>();
cacheInitializerService.InitializeCacheAsync().Wait();*/

app.Run();
