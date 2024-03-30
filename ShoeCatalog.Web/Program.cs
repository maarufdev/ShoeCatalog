using Microsoft.EntityFrameworkCore;
using ShoeCatalog.DataModels.Data;
using ShoeCatalog.Repositories;
using ShoeCatalog.Repositories.Interfaces;
using ShoeCatalog.Services;
using ShoeCatalog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShoeDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
