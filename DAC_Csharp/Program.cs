using DAC_CSharp.Application.Interfaces;
using DAC_CSharp.Application.Services;
using DAC_CSharp.Infrastructure.Database;
using DAC_CSharp.Infrastructure.Repositories;
using Scalar.AspNetCore;

AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.DisableEnhancedModelMetadata", false);
AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.UseEnhancedModelMetadata", true);
AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.ApiExplorer.DisableEnhancedModelMetadata", false);
AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.ApiExplorer.IsEnhancedModelMetadataSupported", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver();
});

var connectionString = "Server=localhost\\SQLEXPRESS;Database=DacBD;Integrated Security=True;TrustServerCertificate=True;";

builder.Services.AddSingleton(new DbConnectionFactory(connectionString));
builder.Services.AddScoped<IMunicipioRepository, MunicipioRepository>();
builder.Services.AddScoped<IEscolaRepository, EscolaRepository>();
builder.Services.AddScoped<IDadosEducacionaisRepository, DadosEducacionaisRepository>();
builder.Services.AddScoped<IMunicipioQueryRepository, MunicipioQueryRepository>();
builder.Services.AddScoped<IEscolaQueryRepository, EscolaQueryRepository>();
builder.Services.AddScoped<IIndicadorEducacionalQueryRepository, IndicadorEducacionalQueryRepository>();
builder.Services.AddScoped<IDashboardQueryRepository, DashboardQueryRepository>();
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();
builder.Services.AddScoped<IImportadorService, ImportadorService>();
builder.Services.AddScoped<IMunicipioQueryService, MunicipioQueryService>();
builder.Services.AddScoped<IEscolaQueryService, EscolaQueryService>();
builder.Services.AddScoped<IIndicadorEducacionalQueryService, IndicadorEducacionalQueryService>();
builder.Services.AddScoped<IDashboardQueryService, DashboardQueryService>();
builder.Services.AddHttpClient<IDownloaderService, DownloaderService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendLocal", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("FrontendLocal");

app.MapOpenApi();
// MapScalarApiReference extension is provided by Scalar.AspNetCore.
app.MapScalarApiReference();
app.MapControllers();
app.MapGet("/", () => "API de importação disponível.");
app.MapGet("/swagger", () => Results.Redirect("/scalar/v1"));

app.Run();