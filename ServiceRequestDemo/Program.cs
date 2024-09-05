using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceRequestDemo.AutoMapper;
using ServiceRequestDemo.DatabaseAccess;
using ServiceRequestDemo.Repository;
using ServiceRequestDemo.Repository.Interfaces;
using ServiceRequestDemo.Service;
using ServiceRequestDemo.Service.Interfaces;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("SQLServerConnection");
builder.Services.AddDbContext<ServiceRequestContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IServiceRequestService, ServiceRequestService>();
builder.Services.AddTransient<IServiceRequestRepository, ServiceRequestRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
