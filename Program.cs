using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using motion_api;
using motion_api.Application.Reports;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Obtener el connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar el DbContext de PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

app.MapReportEndpoints();

app.Run();
