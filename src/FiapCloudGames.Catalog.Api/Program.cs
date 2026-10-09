var builder = WebApplication.CreateBuilder(args);

builder.AddFiapServiceDefaults();
builder.Services.AddFiapApiDefaults();
builder.Services.AddCatalogInfrastructure(builder.Configuration);
builder.Services.AddCatalogPresentation();
builder.Services.AddCatalogOpenApi();

//TODO: verificar o building blocks.
//builder.Services.AddFiapServiceAuthentication(builder.Configuration);

var app = builder.Build();

app.UseFiapApiDefaults();
app.UseCatalogOpenApi();
app.UseAuthentication();
app.UseAuthorization();

app.MapFiapHealthEndpoints();
app.MapControllers();

await app.RunAsync();

public partial class Program;
