using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Queries;
using TaskManager.Queries.Interfaces;
using TaskManager.Repositories;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DbConnection") ??
                                                throw new InvalidOperationException("Connection string is not initialized");

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                                options.UseNpgsql(connectionString));

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

builder.Services.AddScoped<IDocumentQuery, DocumentQuery>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "EquipmentsAuth";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Documents}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();