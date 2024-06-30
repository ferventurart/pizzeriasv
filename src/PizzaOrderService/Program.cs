using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaOrderService;
using PizzaOrderService.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Pizza Order API",
        Description = "An ASP.NET Core Web API for managing pizza orders.",
        Contact = new OpenApiContact
        {
            Name = "Fernando Ventura",
            Url = new Uri("https://github.com/ferventurart")
        },
        License = new OpenApiLicense
        {
            Name = "License MIT",
            Url = new Uri("https://opensource.org/license/mit")
        }
    });
});
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "PizzaOrders");
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("ServiceBus"));
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebApp",
        policy  =>
        {
            policy.WithOrigins("https://localhost:7160")
                  .AllowAnyHeader()
                  .WithMethods("POST", "PUT", "DELETE", "GET");
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.RegisterOrdersEndpoints();

app.UseCors("WebApp");

app.UseHttpsRedirection();

app.Run();