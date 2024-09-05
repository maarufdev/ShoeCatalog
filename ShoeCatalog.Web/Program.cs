using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.Data;
using ShoeCatalog.Domain.Data;
using ShoeCatalog.Domain.Models;
using ShoeCatalog.Repositories;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services;
using ShoeCatalog.Services.Interfaces;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShoeDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});


// configure identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ShoeDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
    options.AccessDeniedPath = "/Home/Error";
});

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:secret_key").Get<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowStripe", builder =>
    {
        builder.WithOrigins("https://localhost:44368/")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IShoeRepository, ShoeRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
// Register Services
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ImageUploadService>();
builder.Services.AddScoped<IShoeServices, ShoeServices>();
builder.Services.AddScoped<IShoeCategoryServices, ShoeCategoryService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowStripe");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var dbContext = services.GetRequiredService<ShoeDbContext>();

    await DataSeeder.SeedData(userManager, roleManager);
}

app.Run();


//https://www.youtube.com/watch?v=wzaoQiS_9dI