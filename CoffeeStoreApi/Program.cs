using System.Security.Claims;
using System.Text;
using CoffeeStoreApi.Authorization;
using CoffeeStoreApi.Models;
using CoffeeStoreApi.Repositories.Implementations;
using CoffeeStoreApi.Repositories.Interfaces;
using CoffeeStoreApi.Seeding;
using CoffeeStoreApi.Services.Auth;
using CoffeeStoreApi.Services.Categories;
using CoffeeStoreApi.Services.Files;
using CoffeeStoreApi.Services.Orders;
using CoffeeStoreApi.Services.Payments;
using CoffeeStoreApi.Services.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
// Add Identity services with EF stores
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Configure Authentication with JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, 
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        NameClaimType = ClaimTypes.Name
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    "{\"message\":\"Token has expired, please login again.\"}");
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(
                "{\"message\":\"Authentication failed.\"}");
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\":\"You are not authorized.\"}");
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };


});

builder.Services.AddScoped<IOrederService, OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IFileService, CoffeeStoreApi.Services.Files.FileService>();
builder.Services.AddScoped<IOrderRepositroy, OrderRepositroy>();
builder.Services.AddScoped<IDeliveryInformationRepositroy, DeliveryInformationRepositroy>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IPaymentService, PaymentService>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:50418")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});




builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Permissions.All)
    {
        options.AddPolicy(permission, policy =>
        {
            policy.RequireClaim("Permission", permission);
        });
    }
});

var app = builder.Build();




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    await DataSeeder.SeedAsync(
        userManager,
        roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



StripeConfiguration.ApiKey =
    builder.Configuration["Stripe:SecretKey"];

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication(); // 
app.UseAuthorization();

app.MapControllers();

app.Run();

