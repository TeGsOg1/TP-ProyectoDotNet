using SGE.WebAPI.Dependencias;
using SGE.WebAPI.Endpoints;
using SGE.WebAPI.Middlewares;
using Scalar.AspNetCore;
using SGE.Aplicacion; 
using SGE.Infraestructura.Extensiones;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi()
    .AddAplicacion() 
    .AddInfraestructura(builder.Configuration) 
    .AddAutenticacionJWT(builder.Configuration) 
    .AddProblemDetails()
    .AddExceptionHandler<ExcepcionGlobalMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "¡La API del SGE está funcionando correctamente!");
app.MapAutorizacionEndpoints();
app.MapExpedienteEndpoints();
app.MapTramiteEndpoints();
app.MapUsuariosEndpoints();

app.Run();
