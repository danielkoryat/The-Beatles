using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Basic_Project.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Basic_ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Basic_ProjectContext") ?? throw new InvalidOperationException("Connection string 'Basic_ProjectContext' not found.")));
builder.Services.AddControllersWithViews();
;
var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tour}/{action=Index}/{id?}");

app.Run();