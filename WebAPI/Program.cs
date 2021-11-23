using Infrastructure.Persistence;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers()
	.AddFluentValidation();

builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseInfrastructure(builder.Configuration);

app.Run();