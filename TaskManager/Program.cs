using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Queries;
using TaskManager.Queries.Interfaces;
using TaskManager.Repositories;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services;
using TaskManager.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Добавляем доступ к HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();


// Настройка аутентификации - упрощенная версия
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Куки на 7 дней
        options.SlidingExpiration = true; // Обновлять срок действия при активности
    });

// Настройка авторизации
builder.Services.AddAuthorization();

// Настройка базы данных
var connectionString = builder.Configuration.GetConnectionString("DbConnection") ??
    throw new InvalidOperationException("Connection string is not initialized");

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseNpgsql(connectionString));

// Регистрация сервисов
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentQuery, DocumentQuery>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Добавляем аутентификацию и авторизацию в pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Documents}/{action=Index}/{id?}");

app.Run();