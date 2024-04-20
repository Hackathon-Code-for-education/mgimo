using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Your API Name",
            Description = "Your API Description",
            Version = "v1",
            TermsOfService = null, 
            Contact = new OpenApiContact 
            {
                
            },
            License = new OpenApiLicense 
            {
                
                Name = "Proprietary",
                Url = new Uri("https://someURLToLicenseInfo.com")
            }
        });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();