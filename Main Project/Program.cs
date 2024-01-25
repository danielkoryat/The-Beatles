using Main_Project.interfaces;
using Main_Project.Services;
using Main_Project.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MainProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Basic_ProjectContext") ?? throw new InvalidOperationException("Connection string 'Basic_ProjectContext' not found.")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseSession();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tour}/{action=Index}/{id?}");

app.Run();