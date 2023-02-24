using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VacationRental.Application;
using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Configuration;
using VacationRental.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// HttpClient
builder.Services.AddHttpClient();

// Application services injection
builder.Services.AddApplicationServices();

// Infrastructure services injection
builder.Services.AddInfrastructureServices();

builder.Services.AddMvc();

/*  
 *  Presentation layer validation pipeline
 *  ATM we are using a custom behaviour pipeline (ValidationBehaviour) to validate model on Mediator requests for queries/commands
 *  Enable to validate on API request model binding pipeline
*/
//builder.Services.AddFluentValidationAutoValidation()
//                .AddFluentValidationClientsideAdapters();

// Dev environment service configurations
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Vacation rental information",
            Version = "v1"
        });

        List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
    }).AddSwaggerGenNewtonsoftSupport();    
}

var app = builder.Build();

// App Dev environment configuration
if (app.Environment.IsDevelopment())
{
    // Local file logger
    var loggerFactory = app.Services.GetService<ILoggerFactory>();
    loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

    // For handling swagger XML documentation
    app.UseStaticFiles();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var dbSeeder = new DatabaseSeeder(context);

        await dbSeeder.Seed();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}



app.UseHttpsRedirection();

//app.UseRouting();

app.MapControllers();

app.Run();
