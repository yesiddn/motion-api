using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using motion_api;
using motion_api.Application.ApiKeys;
using motion_api.Application.Nodes;
using motion_api.Application.Reports;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Obtener el connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar el DbContext de PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddValidation();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

var app = builder.Build();

app.MapReportEndpoints();
app.MapNodeEndpoints();
app.MapApiKeyEndpoints();

app.Run();
