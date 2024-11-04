using System.Reflection;
using Br1InterviewPreparation.API.Middlewares;
using Br1InterviewPreparation.API.Settings;
using Br1InterviewPreparation.Application.Extensions;
using Br1InterviewPreparation.Infrastructure.Data;
using Br1InterviewPreparation.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

// TODO: put this logic somewhere else
var allowedOriginsString = builder.Configuration["CorsSettings:AllowedOrigins"];

if (string.IsNullOrWhiteSpace(allowedOriginsString))
{
    throw new InvalidOperationException("CorsSettings:AllowedOrigins configuration is missing or empty.");
}

var allowedOrigins = allowedOriginsString
    .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)
    .Select(origin => origin.Trim())
    .ToArray();

if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("No valid origins found in CorsSettings:AllowedOrigins configuration.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policyBuilder =>
    {
        policyBuilder.WithOrigins(allowedOrigins)
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    });
});

var app = builder.Build();

await DatabaseInitializer.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
