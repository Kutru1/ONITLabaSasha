using Microsoft.EntityFrameworkCore;
using System;
using LabaONITSasha.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
