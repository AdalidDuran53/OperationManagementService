using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OperationManagementService.Filters;
using OperationManagementService.Implementation;
using OperationManagementService.Security;
using OperationManagementService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen( options => options.OperationFilter<AuthenticationKeyHeader>());

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CustomAuthenticationHandler.SchemaName)
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(
        CustomAuthenticationHandler.SchemaName, options => {
        });

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(0, 1);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

builder.Services.AddScoped<UserImplementation>();
builder.Services.AddScoped<OperationLogImplementation>();
builder.Services.AddDbContext<OperationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.Use(async (context, next) => {
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
