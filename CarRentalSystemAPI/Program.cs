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

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication( x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( x =>
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
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

//swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// unit of work
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

// Repos
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IJWTManagerRepository), typeof(JWTManagerRepository));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

// Services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IRentalService, RentalService>();

// Profiles
builder.Services.AddAutoMapper(typeof(CarProfile));
builder.Services.AddAutoMapper(typeof(UsersProfile));

// Set Database
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
