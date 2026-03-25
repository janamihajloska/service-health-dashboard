using ServiceHealth.Api.Configuration;
using ServiceHealth.Api.Services;
using ServiceHealth.Api.Workers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer(); // Swagger support
builder.Services.AddSwaggerGen();           // Swagger generation
builder.Services.AddHttpClient();
builder.Services.AddScoped<HealthCheckService>();
builder.Services.AddSingleton<HealthStatusCache>();
builder.Services.AddHostedService<HealthCheckWorker>();


builder.Services.Configure<List<ServiceConfig>>(
    builder.Configuration.GetSection("Services"));
builder.Services.Configure<HealthCheckSettings>(
    builder.Configuration.GetSection("HealthCheckSettings"));

var app = builder.Build();

app.UseCors();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();