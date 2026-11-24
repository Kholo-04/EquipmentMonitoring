using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EquipmentMonitoringDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "EquipmentMonitoringDb"
            )
        )
);

builder.Services.AddScoped<PredictionRepository>();
builder.Services.AddScoped<EquipmentRepository>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddHttpClient("MlService", client =>
{
    client.BaseAddress =
        new Uri("http://127.0.0.1:8000");

    client.Timeout =
        TimeSpan.FromSeconds(10);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AngularDevelopment",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:4200"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection(
        "SmtpSettings"
    )
);

builder.Services.AddScoped<
    EmailService
>();

builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection(
        "NotificationSettings"
    )
);

builder.Services.AddScoped<
    EmailNotificationRepository
>();

builder.Services.AddScoped<
    AuditRepository
>();

builder.Services.AddScoped<
    DashboardRepository
>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularDevelopment");

app.UseAuthorization();

app.MapControllers();

app.Run();