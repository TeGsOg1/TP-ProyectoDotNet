using SGE.WebAPI.Dependencias;
using SGE.WebAPI.Endpoints;
using SGE.WebAPI.Middlewares;
using Scalar.AspNetCore;
using SGE.Aplicacion; 
using SGE.Infraestructura; 

var builder = WebApplication.CreateBuilder(args);

// --- Registro de Servicios en el Contenedor DI ---
builder.Services.AddOpenApi()
    .AddAplicacion() 
    .AddInfraestructura(builder.Configuration) 
    .AddAutenticacionJWT(builder.Configuration) 
    .AddProblemDetails()
    .AddExceptionHandler<ExcepcionGlobalMiddleware>();

var app = builder.Build();

// --- Configuración del Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// --- Mapeo de Endpoints ---
app.MapGet("/", () => "¡La API del SGE está funcionando correctamente!");
app.MapAutorizacionEndpoints();
app.MapExpedienteEndpoints();
app.MapTramiteEndpoints();
app.MapUsuariosEndpoints();

app.Run();