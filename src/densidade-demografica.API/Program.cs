using densidade_demografica.API.Infrastructure.Parsers;
using densidade_demografica.API.Repositories.Implementations;
using densidade_demografica.API.Repositories.Interfaces;
using densidade_demografica.API.Services.Implementations;
using densidade_demografica.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Controllers and API
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Brazilian Demographic Density API",
        Version = "v1",
        Description = "API for querying Brazilian demographic density data",
        Contact = new()
        {
            Name = "Ederson Jasper",
            Email = "edersonjasper@outlook.com",
            Url = new Uri("https://github.com/edersonjasper")
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Dependency Injection - SOLID (D - Dependency Inversion)
builder.Services.AddSingleton<ICsvParser, DensidadeDemograficaCsvParser>();
builder.Services.AddSingleton<IDensidadeDemograficaRepository, DensidadeDemograficaCSVRepository>();
builder.Services.AddScoped<IDensidadeDemograficaService, DensidadeDemograficaService>();

// CORS (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0"
}))
.WithName("HealthCheck");

// Load data on startup
try
{
    var dataPath = Path.Combine(app.Environment.ContentRootPath, "Data");
    var repository = app.Services.GetRequiredService<IDensidadeDemograficaRepository>();
    await repository.LoadDataFromCSV(dataPath);
    app.Logger.LogInformation("Data loaded successfully");
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Failed to load initial data");
    throw;
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Required for integration tests
public partial class Program { }