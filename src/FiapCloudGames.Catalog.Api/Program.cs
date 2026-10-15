var builder = WebApplication.CreateBuilder(args);

builder.AddFiapServiceDefaults();
builder.Services.AddFiapApiDefaults();
builder.Services.AddFiapServiceAuthentication(builder.Configuration);
builder.Services.AddCatalogInfrastructure(builder.Configuration);
builder.Services.AddCatalogPresentation();
builder.Services.AddCatalogOpenApi();

var app = builder.Build();

app.UseFiapApiDefaults();
app.UseCatalogOpenApi();
app.UseAuthentication();
app.UseAuthorization();

app.MapFiapHealthEndpoints();
app.MapControllers();

await app.RunAsync();

public partial class Program;
